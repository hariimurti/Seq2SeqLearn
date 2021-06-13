using System;

namespace Seq2SeqLearn.Events
{
    public class StopEventArgs : EventArgs
    {
        public int TrainedData { get; }
        public int TotalData { get; }
        public string Message { get; }
        public bool IsError { get; }

        public StopEventArgs(TrainingInfo info, string msg, bool isError)
        {
            TrainedData = info.TrainedData;
            TotalData = info.TotalData;
            Message = msg;
            IsError = isError;
        }

        public int InPercent()
        {
            var p = (double)TrainedData / TotalData * 100;
            return (int)p;
        }
    }
}
