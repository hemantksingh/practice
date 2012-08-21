using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Swinton.QuotesEngine.Interface;

namespace Swinton.QuotesEngine
{
    class QuoteEngine : IRunnable
    {
        private IRulesProvider _provider;
        private StringBuilder _output;
        private IReader _reader;

        public QuoteEngine(IReader reader, IRulesProvider provider)
        {
            _reader = reader;
            _provider = provider;
            _output = new StringBuilder();
        }
        
        public void Run()
        {
            InsuranceBasis basis = _reader.Read("Input.txt");
            
            CalculatePremium(basis);

            Console.WriteLine(_output);
        }

        public double CalculatePremium(InsuranceBasis basis)
        {
            double netPremium = 0.0;
            
            double basePremium = _provider.GetBasePremium(basis.Type);
            GenerateOutput("BasePremium", basePremium,  ref netPremium);

            double ageRating = _provider.GetRating("Age",basis.Age);
            double premiumChange = CalculatePremiumChange(netPremium, ageRating);
            GenerateOutput("Age", premiumChange, ref netPremium);

            double sexRating = _provider.GetRating("Sex", basis.Sex);
            premiumChange = CalculatePremiumChange(netPremium, sexRating);
            GenerateOutput("Sex", premiumChange, ref netPremium);

            double destinationRating = _provider.GetRating("Destination", basis.Destination);
            premiumChange = CalculatePremiumChange(netPremium, destinationRating);
            GenerateOutput("Destination", premiumChange, ref netPremium);

            double travelPeriodRating = _provider.GetRating("TravelPeriod", basis.TravelPeriod);
            premiumChange = CalculatePremiumChange(netPremium, travelPeriodRating);
            GenerateOutput("PeriodOfTravel", premiumChange, ref netPremium);

            return netPremium;
        }

        private static double CalculatePremiumChange(double netPremium, double premiumBasisRating)
        {
            double baseRating = 1.0;
            return netPremium * (premiumBasisRating - baseRating);
        }

        private double GenerateOutput(string premiumBasis, double effectivePremium,  ref double netPremium)
        {
            //double premiumChange = CalculatePremiumChange(netPremium, _provider.GetRating(premiumBasis, _basis.
            netPremium += effectivePremium;
            
            _output.Append(premiumBasis);
            _output.Append(" (");
            _output.Append(string.Format("{0:00.00}", effectivePremium));
            _output.Append("): ");
            _output.AppendLine(string.Format("{0:00.00}", netPremium));
            
            return netPremium;
        }

        private void LoadPremiumRules()
        {

            XDocument premiumRules = XDocument.Load("PremiumRules.xml");
            IEnumerable<string> premiumBasises = from rule in premiumRules.Descendants("PremiumRule")
                                       select (string) rule.Element("PremiumBasis").Value;

            foreach(string premiumBasis in premiumBasises)
                Console.WriteLine(premiumBasis);
            
            XDocument purchaseOrder = XDocument.Load("PurchaseOrder.xml");
            IEnumerable<XElement> partNos = from item in purchaseOrder.Descendants("Item")
                                    where (int)item.Element("Quantity") *
                                                                (decimal)item.Element("USPrice") > 100
                                    orderby (string)item.Element("PartNumber")
                                    select item;

            foreach (string partNo in partNos)
                Console.WriteLine(partNo);

            string text = "";
            text.WordCount();

        }

        Func<string, string> safe = t =>
        {
            return String.IsNullOrWhiteSpace(t) ? String.Empty : t;
        };
    }
}
