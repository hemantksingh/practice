using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionEvaluator
{
    public interface ITextReader
    {
        int CurrentLineNo { get; }
        string CurrentLine { get; }
        string ReadLine();
        void Clear();
    }
}
