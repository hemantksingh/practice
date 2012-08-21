using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swinton.QuotesEngine.Interface
{
    public interface IReader
    {
        InsuranceBasis Read(string inputFile);
    }
}
