using System;
using Microsoft.Practices.Composite;

namespace OutlookStyleApp.Tests
{
    internal class MockActiveAwareViewModel : IActiveAware
    {
        public event EventHandler IsActiveChanged;

        private void InvokeIsActiveChanged(EventArgs e)
        {
            EventHandler isActiveChangedHandler = IsActiveChanged;
            if (isActiveChangedHandler != null) isActiveChangedHandler(this, e);
        }

        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                InvokeIsActiveChanged(EventArgs.Empty);
            }
        }
    }
}