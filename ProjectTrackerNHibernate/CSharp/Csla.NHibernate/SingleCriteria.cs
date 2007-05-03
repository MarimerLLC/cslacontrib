using System;

namespace Csla.NHibernate
{
	/// <summary>
	/// A criteria class that only holds one value.
	/// </summary>
	/// <typeparam name="T">The <see cref="Type"/> of the value that the criteria holds.</typeparam>
	[Serializable]
	public class SingleCriteria<T> : CriteriaBase
	{
		#region fields

		private T _value;

		#endregion

		#region properties

		/// <summary>
		/// Gets/sets the criteria value.
		/// </summary>
		public T Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion

		#region constructors

		/// <summary>
		/// Creates a new instance of the <see cref="SingleCriteria{T}"/> class.
		/// </summary>		
		/// <param name="type">The <see cref="Type"/> of the Business Object to be created by the CSLA Data Portal.</param>
		/// <param name="value">The value of the criteria.</param>
		public SingleCriteria(Type type, T value) : base(type)
		{
			_value = value;
		}

		#endregion
	}
}