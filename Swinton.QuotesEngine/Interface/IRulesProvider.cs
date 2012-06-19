using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swinton.QuotesEngine.Interface
{
    public interface IRulesProvider
    {
        double GetBasePremium(InsuranceType insuaranceType);
        double GetRating(string premiumBasis, int value);
        double GetRating(string premiumBasis, string value);
    }
}
