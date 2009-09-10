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

namespace PTWpf.Modules.Login
{
    /// <summary>
    /// Use case for all functionality in the main window ...
    /// </summary>
    public class LoginUseCase : ActiveAwareUseCaseController
    {
        private readonly IApplicationModel _applicationModel;

        // The references to these viewmodels will be filled when the app is initialized for the first time. 
        private LoginViewModel _loginViewModel;
        private LoginToolbarViewModel _loginToolbarViewModel;

        // Constructor will be called by Unity through constructor injection paramenters...
        public LoginUseCase(
            // Get the ViewToRegionBinder that the baseclass needs
            IViewToRegionBinder viewtoToRegionBinder
            , IRegionManager regionManager
            // Get the factories that can create the viewmodels
            , ObjectFactory<LoginViewModel> loginViewModel
            , ObjectFactory<LoginToolbarViewModel> loginToolbarViewModel
            , IApplicationModel applicationModel)
            : base(viewtoToRegionBinder)
        {
            this._applicationModel = applicationModel;

            // Just before the view is initialized for the first time
            this.AddInitializationMethods(
                // Create the emailViewModel and assign it to this variable
               () => this._loginViewModel = loginViewModel.CreateInstance()
               , () => this._loginToolbarViewModel = loginToolbarViewModel.CreateInstance());
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
        }
    }
}
