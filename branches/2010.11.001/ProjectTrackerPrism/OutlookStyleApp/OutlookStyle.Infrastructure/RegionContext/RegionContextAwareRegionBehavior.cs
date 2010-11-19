using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.Composite.Presentation.Regions;
using OutlookStyle.Infrastructure.RegionContext;

namespace OutlookStyle.Infrastructure.RegionContext
{
    /// <summary>
    /// Behavior that forwards the RegionContext to any class that implements the IRegionContextAwareBehavior. 
    /// This is an alternative to the <see cref="Microsoft.Practices.Composite.Presentation.Regions.RegionContext.GetObservableContext"/> method in Prism.
    /// </summary>
    public class RegionContextAwareRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "RegionContextAwareRegionBehavior";

        protected override void OnAttach()
        {
            // When the region context changes, Update the views with the new regioncontext
            this.Region.PropertyChanged += OnRegionContextChanged_SetRegionContextToViews;

            // When a view is added or removed, hook (or unhook) the events to update the regioncontext
            this.Region.Views.RegisterAddAndRemoveDelegates<IRegionContextAware>(
                OnViewAddedStartMonitoringViewForRegionContextChanges
                , OnViewRemovedStopMonitoringViewForContextChanges);
        }

        private void OnViewRemovedStopMonitoringViewForContextChanges(IRegionContextAware regionContextAware)
        {
            regionContextAware.RegionContext.PropertyChanged -= View_RegionContextChanged;
        }

        private void OnViewAddedStartMonitoringViewForRegionContextChanges(IRegionContextAware regionContextAware)
        {
            regionContextAware.RegionContext.PropertyChanged += View_RegionContextChanged;
        }


        void View_RegionContextChanged(object sender, EventArgs e)
        {
            ObservableObject<object> regionContext = sender as ObservableObject<object>;
            if (regionContext == null)
                return;

            if (this.Region.Context != regionContext.Value)
            {
                this.Region.Context = regionContext.Value;
            }
        }

        void OnRegionContextChanged_SetRegionContextToViews(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Context")
            {
                foreach (var view in this.Region.Views)
                {
                    IRegionContextAware regionContextAware = view as IRegionContextAware;
                    if (regionContextAware == null)
                        break;

                    if (regionContextAware.RegionContext.Value != this.Region.Context)
                    {
                        regionContextAware.RegionContext.Value = this.Region.Context;
                    }
                }
            }
        }
    }
}