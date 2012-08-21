using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Swinton.QuotesEngine.Interface;

namespace Swinton.QuotesEngine
{
    class InsuranceInputReader : IReader
    {
        public InsuranceBasis Read(string inputFile)
        {
            InsuranceBasis basis = new InsuranceBasis();
            try
            {
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(inputFile))
                {
                    String line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] inputs = line.Split(':');

                        switch (inputs[0])
                        {
                            case "Type":
                                basis.Type = (InsuranceType) Enum.Parse(typeof(InsuranceType), inputs[1]);
                                break;
                            case "Age":
                                basis.Age = Convert.ToInt16(inputs[1]);
                                break;
                            case "Sex":
                                basis.Sex = inputs[1];
                                break;
                            case "Destination":
                                basis.Destination = inputs[1];
                                break;
                            case "PeriodOfTravel":
                                basis.TravelPeriod = Convert.ToInt32(inputs[1]);
                                break;
                        }
                    }
                }

                return basis;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error occured while reading the file: {0}", inputFile), e);
            }
        }
    }
}
