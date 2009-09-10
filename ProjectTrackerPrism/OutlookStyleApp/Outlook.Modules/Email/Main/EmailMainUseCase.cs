using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;

namespace Outlook.Modules.Email
{
    /// <summary>
    /// Use case for all E-mail functionality in the main window like showing the inbox, reading emails in the preview window, etc..
    /// </summary>
    public class EmailMainUseCase : ActiveAwareUseCaseController
    {
        private readonly IApplicationModel ApplicationModel;

        // The references to these viewmodels will be filled when the app is initialized for the first time. 
        private EmailMainViewModel emailViewModel;
        private EmailToolBarViewModel emailToolBarViewModel;

        public EmailMainUseCase(
            // Get the ViewToRegionBinder that the baseclass needs
            IViewToRegionBinder viewtoToRegionBinder
            // Get the factories that can create the viewmodels
            , ObjectFactory<EmailMainViewModel> emailViewFactory
            , ObjectFactory<EmailToolBarViewModel> emailToolBarFactory
            , IApplicationModel applicationModel) : base(viewtoToRegionBinder)
        {
            ApplicationModel = applicationModel;

            // Just before the view is initialized for the first time
            this.AddInitializationMethods(
                    // Create the emailViewModel and assign it to this variable
                () => this.emailViewModel = emailViewFactory.CreateInstance()
                    // Create the toolbarViewModel and assign it to this variable
                , () => this.emailToolBarViewModel = emailToolBarFactory.CreateInstance());
        }

        public override string Name
        {
            get { return "E-Mail"; }
        }

        protected override void OnFirstActivation()
        {
            // Make sure the Toolbar and the mainregion get displayed when this use case is activated
            // and removed again when it becomes inactive. 
            this.ViewToRegionBinder.Add("ToolbarRegion", this.emailToolBarViewModel);
            this.ViewToRegionBinder.Add("MainRegion", this.emailViewModel);

            // Hook up the events from the emailToolbar command.
            this.emailToolBarViewModel.NewEmailCommand.Value = new DelegateCommand<object>(NewEmail);
            this.emailToolBarViewModel.ReplyCommand.Value = new DelegateCommand<object>(ReplyToEmail);
        }

        private void NewEmail(object notUsed)
        {
            NewEmailUseCase newEmailUseCase = ApplicationModel.CreateObjectInScopedRegionManager<NewEmailUseCase>();
            newEmailUseCase.Message = new EmailMessage();
            ApplicationModel.ShowUseCase(newEmailUseCase);

        }

        private void ReplyToEmail(object notUsed)
        {
            EmailMessage currentEmail = this.emailViewModel.SelectedEmail.Value;

            if (currentEmail == null)
                return;
            NewEmailUseCase newEmailUseCase = ApplicationModel.CreateObjectInScopedRegionManager<NewEmailUseCase>();
            newEmailUseCase.Message = new EmailMessage()
                                          {
                                              Body = "------------------------------------------------\r\n" + currentEmail.Body,
                                              From = currentEmail.To,
                                              To = currentEmail.From, 
                                              Subject = "Re: " + currentEmail.Subject
                                          };
            ApplicationModel.ShowUseCase(newEmailUseCase);

        }
    }
}
