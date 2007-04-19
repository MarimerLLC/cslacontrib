using System;
using System.Data;

namespace CslaSrd.Data
{
  /// <summary>
  /// This is an extension of the CSLA Framework SafeDataReader that provides
  /// support for additional "Smart" datatypes.
  /// </summary>
  public class SmartSafeDataReader : Csla.Data.SafeDataReader , IDataReader
  {
    /// <summary>
    /// Initializes the SrdSafeDataReader object to use data from
    /// the provided DataReader object.
    /// </summary>
    /// <param name="dataReader">The source DataReader object containing the data.</param>
    public SmartSafeDataReader(IDataReader dataReader) : base( dataReader)
    {
    }


    #region SmartBool
    /// <summary>
    /// Gets a <see cref="SmartBool" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into min possible value.
    /// See Chapter 5 for more details on the SmartBool class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public CslaSrd.SmartBool GetSmartBool(string name)
    {
        return GetSmartBool(base.DataReader.GetOrdinal(name), true);
    }

    /// <summary>
    /// Gets a <see cref="SmartBool" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into the min possible value.
    /// See Chapter 5 for more details on the SmartBool class.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual CslaSrd.SmartBool GetSmartBool(int i)
    {
        return GetSmartBool(i, true);
    }

    /// <summary>
    /// Gets a <see cref="SmartBool" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into either the min or max possible value
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartBool class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty item.</param>
    public CslaSrd.SmartBool GetSmartBool(string name, bool minIsEmpty)
    {
        return GetSmartBool(base.DataReader.GetOrdinal(name), minIsEmpty);
    }

    /// <summary>
    /// Gets a <see cref="SmartBool"/> from the datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty item.</param>
    public virtual CslaSrd.SmartBool GetSmartBool(
      int i, bool minIsEmpty)
    {
        if (base.DataReader.IsDBNull(i))
            return new CslaSrd.SmartBool(minIsEmpty);
        else
            return new CslaSrd.SmartBool(
              base.DataReader.GetBoolean(i), minIsEmpty);
    }
    #endregion SmartBool


