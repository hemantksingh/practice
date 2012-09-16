using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionEvaluator
{
    public interface ILanguageTranslator
    {
        Symbol CurrentSymbol { get; }
        string IdentifierOrText { get; }

        void GetSymbol();
        void Error(string message, params  object[] args);
    }
}
