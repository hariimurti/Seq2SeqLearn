using System;
using System.Collections.Generic;
using System.Linq;

namespace Seq2SeqLearn
{
    [Serializable]
    public class AttentionDecoder
    {
        private List<LSTMAttentionDecoderCell> decoders = new List<LSTMAttentionDecoderCell>();
        private int hdim { get; set; }
        private int dim { get; set; }
        private int depth { get; set; }
        private AttentionUnit Attention { get; set; }

        public AttentionDecoder(int hdim, int dim, int depth)
        {
            decoders.Add(new LSTMAttentionDecoderCell(hdim, dim));
            for (int i = 1; i < depth; i++)
            {
                decoders.Add(new LSTMAttentionDecoderCell(hdim, hdim));
            }
            Attention = new AttentionUnit(hdim);
            this.hdim = hdim;
            this.dim = dim;
            this.depth = depth;
        }

        public void Reset()
        {
            foreach (var item in decoders)
            {
                item.Reset();
            }
        }

        public WeightMatrix Decode(WeightMatrix input, List<WeightMatrix> encoderOutput, ComputeGraph g)
        {
            var V = input;
            var lastStatus = this.decoders.FirstOrDefault().ct;
            var context = Attention.Perform(encoderOutput, lastStatus, g);
            foreach (var encoder in decoders)
            {
                var e = encoder.Step(context, V, g);
                V = e;
            }

            return V;
        }

        public WeightMatrix Decode(WeightMatrix input, WeightMatrix encoderOutput, ComputeGraph g)
        {
            var V = input;
            var lastStatus = this.decoders.FirstOrDefault().ct;
            var context = Attention.Perform(encoderOutput, lastStatus, g);
            foreach (var encoder in decoders)
            {
                var e = encoder.Step(context, V, g);
                V = e;
            }

            return V;
        }

        public List<WeightMatrix> getParams()
        {
            List<WeightMatrix> response = new List<WeightMatrix>();

            foreach (var item in decoders)
            {
                response.AddRange(item.getParams());
            }
            response.AddRange(Attention.getParams());
            return response;
        }
    }
}