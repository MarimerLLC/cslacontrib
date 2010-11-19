using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Csla;
using Csla.Reflection;
using Csla.Data;
using Csla.Windows;

namespace MyCsla.Windows
{
    /// <summary>
    /// As an alternative to ComboBox - use this form when there are more then 100 items in the list. 
    /// </summary>
    public partial class ListSelectForm : Form
    {
        #region Nested type: Item

        internal class Item
        {
            public object Key { get; set; }
            public string Value { get; set; }
        }

        #endregion

        #region Nested type: MyNameValueList

        internal class MyNameValueList : List<Item>
        {
            public static MyNameValueList GetNameValueList(IEnumerable<object> source, string ValueMember,
                                                           string DisplayMember)
            {
                var list = new MyNameValueList();

                foreach (var o in source)
                {
                    var key = MethodCaller.CallPropertyGetter(o, ValueMember);
                    var value = (string) MethodCaller.CallPropertyGetter(o, DisplayMember);

                    list.Add(new Item {Key = key, Value = value});
                }
                return list;
            }
        }

        #endregion

        #region PUBLIC METHODS

        private IList<Item> myList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSelectForm"/> class.
        /// </summary>
        public ListSelectForm()
        {
            InitializeComponent();
            Closing += ListFormClosing;

            ValueMember = "Key";
            DisplayMember = "Value";
        }

        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        public object SelectedValue { get; private set; }

        /// <summary>
        /// Gets or sets the selected key.
        /// </summary>
        /// <value>The selected key.</value>
        public object SelectedKey { get; private set; }

        /// <summary>
        /// Gets or sets the key field.
        /// </summary>
        /// <value>The key field.</value>
        public string DisplayMember { get; set; }

        /// <summary>
        /// Gets or sets the value field.
        /// </summary>
        /// <value>The value field.</value>
        public string ValueMember { get; set; }

        /// <summary>
        /// Sets the initial search string.
        /// </summary>
        /// <value>The initial search string.</value>
        public string InitialSearchString
        {
            set { sokTextBox.Text = value; }
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            set { Text = value; }
        }

        /// <summary>
        /// Sets the list label.
        /// </summary>
        /// <value>The list label.</value>
        public string ListLabel
        {
            set { listLabel.Text = "&" + value + ":"; }
        }

        /// <summary>
        /// Sets the source list.
        /// </summary>
        /// <value>The source list.</value>
        public IEnumerable<object> SourceList
        {
            set
            {
                var list =
                    new FilteredBindingList<Item>(
                        new SortedBindingList<Item>(MyNameValueList.GetNameValueList(value, ValueMember, DisplayMember)));
                list.FilterProvider += MyFilterProvider;
                myList = list;
                BindUI();
            }
        }

        #endregion

        #region PRIVATE METHODS/EVENTS 

        private void ListFormClosing(object sender, CancelEventArgs e)
        {
            BindingHelper.UnbindBindingSource(listSource, true, false);
        }

        private static bool MyFilterProvider(object item, object filter)
        {
            return (((string) item).ToLower().Contains(((string) filter).ToLower()));
        }

        private void BindUI()
        {
            BindingHelper.RebindBindingSource(listSource, myList);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (listSource.Current == null) return;

            var item = listSource.Current as Item;

            if (!(item == null))
            {
                SelectedKey = item.Key;
                SelectedValue = item.Value;
            }
            DialogResult = DialogResult.OK;
        }

        private void FilterList(string filter)
        {
            var list = myList as FilteredBindingList<Item>;
            if (list == null) return;

            list.ApplyFilter("Value", filter);

            acceptButton.Enabled = list.Count > 0;
        }

        private void sokTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterList(sokTextBox.Text);
        }

        #endregion
    }
}