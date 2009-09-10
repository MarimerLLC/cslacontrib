using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Regions;
using OutlookStyle.Infrastructure.UseCase;

namespace OutlookStyle.Infrastructure
{
    /// <summary>
    /// Behavior that will monitor any IActiveAware views in a region. If one of them becomes active, it will also set it to
    /// active in the region. By design, the regions normally only allow activation through the region, but i thought 
    /// it would be nice to show how to do this two way. 
    /// </summary>
    public class TwoWayActiveAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = "TwoWayActiveAwareBehavior";
        protected override void OnAttach()
        {
            this.Region.Views.RegisterAddAndRemoveDelegates<IActiveAware>(
                OnAddRegisterForViewIsActiveChangedEvent
                , OnRemoveUnRegisterForViewIsActiveChangedEvent);
        }

        private void OnRemoveUnRegisterForViewIsActiveChangedEvent(IActiveAware activeAware)
        {
            activeAware.IsActiveChanged -= SetActivationInRegion;
        }

        private void OnAddRegisterForViewIsActiveChangedEvent(IActiveAware activeAware)
        {
            activeAware.IsActiveChanged += SetActivationInRegion;
        }

        void SetActivationInRegion(object sender, EventArgs e)
        {
            IActiveAware activeAware = sender as IActiveAware;
            if (activeAware == null)
                return;

            if (activeAware.IsActive &&
                !this.Region.ActiveViews.Contains(activeAware))
            {
                this.Region.Activate(activeAware);
            }
            else if (!activeAware.IsActive && this.Region.ActiveViews.Contains(activeAware))
            {
                this.Region.Deactivate(activeAware);
            }
        }
    }
}
