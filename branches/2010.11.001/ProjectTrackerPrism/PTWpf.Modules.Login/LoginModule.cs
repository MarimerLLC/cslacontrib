using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;

namespace PTWpf.Modules.Login
{
    public class LoginModule : IModule
    {
        private readonly LoginUseCase _loginUseCase;

        private readonly IModelVisualizationRegistry _modelVisualizationRegistry;
        private readonly IApplicationModel _applicationModel;


        public LoginModule(
            IApplicationModel applicationModel
            , LoginUseCase loginUseCase
            , IModelVisualizationRegistry modelVisualizationRegistry)
        {
            this._applicationModel = applicationModel;
            this._loginUseCase = loginUseCase;
            this._modelVisualizationRegistry = modelVisualizationRegistry;
        }

        public void Initialize()
        {
            // Register visualizations for these view models. This means: whenever a viewmodel is displayed, 
            // use this type of view to visualize it. 
            this._modelVisualizationRegistry.Register<LoginViewModel, LoginView>();
            this._modelVisualizationRegistry.Register<LoginToolbarViewModel, LoginToolbarView>();

            // Add the main Login use case to the application model. This will make sure it displays a button for the usecase in the 
            // buttonbar. Clicking the button will activate the use case. 
            this._applicationModel.AddMainUseCase(this._loginUseCase);
        }
    }
}