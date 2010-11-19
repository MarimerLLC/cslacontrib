using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Regions;
using OutlookStyle.Infrastructure.ModelVisualization;

namespace OutlookStyle.Infrastructure.NewWindow
{
    public class NewWindowRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "NewWindowRegionBehavior";

        protected override void OnAttach()
        {
            this.Region.ActiveViews.RegisterAddAndRemoveDelegates<object>(ItemAdded, ItemRemoved);
        }

        private void ItemAdded(object item)
        {
            IWindow window = GetWindow(item);

            if (window != null)
            {
                window.Show();
            }
        }

        private void ItemRemoved(object item)
        {
            IWindow window = GetWindow(item);
            if (window != null)
            {
                window.Close();
                this.Region.Remove(item);
            }
        }

        /// <summary>
        /// If the item in the region is a window (possibly wrapped in a modelvisualizer), then get it 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>the item as a window</returns>
        private IWindow GetWindow(object item)
        {
            IWindow window = item as IWindow;
            if (window == null)
            {
                IModelVisualizer modelVisualizer = item as IModelVisualizer;
                window = modelVisualizer.View as IWindow;
            }
            return window;
        }

    }
}
