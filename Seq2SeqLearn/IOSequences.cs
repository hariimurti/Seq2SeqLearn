using System;
using System.Collections.Generic;

namespace Seq2SeqLearn
{
    [Serializable]
    public class IOSequences
    {
        public List<List<string>> Input { get; set; }
        public List<List<string>> Output { get; set; }
    }
}
