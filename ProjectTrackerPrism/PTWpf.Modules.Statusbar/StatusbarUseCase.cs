using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;
using Microsoft.Practices.Composite.Regions;
using PTWpf.Modules.Statusbar;

namespace PTWpf.Modules.Security
{
    /// <summary>
    /// Use case for all E-mail functionality in the main window like showing the inbox, reading emails in the preview window, etc..
    /// </summary>
    public class StatusbarUseCase : ActiveAwareUseCaseController
    {
        private readonly IApplicationModel _applicationModel;

        // The references to these viewmodels will be filled when the app is initialized for the first time. 
        private StatusbarViewModel _loginViewModel;

        public LoginUseCase(
            // Get the ViewToRegionBinder that the baseclass needs
            IViewToRegionBinder viewtoToRegionBinder
            , IRegionManager regionManager
            // Get the factories that can create the viewmodels
            , ObjectFactory<StatusbarViewModel> loginViewModel
            , IApplicationModel applicationModel)
            : base(viewtoToRegionBinder)
        {
            this._applicationModel = applicationModel;

            // Just before the view is initialized for the first time
            this.AddInitializationMethods(
                // Create the emailViewModel and assign it to this variable
                () => this._loginViewModel = loginViewModel.CreateInstance());
        }

        public override string Name
        {
            get { return "Login"; }
        }

        protected override void OnFirstActivation()
        {
            // Make sure the Toolbar and the mainregion get displayed when this use case is activated
            // and removed again when it becomes inactive. 
            this.ViewToRegionBinder.Add("MainRegion", this._loginViewModel);
            this.ViewToRegionBinder.Add("ToolbarRegion", this._loginToolbarViewModel);
            // Hook up the events from the emailToolbar command.
            //this.emailToolBarViewModel.NewEmailCommand.Value = new DelegateCommand<object>(NewEmail);
            //this.emailToolBarViewModel.ReplyCommand.Value = new DelegateCommand<object>(ReplyToEmail);
        }
    }
}
