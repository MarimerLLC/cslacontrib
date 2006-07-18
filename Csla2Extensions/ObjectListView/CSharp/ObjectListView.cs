using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace Csla
{
	/// <summary>
	/// Represents a databindable, customized, strongly-typed view of a System.Collections.Generic.IList<T> for sorting, filtering, searching, editing, and navigation.
	/// </summary>
	/// <author>Brian Criswell</author>
	/// <license>CREATIVE COMMONS - Attribution 2.5 License http://creativecommons.org/ </license>
	public class ObjectListView<T> : IBindingListView, IList<T>
	{
		#region Fields

		private IList<T> _list;

		// Sorting fields
		private ListSortDescriptionCollection _sorts;
		private List<ListItem> _sortIndex;

		// Filtering fields
		private PropertyDescriptorCollection _columns = TypeDescriptor.GetProperties(typeof(T));
		private DataTable _filteredTable = new DataTable();
		private DataRow _filteredRow;
		private DataView _filteredView;

		//IBindingList fields
		private bool _supportsBinding = false;
		private IBindingList _iBindingList = null;
		
		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		public ObjectListView(IList<T> list)
			: this(list, null, string.Empty)
		{
		}

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		/// <param name="sorts">The sorts to apply.</param>
		public ObjectListView(IList<T> list, ListSortDescriptionCollection sorts)
			: this(list, sorts, string.Empty)
		{
		}

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		/// <param name="filter">The filter to apply.</param>
		public ObjectListView(IList<T> list, string filter)
			: this(list, null, filter)
		{
		}

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		/// <param name="propertyName">The name of the property by which to sort.</param>
		/// <param name="direction">The direction by which to sort.</param>
		/// <param name="filter">The filter to apply.</param>
		public ObjectListView(IList<T> list, string propertyName, ListSortDirection direction, string filter)
			: this(list, TypeDescriptor.GetProperties(typeof(T))[propertyName], direction, filter)
		{
		}

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		/// <param name="property">The property by which to sort.</param>
		/// <param name="direction">The direction by which to sort.</param>
		/// <param name="filter">The filter to apply.</param>
		public ObjectListView(IList<T> list, PropertyDescriptor property, ListSortDirection direction, string filter)
			: this(list, new ListSortDescriptionCollection(new ListSortDescription[] { new ListSortDescription(property, direction) }), filter)
		{
		}

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		/// <param name="sorts">The sorts to apply.</param>
		/// <param name="filter">The filter to apply.</param>
		public ObjectListView(IList<T> list, ListSortDescriptionCollection sorts, string filter)
		{
			if (filter == null) filter = string.Empty;

			_list = list;

			foreach (PropertyDescriptor prop in _columns)
			{
				_filteredTable.Columns.Add(prop.Name, prop.PropertyType);
			}

			_filteredRow = _filteredTable.Rows.Add();

			_filteredView = new DataView(_filteredTable);
			_filteredView.RowFilter = filter;

			if (list is IBindingList)
			{
				_supportsBinding = true;
				_iBindingList = (IBindingList)list;
				_iBindingList.ListChanged += new ListChangedEventHandler(_iBindingList_ListChanged);
			}

			this.ApplySort(sorts);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reacts to changes in the source list.
		/// </summary>
		/// <param name="sender">The sender of the change.</param>
		/// <param name="e">The change information.</param>
		void _iBindingList_ListChanged(object sender, ListChangedEventArgs e)
		{
			switch (e.ListChangedType)
			{
				case ListChangedType.ItemAdded:
					ListItem addedItem = new ListItem(e.NewIndex);
					this.ApplyFilter(addedItem);

					if (addedItem.Visible)
					{
						this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, this.InsertInOrder(addedItem)));
					}
					else
					{
						this.InsertInOrder(addedItem);
					}

					break;
				case ListChangedType.ItemChanged:
					for (int i = 0; i < _sortIndex.Count; i++)
					{
						if (_sortIndex[i].BaseIndex == e.NewIndex)
						{
							int oldIndex = this.SortedIndex(_sortIndex[i].BaseIndex);
							ListItem changedItem = _sortIndex[i];
							bool wasVisible = changedItem.Visible;

							if ((i > 0 && CompareObject(_list[e.NewIndex], _list[_sortIndex[i - 1].BaseIndex]) < 0)
								|| (i < _sortIndex.Count - 1 && CompareObject(_list[e.NewIndex], _list[_sortIndex[i + 1].BaseIndex]) > 0))
							{
								if (wasVisible)
								{
									this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, oldIndex));
								}
								_sortIndex.RemoveAt(i);

								this.InsertInOrder(changedItem);
							}

							this.ApplyFilter(changedItem);
							int newIndex = this.SortedIndex(changedItem.BaseIndex);

							if (wasVisible)
							{
								if (changedItem.Visible)
								{
									if (newIndex != oldIndex)
									{
										this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, newIndex, oldIndex));
									}
								}
								else
								{
									this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, oldIndex));
								}
							}
							else
							{
								if (changedItem.Visible)
								{
									this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, newIndex));
								}
							}

							break;
						}
					}

					break;
				case ListChangedType.ItemDeleted:
					int deletedIndex = -1;

					for (int i = 0; i < _sortIndex.Count; i++)
					{
						if (_sortIndex[i].BaseIndex == e.NewIndex)
						{
							deletedIndex = this.SortedIndex(_sortIndex[i].BaseIndex);
							_sortIndex.RemoveAt(i);
							break;
						}
					}


					for (int i = 0; i < _sortIndex.Count; i++)
					{
						if (_sortIndex[i].BaseIndex > e.NewIndex)
						{
							_sortIndex[i].BaseIndex--;
						}
					}

					if (deletedIndex >= 0)
					{
						this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, deletedIndex));
					}

					break;
				case ListChangedType.Reset:
					ListSortDescriptionCollection sorts = _sorts;
					this.ApplySort(sorts);
					break;
			}
		}

		/// <summary>
		/// Reapplies the filter to the list.
		/// </summary>
		private void ApplyFilter()
		{
			for (int i = 0; i < _sortIndex.Count; i++)
			{
				this.ApplyFilter(_sortIndex[i]);
			}
		}

		/// <summary>
		/// Applies the filter to a single item.
		/// </summary>
		/// <param name="item">The item to filter.</param>
		private void ApplyFilter(ListItem item)
		{
			T itemObject = _list[item.BaseIndex];
			

			foreach (PropertyDescriptor prop in _columns)
			{
				_filteredRow[prop.Name] = prop.GetValue(itemObject);
			}

			item.Visible = _filteredView.Count > 0;
		}

		/// <summary>
		/// A helper method that compares to values.
		/// </summary>
		/// <param name="valueA">The first value.</param>
		/// <param name="valueB">The second value.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared.
		/// The return value has these meanings: Less than zero valueA is less than valueB.
		/// Zero valueA is equal to valueB. Greater than zero valueA is greater than valueB. 
		/// </returns>
		private int Compare(object valueA, object valueB)
		{
			if (object.Equals(valueA, valueB))
			{
				return 0;
			}

			if (valueA is IComparable)
			{
				return ((IComparable)valueA).CompareTo(valueB);
			}

			return valueA.ToString().CompareTo(valueB.ToString());
		}

		/// <summary>
		/// A helper method that compares items in the list.
		/// </summary>
		/// <param name="objectA">The first item.</param>
		/// <param name="objectB">The second item.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared.
		/// The return value has these meanings: Less than zero objectA should be before objectB.
		/// Zero the order of objectA and objectB should not be changed.
		/// Greater than zero objectA should be after objectB.
		/// </returns>
		private int CompareObject(T objectA, T objectB)
		{
			if (_sorts == null || (objectA == null && objectB == null))
			{
				return 0;
			}
			else if (objectA == null)
			{
				return -1;
			}
			else if (objectB == null)
			{
				return 1;
			}

			int comparison = 0;

			for (int i = 0; i < _sorts.Count; i++)
			{
				PropertyDescriptor prop = _sorts[i].PropertyDescriptor;

				comparison = this.Compare(prop.GetValue(objectA), prop.GetValue(objectB));

				if (comparison != 0)
				{
					if (_sorts[i].SortDirection == ListSortDirection.Descending)
					{
						comparison *= -1;
					}

					break;
				}
			}

			return comparison;
		}

		/// <summary>
		/// Inserts an item in order.
		/// </summary>
		/// <param name="item">The item to insert.</param>
		/// <returns>The sorted index of the item.</returns>
		private int InsertInOrder(ListItem item)
		{
			if (_sorts == null || _sortIndex.Count == 0)
			{
				_sortIndex.Add(item);
				return _sortIndex.Count - 1;
			}

			T itemObject = _list[item.BaseIndex];

			for (int i = _sortIndex.Count - 1; i >= 0; i--)
			{
				int comparison = CompareObject(itemObject, _list[_sortIndex[i].BaseIndex]);

				if (comparison >= 0)
				{
					if (i == _sortIndex.Count - 1)
					{
						_sortIndex.Add(item);
						return _sortIndex.Count - 1;
					}
					else
					{
						_sortIndex.Insert(i + 1, item);
						return i + 1;
					}
				}
			}

			_sortIndex.Insert(0, item);
			return 0;
		}

		/// <summary>
		/// Gets the original index of an item in the source list.
		/// </summary>
		/// <param name="sortedIndex">The index of the item in the sorted list.</param>
		/// <returns>The index of the item in the source list.</returns>
		private int OriginalIndex(int sortedIndex)
		{
			int filteredIndex = -1;

			for (int i = 0; i < _sortIndex.Count; i++)
			{
				if (_sortIndex[i].Visible)
				{
					filteredIndex++;
				}

				if (filteredIndex == sortedIndex)
				{
					return _sortIndex[i].BaseIndex;
				}
			}

			throw new IndexOutOfRangeException();
		}

		/// <summary>
		/// Gets the sorted index of an item.
		/// </summary>
		/// <param name="originalIndex">The index of the item in the source list.</param>
		/// <returns>The sorted index of the item.</returns>
		private int SortedIndex(int originalIndex)
		{
			int filteredIndex = -1;
			ListItem item;

			for (int i = 0; i < _sortIndex.Count; i++)
			{
				item = _sortIndex[i];

				if(item.Visible)
				{
					filteredIndex++;
				}

				if (item.BaseIndex == originalIndex)
				{
					if (item.Visible)
					{
						return filteredIndex;
					}

					return -1;
				}
			}

			throw new IndexOutOfRangeException();
		}

		#endregion

		#region ListItem class

		/// <summary>
		/// Provides a lookup from a sorted item into the source list and records whether the item is currently filtered.
		/// </summary>
		private class ListItem
		{

			private bool _visible;
			private int _baseIndex;

			/// <summary>
			/// Gets or sets the base (unsorted) index of a sorted item.
			/// </summary>
			public int BaseIndex
			{
				get { return _baseIndex; }
				set { _baseIndex = value; }
			}

			/// <summary>
			/// Gets or sets whether an item is visible.
			/// </summary>
			public bool Visible
			{
				get { return _visible; }
				set { _visible = value; }
			}

			/// <summary>
			/// Creates a new instance of a sorted item.
			/// </summary>
			/// <param name="baseIndex">The initial index into the source list.</param>
			public ListItem(int baseIndex)
			{
				_visible = true;
				_baseIndex = baseIndex;
			}
		}

		#endregion

		#region ObjectListView enumerator

		/// <summary>
		/// An enumerator for navigating through an ObjectListView
		/// </summary>
		private class ObjectListViewEnumerator : IEnumerator<T>
		{
			#region Fields

			private IList<T> _list;
			private List<ListItem> _sortIndex;
			private int _index;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the Csla.ObjectListView<T>.ObjectListViewEnumerator class
			/// with the given System.Collections.Generic.IList<T> and System.Collections.Generic.List<ListItem>.
			/// </summary>
			/// <param name="list">The source list.</param>
			/// <param name="sortIndex">The sorted list.</param>
			public ObjectListViewEnumerator(
			  IList<T> list,
			  List<ListItem> sortIndex)
			{
				_list = list;
				_sortIndex = sortIndex;
				Reset();
			}

			#endregion

			#region Properties

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public T Current
			{
				get { return _list[_sortIndex[_index].BaseIndex]; }
			}

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			Object System.Collections.IEnumerator.Current
			{
				get { return this.Current; }
			}

			#endregion

			#region Methods

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext()
			{
				for (int i = _index + 1; i < _sortIndex.Count; i++)
				{
					if (_sortIndex[i].Visible)
					{
						_index = i;
						return true;
					}
				}

				return false;
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			public void Reset()
			{
				_index = -1;
			}

			#endregion

			#region IDisposable Support

			private bool _disposedValue = false; // To detect redundant calls.

			/// <summary>
			/// Implements IDisposable.
			/// </summary>
			/// <param name="disposing">Whether to release managed resources.</param>
			protected virtual void Dispose(bool disposing)
			{
				if (!_disposedValue)
				{
					if (disposing)
					{
						// Free unmanaged resources when explicitly called
					}
					// Free shared unmanaged resources
				}
				_disposedValue = true;
			}

			/// <summary>
			/// Implements IDisposable.
			/// </summary>
			void IDisposable.Dispose()
			{
				// Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			~ObjectListViewEnumerator()
			{
				Dispose(false);
			}

			#endregion
		}

		#endregion

		#region IBindingListView Members

		/// <summary>
		/// Applies a series of sort properties and directions to this ObjectListView.
		/// </summary>
		/// <param name="sorts">The sort directions and properties.</param>
		public void ApplySort(System.ComponentModel.ListSortDescriptionCollection sorts)
		{
			_sorts = sorts;

			_sortIndex = new List<ListItem>(_list.Count);

			for (int i = 0; i < _list.Count; i++)
			{
				this.ApplyFilter(_sortIndex[this.InsertInOrder(new ListItem(i))]);
			}

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}

		/// <summary>
		/// Gets or sets the expression used to filter which items are viewed in the Csla.ObjectListView.
		/// </summary>
		public string Filter
		{
			get
			{
				return _filteredView.RowFilter;
			}
			set
			{
				if (value == null) value = string.Empty;

				if (_filteredView.RowFilter != value)
				{
					_filteredView.RowFilter = value;
					this.ApplyFilter();
					this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
				}
			}
		}

		/// <summary>
		/// Removes the filter on the Csla.ObjectListView.
		/// </summary>
		public void RemoveFilter()
		{
			if (_filteredView.RowFilter.Length > 0)
			{
				_filteredView.RowFilter = string.Empty;

				for (int i = 0; i < _sortIndex.Count; i++)
				{
					_sortIndex[i].Visible = true;
				}

				this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
			}
		}

		/// <summary>
		/// Gets the sort properties and directions of the Csla.ObjectListView.
		/// </summary>
		public System.ComponentModel.ListSortDescriptionCollection SortDescriptions
		{
			get { return _sorts; }
		}

		/// <summary>
		/// Gets whether advanced sorting is supported.
		/// </summary>
		public bool SupportsAdvancedSorting
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether filtering is supported.
		/// </summary>
		public bool SupportsFiltering
		{
			get { return true; }
		}

		#endregion

		#region IBindingList Members

		/// <summary>
		/// Adds an index to the source IBindingList.
		/// </summary>
		/// <param name="property"></param>
		public void AddIndex(System.ComponentModel.PropertyDescriptor property)
		{
			if (_supportsBinding)
			{
				_iBindingList.AddIndex(property);
			}
		}

		/// <summary>
		/// Adds a new item to the source IBindingList.
		/// </summary>
		/// <returns></returns>
		public object AddNew()
		{
			if (_supportsBinding)
			{
				return _iBindingList.AddNew();
			}
			
			return null;
		}

		/// <summary>
		/// Gets whether the source IBindingList allows edits.
		/// </summary>
		public bool AllowEdit
		{
			get
			{
				if (_supportsBinding)
				{
					return _iBindingList.AllowEdit;
				}

				return true;
			}
		}

		/// <summary>
		/// Gets whether the source IBindingList allows new items.
		/// </summary>
		public bool AllowNew
		{
			get
			{
				if (_supportsBinding)
				{
					return _iBindingList.AllowNew;
				}

				return true;
			}
		}

		/// <summary>
		/// Gets whether the source IBindingList allows removing of items.
		/// </summary>
		public bool AllowRemove
		{
			get
			{
				if (_supportsBinding)
				{
					return _iBindingList.AllowRemove;
				}

				return true;
			}
		}

		/// <summary>
		/// Applies a sort property and direction to the Csla.ObjectListView.
		/// </summary>
		/// <param name="property">The sort property.</param>
		/// <param name="direction">The sort direction.</param>
		public void ApplySort(System.ComponentModel.PropertyDescriptor property, System.ComponentModel.ListSortDirection direction)
		{
			if (property == null) throw new ArgumentNullException("property");
			
			this.ApplySort(
				new ListSortDescriptionCollection(new ListSortDescription[] { new ListSortDescription(property, direction) }));
		}

		/// <summary>
		/// Gets the first item whose property matches the key.
		/// </summary>
		/// <param name="propertyName">The name of the property to search.</param>
		/// <param name="key">The value for which to search.</param>
		/// <returns>The index of the first item whose property matches the key.</returns>
		public int Find(string propertyName, object key)
		{
			return this.Find(TypeDescriptor.GetProperties(typeof(T))[propertyName], key);
		}

		/// <summary>
		/// Gets the first item whose property matches the key.
		/// </summary>
		/// <param name="property">The property to search.</param>
		/// <param name="key">The value for which to search.</param>
		/// <returns>The index of the first item whose property matches the key.</returns>
		public int Find(System.ComponentModel.PropertyDescriptor property, object key)
		{
			if (property == null) throw new ArgumentNullException("property");

			foreach (T item in this)
			{
				if(object.Equals(key, property.GetValue(item)))
				{
					return this.IndexOf(item);
				}
			}

			return -1;
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView is sorted.
		/// </summary>
		public bool IsSorted
		{
			get { return _sorts != null; }
		}

		/// <summary>
		/// Occurs when the Csla.ObjectListView changes.
		/// </summary>
		public event System.ComponentModel.ListChangedEventHandler ListChanged;
		
		/// <summary>
		/// Raises the System.Data.DataView.ListChanged event.
		/// </summary>
		/// <param name="e">A System.ComponentModel.ListChangedEventArgs that contains the event data.</param>
		protected void OnListChanged(ListChangedEventArgs e)
		{
			if (ListChanged != null)
				ListChanged(this, e);
		}

		/// <summary>
		/// Removes an index from the source System.ComponentModel.IBindingList.
		/// </summary>
		/// <param name="property">The property from which to remove the index.</param>
		public void RemoveIndex(System.ComponentModel.PropertyDescriptor property)
		{
			if (_supportsBinding)
			{
				_iBindingList.RemoveIndex(property);
			}
		}

		/// <summary>
		/// Removes a sort from the Csla.ObjectListView.
		/// </summary>
		public void RemoveSort()
		{
			if (_sorts != null)
			{
				this.ApplySort(null);
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
			}
		}

		/// <summary>
		/// Gets the sort direction of the first sort property.
		/// </summary>
		public ListSortDirection SortDirection
		{
			get
			{
				if (_sorts == null)
				{
					return ListSortDirection.Ascending;
				}

				return _sorts[0].SortDirection;
			}
		}

		/// <summary>
		/// Gets the first property by which the Csla.ObjectListView is sorted.
		/// </summary>
		public PropertyDescriptor SortProperty
		{
			get
			{
				if (_sorts == null)
				{
					return null;
				}

				return _sorts[0].PropertyDescriptor;
			}
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView supports change notification.
		/// </summary>
		public bool SupportsChangeNotification
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView supports searching.
		/// </summary>
		public bool SupportsSearching
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView supports sorting.
		/// </summary>
		public bool SupportsSorting
		{
			get { return true; }
		}

		#endregion

		#region IList Members

		/// <summary>
		/// Adds an object to the source list.
		/// </summary>
		/// <param name="value">The new object.</param>
		/// <returns>The sorted index of the object.</returns>
		int IList.Add(object value)
		{
			this.Add((T)value);
			return SortedIndex(_list.Count - 1);
		}

		/// <summary>
		/// Clears the source list.
		/// </summary>
		public void Clear()
		{
			_list.Clear();
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView contains the given object.
		/// </summary>
		/// <param name="value">The object for which to check.</param>
		/// <returns>Whether the Csla.ObjectListView contains the given object.</returns>
		bool IList.Contains(object value)
		{
			return this.Contains((T)value);
		}

		/// <summary>
		/// Gets the sorted index of the given object.
		/// </summary>
		/// <param name="value">The object for which to check.</param>
		/// <returns>The sorted index of the given object, or -1 if the object is not in the Csla.ObjectListView.</returns>
		int IList.IndexOf(object value)
		{
			return this.IndexOf((T)value);
		}

		/// <summary>
		/// Inserts an item in the Csla.ObjectListView.
		/// </summary>
		/// <param name="index">The index at which to insert.</param>
		/// <param name="value">The item to insert.</param>
		/// <remarks>Throws a System.NotImplementedException.</remarks>
		void IList.Insert(int index, object value)
		{
			throw new NotImplementedException("Cannot insert objects.");
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView is a fixed size.
		/// </summary>
		public bool IsFixedSize
		{
			get { return false; }
		}

		/// <summary>
		/// Gets whether the source list is read only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return _list.IsReadOnly; }
		}

		/// <summary>
		/// Removes an item from the Csla.ObjectListView.
		/// </summary>
		/// <param name="value">The item to remove.</param>
		void IList.Remove(object value)
		{
			this.Remove((T)value);
		}

		/// <summary>
		/// Removes the item at the sorted index.
		/// </summary>
		/// <param name="index">The sorted index to remove.</param>
		public void RemoveAt(int index)
		{
			_list.RemoveAt(this.OriginalIndex(index));
		}

		/// <summary>
		/// Gets or sets the item at the given sorted index.
		/// </summary>
		/// <param name="index">The sorted index.</param>
		/// <returns>The item at the given sorted index.</returns>
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (T)value;
			}
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Copies the elements of the Csla.ObjectListView into the given array starting at the given index.
		/// </summary>
		/// <param name="array">The array into which to copy.</param>
		/// <param name="index">The index at which to start the copying.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo((T[])array, index);
		}

		/// <summary>
		/// Gets the number of elements in the Csla.ObjectListView.
		/// </summary>
		public int Count
		{
			get
			{
				if (_filteredView.RowFilter.Length == 0)
				{
					return _list.Count;
				}

				int filteredCount = 0;

				for (int i = 0; i < _sortIndex.Count; i++)
				{
					if (_sortIndex[i].Visible)
					{
						filteredCount++;
					}
				}

				return filteredCount;
			}
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView is synchronized.
		/// </summary>
		public bool IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the SyncRoot.
		/// </summary>
		public object SyncRoot
		{
			get { return _list; }
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Gets an System.Collections.IEnumerator to navigate over the Csla.ObjectListView.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region IList<T> Members

		/// <summary>
		/// Gets the index of the given item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The index of the given item.</returns>
		public int IndexOf(T item)
		{
			return SortedIndex(_list.IndexOf(item));
		}

		/// <summary>
		/// Inserts an item in the Csla.ObjectListView.
		/// </summary>
		/// <param name="index">The index at which to insert.</param>
		/// <param name="value">The item to insert.</param>
		/// <remarks>Throws a System.NotImplementedException.</remarks>
		void IList<T>.Insert(int index, T item)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		/// <summary>
		/// Gets or sets the item at the given sorted index.
		/// </summary>
		/// <param name="index">The sorted index.</param>
		/// <returns>The item at the given sorted index.</returns>
		public T this[int index]
		{
			get
			{
				return _list[OriginalIndex(index)];
			}
			set
			{
				_list[OriginalIndex(index)] = value;
			}
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Adds the given item to the source list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void Add(T item)
		{
			_list.Add(item);
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView contains the given item.
		/// </summary>
		/// <param name="item">The item for which to check.</param>
		/// <returns>Whether the Csla.ObjectListView contains the given item.</returns>
		public bool Contains(T item)
		{
			if (_filteredView.RowFilter.Length == 0)
			{
				return _list.Contains(item);
			}

			foreach (T filteredItem in this)
			{
				if (filteredItem.Equals(item))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Copies the elements of the Csla.ObjectListView into the given array starting at the given index.
		/// </summary>
		/// <param name="array">The array into which to copy.</param>
		/// <param name="arrayIndex">The index at which to start the copying.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (_filteredView.RowFilter.Length == 0)
			{
				_list.CopyTo(array, arrayIndex);
			}

			int i = 0;
			foreach (T item in this)
			{
				array[i] = item;
				i++;
			}
		}

		/// <summary>
		/// Removes the given item from the source list.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>Whether the item was removed.</returns>
		/// <remarks>Only removes the item if it is currently visible in the Csla.ObjectListView.</remarks>
		public bool Remove(T item)
		{
			if (_filteredView.RowFilter.Length == 0)
			{
				return _list.Remove(item);
			}

			foreach (T filteredItem in this)
			{
				if (filteredItem.Equals(item))
				{
					return _list.Remove(item);
				}
			}

			return false;
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Gets a System.Collections.Generic.IEnumerator<T> to navigate over the Csla.ObjectListView.
		/// </summary>
		/// <returns>A new enumerator.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			if (_sorts == null && _filteredView.RowFilter.Length == 0)
			{
				return _list.GetEnumerator();
			}

			return new ObjectListViewEnumerator(_list, _sortIndex);
		}

		#endregion
	}
}
