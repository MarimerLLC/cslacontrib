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
using PTWpf.Modules.Statusbar;

namespace PTWpf.Modules.Statubar
{
    public class StatusbarModule : IModule
    {
        private readonly IApplicationModel _applicationModel;
        private readonly StatusbarUseCase _statusbarUseCaseUseCase;
        private readonly IModelVisualizationRegistry _modelVisualizationRegistry;


        public StatusbarModule(
            IApplicationModel applicationModel
            , StatusbarUseCase loginUseCase
            , IModelVisualizationRegistry modelVisualizationRegistry)
        {
            this._applicationModel = applicationModel;
            this._statusbarUseCaseUseCase = loginUseCase;
            this._modelVisualizationRegistry = modelVisualizationRegistry;
        }

        public void Initialize()
        {
            // Register visualizations for these view models. This means: whenever a viewmodel is displayed, 
            // use this type of view to visualize it. 
            this._modelVisualizationRegistry.Register<StatusbarViewModel, StatubarView>();

            // Add the main email usecase to the application model. This will make sure it displays a button for the usecase in the 
            // buttonbar. Clicking the button will activate the use case. 
            this._applicationModel.AddMainUseCase(this._statusbarUseCaseUseCase);
        }
    }
}