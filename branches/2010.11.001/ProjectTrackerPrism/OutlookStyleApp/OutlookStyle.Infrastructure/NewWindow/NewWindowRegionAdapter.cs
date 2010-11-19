using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;

namespace OutlookStyle.Infrastructure.NewWindow
{
    public class NewWindowRegionAdapter : RegionAdapterBase<NewWindowControl>
    {
        public NewWindowRegionAdapter(IRegionBehaviorFactory factory) : base(factory)
        {
            
        }
        protected override void Adapt(IRegion region, NewWindowControl regionTarget)
        {
            
        }

        protected override IRegion CreateRegion()
        {
            return new Region();
        }

        protected override void AttachBehaviors(IRegion region, NewWindowControl regionTarget)
        {
            region.Behaviors.Add(NewWindowRegionBehavior.BehaviorKey, new NewWindowRegionBehavior());
        }

    }
}