using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Swinton.QuotesEngine.Interface;

namespace Swinton.QuotesEngine
{
    public class QuoteEngineModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IReader>().To<InsuranceInputReader>();
            Bind<IRulesProvider>().To<PremiumRulesProvider>();
            Bind<IRunnable>().To<QuoteEngine>();
        }
    }
}
