/***************************************************************************
 *   NameValueList.cs
 *   -------------------
 *   begin                : November 14, 2006
 *   website              : http://www.codeplex.com/CslaVSTemplates
 *   template by          : Keith Smith
 *   email                : mobile5guy@gmail.com
 * 
 *   Change History:
 *      11/14/2006 Created
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using Csla;
using Csla.Data;

namespace $rootnamespace$
{
    [Serializable()]
    public class $safeitemrootname$ : NameValueListBase<int, string>
    {
        //Friendly name for the business object.
        [NonSerialized]
        const string BUSINESS_OBJECT_NAME = "$safeitemrootname$";

        //replace "configkey" with your key attribute from the application configuration file.
        [NonSerialized]
        const string DATABASE_NAME = "configkey";

        #region Factory Methods
        private static $safeitemrootname$ _list;
        public static $safeitemrootname$ Get$safeitemrootname$()
        {
            if( _list == null )
                _list = DataPortal.Fetch<$safeitemrootname$>(
                new Criteria( typeof( $safeitemrootname$ ) ) );
            return _list;
        }
        public static void InvalidateCache()
        {
            _list = null;
        }
        private $safeitemrootname$()
        { /* require use of factory methods */
        }
        #endregion
        #region Data Access
        protected override void DataPortal_Fetch( object criteria )
        {
            RaiseListChangedEvents = false;
            IsReadOnly = false;
            // TODO: load values

            using( SqlConnection cn = new SqlConnection( ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString ) )
            {
                cn.Open();
                using( SqlCommand cm = cn.CreateCommand() )
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "usp$safeitemrootname$ByID";
                    cm.Parameters.AddWithValue( "@ID", criteria.Id );
                    using( SafeDataReader dr = new SafeDataReader( cm.ExecuteReader() ) )
                    {
                        while( dr.Read() )
                        {
                            Add( new NameValueListBase<int, string>.
                            NameValuePair( dr.GetInt32( 0 ), dr.GetString( 1 ) ) );
                        }
                    }
                }
            }

            IsReadOnly = true;
            RaiseListChangedEvents = true;
        }
        #endregion
    }
}