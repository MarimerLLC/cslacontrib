using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Regions;
using OutlookStyle.Infrastructure.ModelVisualization;

namespace OutlookStyle.Infrastructure.NewWindow
{
    public class ScopedRegionManagerAwareRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "ScopedRegionManagerAwareRegionBehavior";

        protected override void OnAttach()
        {
            this.Region.Views.RegisterAddAndRemoveDelegates<IModelVisualizer>(RegisterScopedRegionManagerToView, RemoveScopedRegionManagerFromView);
        }

        private void RegisterScopedRegionManagerToView(IModelVisualizer modelVisualizer)
        {
            IRegionManagerAware regionManagerAware = modelVisualizer.ViewModel as IRegionManagerAware;

            if (regionManagerAware != null)
            {
                RegionManager.SetRegionManager(modelVisualizer.View, regionManagerAware.RegionManager);
            }
        }

        private void RemoveScopedRegionManagerFromView(IModelVisualizer modelVisualizer)
        {
            IRegionManagerAware regionManagerAware = modelVisualizer.ViewModel as IRegionManagerAware;

            if (regionManagerAware != null)
            {
                RegionManager.SetRegionManager(modelVisualizer.View, null);
            }
        }

    }
}
