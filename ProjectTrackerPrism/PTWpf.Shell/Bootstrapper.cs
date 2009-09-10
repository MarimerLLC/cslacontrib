using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Presentation.Regions.Behaviors;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.RegionContext;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;
using OutlookStyleApp;
using PTWpf.Modules.Admin;
using PTWpf.Modules.Login;
using PTWpf.Modules.Project;
using PTWpf.Modules.Resource;
using PTWpf.Modules.Roles;

namespace PTWpf.Shell
{
    /// <summary>
    /// The bootstrapper is the glue between all the components and modules.  
    /// </summary>
    public class Bootstrapper : UnityBootstrapper
    {

        public Bootstrapper()
        {
        }

        protected override System.Windows.DependencyObject CreateShell()
        {
            var shell = Container.Resolve<Shell>();
            var model = Container.Resolve<IApplicationModel>();
            shell.DataContext = model;
            shell.Show();
            return shell;
        }

        protected override void ConfigureContainer()
        {
            // Register the implementations for the system interfaces

            // Register the application model as a singleton.
            this.Container.RegisterType<IApplicationModel, ApplicationModel>(new ContainerControlledLifetimeManager());

            // Register the ViewToRegionBinder
            this.Container.RegisterType<IViewToRegionBinder, ViewToRegionBinder>();

            // Register the visualizationregistry as a singleton. This has to be a singleton, so all registrations are added in this instance.
            this.Container.RegisterType<IModelVisualizationRegistry, ModelVisualizationRegistry>(new ContainerControlledLifetimeManager());

            base.ConfigureContainer();
        }

        /// <summary>
        /// Configures the <see cref="T:Microsoft.Practices.Composite.Presentation.Regions.IRegionBehaviorFactory"/>. This will be the list of default
        /// behaviors that will be added to a region.
        /// </summary>
        /// <returns></returns>
        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            // I have created a bunch of new regionbehaviors. Make sure they are added to ALL regions.

            var defaultRegionBehaviors = Container.TryResolve<IRegionBehaviorFactory>();

            // Replace the default RegionmanagerRegistrationBehavior with a behavior that will wrap the region
            // in a 'Visualizing' Region
            defaultRegionBehaviors.AddIfMissing(VisualizingRegionManagerRegistrationBehavior.BehaviorKey, typeof(VisualizingRegionManagerRegistrationBehavior));

            // Make sure the autopopulate regionbehavior doesn't get added to the regions. It will be added to the VisualizingRegions by the VisualizingRegionManagerRegistrationBehavior
            defaultRegionBehaviors.AddIfMissing(AutoPopulateRegionBehavior.BehaviorKey, typeof(NullRegionBehavior));

            // Also add a behavior that forwards the RegionContext to IRegionContextAware controls
            defaultRegionBehaviors.AddIfMissing(RegionContextAwareRegionBehavior.BehaviorKey, typeof(RegionContextAwareRegionBehavior));

            // Add a behavior that will assign the regionmanager of a viewmodel to the view that is visualizing it. 
            defaultRegionBehaviors.AddIfMissing(ScopedRegionManagerAwareRegionBehavior.BehaviorKey, typeof(ScopedRegionManagerAwareRegionBehavior));

            // Add a behavior that will synchronize the active aware from a view back into the region
            defaultRegionBehaviors.AddIfMissing(TwoWayActiveAwareBehavior.BehaviorKey, typeof(TwoWayActiveAwareBehavior));


            // Now add the default behaviors
            base.ConfigureDefaultRegionBehaviors();


            return defaultRegionBehaviors;
        }

        /// <summary>
        /// Configures the default region adapter mappings to use in the application, in order
        /// to adapt UI controls defined in XAML to use a region and register it automatically.
        /// May be overwritten in a derived class to add specific mappings required by the application.
        /// </summary>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.Composite.Presentation.Regions.RegionAdapterMappings"/> instance containing all the mappings.
        /// </returns>
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var regionAdapterMappings = base.ConfigureRegionAdapterMappings();

            // Register an adaptermapping for the NewWindowControl, so this can be used as a region. Note, this is still work in progress. 
            regionAdapterMappings.RegisterMapping(typeof(NewWindowControl), this.Container.Resolve<NewWindowRegionAdapter>());

            // Register a visualization for all use case controllers
            var registry = this.Container.Resolve<ModelVisualizationRegistry>();
            registry.Register<IActiveAwareUseCaseController, Popup>();

            return regionAdapterMappings;
        }


        /// <summary>
        /// Returns the module catalog that will be used to initialize the modules.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="T:Microsoft.Practices.Composite.Modularity.IModuleCatalog"/> that will be used to initialize the modules.
        /// </returns>
        /// <remarks>
        /// When using the default initialization behavior, this method must be overwritten by a derived class.
        /// </remarks>
        protected override IModuleCatalog GetModuleCatalog()
        {
            // Populate the modulecatalog. I have cheated a bit and placed several modules in a single assembly. Nothing prevents you from doing this
            // but it does kind of defeat the purpose of modularity.. Don't try this at home or at all! 
            var catalog = new ModuleCatalog();
            catalog.AddModule(typeof(LoginModule));
            catalog.AddModule(typeof(ResourceModule));
            catalog.AddModule(typeof(ProjectModule));
            catalog.AddModule(typeof(RolesModule));
            return catalog;
        }
    }
}
