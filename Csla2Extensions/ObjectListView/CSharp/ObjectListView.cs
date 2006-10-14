using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace Csla
{
	/// <summary>
	/// Represents a databindable, customized, view of a System.Collections.IList for sorting, filtering, searching, editing, and navigation.
	/// </summary>
	/// <author>Brian Criswell</author>
	/// <license>CREATIVE COMMONS - Attribution 2.5 License http://creativecommons.org/ </license>
	[
	DefaultEvent("ListChanged"),
	Editor("Microsoft.VSDesigner.Data.Design.DataSourceEditor, Microsoft.VSDesigner", "System.Drawing.Design.UITypeEditor, System.Drawing"),
	DefaultProperty("List")
	]
	public class ObjectListView : MarshalByValueComponent, IBindingListView, ITypedList
	{
		#region Fields

		// Source list
		private IList _list;
		private ObjectView _defaultItem;
		private bool _noProperties = false;

		// Sorting fields
		private ListSortDescriptionCollection _sorts;
		private List<ObjectView> _sortIndex;

		// Filtering fields
		private Type _indexedType;
		private PropertyDescriptorCollection _objectProperties = new PropertyDescriptorCollection(null);
		private DataRow _filteredRow;
		private DataView _filteredView = new DataView();

		//IBindingList fields
		private IBindingList _iBindingList = null;
		
		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		public ObjectListView()
			: this(null, string.Empty, string.Empty)
		{
		}

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		public ObjectListView(IList list)
			: this(list, string.Empty, string.Empty)
		{
		}

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		/// <param name="sort">The property or properties, and sort order for the <see cref="Csla.ObjectListView"/>.</param>
		public ObjectListView(IList list, string sort)
			: this(list, sort, string.Empty)
		{
		}

		/// <summary>
		/// Creates a new instance of an ObjectListView
		/// </summary>
		/// <param name="list">The source list.</param>
		/// <param name="sort">The property or properties, and sort order for the <see cref="Csla.ObjectListView"/>.</param>
		/// <param name="filter">The filter to apply.</param>
		public ObjectListView(IList list, string sort, string filter)
		{
			this.List = list;

			_filteredView.RowFilter = filter;

			this.Sort = sort;
		}

		#endregion

		#region Handle List Changes

		bool _addingItem = false;

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
					if (!_addingItem)
					{
						ObjectView addedItem = ObjectView.NewObjectView(this, _list[e.NewIndex], e.NewIndex);
						addedItem.PropertyChanged += new PropertyChangedEventHandler(ObjectView_PropertyChanged);
						addedItem.ApplyFilter(_filteredView);

						if (addedItem.Visible)
						{
							this.OnListChanged(ListChangedType.ItemAdded, this.InsertInOrder(addedItem, 0, _sortIndex.Count));
						}
						else
						{
							this.InsertInOrder(addedItem, 0, _sortIndex.Count);
						}
					}

					break;
				case ListChangedType.ItemChanged:
					if (e.NewIndex >= 0)
					{
						for (int i = 0; i < _sortIndex.Count; i++)
						{
							if (_sortIndex[i].BaseIndex == e.NewIndex)
							{
								if (!_sortIndex[i].IsEdit)
								{
									HandleChange(i, e.PropertyDescriptor);
								}

								break;
							}
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
							_sortIndex[i].PropertyChanged -= new PropertyChangedEventHandler(ObjectView_PropertyChanged);
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
						this.OnListChanged(ListChangedType.ItemDeleted, deletedIndex);
					}

					break;
				case ListChangedType.Reset:
					ListSortDescriptionCollection sorts = _sorts;
					((IBindingListView)this).ApplySort(sorts);
					break;
			}
		}

		private void ObjectView_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			int index = this.SortedIndex(((ObjectView)sender).BaseIndex);

			if (index >= 0)
			{
				PropertyDescriptor prop = null;

				if (!string.IsNullOrEmpty(e.PropertyName))
				{
					prop = _objectProperties[e.PropertyName];
				}

				this.HandleChange(index, prop);
			}
		}

		private void HandleChange(int index, PropertyDescriptor prop)
		{
			int baseIndex = _sortIndex[index].BaseIndex;
			int oldIndex = this.SortedIndex(baseIndex);
			ObjectView changedItem = _sortIndex[index];
			bool wasVisible = changedItem.Visible;

			bool needsSorting = false;

			if (index > 0)
			{
				needsSorting = CompareObject(changedItem, _sortIndex[index - 1]) < 0;

				if (needsSorting)
				{
					_sortIndex.RemoveAt(index);
					this.InsertInOrder(changedItem, 0, index);
				}
			}

			if (!needsSorting && index < _sortIndex.Count - 1)
			{
				needsSorting = CompareObject(changedItem, _sortIndex[index + 1]) > 0;

				if (needsSorting)
				{
					_sortIndex.RemoveAt(index);
					this.InsertInOrder(changedItem, index + 1, _sortIndex.Count);
				}
			}

			changedItem.ApplyFilter(_filteredView);
			int newIndex = this.SortedIndex(changedItem.BaseIndex);

			if (wasVisible)
			{
				if (changedItem.Visible)
				{
					if (newIndex == oldIndex)
					{
						if (prop == null)
						{
							this.OnListChanged(ListChangedType.ItemChanged, oldIndex);
						}
						else
						{
							this.OnListChanged(ListChangedType.ItemChanged, oldIndex, prop);
						}
					}
					else
					{
						this.OnListChanged(ListChangedType.ItemMoved, newIndex, oldIndex);
					}
				}
				else
				{
					this.OnListChanged(ListChangedType.ItemDeleted, oldIndex);
				}
			}
			else
			{
				if (changedItem.Visible)
				{
					this.OnListChanged(ListChangedType.ItemAdded, newIndex);
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reapplies the filter to the list.
		/// </summary>
		private void ApplyFilter()
		{
			for (int i = 0; i < _sortIndex.Count; i++)
			{
				_sortIndex[i].ApplyFilter(_filteredView);
			}
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
		private int CompareObject(ObjectView objectA, ObjectView objectB)
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

				object valueA = objectA[prop.Name];
				object valueB = objectB[prop.Name];

				if (objectA.Object is IExtendSort)
					valueA = ((IExtendSort)objectA.Object).GetSortValue(prop, valueA);

				if (objectB.Object is IExtendSort)
					valueB = ((IExtendSort)objectB.Object).GetSortValue(prop, valueB);

				this.OnExtendSort(prop, valueA);
				this.OnExtendSort(prop, valueB);

				comparison = this.Compare(valueA, valueB);

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
		/// <param name="lowIndex">The index of the lowest item to check.</param>
		/// <param name="highIndex">One more than the highest index to check.</param>
		/// <returns>The sorted index of the item.</returns>
		private int InsertInOrder(ObjectView item, int lowIndex, int highIndex)
		{
			if (_sorts == null || _sorts.Count == 0 || _sortIndex.Count == 0)
			{
				_sortIndex.Add(item);
				return _sortIndex.Count - 1;
			}

			if (lowIndex == highIndex)
			{
				// We have gotten to the end of our test
				if (highIndex == _sortIndex.Count)
				{
					_sortIndex.Add(item);
					return _sortIndex.Count - 1;
				}
				else
				{
					_sortIndex.Insert(highIndex, item);
					return highIndex;
				}
			}

			int testIndex = (lowIndex + highIndex) / 2;
			int comparison = CompareObject(item, _sortIndex[testIndex]);

			if (comparison == 0)
			{
				_sortIndex.Insert(testIndex, item);
				return testIndex;
			}
			else if (comparison < 0)
			{
				return this.InsertInOrder(item, lowIndex, testIndex);
			}
			else
			{
				return this.InsertInOrder(item, testIndex + 1, highIndex);
			}
		}

		/// <summary>
		/// Gets the unfiltered but sorted index of an item in the sorted list.
		/// </summary>
		/// <param name="sortedIndex">The index of the item in the filtered, sorted list.</param>
		/// <returns>The index of the item in the sorted list.</returns>
		private int UnfilteredIndex(int sortedIndex)
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
					return i;
				}
			}

			throw new IndexOutOfRangeException();
		}

		/// <summary>
		/// Gets the original index of an item in the source list.
		/// </summary>
		/// <param name="sortedIndex">The index of the item in the sorted list.</param>
		/// <returns>The index of the item in the source list.</returns>
		private int OriginalIndex(int sortedIndex)
		{
			return _sortIndex[UnfilteredIndex(sortedIndex)].BaseIndex;
		}

		/// <summary>
		/// Gets the sorted index of an item.
		/// </summary>
		/// <param name="originalIndex">The index of the item in the source list.</param>
		/// <returns>The sorted index of the item.</returns>
		private int SortedIndex(int originalIndex)
		{
			int filteredIndex = -1;

			for (int i = 0; i < _sortIndex.Count; i++)
			{
				ObjectView item = _sortIndex[i];

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

		#region Properties

		/// <summary>
		/// Gets or sets the source list of the ObjectListView.
		/// </summary>
		[
		Description("Indicates the list this ObjectListView uses to get data."),
		RefreshProperties(RefreshProperties.All),
		Category("Data"),
		DefaultValue(null),
		AttributeProvider(typeof(IListSource))
		]
		public IList List
		{
			get { return _list; }
			set
			{
				if (!object.Equals(_list, value))
				{
					if (_iBindingList != null)
					{
						_iBindingList.ListChanged -= new ListChangedEventHandler(_iBindingList_ListChanged);
					}

					_list = null;
					_noProperties = false;
					_indexedType = null;
					_objectProperties = new PropertyDescriptorCollection(null);
					_iBindingList = null;
					_filteredRow = null;
					_filteredView.Table = null;

					if (value != null)
					{
						_list = value;

						PropertyDescriptorCollection props;

						if (value is Array)
						{
							_indexedType = value.GetType().GetElementType();
							props = TypeDescriptor.GetProperties(_indexedType);
						}
						else
						{
							_indexedType = System.Windows.Forms.ListBindingHelper.GetListItemType(value);
							props = System.Windows.Forms.ListBindingHelper.GetListItemProperties(value, null);
						}
						
						for (int i = 0; i < props.Count; i++)
						{
							_objectProperties.Add(new ObjectView.ObjectViewPropertyDescriptor(props[i]));
						}

						DataTable table = new DataTable("Table");

						foreach (PropertyDescriptor prop in _objectProperties)
						{
							if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
							{
								Type nullableType = prop.PropertyType.GetGenericArguments()[0];

								table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
							}
							else
								table.Columns.Add(prop.Name, prop.PropertyType);
						}

						if (table.Columns.Count == 0)
						{
							_noProperties = true;
							table.Columns.Add("Value", typeof(object));
						}

						_filteredRow = table.Rows.Add();
						_filteredView.Table = table;

						if (_noProperties)
						{
							_objectProperties.Add(new ObjectView.ObjectViewPropertyDescriptor(TypeDescriptor.GetProperties(_filteredView[0])[0]));
						}
					}

					if (value is IBindingList)
					{
						_iBindingList = (IBindingList)value;
						_iBindingList.ListChanged += new ListChangedEventHandler(_iBindingList_ListChanged);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets whether to include a default item in the ObjectListView.
		/// </summary>
		[
		Category("Data"),
		Description("Indicates whether this ObjectListView should display a default item as the first item in the list."),
		DefaultValue(false)
		]
		public bool IncludeDefault
		{
			get
			{
				return _defaultItem != null;
			}
			set
			{
				if (value)
				{
					if (!this.IncludeDefault)
					{
						_defaultItem = new ObjectView.DefaultItemObjectView(this);
						// Set the newIndex to -1 as OnListChanged will increment it
						// because IncludeDefault is now true.
						this.OnListChanged(ListChangedType.ItemAdded, -1);
					}
				}
				else
				{
					if (this.IncludeDefault)
					{
						_defaultItem = null;
						// Set the newIndex to 0 as OnListChanged will no longer increment it
						// because IncludeDefault is now false.
						this.OnListChanged(ListChangedType.ItemDeleted, 0);
					}
				}
			}
		}

		internal Type IndexedType
		{
			get { return _indexedType; }
		}

		internal bool NoProperties
		{
			get { return _noProperties; }
		}

		internal PropertyDescriptorCollection ObjectProperties
		{
			get { return _objectProperties; }
		}

		/// <summary>
		/// Gets or sets the sort property or properties, and sort order for the <see cref="Csla.ObjectListView"/>
		/// </summary>
		[
		Category("Data"),
		Description("Indicates the name of the properties and the order in which items are returned by this ObjectListView."),
		DefaultValue("")
		]
		public string Sort
		{
			get
			{
				((IBindingListView)_filteredView).ApplySort(_sorts);
				return _filteredView.Sort;
			}
			set
			{
				if (_filteredView.Sort != value || _sortIndex == null)
				{
					_filteredView.Sort = value;
					((IBindingListView)this).ApplySort(((IBindingListView)_filteredView).SortDescriptions);
				}
			}
		}
		
		#endregion

		#region ExtendSort event

		/// <summary>
		/// Extends the sorting algorithm by allowing a listener to exchange a comparison value with another value.
		/// </summary>
		public event EventHandler<ExtendSortEventArgs> ExtendSort;

		private void OnExtendSort(PropertyDescriptor property, object value)
		{
			EventHandler<ExtendSortEventArgs> temp = ExtendSort;

			if (temp != null)
			{
				temp(this, new ExtendSortEventArgs(property, value));
			}
		}

		#endregion

		#region ObjectListView enumerator

		/// <summary>
		/// An enumerator for navigating through an ObjectListView
		/// </summary>
		private class ObjectListViewEnumerator : IEnumerator
		{
			#region Fields

			private ObjectListView _parent;
			private int _index;
			private bool _isValid = true;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the Csla.ObjectListView.ObjectListViewEnumerator class
			/// with the given System.Collections.Generic.IList and System.Collections.Generic.List&lt;ListItem>.
			/// </summary>
			/// <param name="parent">The parent ObjectListView.</param>
			public ObjectListViewEnumerator(ObjectListView parent)
			{
				_parent = parent;
				_parent.ListChanged += new ListChangedEventHandler(_parent_ListChanged);
				Reset();
			}

			#endregion

			#region Properties

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public ObjectView Current
			{
				get
				{
					if (_index < 0)
					{
						throw new InvalidOperationException("The enumerator is positioned before the first element of the collection.");
					}
					else if (_index >= _parent.Count)
					{
						throw new InvalidOperationException("The enumerator is positioned after the last element of the collection.");
					}
					
					return _parent[_index];
				}
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
			/// Invalidates the enumerator because the underlying list has changed.
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="e"></param>
			private void _parent_ListChanged(object sender, ListChangedEventArgs e)
			{
				_isValid = false;
			}

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext()
			{
				if (!_isValid)
				{
					throw new InvalidOperationException("The collection was modified after the enumerator was created.");
				}

				_index++;

				return _index < _parent.Count;
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			public void Reset()
			{
				if (!_isValid)
				{
					throw new InvalidOperationException("The collection was modified after the enumerator was created.");
				}

				_index = -1;
			}

			#endregion
		}

		#endregion

		#region IBindingListView Members

		/// <summary>
		/// Applies a series of sort properties and directions to this ObjectListView.
		/// </summary>
		/// <param name="sorts">The sort directions and properties.</param>
		void IBindingListView.ApplySort(System.ComponentModel.ListSortDescriptionCollection sorts)
		{
			_sorts = sorts;

			if (_sortIndex != null)
			{
				for (int i = 0; i < _sortIndex.Count; i++)
				{
					_sortIndex[i].PropertyChanged -= new PropertyChangedEventHandler(ObjectView_PropertyChanged);
				}
			}

			if (_list == null)
			{
				_sortIndex = new List<ObjectView>();
			}
			else
			{
				_sortIndex = new List<ObjectView>(_list.Count);

				for (int i = 0; i < _list.Count; i++)
				{
					ObjectView item = ObjectView.NewObjectView(this, _list[i], i);
					item.PropertyChanged += new PropertyChangedEventHandler(ObjectView_PropertyChanged);
					item.ApplyFilter(_filteredView);
					this.InsertInOrder(item, 0, _sortIndex.Count);
				}
			}

			this.OnListChanged(ListChangedType.Reset, -1);
		}

		/// <summary>
		/// Gets or sets the expression used to filter which items are viewed in the Csla.ObjectListView.
		/// </summary>
		[
		Category("Data"),
		Description("Indicates an expression used to filter the items returned by this ObjectListView."),
		DefaultValue("")
		]
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
					this.OnListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		/// <summary>
		/// Removes the filter on the Csla.ObjectListView.
		/// </summary>
		void IBindingListView.RemoveFilter()
		{
			this.Filter = string.Empty;
		}

		/// <summary>
		/// Gets the sort properties and directions of the Csla.ObjectListView.
		/// </summary>
		ListSortDescriptionCollection IBindingListView.SortDescriptions
		{
			get { return _sorts; }
		}

		/// <summary>
		/// Gets whether advanced sorting is supported.
		/// </summary>
		bool IBindingListView.SupportsAdvancedSorting
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether filtering is supported.
		/// </summary>
		bool IBindingListView.SupportsFiltering
		{
			get { return true; }
		}
		
		#endregion

		#region IBindingList Members

		/// <summary>
		/// Adds an index to the source IBindingList.
		/// </summary>
		/// <param name="property"></param>
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			if (_iBindingList != null)
			{
				_iBindingList.AddIndex(property);
			}
		}

		/// <summary>
		/// Adds a new item to the source IBindingList.
		/// </summary>
		/// <returns></returns>
		public virtual ObjectView AddNew()
		{
			if (this.AllowNew)
			{
				if (_list == null)
				{
					throw new InvalidOperationException("Cannot add new items when the list is not set.");
				}

				try
				{
					_addingItem = true;
					ObjectView item;

					if (_iBindingList != null)
					{
						item = ObjectView.NewObjectView(this, _iBindingList.AddNew(), _list.Count - 1, true);
					}
					else
					{
						Object obj = Activator.CreateInstance(_indexedType);
						_list.Add(obj);
						item = ObjectView.NewObjectView(this, obj, _list.Count - 1, true);
					}

					item.PropertyChanged += new PropertyChangedEventHandler(ObjectView_PropertyChanged);
					item.BeginEdit();
					_sortIndex.Add(item);
					return item;
				}
				finally
				{
					_addingItem = false;
				}
			}

			throw new NotSupportedException("Adding new items is not supported."); 
		}

		object IBindingList.AddNew()
		{
			return this.AddNew();
		}

		private bool _allowEdit = true;

		/// <summary>
		/// Gets whether the source IBindingList allows edits.
		/// </summary>
		[
		Category("Data"),
		Description("Indicates whether this ObjectListView and the user interface associated with it allows edits."),
		DefaultValue(true)
		]
		public bool AllowEdit
		{
			get
			{
				bool allowEdit = _allowEdit;

				if (_iBindingList != null)
				{
					allowEdit &= _iBindingList.AllowEdit;
				}
				else if(_list != null)
				{
					allowEdit &= !_list.IsReadOnly;
				}

				return allowEdit;
			}
			set { _allowEdit = value; }
		}

		private bool _allowNew = true;

		/// <summary>
		/// Gets whether the source IBindingList allows new items.
		/// </summary>
		[
		Category("Data"),
		Description("Indicates whether this ObjectListView and the user interface associated with it allows new items to be added."),
		DefaultValue(true)
		]
		public bool AllowNew
		{
			get
			{
				bool allowNew = _allowNew;

				if (_iBindingList != null)
				{
					allowNew &= _iBindingList.AllowNew;
				}
				else if (_list != null)
				{
					_allowNew &= !_list.IsReadOnly && !_list.IsFixedSize;
				}

				return allowNew;
			}
			set { _allowNew = value; }
		}

		private bool _allowRemove = true;

		/// <summary>
		/// Gets whether the source IBindingList allows removing of items.
		/// </summary>
		[
		Category("Data"),
		Description("Indicates whether this ObjectListView and the user interface associated with it allows items to be removed."),
		DefaultValue(true)
		]
		public bool AllowRemove
		{
			get
			{
				bool allowRemove = _allowRemove;

				if (_iBindingList != null)
				{
					allowRemove &= _iBindingList.AllowRemove;
				}
				else if (_list != null)
				{
					allowRemove &= !_list.IsReadOnly & !_list.IsFixedSize;
				}

				return allowRemove;
			}
			set { _allowRemove = value; }
		}

		/// <summary>
		/// Applies a sort property and direction to the Csla.ObjectListView.
		/// </summary>
		/// <param name="property">The sort property.</param>
		/// <param name="direction">The sort direction.</param>
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			if (property == null) throw new ArgumentNullException("property");

			((IBindingListView)this).ApplySort(new ListSortDescriptionCollection(new ListSortDescription[] { new ListSortDescription(property, direction) }));
		}

		/// <summary>
		/// Gets the first item whose property matches the key.
		/// </summary>
		/// <param name="propertyName">The name of the property to search.</param>
		/// <param name="key">The value for which to search.</param>
		/// <returns>The index of the first item whose property matches the key.</returns>
		public int Find(string propertyName, object key)
		{
			return ((IBindingList)this).Find(_objectProperties[propertyName], key);
		}

		/// <summary>
		/// Gets the first item whose property matches the key.
		/// </summary>
		/// <param name="property">The property to search.</param>
		/// <param name="key">The value for which to search.</param>
		/// <returns>The index of the first item whose property matches the key.</returns>
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			if (((IBindingList)this).SupportsSearching)
			{
				if (property == null) throw new ArgumentNullException("property");

				if (this.IncludeDefault && object.Equals(key, _defaultItem[property.Name]))
				{
					return 0;
				}

				int index = -1;

				for (int i = 0; i < _sortIndex.Count; i++)
				{
					if (_sortIndex[i].Visible)
					{
						index++;

						if (object.Equals(key, _sortIndex[i][property.Name]))
						{
							if (this.IncludeDefault)
							{
								return index + 1;
							}

							return index;
						}
					}
				}

				return -1;
			}

			throw new NotSupportedException("Searching is not supported.");
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView is sorted.
		/// </summary>
		bool IBindingList.IsSorted
		{
			get { return _sorts != null; }
		}

		/// <summary>
		/// Occurs when the list managed by the ObjectListView changes.
		/// </summary>
		public event ListChangedEventHandler ListChanged;

		/// <summary>
		/// Raises the System.Data.DataView.ListChanged event.
		/// </summary>
		/// <param name="listChangedType">The type of change.</param>
		/// <param name="newIndex">The index of the row affected by the change.</param>
		/// <remarks>
		/// Used when adding or deleting a row.  Also used with an index of -1 when resetting the
		/// list.  Also used when a row has been changed but that change is not the result of a
		/// specific property changing in the row.
		/// </remarks>
		protected void OnListChanged(ListChangedType listChangedType, int newIndex)
		{
			if (_defaultItem != null)
				newIndex++;

			if (ListChanged != null)
				this.ListChanged(this, new ListChangedEventArgs(listChangedType, newIndex));
		}

		/// <summary>
		/// Raises the System.Data.DataView.ListChanged event.
		/// </summary>
		/// <param name="listChangedType">The type of change.</param>
		/// <param name="newIndex">The index of the row affected by the change.</param>
		/// <param name="oldIndex">The previous index of the row.</param>
		/// <remarks>Used when moving a row.</remarks>
		protected void OnListChanged(ListChangedType listChangedType, int newIndex, int oldIndex)
		{
			if (_defaultItem != null)
			{
				newIndex++;
				oldIndex++;
			}

			if (ListChanged != null)
				this.ListChanged(this, new ListChangedEventArgs(listChangedType, newIndex, oldIndex));
		}

		/// <summary>
		/// Raises the System.Data.DataView.ListChanged event.
		/// </summary>
		/// <param name="listChangedType">The type of change.</param>
		/// <param name="newIndex">The index of the row affected by the change.</param>
		/// <param name="propDesc">The property that changed on the row.</param>
		/// <remarks>Used when a change in the row is caused by a changing property.</remarks>
		protected void OnListChanged(ListChangedType listChangedType, int newIndex, PropertyDescriptor propDesc)
		{
			if (_defaultItem != null)
				newIndex++;

			if (ListChanged != null)
				this.ListChanged(this, new ListChangedEventArgs(listChangedType, newIndex, propDesc));
		}

		/// <summary>
		/// Removes an index from the source System.ComponentModel.IBindingList.
		/// </summary>
		/// <param name="property">The property from which to remove the index.</param>
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			if (_iBindingList != null)
			{
				_iBindingList.RemoveIndex(property);
			}
		}

		/// <summary>
		/// Removes a sort from the Csla.ObjectListView.
		/// </summary>
		void IBindingList.RemoveSort()
		{
			if (_sorts != null)
			{
				((IBindingListView)this).ApplySort(null);
				OnListChanged(ListChangedType.Reset, -1);
			}
		}

		/// <summary>
		/// Gets the sort direction of the first sort property.
		/// </summary>
		ListSortDirection IBindingList.SortDirection
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
		PropertyDescriptor IBindingList.SortProperty
		{
			get
			{
				if (_sorts == null || _sorts.Count == 0)
				{
					return null;
				}

				return _sorts[0].PropertyDescriptor;
			}
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView supports change notification.
		/// </summary>
		bool IBindingList.SupportsChangeNotification
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView supports searching.
		/// </summary>
		bool IBindingList.SupportsSearching
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView supports sorting.
		/// </summary>
		bool IBindingList.SupportsSorting
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Clears the source list.
		/// </summary>
		void IList.Clear()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView contains the given object.
		/// </summary>
		/// <param name="value">The object for which to check.</param>
		/// <returns>Whether the Csla.ObjectListView contains the given object.</returns>
		bool IList.Contains(object value)
		{
			return ((IList)this).IndexOf(value) >= 0;
		}

		/// <summary>
		/// Gets the sorted index of the given object.
		/// </summary>
		/// <param name="value">The object for which to check.</param>
		/// <returns>The sorted index of the given object, or -1 if the object is not in the Csla.ObjectListView.</returns>
		int IList.IndexOf(object value)
		{
			int index = -1;

			if (value is ObjectView)
			{
				if (this.IncludeDefault && _defaultItem.Equals(value))
				{
					return 0;
				}

				for (int i = 0; i < _sortIndex.Count; i++)
				{
					if (_sortIndex[i].Equals(value))
					{
						index = i;
						if (this.IncludeDefault)
						{
							index++;
						}
						break;
					}
				}
			}

			return index;
		}

		/// <summary>
		/// Inserts an item in the Csla.ObjectListView.
		/// </summary>
		/// <param name="index">The index at which to insert.</param>
		/// <param name="value">The item to insert.</param>
		/// <remarks>Throws a System.NotImplementedException.</remarks>
		void IList.Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets whether the Csla.ObjectListView is a fixed size.
		/// </summary>
		bool IList.IsFixedSize
		{
			get { return _list.IsFixedSize; }
		}

		/// <summary>
		/// Gets whether the source list is read only.
		/// </summary>
		bool IList.IsReadOnly
		{
			get { return _list.IsReadOnly; }
		}

		/// <summary>
		/// Removes an item from the Csla.ObjectListView.
		/// </summary>
		/// <param name="value">The item to remove.</param>
		void IList.Remove(object value)
		{
			if (this.AllowRemove)
			{
				if (value == _defaultItem)
				{
					// Cannot delete the default item, but do not raise an exception.
					return;
				}

				for (int i = 0; i < _sortIndex.Count; i++)
				{
					if (_sortIndex[i].Equals(value))
					{
						_list.RemoveAt(_sortIndex[i].BaseIndex);
						return;
					}
				}

				throw new IndexOutOfRangeException();
			}

			throw new InvalidOperationException();
		}

		/// <summary>
		/// Removes the item at the sorted index.
		/// </summary>
		/// <param name="index">The sorted index to remove.</param>
		void IList.RemoveAt(int index)
		{
			if (this.AllowRemove)
			{
				if (this.IncludeDefault)
				{
					if (index == 0)
					{
						// Cannot delete the default item, but do not raise an exception.
						return;
					}

					index--;
				}

				_list.RemoveAt(this.OriginalIndex(index));
				return;
			}

			throw new InvalidOperationException();
		}

		/// <summary>
		/// Gets or sets the item at the given sorted index.
		/// </summary>
		/// <param name="index">The sorted index.</param>
		/// <returns>The item at the given sorted index.</returns>
		public ObjectView this[int index]
		{
			get
			{
				if (this.IncludeDefault)
				{
					if (index == 0)
					{
						return _defaultItem;
					}

					index--;
				}

				return _sortIndex[this.UnfilteredIndex(index)];
			}
		}

		object IList.this[int index]
		{
			get { return this[index]; }
			set { throw new NotImplementedException(); }
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Copies the elements of the Csla.ObjectListView into the given array starting at the given index.
		/// </summary>
		/// <param name="array">The array into which to copy.</param>
		/// <param name="index">The index at which to start the copying.</param>
		public void CopyTo(Array array, int index)
		{
			if (_list == null)
			{
				throw new InvalidOperationException("Cannot copy while the list is not set.");
			}

			if (this.Filter.Length == 0)
			{
				_list.CopyTo(array, index);
			}
			else
			{
				for (int i = 0; i < this.Count; i++)
				{
					array.SetValue(this[i], i);
				}
			}
		}

		/// <summary>
		/// Gets the number of elements in the Csla.ObjectListView.
		/// </summary>
		[Browsable(false)]
		public int Count
		{
			get
			{
				int filteredCount = 0;

				if (_list != null)
				{
					if (this.Filter.Length == 0)
					{
						filteredCount = _list.Count;
					}
					else
					{
						for (int i = 0; i < _sortIndex.Count; i++)
						{
							if (_sortIndex[i].Visible)
							{
								filteredCount++;
							}
						}
					}

					if (this.IncludeDefault)
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
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the SyncRoot.
		/// </summary>
		object ICollection.SyncRoot
		{
			get { return this; }
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Gets an System.Collections.IEnumerator to navigate over the Csla.ObjectListView.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return new ObjectListViewEnumerator(this);
		}

		#endregion

		#region ITypedList Members

		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection pdc;

			if (null == listAccessors)
			{
				// Return properties in sort order.
				pdc = _objectProperties;
			}
			else
			{
				// Return child list shape.
				PropertyDescriptorCollection itemProps = System.Windows.Forms.ListBindingHelper.GetListItemProperties(listAccessors[0].PropertyType);
				PropertyDescriptorCollection props = new PropertyDescriptorCollection(null);
				for (int i = 0; i < itemProps.Count; i++)
				{
					props.Add(new ObjectView.ObjectViewPropertyDescriptor(itemProps[i]));
				}

				pdc = props;
			}

			return pdc;
		}

		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			if (_indexedType != null)
				return _indexedType.Name;

			if (_list != null && _list.GetType().IsArray)
			{
				string name = _list.GetType().Name;
				return name.Substring(0, name.Length - 2);
			}

			return string.Empty;
		}

		#endregion
	}
}
