using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation;
using OutlookStyle.Infrastructure.RegionContext;

namespace Outlook.Modules.Email
{
    public class EmailDetailViewModel : IRegionContextAware
    {
        public EmailDetailViewModel()
        {
            this.RegionContext = new ObservableObject<Object>();
            this.RegionContext.PropertyChanged += new PropertyChangedEventHandler(RegionContext_PropertyChanged);
        }

        void RegionContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
        public event EventHandler<EventArgs> RegionContextChanged;

        private void InvokeRegionContextChanged(EventArgs e)
        {
            EventHandler<EventArgs> regionContextChangedHandler = RegionContextChanged;
            if (regionContextChangedHandler != null) regionContextChangedHandler(this, e);
        }

        
        public ObservableObject<Object> RegionContext
        {
            get; private set;
        }
    }
}
