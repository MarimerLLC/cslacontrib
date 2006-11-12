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
    /// A null is converted into either the min or max possible date
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartInt16 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty date.</param>
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
    /// value of a data means an empty date.</param>
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
    /// A null is converted into either the min or max possible date
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartInt32 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty date.</param>
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
    /// value of a data means an empty date.</param>
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
    /// A null is converted into either the min or max possible date
    /// depending on the MinIsEmpty parameter. See Chapter 5 for more
    /// details on the SmartInt64 class.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="minIsEmpty">
    /// A flag indicating whether the min or max 
    /// value of a data means an empty date.</param>
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
    /// value of a data means an empty date.</param>
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

}
}