    #region SmartInt16
    /// <summary>
    /// Gets a <see cref="SmartInt16" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into min possible int
    /// See Chapter 5 for more details on the SmartInt16 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public CslaSrd.SmartInt16 GetSmartInt16(string name)
    {
        return GetSmartInt16(base.DataReader.GetOrdinal(name), true);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt16" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into the min possible int
    /// See Chapter 5 for more details on the SmartInt16 class.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual CslaSrd.SmartInt16 GetSmartInt16(int i)
    {
        return GetSmartInt16(i, true);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt16" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into either the min or max possible value
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartInt16 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty value.</param>
    public CslaSrd.SmartInt16 GetSmartInt16(string name, bool minIsEmpty)
    {
        return GetSmartInt16(base.DataReader.GetOrdinal(name), minIsEmpty);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt16"/> from the datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty value.</param>
    public virtual CslaSrd.SmartInt16 GetSmartInt16(
      int i, bool minIsEmpty)
    {
        if (base.DataReader.IsDBNull(i))
            return new CslaSrd.SmartInt16(minIsEmpty);
        else
            return new CslaSrd.SmartInt16(
              base.DataReader.GetInt16(i), minIsEmpty);
    }
    #endregion SmartInt16

    #region SmartInt32
    /// <summary>
    /// Gets a <see cref="SmartInt32" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into min possible int
    /// See Chapter 5 for more details on the SmartInt32 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public CslaSrd.SmartInt32 GetSmartInt32(string name)
    {
        return GetSmartInt32(base.DataReader.GetOrdinal(name), true);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt32" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into the min possible int
    /// See Chapter 5 for more details on the SmartInt32 class.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual CslaSrd.SmartInt32 GetSmartInt32(int i)
    {
      return GetSmartInt32(i, true);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt32" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into either the min or max possible value
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartInt32 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty value.</param>
    public CslaSrd.SmartInt32 GetSmartInt32(string name, bool minIsEmpty)
    {
        return GetSmartInt32(base.DataReader.GetOrdinal(name), minIsEmpty);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt32"/> from the datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty value.</param>
    public virtual CslaSrd.SmartInt32 GetSmartInt32(
      int i, bool minIsEmpty)
    {
        if (base.DataReader.IsDBNull(i))
        return new CslaSrd.SmartInt32(minIsEmpty);
      else
        return new CslaSrd.SmartInt32(
          base.DataReader.GetInt32(i), minIsEmpty);
    }
    #endregion SmartInt32

    #region SmartInt64
    /// <summary>
    /// Gets a <see cref="SmartInt64" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into min possible int
    /// See Chapter 5 for more details on the SmartInt64 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public CslaSrd.SmartInt64 GetSmartInt64(string name)
    {
        return GetSmartInt64(base.DataReader.GetOrdinal(name), true);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt64" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into the min possible int
    /// See Chapter 5 for more details on the SmartInt64 class.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual CslaSrd.SmartInt64 GetSmartInt64(int i)
    {
        return GetSmartInt64(i, true);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt64" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into either the min or max possible value
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartInt64 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty value.</param>
    public CslaSrd.SmartInt64 GetSmartInt64(string name, bool minIsEmpty)
    {
        return GetSmartInt64(base.DataReader.GetOrdinal(name), minIsEmpty);
    }

    /// <summary>
    /// Gets a <see cref="SmartInt64"/> from the datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty value.</param>
    public virtual CslaSrd.SmartInt64 GetSmartInt64(
      int i, bool minIsEmpty)
    {
        if (base.DataReader.IsDBNull(i))
            return new CslaSrd.SmartInt64(minIsEmpty);
        else
            return new CslaSrd.SmartInt64(
              base.DataReader.GetInt64(i), minIsEmpty);
    }
    #endregion SmartInt64

    #region SmartFloat
    /// <summary>
    /// Gets a <see cref="SmartFloat" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into min possible value.
    /// See Chapter 5 for more details on the SmartFloat class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public CslaSrd.SmartFloat GetSmartFloat(string name)
    {
        return GetSmartFloat(base.DataReader.GetOrdinal(name), true);
    }

    /// <summary>
    /// Gets a <see cref="SmartFloat" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into the min possible value.
    /// See Chapter 5 for more details on the SmartFloat class.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual CslaSrd.SmartFloat GetSmartFloat(int i)
    {
        return GetSmartFloat(i, true);
    }

    /// <summary>
    /// Gets a <see cref="SmartFloat" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into either the min or max possible value
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartFloat class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty item.</param>
    public CslaSrd.SmartFloat GetSmartFloat(string name, bool minIsEmpty)
    {
        return GetSmartFloat(base.DataReader.GetOrdinal(name), minIsEmpty);
    }

    /// <summary>
    /// Gets a <see cref="SmartFloat"/> from the datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty item.</param>
    public virtual CslaSrd.SmartFloat GetSmartFloat(
      int i, bool minIsEmpty)
    {
        if (base.DataReader.IsDBNull(i))
            return new CslaSrd.SmartFloat(minIsEmpty);
        else
                return new CslaSrd.SmartFloat(
                  base.DataReader.GetFloat(i), minIsEmpty);
    }
    #endregion SmartFloat


    #region SmartDecimal
    /// <summary>
    /// Gets a <see cref="SmartDecimal" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into min possible value.
    /// See Chapter 5 for more details on the SmartDecimal class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public CslaSrd.SmartDecimal GetSmartDecimal(string name)
    {
        return GetSmartDecimal(base.DataReader.GetOrdinal(name), true);
    }

    /// <summary>
    /// Gets a <see cref="SmartDecimal" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into the min possible value.
    /// See Chapter 5 for more details on the SmartDecimal class.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual CslaSrd.SmartDecimal GetSmartDecimal(int i)
    {
        return GetSmartDecimal(i, true);
    }

    /// <summary>
    /// Gets a <see cref="SmartDecimal" /> from the datareader.
    /// </summary>
    /// <remarks>
    /// A null is converted into either the min or max possible value
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartDecimal class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty item.</param>
    public CslaSrd.SmartDecimal GetSmartDecimal(string name, bool minIsEmpty)
    {
        return GetSmartDecimal(base.DataReader.GetOrdinal(name), minIsEmpty);
    }

    /// <summary>
    /// Gets a <see cref="SmartDecimal"/> from the datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty item.</param>
    public virtual CslaSrd.SmartDecimal GetSmartDecimal(
      int i, bool minIsEmpty)
    {
        if (base.DataReader.IsDBNull(i))
            return new CslaSrd.SmartDecimal(minIsEmpty);
        else
            return new CslaSrd.SmartDecimal(
              base.DataReader.GetDecimal(i), minIsEmpty);
    }
    #endregion SmartDecimal

}
}
