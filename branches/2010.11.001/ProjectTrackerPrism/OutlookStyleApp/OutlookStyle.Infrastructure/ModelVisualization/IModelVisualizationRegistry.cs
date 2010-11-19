using System.Collections.Generic;
using System.Windows;

namespace OutlookStyle.Infrastructure.ModelVisualization
{
    /// <summary>
    /// Interface for the ModelVisualizationRegistry. Allows you to register mappigns between a model and a view that can 
    /// visualize that model. 
    /// </summary>
    public interface IModelVisualizationRegistry
    {
        /// <summary>
        /// Register a visualization
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TView">The type of the view</typeparam>
        void Register<TModel, TView>();

        /// <summary>
        /// Get the list of registered visualizations
        /// </summary>
        IEnumerable<ModelVisualizationRegistration> ModelVisualizations { get; }

        /// <summary>
        /// Create a visualization for the specified object. 
        /// </summary>
        /// <param name="objectToVisualize"></param>
        /// <returns></returns>
        FrameworkElement CreateVisualization(object objectToVisualize);
    }
}