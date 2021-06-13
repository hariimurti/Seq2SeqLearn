using Seq2SeqLearn.Events;
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

        private readonly Optimizer optimizer = new Optimizer();
        private Dictionary<string, int> wordToIndex = new Dictionary<string, int>();
        private Dictionary<int, string> indexToWord = new Dictionary<int, string>();
        private List<string> vocab = new List<string>();

        // default values
        public int MaxPredictionWord = 100; // max length of generated sentences
        // optimization  hyperparameters
        public double RegC = 0.000001; // L2 regularization strength
        public double LearningRate = 0.001; // learning rate
        public double ClipVal = 5.0; // clip gradients at this value

        public event Action<ProgressEventArgs> OnProgress;
        public event Action<CompleteEventArgs> OnComplete;

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

        public void SetData(int inputSize, int hiddenSize, int depth, List<List<string>> inputSeq, List<List<string>> outputSeq, bool useDropout)
        {
            // IO is changed
            var isChanged = io.Input?.AreEqual(inputSeq) == false || io.Output?.AreEqual(outputSeq) == false;
            // Model is changed
            if (model.InputSize != inputSize) isChanged = true;
            if (model.HiddenSize != hiddenSize) isChanged = true;
            if (model.Depth != depth) isChanged = true;
            if (model.UseDropout != useDropout) isChanged = true;

            if (isChanged)
            {
                io = new IOSequences
                {
                    Input = inputSeq,
                    Output = outputSeq
                };
                OneHotEncoding(inputSeq, outputSeq);

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

        public void Training(int trainingEpoch)
        {
            // set training epoch
            model.Training.TotalEpoch = trainingEpoch;
            model.Training.TotalData = trainingEpoch * io.Input.Count;

            for (int ep = 0; ep < trainingEpoch; ep++)
            {
                Random r = new Random();
                for (int itr = 0; itr < io.Input.Count; itr++)
                {
                    // sample sentence from data
                    List<WeightMatrix> encoded = new List<WeightMatrix>();
                    EncodeInput(r, out List<string> outputSentence, out ComputeGraph g, out double cost, encoded);
                    cost = DecodeOutput(outputSentence, g, cost, encoded);

                    g.backward();
                    UpdateParameters();
                    ResetCoders();

                    // calc trained data
                    model.Training.TrainedData = (ep * io.Input.Count) + itr + 1;
                    OnProgress?.Invoke(new ProgressEventArgs(ep + 1, cost / outputSentence.Count, model.Training));
                }

                // update training data
                model.Training.NextEpoch = ep + 1;
                model.Training.LastTime = DateTime.Now;
            }

            OnComplete?.Invoke(new CompleteEventArgs(model.Training));
        }

        public List<string> Predict(List<string> inputSentence)
        {
            ResetCoders();

            var inputReverse = inputSentence.ToList();
            inputReverse.Reverse();

            var g = new ComputeGraph(false);
            var encoded = new List<WeightMatrix>();
            for (int i = 0; i < inputSentence.Count; i++)
            {
                int ix1 = wordToIndex[inputSentence[i]];
                var v1 = g.PeekRow(model.Embedding, ix1);
                var m1 = model.encoder.Encode(v1, g);

                int ix2 = wordToIndex[inputReverse[i]];
                var v2 = g.PeekRow(model.Embedding, ix2);
                var m2 = model.reversEncoder.Encode(v2, g);

                encoded.Add(g.concatColumns(m1, m2));
            }

            var ix_input = 1;
            var outputSentence = new List<string>();
            while (true)
            {
                var ix = g.PeekRow(model.Embedding, ix_input);
                var eOutput = model.decoder.Decode(ix, encoded, g);
                if (model.UseDropout)
                {
                    for (int i = 0; i < eOutput.Weight.Length; i++)
                    {
                        eOutput.Weight[i] *= 0.2;
                    }
                }
                var mOutput = g.add(g.mul(eOutput, model.Whd), model.Bd);
                if (model.UseDropout)
                {
                    for (int i = 0; i < mOutput.Weight.Length; i++)
                    {
                        mOutput.Weight[i] *= 0.2;
                    }
                }
                var probs = g.SoftmaxWithCrossEntropy(mOutput);
                var prob = probs.Weight[0];
                var pred = 0;
                for (int i = 1; i < probs.Weight.Length; i++)
                {
                    if (probs.Weight[i] > prob)
                    {
                        prob = probs.Weight[i];
                        pred = i;
                    }
                }
                if (pred == 0) break; // END token predicted
                if (outputSentence.Count > MaxPredictionWord) break; // something is wrong

                outputSentence.Add(indexToWord[pred]);
                ix_input = pred;
            }

            return outputSentence;
        }

        private void OneHotEncoding(List<List<string>> inputSeq, List<List<string>> outputSeq)
        {
            // count up all words
            Dictionary<string, int> d = new Dictionary<string, int>();
            wordToIndex = new Dictionary<string, int>();
            indexToWord = new Dictionary<int, string>();
            vocab = new List<string>();
            for (int j = 0, n2 = inputSeq.Count; j < n2; j++)
            {
                var item = inputSeq[j];
                for (int i = 0, n = item.Count; i < n; i++)
                {
                    var txti = item[i];
                    if (d.Keys.Contains(txti)) { d[txti] += 1; }
                    else { d.Add(txti, 1); }
                }

                var item2 = outputSeq[j];
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

        private void EncodeInput(Random r, out List<string> outputSentence, out ComputeGraph g, out double cost, List<WeightMatrix> encoded)
        {
            var sentIndex = r.Next(0, io.Input.Count);
            var inputSentence = io.Input[sentIndex];
            var reversSentence = io.Input[sentIndex].ToList();
            reversSentence.Reverse();
            outputSentence = io.Output[sentIndex];
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

        private double DecodeOutput(List<string> outputSentence, ComputeGraph g, double cost, List<WeightMatrix> encoded)
        {
            int ix_input = 1;
            for (int i = 0; i < outputSentence.Count + 1; i++)
            {
                int ix_target;
                if (i == outputSentence.Count)
                {
                    ix_target = 0;
                }
                else
                {
                    ix_target = wordToIndex[outputSentence[i]];
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
            var newModel = model.encoder.getParams();
            newModel.AddRange(model.decoder.getParams());
            newModel.AddRange(model.reversEncoder.getParams());
            newModel.Add(model.Embedding);
            newModel.Add(model.Whd);
            newModel.Add(model.Bd);
            optimizer.setp(newModel, LearningRate, RegC, ClipVal);
        }

        private void ResetCoders()
        {
            model.encoder.Reset();
            model.reversEncoder.Reset();
            model.decoder.Reset();
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