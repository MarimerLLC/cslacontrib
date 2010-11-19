using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Presentation.Regions.Behaviors;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ModelVisualization;
using PTWpf.Modules.ModuleEvents;
using PTWpf.Shell;

namespace OutlookStyleApp
{
    /// <summary>
    /// Implementation of the ApplicationModel that is specific to the OutlookStyleApp. 
    /// </summary>
    public class ApplicationModel : IApplicationModel
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private IRegion mainUseCases;
        private IEventAggregator eventAggregtor;
        private readonly DelegateCommand<IActiveAwareUseCaseController> activateUseCaseCommand;
        private readonly DelegateCommand<IActiveAwareUseCaseController> deactivateUseCaseCommand;

        public ApplicationModel(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator, IModelVisualizationRegistry visualizationRegistry)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregtor = eventAggregator;

            activateUseCaseCommand = new DelegateCommand<IActiveAwareUseCaseController>(ActivateUseCase);
            deactivateUseCaseCommand = new DelegateCommand<IActiveAwareUseCaseController>(DeactivateUseCase);
            
            CreateMainUseCasesRegion();

            visualizationRegistry.Register<StatusbarViewModel, StatusbarView>();

            regionManager.RegisterViewWithRegion("StatusbarRegion", typeof (StatusbarViewModel));

        }

        private void CreateMainUseCasesRegion()
        {
            // Use a SingleActiveRegion to store the usecases in. Note, none of the default behavors are added if I create the region this way
            mainUseCases = new SingleActiveRegion();

            // Add a behavior that will propagate any ActiveAware changes in the UseCases to this. So an usecase can set itself to active and 
            // thus show it. Normally, the ActiveAware only flows down, so I needed to create a new behavior for this. 
            mainUseCases.Behaviors.Add(TwoWayActiveAwareBehavior.BehaviorKey, new TwoWayActiveAwareBehavior());

            // We will still need the Prism RegionActiveAwareBehavior to sync the activeware to the views. (Since none of the default behavors are now added)
            mainUseCases.Behaviors.Add(RegionActiveAwareBehavior.BehaviorKey, new RegionActiveAwareBehavior());
        }


        /// <summary>
        /// Activates the use case.
        /// </summary>
        /// <param name="activeAwareUseCaseController">The active aware use case controller.</param>
        public void ActivateUseCase(IActiveAwareUseCaseController activeAwareUseCaseController)
        {
            this.mainUseCases.Activate(activeAwareUseCaseController);
        }

        public void ActivateUseCaseByName(string activeAwareModuleApplicationControllerName)
        {
            if (activeAwareModuleApplicationControllerName != null)
            {
                foreach (IActiveAwareUseCaseController useCaseController in this.MainUseCases)
                {
                    if (useCaseController.Name == activeAwareModuleApplicationControllerName)
                    {
                        this.ActivateUseCase(useCaseController);
                        break;
                    }
                }
            }
        }
        public void ShowUseCase(IActiveAwareUseCaseController useCase)
        {
            var region = regionManager.Regions["NewWindowRegion"];
            region.Add(useCase);
            useCase.IsActive = true;
        }

        public void DeactivateUseCase(IActiveAwareUseCaseController activeAwareModuleApplicationController)
        {
            this.mainUseCases.Deactivate(activeAwareModuleApplicationController);
        }

        public void DeactivateUseCaseByName(string activeAwareModuleApplicationControllerName)
        {
            if (activeAwareModuleApplicationControllerName != null)
            {
                foreach (IActiveAwareUseCaseController useCaseController in this.MainUseCases)
                {
                    if (useCaseController.Name == activeAwareModuleApplicationControllerName)
                    {
                        this.DeactivateUseCase(useCaseController);
                        break;
                    }
                }
            }
        }
        
        public T CreateObjectInScopedRegionManager<T>()
            where T : IRegionManagerAware
        {
            var childContainer = container.CreateChildContainer();
            var regionManager = container.Resolve<IRegionManager>();

            var scopedRegionManager = regionManager.CreateRegionManager();
            childContainer.RegisterInstance<IRegionManager>(scopedRegionManager);

            return childContainer.Resolve<T>();
        }

        /// <summary>
        /// Add a main usecase to the list of main usecases
        /// </summary>
        /// <param name="activeAwareUseCaseController"></param>
        public void AddMainUseCase(IActiveAwareUseCaseController activeAwareUseCaseController)
        {
            this.mainUseCases.Add(activeAwareUseCaseController);
        }

        /// <summary>
        /// The list of main use cases
        /// </summary>
        /// <value></value>
        public IViewsCollection MainUseCases
        {
            get { return mainUseCases.Views; }
        }

        /// <summary>
        /// Activate a usecase. The usecase to activate should be in the command parameter
        /// </summary>
        /// <value></value>
        public ICommand ActivateUseCaseCommand
        {
            get { return activateUseCaseCommand; }
        }

        /// <summary>
        /// Gets the active use cases.
        /// </summary>
        /// <value>The active use cases.</value>
        public IViewsCollection ActiveUseCases
        {
            get
            {
                return this.mainUseCases.ActiveViews;
            }
        }

    }
}
