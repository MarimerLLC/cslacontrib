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

namespace PTWpf.Modules.Roles
{
    public class RolesModule : IModule
    {
        private readonly RolesListUseCase _rolesListUseCase;

        private readonly IModelVisualizationRegistry _modelVisualizationRegistry;
        private readonly IApplicationModel _applicationModel;


        public RolesModule(
            IApplicationModel applicationModel
            , RolesListUseCase rolesListUseCase
            , IModelVisualizationRegistry modelVisualizationRegistry)
        {
            this._applicationModel = applicationModel;
            this._rolesListUseCase = rolesListUseCase;
            this._modelVisualizationRegistry = modelVisualizationRegistry;
        }

        public void Initialize()
        {
            // Register visualizations for these view models. This means: whenever a viewmodel is displayed, 
            // use this type of view to visualize it. 
            this._modelVisualizationRegistry.Register<RolesListViewModel, RolesListView>();
            this._modelVisualizationRegistry.Register<RolesToolbarViewModel, RolesToolbarView>();

            // Add the main RolesEdit usecase to the application model. This will make sure it displays a button for the usecase in the 
            // buttonbar. Clicking the button will activate the use case. 
            this._applicationModel.AddMainUseCase(this._rolesListUseCase);
        }
    }
}