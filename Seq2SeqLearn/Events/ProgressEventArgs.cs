using System;

namespace Seq2SeqLearn.Events
{
    public class ProgressEventArgs : EventArgs
    {
        public int Epoch { get; }
        public double Cost { get; }
        public int TrainedData { get; }
        public int TotalData { get; }

        public ProgressEventArgs(int ep, double cost, TrainingData data)
        {
            Epoch = ep;
            Cost = cost;
            TrainedData = data.TrainedData;
            TotalData = data.TotalData;
        }

        public int InPercent()
        {
            var p = (double)TrainedData / TotalData * 100;
            return (int)p;
        }
    }
}
