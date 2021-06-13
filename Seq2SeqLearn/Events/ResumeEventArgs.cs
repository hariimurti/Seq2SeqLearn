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

        public ResumeEventArgs(TrainingInfo info)
        {
            StartEpoch = info.NextEpoch;
            EndEpoch = info.TotalEpoch;
            TrainedData = info.TrainedData;
            TotalData = info.TotalData;
            LastTime = info.LastTime;
        }

        public int InPercent()
        {
            var p = (double)TrainedData / TotalData * 100;
            return (int)p;
        }
    }
}
