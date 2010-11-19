using System;
using System.ComponentModel;

namespace Csla
{
	/// <summary>
	/// Provides data for the <see cref="Csla.ObjectListView.ExtendSort"/> event.
	/// </summary>
	/// <author>Brian Criswell</author>
	/// <license>CREATIVE COMMONS - Attribution 2.5 License http://creativecommons.org/ </license>
	public class ExtendSortEventArgs : EventArgs
	{
		#region Constructor

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="property">The property descriptor associated with the comparison value.</param>
		/// <param name="value">The comparison value.</param>
		/// <exception cref="System.ArgumentNullException">The property descriptor was null.</exception>
		internal ExtendSortEventArgs(PropertyDescriptor property, object value)
		{
			if (_property == null)
				throw new ArgumentNullException("property");

			_property = property;
			_value = value;
		}

		#endregion

		#region Fields

		private PropertyDescriptor _property;
		private object _value;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the property descriptor associated with the comparison value.
		/// </summary>
		public PropertyDescriptor Property
		{
			get { return _property; }
		}

		/// <summary>
		/// Gets or sets the comparison value.
		/// </summary>
		public object Value
		{
			get { return _value; }
			set { _value = value; }
		}	

		#endregion
	}
}
