using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Outlook.Modules.Email.NewEmail;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;

namespace Outlook.Modules.Email
{
    public class EmailModule : IModule
    {
        private readonly IApplicationModel applicationModel;
        private readonly IRegionManager regionManager;
        private readonly EmailMainUseCase mainEmailUseCase;
        private readonly IModelVisualizationRegistry modelVisualizationRegistry;

        
        public EmailModule(
            IApplicationModel applicationModel
            , IRegionManager regionManager
            , EmailMainUseCase mainEmailUseCase
            , IModelVisualizationRegistry modelVisualizationRegistry)
        {
            
            this.applicationModel = applicationModel;
            this.regionManager = regionManager;
            this.mainEmailUseCase = mainEmailUseCase;
            this.modelVisualizationRegistry = modelVisualizationRegistry;
        }

        public void Initialize()
        {
            // Register visualizations for these view models. This means: whenever a viewmodel is displayed, 
            // use this type of view to visualize it. 
            modelVisualizationRegistry.Register<EmailToolBarViewModel, EmailToolBar>();
            modelVisualizationRegistry.Register<EmailMainViewModel, EmailMainView>();
            modelVisualizationRegistry.Register<EmailDetailViewModel, EmailDetailView>();
            modelVisualizationRegistry.Register<NewEmailToolBarViewModel, NewEmailToolBar>();
            modelVisualizationRegistry.Register<NewEmailViewModel, NewEmailView>();

            // Add the main email usecase to the application model. This will make sure it displays a button for the usecase in the 
            // buttonbar. Clicking the button will activate the use case. 
            applicationModel.AddMainUseCase(mainEmailUseCase);

            // Make sure the EmailDetails are always displayed when the EmailDetailRegion is displayed (View Discovery technique)
            regionManager.RegisterViewWithRegion("EmailDetailRegion", typeof (EmailDetailViewModel));
            
        }

    }
}