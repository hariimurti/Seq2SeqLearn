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

        // training info
        public TrainingInfo Info { get; set; }

        public ModelAttentionData()
        {
            Info = new TrainingInfo();
        }
    }

    [Serializable]
    public class TrainingInfo
    {
        public int NextEpoch { get; set; }
        public int TotalEpoch { get; set; }
        public int TrainedData { get; set; }
        public int TotalData { get; set; }
        public DateTime LastTime { get; set; }

        public bool IsComplete()
        {
            if (TotalEpoch == 0) return false;
            return TotalEpoch == NextEpoch;
        }
    }
}
