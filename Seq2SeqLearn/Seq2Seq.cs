using Seq2SeqLearn.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seq2SeqLearn
{
    public class Seq2Seq
    {
        private IOSequences io = new IOSequences();
        private readonly FileBinary binIO = new FileBinary("Seq2SeqIO.bin");

        private ModelAttentionData model = new ModelAttentionData();
        private readonly FileBinary binModel = new FileBinary("Seq2SeqModel.bin");

        private readonly Optimizer solver = new Optimizer();
        private Dictionary<string, int> wordToIndex = new Dictionary<string, int>();
        private Dictionary<int, string> indexToWord = new Dictionary<int, string>();
        private List<string> vocab = new List<string>();

        // default values
        public int MaxPredictionWord = 100; // max length of generated sentences
        // optimization  hyperparameters
        public double RegC = 0.000001; // L2 regularization strength
        public double LearningRate = 0.001; // learning rate
        public double ClipVal = 5.0; // clip gradients at this value

        public event EventHandler IterationDone;

        public Seq2Seq()
        {
            // Load existing data
            if (!binModel.IsExist() && !binIO.IsExist()) return;
            if (binModel.Read() is ModelAttentionData model && binIO.Read() is IOSequences io)
            {
                this.model = model;
                this.io = io;
                OneHotEncoding(io.Input, io.Output);
            }
        }

        public void SetData(int inputSize, int hiddenSize, int depth, List<List<string>> input, List<List<string>> output, bool useDropout)
        {
            // IO is changed
            var isChanged = io.Input?.AreEqual(input) == false || io.Output?.AreEqual(output) == false;
            // Model is changed
            if (model.InputSize != inputSize) isChanged = true;
            if (model.HiddenSize != hiddenSize) isChanged = true;
            if (model.Depth != depth) isChanged = true;
            if (model.UseDropout != useDropout) isChanged = true;

            if (isChanged)
            {
                io = new IOSequences
                {
                    Input = input,
                    Output = output
                };
                OneHotEncoding(input, output);

                model = new ModelAttentionData
                {
                    InputSize = inputSize,
                    HiddenSize = hiddenSize,

                    Embedding = new WeightMatrix(vocab.Count + 2, inputSize, true),
                    encoder = new Encoder(hiddenSize, inputSize, depth),
                    reversEncoder = new Encoder(hiddenSize, inputSize, depth),
                    decoder = new AttentionDecoder(hiddenSize, inputSize, depth),
                    UseDropout = useDropout,

                    Whd = new WeightMatrix(hiddenSize, vocab.Count + 2, true),
                    Bd = new WeightMatrix(1, vocab.Count + 2, 0),
                    Depth = depth
                };

                // delete existing trained model
                binIO.Delete();
                binModel.Delete();
            }
        }

        private void OneHotEncoding(List<List<string>> _input, List<List<string>> _output)
        {
            // count up all words
            Dictionary<string, int> d = new Dictionary<string, int>();
            wordToIndex = new Dictionary<string, int>();
            indexToWord = new Dictionary<int, string>();
            vocab = new List<string>();
            for (int j = 0, n2 = _input.Count; j < n2; j++)
            {
                var item = _input[j];
                for (int i = 0, n = item.Count; i < n; i++)
                {
                    var txti = item[i];
                    if (d.Keys.Contains(txti)) { d[txti] += 1; }
                    else { d.Add(txti, 1); }
                }

                var item2 = _output[j];
                for (int i = 0, n = item2.Count; i < n; i++)
                {
                    var txti = item2[i];
                    if (d.Keys.Contains(txti)) { d[txti] += 1; }
                    else { d.Add(txti, 1); }
                }
            }

            // NOTE: start at one because we will have START and END tokens!
            // that is, START token will be index 0 in model word vectors
            // and END token will be index 0 in the next word softmax
            var q = 2;
            foreach (var ch in d)
            {
                if (ch.Value >= 1)
                {
                    // add word to vocab
                    wordToIndex[ch.Key] = q;
                    indexToWord[q] = ch.Key;
                    vocab.Add(ch.Key);
                    q++;
                }
            }
        }

        public void Train(int trainingEpoch)
        {
            for (int ep = 0; ep < trainingEpoch; ep++)
            {
                Random r = new Random();
                for (int itr = 0; itr < io.Input.Count; itr++)
                {
                    // sample sentence from data
                    List<string> OutputSentence;
                    ComputeGraph g;
                    double cost;
                    List<WeightMatrix> encoded = new List<WeightMatrix>();
                    Encode(r, out OutputSentence, out g, out cost, encoded);
                    cost = DecodeOutput(OutputSentence, g, cost, encoded);

                    g.backward();
                    UpdateParameters();
                    Reset();
                    if (IterationDone != null)
                    {
                        IterationDone(this, new CostEventArg()
                        {
                            Cost = cost / OutputSentence.Count
                            ,
                            Iteration = ep
                        });
                    }
                }
            }
        }

        private void Encode(Random r, out List<string> OutputSentence, out ComputeGraph g, out double cost, List<WeightMatrix> encoded)
        {
            var sentIndex = r.Next(0, io.Input.Count);
            var inputSentence = io.Input[sentIndex];
            var reversSentence = io.Input[sentIndex].ToList();
            reversSentence.Reverse();
            OutputSentence = io.Output[sentIndex];
            g = new ComputeGraph();

            cost = 0.0;

            for (int i = 0; i < inputSentence.Count; i++)
            {
                int ix_source = wordToIndex[inputSentence[i]];
                int ix_source2 = wordToIndex[reversSentence[i]];
                var x = g.PeekRow(model.Embedding, ix_source);
                var eOutput = model.encoder.Encode(x, g);
                var x2 = g.PeekRow(model.Embedding, ix_source2);
                var eOutput2 = model.reversEncoder.Encode(x2, g);
                encoded.Add(g.concatColumns(eOutput, eOutput2));
            }
        }

        private double DecodeOutput(List<string> OutputSentence, ComputeGraph g, double cost, List<WeightMatrix> encoded)
        {
            int ix_input = 1;
            for (int i = 0; i < OutputSentence.Count + 1; i++)
            {
                int ix_target;
                if (i == OutputSentence.Count)
                {
                    ix_target = 0;
                }
                else
                {
                    ix_target = wordToIndex[OutputSentence[i]];
                }

                var x = g.PeekRow(model.Embedding, ix_input);
                var eOutput = model.decoder.Decode(x, encoded, g);
                if (model.UseDropout)
                {
                    eOutput = g.Dropout(eOutput, 0.2);
                }
                var o = g.add(
                       g.mul(eOutput, model.Whd), model.Bd);
                if (model.UseDropout)
                {
                    o = g.Dropout(o, 0.2);
                }

                var probs = g.SoftmaxWithCrossEntropy(o);

                cost += -Math.Log(probs.Weight[ix_target]);

                o.Gradient = probs.Weight;
                o.Gradient[ix_target] -= 1;
                ix_input = ix_target;
            }
            return cost;
        }

        private void UpdateParameters()
        {
            var updated = model.encoder.getParams();
            updated.AddRange(model.decoder.getParams());
            updated.AddRange(model.reversEncoder.getParams());
            updated.Add(model.Embedding);
            updated.Add(model.Whd);
            updated.Add(model.Bd);
            solver.setp(updated, LearningRate, RegC, ClipVal);
        }

        private void Reset()
        {
            model.encoder.Reset();
            model.reversEncoder.Reset();
            model.decoder.Reset();
        }

        public List<string> Predict(List<string> inputSeq)
        {
            model.reversEncoder.Reset();
            model.encoder.Reset();
            model.decoder.Reset();

            List<string> result = new List<string>();

            var G2 = new ComputeGraph(false);

            List<string> revseq = inputSeq.ToList();
            revseq.Reverse();
            List<WeightMatrix> encoded = new List<WeightMatrix>();
            for (int i = 0; i < inputSeq.Count; i++)
            {
                int ix = wordToIndex[inputSeq[i]];
                int ix2 = wordToIndex[revseq[i]];
                var x2 = G2.PeekRow(model.Embedding, ix);
                var o = model.encoder.Encode(x2, G2);
                var x3 = G2.PeekRow(model.Embedding, ix2);
                var eOutput2 = model.reversEncoder.Encode(x3, G2);

                var d = G2.concatColumns(o, eOutput2);

                encoded.Add(d);
            }

            var ix_input = 1;
            while (true)
            {
                var x = G2.PeekRow(model.Embedding, ix_input);
                var eOutput = model.decoder.Decode(x, encoded, G2);
                if (model.UseDropout)
                {
                    for (int i = 0; i < eOutput.Weight.Length; i++)
                    {
                        eOutput.Weight[i] *= 0.2;
                    }
                }
                var o = G2.add(
                       G2.mul(eOutput, model.Whd), model.Bd);
                if (model.UseDropout)
                {
                    for (int i = 0; i < o.Weight.Length; i++)
                    {
                        o.Weight[i] *= 0.2;
                    }
                }
                var probs = G2.SoftmaxWithCrossEntropy(o);
                var maxv = probs.Weight[0];
                var maxi = 0;
                for (int i = 1; i < probs.Weight.Length; i++)
                {
                    if (probs.Weight[i] > maxv)
                    {
                        maxv = probs.Weight[i];
                        maxi = i;
                    }
                }
                var pred = maxi;

                if (pred == 0) break; // END token predicted, break out

                if (result.Count > MaxPredictionWord) { break; } // something is wrong
                var letter2 = indexToWord[pred];
                result.Add(letter2);
                ix_input = pred;
            }

            return result;
        }

        public void Save()
        {
            var binIO = new FileBinary("Seq2SeqIO.bin");
            binIO.Write(io);

            var binModel = new FileBinary("Seq2SeqModel.bin");
            binModel.Write(model);
        }
    }
}