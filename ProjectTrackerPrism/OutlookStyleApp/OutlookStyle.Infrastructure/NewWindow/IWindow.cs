using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookStyle.Infrastructure.NewWindow
{
    /// <summary>
    /// Interface for opening up new windows. 
    /// </summary>
    public interface IWindow
    {
        void Show();
        void Close();
    }
}
