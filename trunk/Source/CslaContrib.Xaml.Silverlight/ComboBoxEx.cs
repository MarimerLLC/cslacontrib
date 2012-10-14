using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CslaContrib.Xaml.Silverlight
{
  public class ComboBoxEx : ComboBox
  {
    #region Fields

    private bool _suppressSelectionChangedUpdatesRebind = false;

    #endregion

    #region Properties

    public static readonly DependencyProperty SelectedValueProperProperty =
        DependencyProperty.Register(
            "SelectedValueProper",
            typeof(object),
            typeof(ComboBoxEx),
            new PropertyMetadata((o, dp) =>
            {
              var comboBoxEx = o as ComboBoxEx;
              if (comboBoxEx == null)
                return;

              comboBoxEx.SetSelectedValueSuppressingChangeEventProcessing(dp.NewValue);
            }));

    [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
    public object SelectedValueProper
    {
      get { return (object)GetValue(SelectedValueProperProperty); }
      set { SetValue(SelectedValueProperProperty, value); }
    }

    #endregion

    #region Constructor and Overrides

    public ComboBoxEx()
    {
      SelectionChanged += ComboBoxEx_SelectionChanged;
    }

    /// <summary>
    /// Updates the current selected item when the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection has changed.
    /// </summary>
    /// <param name="e">Contains data about changes in the items collection.</param>
    protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      // Must re-apply value here because the combobox has a bug that 
      // despite the fact that the binding still exists, it doesn't 
      // re-evaluate and subsequently drops the binding on the change event
      SetSelectedValueSuppressingChangeEventProcessing(SelectedValueProper);
    }

    #endregion

    #region Events

    private void ComboBoxEx_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // Avoid recursive stack overflow
      if (_suppressSelectionChangedUpdatesRebind)
        return;

      if (e.AddedItems != null && e.AddedItems.Count > 0)
      {
        //SelectedValueProper = GetMemberValue( e.AddedItems[0] );
        SelectedValueProper = SelectedValue; // This is faster than GetMemberValue
      }
      // Do not apply the value if no items are selected (ie. the else)
      // because that just passes on the null-value bug from the combobox
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Gets the member value based on the Selected Value Path
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    private object GetMemberValue(object item)
    {
      return item.GetType().GetProperty(SelectedValuePath).GetValue(item, null);
    }

    /// <summary>
    /// Sets the selected value suppressing change event processing.
    /// </summary>
    /// <param name="newSelectedValue">The new selected value.</param>
    private void SetSelectedValueSuppressingChangeEventProcessing(object newSelectedValue)
    {
      try
      {
        _suppressSelectionChangedUpdatesRebind = true;
        SelectedValue = newSelectedValue;
      }
      finally
      {
        _suppressSelectionChangedUpdatesRebind = false;
      }
    }

    #endregion
  }
}
