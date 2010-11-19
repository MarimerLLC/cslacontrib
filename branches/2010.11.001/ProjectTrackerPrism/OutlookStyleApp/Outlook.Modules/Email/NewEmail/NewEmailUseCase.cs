using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Regions;
using Outlook.Modules.Email.NewEmail;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;

namespace Outlook.Modules.Email
{
    /// <summary>
    ///     Note, the NewEmailUsecase is still work in progress...
    /// </summary>
    public class NewEmailUseCase : ActiveAwareUseCaseController, IRegionManagerAware
    {
        private NewEmailViewModel newEmailViewModel;
        private NewEmailToolBarViewModel newEmailToolBarViewModel;
        private readonly IExchangeService exchangeService;
        

        public NewEmailUseCase(
            IViewToRegionBinder viewtoToRegionBinder,
            ObjectFactory<NewEmailViewModel> newEmailViewFactory,
            ObjectFactory< NewEmailToolBarViewModel> newEmailToolBarFactory,
            IExchangeService exchangeService, 
            IRegionManager regionManager) : base (viewtoToRegionBinder)
        {
            this.AddInitializationMethods(
                () => this.newEmailViewModel = newEmailViewFactory.CreateInstance()
                , () => this.newEmailToolBarViewModel = newEmailToolBarFactory.CreateInstance());
            
            this.exchangeService = exchangeService;
            this.RegionManager = regionManager;
        }

        public override string Name
        {
            get { return "New E-mail"; }
        }

        protected override void OnFirstActivation()
        {
            this.newEmailViewModel.Email.Value = this.Message;

            this.ViewToRegionBinder.Add("ToolbarRegion", newEmailToolBarViewModel);
            this.ViewToRegionBinder.Add("MainRegion", newEmailViewModel);
            
            this.newEmailToolBarViewModel.SendEmailCommand.Value = new DelegateCommand<EmailMessage>(SendEmail);
            
        }

        private void SendEmail(EmailMessage message)
        {
            exchangeService.SendEmail(message);
            this.IsActive = false;
        }

        public IRegionManager RegionManager
        {
            get; private set;
        }

        public EmailMessage Message { get; set; }
    }
}
