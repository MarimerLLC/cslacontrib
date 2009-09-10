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

namespace PTWpf.Modules.Resource
{
    public class ResourceModule : IModule
    {
        private readonly ResourceUseCase _resourceUseCase;

        private readonly IModelVisualizationRegistry _modelVisualizationRegistry;
        private readonly IApplicationModel _applicationModel;


        public ResourceModule(
            IApplicationModel applicationModel
            , ResourceUseCase resourceUseCase
            , IModelVisualizationRegistry modelVisualizationRegistry)
        {
            this._applicationModel = applicationModel;
            this._resourceUseCase = resourceUseCase;
            this._modelVisualizationRegistry = modelVisualizationRegistry;
        }

        public void Initialize()
        {
            // Add the main Login usecase to the application model. This will make sure it displays a button for the usecase in the 
            // buttonbar. Clicking the button will activate the use case. 
            this._applicationModel.AddMainUseCase(this._resourceUseCase);

        }
    }
}