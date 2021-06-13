using System;

namespace Seq2SeqLearn.Events
{
    public class CompleteEventArgs : EventArgs
    {
        public int TrainedData { get; }
        public DateTime LastTime { get; }
        public bool StartOver { get; }

        public CompleteEventArgs(TrainingInfo info, bool startover = true)
        {
            TrainedData = info.TrainedData;
            LastTime = info.LastTime;
            StartOver = startover;
        }
    }
}
