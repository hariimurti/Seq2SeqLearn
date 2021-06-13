using System;

namespace Seq2SeqLearn.Events
{
    public class CompleteEventArgs : EventArgs
    {
        public int TrainedData { get; }
        public DateTime LastTime { get; }
        public bool StartOver { get; }

        public CompleteEventArgs(TrainingData data, bool startover = true)
        {
            TrainedData = data.TrainedData;
            LastTime = data.LastTime;
            StartOver = startover;
        }
    }
}
