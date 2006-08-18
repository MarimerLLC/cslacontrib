using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Csla
{
	/// <summary>
	/// Represents a databindable, transactioned view of an object.
	/// </summary>
	/// <author>Brian Criswell</author>
	/// <license>CREATIVE COMMONS - Attribution 2.5 License http://creativecommons.org/ </license>
	public class ObjectView : IDataErrorInfo, IEditableObject, INotifyPropertyChanged, ICustomTypeDescriptor
	{
		#region Fields

		private ObjectListView _parent;
		private object _object;

		#endregion

		#region Constructor

		internal ObjectView(ObjectListView parent, object obj, int baseIndex)
			: this(parent, obj, baseIndex, false)
		{
		}

		internal ObjectView(ObjectListView parent, object obj, int baseIndex, bool isNew)
		{
			_parent = parent;
			_object = obj;
			_isNew = isNew;
			_baseIndex = baseIndex;

			if (_object is INotifyPropertyChanged)
			{
				((INotifyPropertyChanged)_object).PropertyChanged += new PropertyChangedEventHandler(Object_PropertyChanged);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets a value.
		/// </summary>
		/// <param name="index">The index of the value to retrieve.</param>
		/// <returns>The value at the index.</returns>
		protected virtual object GetValue(int index)
		{
			return this.Parent.ObjectProperties[index].GetValue(this.Object);
		}

		/// <summary>
		/// Gets a value.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <returns>The value of the name.</returns>
		protected virtual object GetValue(string name)
		{
			return this.Parent.ObjectProperties[name].GetValue(this.Object);
		}

		/// <summary>
		/// Sets the value at the given index.
		/// </summary>
		/// <param name="index">The index of the value to set.</param>
		/// <param name="value">The value to set.</param>
		protected virtual void SetValue(int index, object value)
		{
			this.Parent.ObjectProperties[index].SetValue(this.Object, value);
		}

		/// <summary>
		/// Sets the value for the given name.
		/// </summary>
		/// <param name="name">The name of the value to set.</param>
		/// <param name="value">The value to set.</param>
		protected virtual void SetValue(string name, object value)
		{
			this.Parent.ObjectProperties[name].SetValue(this.Object, value);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the parent ObjectListView.
		/// </summary>
		protected ObjectListView Parent
		{
			get { return _parent; }
		}

		/// <summary>
		/// Gets or sets the corresponding property on the underlying object.
		/// </summary>
		/// <param name="index">The index of the property.</param>
		/// <returns>The value of the property.</returns>
		public object this[int index]
		{
			get { return this.GetValue(index); }
			set { this.SetValue(index, value); }
		}

		/// <summary>
		/// Gets or sets the corresponding property on the underlying object.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns>The value of the property.</returns>
		public object this[string name]
		{
			get { return this.GetValue(name); }
			set { this.SetValue(name, value); }
		}

		/// <summary>
		/// Gets the underlying object.
		/// </summary>
		public object Object
		{
			get { return _object; }
		}

		#endregion

		#region Sorting and Filtering

		private int _baseIndex = -1;

		/// <summary>
		/// Gets or sets the index in the source list of the item that this ObjectView represents.
		/// </summary>
		internal int BaseIndex
		{
			get { return _baseIndex; }
			set { _baseIndex = value; }
		}

		private bool _visible = true;

		/// <summary>
		/// Gets or sets whether this ObjectView is visible.
		/// </summary>
		internal bool Visible
		{
			get { return _visible; }
		}

		internal bool ApplyFilter(System.Data.DataView filteredView)
		{
			if (filteredView == null)
			{
				_visible = true;
				return _visible;
			}

			System.Data.DataRow row = filteredView.Table.Rows[0];

			foreach (System.ComponentModel.PropertyDescriptor prop in this.Parent.ObjectProperties)
			{
				object value = prop.GetValue(this.Object);

				if (value == null)
				{
					row[prop.Name] = DBNull.Value;
				}
				else
				{
					row[prop.Name] = prop.GetValue(this.Object);
				}
			}

			_visible = filteredView.Count > 0;

			return _visible;
		}

		#endregion

		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get
			{
				if (_object is IDataErrorInfo)
				{
					return ((IDataErrorInfo)_object).Error;
				}

				return string.Empty;
			}
		}

		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				if (_object is IDataErrorInfo)
				{
					return ((IDataErrorInfo)_object)[columnName];
				}

				return string.Empty;
			}
		}

		#endregion

		#region IEditableObject Members

		private Dictionary<string, object> _cache;

		/// <summary>
		/// Gets whether the ObjectView is in edit mode.
		/// </summary>
		public bool IsEdit
		{
			get { return _isNew || _cache != null; }
		}

		private bool _isNew = false;

		/// <summary>
		/// Gets whether the ObjectView is the new ObjectView for the ObjectListView
		/// </summary>
		public bool IsNew
		{
			get { return _isNew; }
		}

		/// <summary>
		/// Puts the ObjectView into edit mode.
		/// </summary>
		public void BeginEdit()
		{
			if (_cache == null && !_isNew)
			{
				_cache = new Dictionary<string, object>();
				foreach (PropertyDescriptor prop in _parent.ObjectProperties)
				{
					_cache.Add(prop.Name, this.GetValue(prop.Name));
				}
			}
		}

		/// <summary>
		/// Reverts the changes made to this ObjectView.
		/// </summary>
		public void CancelEdit()
		{
			if (_isNew)
			{
				((IBindingList)_parent).Remove(this);
			}
			else if (_cache != null)
			{
				foreach (string key in _cache.Keys)
				{
					this.SetValue(key, _cache[key]);
				}

				_cache = null;

				this.OnPropertyChanged(string.Empty);
			}
		}

		/// <summary>
		/// Accepts the changes made to this ObjectView.
		/// </summary>
		public void EndEdit()
		{
			if (this.IsEdit)
			{
				_cache = null;
				_isNew = false;
				this.OnPropertyChanged(string.Empty);
			}
		}

		#endregion

		#region INotifyPropertyChanged Members

		private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!this.IsEdit)
			{
				this.OnPropertyChanged(e.PropertyName);
			}
		}

		/// <summary>
		/// Occurs when the object managed by the ObjectView changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Call this method to raise the PropertyChanged event
		/// for a specific property.
		/// </summary>
		/// <remarks>
		/// This method may be called by properties in the business
		/// class to indicate the change in a specific property.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region ICustomTypeDescriptor Members

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this.Parent.IndexedType);
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return null;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return null;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(this.Parent.IndexedType, attributes);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return TypeDescriptor.GetProperties(this.Parent.IndexedType);
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this.Object;
		}

		#endregion
	}
}
