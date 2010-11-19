using System;
using System.ComponentModel;

namespace Csla
{
	/// <summary>
	/// Allows an object to alter how it is sorted.
	/// </summary>
	/// <author>Brian Criswell</author>
	/// <license>CREATIVE COMMONS - Attribution 2.5 License http://creativecommons.org/ </license>
	public interface IExtendSort
	{
		/// <summary>
		/// Extends a sort by optionally replacing a sort value
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		object GetSortValue(PropertyDescriptor property, object value);
	}
}
