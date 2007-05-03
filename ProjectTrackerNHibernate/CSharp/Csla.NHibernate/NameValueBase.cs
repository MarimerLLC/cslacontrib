using System;
using Csla;

namespace Csla.NHibernate
{
	/// <summary>
	/// Abstract base class to represent an individual name value pair object.
	/// </summary>
	/// <typeparam name="K">Is the <c>Key</c>.</typeparam>
	/// <typeparam name="V">Is the <c>Value</c>.</typeparam>
	[Serializable]
	public abstract class NameValueBase<K, V>
	{
		/// <summary>
		/// Converts a instance that inherits from the <c>NameValueBase</c> to
		/// the CSLA <see cref="NameValueListBase{K,V}.NameValuePair"/> type.
		/// </summary>
		/// <returns>A <see cref="NameValueListBase{K,V}.NameValuePair"/> instance object.</returns>
		public abstract NameValueListBase<K, V>.NameValuePair ToNameValuePair();
	}
}
