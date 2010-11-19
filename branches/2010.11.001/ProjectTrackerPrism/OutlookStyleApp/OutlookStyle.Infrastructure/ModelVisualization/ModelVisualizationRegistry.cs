using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Unity;

namespace OutlookStyle.Infrastructure.ModelVisualization
{

    /// <summary>
    /// Allows you to register mappigns between a model and a view that can 
    /// visualize that model. 
    /// </summary>
    public class ModelVisualizationRegistry : IModelVisualizationRegistry
    {
        private readonly IUnityContainer container;
        private List<ModelVisualizationRegistration> modelVisualizations;


        /// <summary>
        /// Initializes a new instance of the <see cref="ModelVisualizationRegistry"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ModelVisualizationRegistry(IUnityContainer container)
        {
            this.container = container;
            this.modelVisualizations = new List<ModelVisualizationRegistration>();
        }

        /// <summary>
        /// Register a visualization
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TView">The type of the view</typeparam>
        public void Register<TModel, TView>()
        {
            this.modelVisualizations.Add(new ModelVisualizationRegistration()
                {ModelType = typeof(TModel), ViewType = typeof(TView)});
        }

        /// <summary>
        /// Get the list of registered visualizations
        /// </summary>
        /// <value></value>
        public IEnumerable<ModelVisualizationRegistration> ModelVisualizations
        {
            get
            {
                return this.modelVisualizations;
            }
        }

        /// <summary>
        /// Create a visualization for the specified object.
        /// </summary>
        /// <param name="objectToVisualize"></param>
        /// <returns></returns>
        public FrameworkElement CreateVisualization(object objectToVisualize)
        {
            if (objectToVisualize == null) throw new ArgumentNullException("objectToVisualize");

            ModelVisualizationRegistration registration = GetRegistrationForExactType(objectToVisualize);

            if (registration == null)
            {
                registration = GetRegistrationForInterface(objectToVisualize);
            }

            if (registration == null)
            {
                if (!MustVisualize(objectToVisualize))
                    return objectToVisualize as FrameworkElement;

                throw new InvalidOperationException(string.Format("No Visualization registered for {0}", objectToVisualize.GetType()));
            }
            return CreateVisualization(objectToVisualize, registration);

        }

        private ModelVisualizationRegistration GetRegistrationForInterface(object objectToVisualize)
        {
            return this.ModelVisualizations.FirstOrDefault((reg) => objectToVisualize.GetType().GetInterfaces().Contains(reg.ModelType));
        }

        private ModelVisualizationRegistration GetRegistrationForExactType(object objectToVisualize)
        {
            return this.ModelVisualizations.FirstOrDefault((reg) => reg.ModelType == objectToVisualize.GetType());
        }

        /// <summary>
        /// Should the specified object be visualized? This implementation only requires visualization for NON framework elements
        /// </summary>
        /// <param name="objectToVisualize">The object to visualize.</param>
        /// <returns></returns>
        protected virtual bool MustVisualize(object objectToVisualize)
        {
            return !(objectToVisualize is FrameworkElement);
        }

        /// <summary>
        /// Creates the visualization for the specified object.
        /// </summary>
        /// <param name="objectToVisualize">The object to visualize.</param>
        /// <param name="registration">The registration.</param>
        /// <returns></returns>
        protected virtual FrameworkElement CreateVisualization(object objectToVisualize, ModelVisualizationRegistration registration)
        {
            var view = this.container.Resolve(registration.ViewType) as FrameworkElement;

            return new ModelVisualizer(objectToVisualize, view);
        }
    }
}