using System;

namespace Seq2SeqLearn.Events
{
    public class ResumeEventArgs : EventArgs
    {
        public int StartEpoch { get; }
        public int EndEpoch { get; }
        public int TrainedData { get; }
        public int TotalData { get; }
        public DateTime LastTime { get; }

        public ResumeEventArgs(TrainingData data)
        {
            StartEpoch = data.NextEpoch;
            EndEpoch = data.TotalEpoch;
            TrainedData = data.TrainedData;
            TotalData = data.TotalData;
            LastTime = data.LastTime;
        }

        public int InPercent()
        {
            var p = (double)TrainedData / TotalData * 100;
            return (int)p;
        }
    }
}
