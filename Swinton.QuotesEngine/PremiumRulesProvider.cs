using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Swinton.QuotesEngine.Interface;

namespace Swinton.QuotesEngine
{
    class PremiumRulesProvider : IRulesProvider
    {
        private XDocument _premiumRules;

        public PremiumRulesProvider()
        {
            this._premiumRules = XDocument.Load("PremiumRules.xml");
        }

        public double GetBasePremium(InsuranceType insuranceType)
        {
            string basePremium = (from bp in _premiumRules.Descendants("BasePremium")
                                  select bp.Element(insuranceType.ToString())).Single().Value;

            return Convert.ToDouble(basePremium);
        }

        public double GetRating(string premiumBasis, int premiumBasisValue)
        {
            IEnumerable<XElement> premiumRules = PremiumRulesFor(premiumBasis);

            IEnumerable<XElement> premiumRule =
                premiumRules.Where(rule => premiumBasisValue >= Convert.ToInt16(rule.Element("PremiumBasis").Attribute("Min").Value) &&
                                          premiumBasisValue <= Convert.ToInt16(rule.Element("PremiumBasis").Attribute("Max").Value));


            return Convert.ToDouble(premiumRule.Descendants("Rating").Single().Value);
        }

        public double GetRating(string premiumBasis, string premiumBasisValue)
        {
            IEnumerable<XElement> premiumRules = PremiumRulesFor(premiumBasis);

            IEnumerable<XElement> premiumRule =
                premiumRules.Where(rule => rule.Element("PremiumBasis").Attribute("Type").Value == premiumBasisValue);

            return Convert.ToDouble(premiumRule.Descendants("Rating").Single().Value);
        }

        private IEnumerable<XElement> PremiumRulesFor(string premiumBasis)
        {
            IEnumerable<XElement> basisPremiumRules = from rule in _premiumRules.Descendants("PremiumRule")
                                                      where rule.Element("PremiumBasis").Value == premiumBasis
                                                      select rule;
            return basisPremiumRules;
        }
    }
}
