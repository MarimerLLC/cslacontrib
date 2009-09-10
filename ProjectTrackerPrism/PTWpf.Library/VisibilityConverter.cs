using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTWpf.Library
{
  public class VisibilityConverter : System.Windows.Data.IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, 
      object parameter, System.Globalization.CultureInfo culture)
    {
        bool visible = true;
        if (value is bool)
        {
            visible = (bool) value;
        }
        else if (value is int)
        {
            if (((int)value) == 0)
                visible = false;
        }

        if(visible)
        return System.Windows.Visibility.Visible;
        else
        return System.Windows.Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, 
      object parameter, System.Globalization.CultureInfo culture)
    {
      return false;
    }

    #endregion
  }
}
