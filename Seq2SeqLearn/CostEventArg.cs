using System;

namespace Seq2SeqLearn
{
    public class CostEventArg : EventArgs
    {
        public double Cost { get; set; }
        public int Iteration { get; set; }
    }
}