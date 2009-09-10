using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;

namespace Outlook.Modules.Exchange
{
    public class ExchangeModule : IModule
    {
        private readonly IUnityContainer container;

        public ExchangeModule(IUnityContainer container)
        {
            this.container = container;
        }

        public void Initialize()
        {
            container.RegisterType<IExchangeService, ExchangeService>();
        }
    }
}
