using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite;
using OutlookStyle.Infrastructure.NewWindow;

namespace OutlookStyleApp
{
    /// <summary>
    /// This is a 
    /// </summary>
    public partial class Popup : Window, IWindow, IActiveAware
    {
        public Popup()
        {
            InitializeComponent();
        }

        private bool isActive;

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                if (value != isActive)
                {
                    isActive = value;
                    InvokeIsActiveChanged(EventArgs.Empty);
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.IsActive = false;
        }

        public event EventHandler IsActiveChanged;

        private void InvokeIsActiveChanged(EventArgs e)
        {
            EventHandler changed = IsActiveChanged;
            if (changed != null) changed(this, e);
        }
    }
}
