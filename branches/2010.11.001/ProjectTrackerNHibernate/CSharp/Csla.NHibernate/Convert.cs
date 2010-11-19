using System;
using Nullables;

namespace Csla.NHibernate
{
	public static class Convert
	{
		/// <summary>
		/// Converts a <see cref="NullableDateTime"/> to a <see cref="SmartDate"/>.
		/// </summary>
		/// <param name="nullableDateTime">A <see cref="NullableDateTime"/> instance object.</param>
		/// <returns>A <see cref="SmartDate"/> instance object.</returns>
		public static SmartDate ToSmartDate(NullableDateTime nullableDateTime)
		{
			if (nullableDateTime.HasValue)
				return new SmartDate(nullableDateTime.Value);
			else
				throw new Exception("Cannot convert an empty NullableDateTime to a SmartDate");
		}

		/// <summary>
		/// Converts a <see cref="NullableDateTime"/> to a <see cref="SmartDate"/>.
		/// </summary>
		/// <param name="nullableDateTime">A <see cref="NullableDateTime"/> instance object.</param>
		/// <param name="emptyIsMin">Indicates whether <lang>null</lang> should be treated as <see cref="DateTime.MinValue"/></param>
		/// <returns>A <see cref="SmartDate"/> instance object.</returns>
		public static SmartDate ToSmartDate(NullableDateTime nullableDateTime, bool emptyIsMin)
		{
			if (nullableDateTime.HasValue)
				return new SmartDate(nullableDateTime.Value);
			else
				return new SmartDate(emptyIsMin);
		}

		/// <summary>
		/// Converts a <see cref="SmartDate"/> to a <see cref="DateTime"/>.
		/// </summary>
		/// <param name="smartDate"></param>
		/// <returns>A new <see cref="System.DateTime"/> instance object.</returns>
		public static DateTime ToDateTime(SmartDate smartDate)
		{
			if (smartDate.IsEmpty)
				throw new Exception("Cannot convert an empty SmartDate to a DateTime");
			else
				return new DateTime(smartDate.Date.Ticks);
		}

		/// <summary>
		/// Converts a <see cref="SmartDate"/> to a <see cref="NullableDateTime"/>.
		/// </summary>
		/// <param name="smartDate">A <see cref="SmartDate"/> instance object.</param>
		/// <returns>A <see cref="NullableDateTime"/> instance object.</returns>
		public static NullableDateTime ToNullableDateTime(SmartDate smartDate)
		{
			if (smartDate.IsEmpty)
				return new NullableDateTime();
			else
				return new NullableDateTime(smartDate.Date);
		}
	}
}
