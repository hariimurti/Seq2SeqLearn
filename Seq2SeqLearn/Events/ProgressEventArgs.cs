using System;

namespace Seq2SeqLearn.Events
{
    public class ProgressEventArgs : EventArgs
    {
        public int Epoch { get; }
        public double Cost { get; }
        public int TrainedData { get; }
        public int TotalData { get; }

        public ProgressEventArgs(int ep, double cost, TrainingInfo info)
        {
            Epoch = ep;
            Cost = cost;
            TrainedData = info.TrainedData;
            TotalData = info.TotalData;
        }

        public int InPercent()
        {
            var p = (double)TrainedData / TotalData * 100;
            return (int)p;
        }
    }
}
