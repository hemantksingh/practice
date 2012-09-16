using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionEvaluator
{
    public interface ICondition
    {
        ContitionType ContitionType { get; }
        bool IsSatisfied();
    }
}
