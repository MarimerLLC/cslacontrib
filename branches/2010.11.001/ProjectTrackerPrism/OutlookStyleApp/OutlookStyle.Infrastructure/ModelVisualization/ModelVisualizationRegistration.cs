using System;


namespace OutlookStyle.Infrastructure.ModelVisualization
{
    /// <summary>
    /// Class that specifies a visualization for a model. 
    /// </summary>
    public class ModelVisualizationRegistration
    {
        /// <summary>
        /// The type of the visualization
        /// </summary>
        public Type ViewType { get; set; }
        
        /// <summary>
        /// The type of the model that will be visualized. 
        /// </summary>
        public Type ModelType { get; set; }
        
    }
}