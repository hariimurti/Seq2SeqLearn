using System;

namespace Seq2SeqLearn
{
    [Serializable]
    public class ModelAttentionData
    {
        public int InputSize;
        public int HiddenSize;

        public WeightMatrix Embedding;
        public Encoder encoder;
        public Encoder reversEncoder;
        public AttentionDecoder decoder;
        public bool UseDropout { get; set; }

        // Output Layer Weights
        public WeightMatrix Whd { get; set; }
        public WeightMatrix Bd { get; set; }
        public int Depth { get; set; }
    }
}
