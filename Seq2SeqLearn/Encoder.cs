using System;
using System.Collections.Generic;

namespace Seq2SeqLearn
{
    [Serializable]
    public class Encoder
    {
        private List<LSTMCell> encoders = new List<LSTMCell>();
        private int hdim { get; set; }
        private int dim { get; set; }
        private int depth { get; set; }

        public Encoder(int hdim, int dim, int depth)
        {
            encoders.Add(new LSTMCell(hdim, dim));

            //for (int i = 1; i < depth; i++)
            //{
            //   encoders.Add(new LSTMCell(hdim, hdim));

            //}
            this.hdim = hdim;
            this.dim = dim;
            this.depth = depth;
        }

        public void Reset()
        {
            foreach (var item in encoders)
            {
                item.Reset();
            }
        }

        public WeightMatrix Encode(WeightMatrix V, ComputeGraph g)
        {
            foreach (var encoder in encoders)
            {
                var e = encoder.Step(V, g);
                V = e;
            }
            return V;
        }

        public List<WeightMatrix> Encode2(WeightMatrix V, ComputeGraph g)
        {
            List<WeightMatrix> res = new List<WeightMatrix>();
            foreach (var encoder in encoders)
            {
                var e = encoder.Step(V, g);
                V = e;
                res.Add(e);
            }
            return res;
        }

        public List<WeightMatrix> getParams()
        {
            List<WeightMatrix> response = new List<WeightMatrix>();

            foreach (var item in encoders)
            {
                response.AddRange(item.getParams());
            }

            return response;
        }
    }
}