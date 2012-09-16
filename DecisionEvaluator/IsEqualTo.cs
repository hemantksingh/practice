using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionEvaluator
{
    class IsEqualTo<T> : ICondition where T : class
    {
        private readonly T x;

        private readonly T y;

        public IsEqualTo(T x, T y)
        {
            this.x = x;
            this.y = y;
        }

        public ContitionType ContitionType
        {
            get
            {
                return ContitionType.IsEqualTo;
            }
        }

        public bool IsSatisfied()
        {
            return x == y;
        }
    }
}
