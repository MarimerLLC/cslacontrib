/***************************************************************************
 *   ReadOnlyList.cs
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
    public class $safeitemrootname$ :
    ReadOnlyListBase<$safeitemrootname$, ReadOnlyChild>
    {
        //Friendly name for the business object.
        [NonSerialized]
        const string BUSINESS_OBJECT_NAME = "$safeitemrootname$";

        //replace "configkey" with your key attribute from the application configuration file.
        [NonSerialized]
        const string DATABASE_NAME = "configkey";

        #region Authorization Rules
        //Can't use string.Format() on a const.
        [NonSerialized]
        const string NOT_AUTHORIZED_UPDATE = "User not authorized to update " + BUSINESS_OBJECT_NAME + ".";
        [NonSerialized]
        const string NOT_AUTHORIZED_DELETE = "User not authorized to delete " + BUSINESS_OBJECT_NAME + ".";
        [NonSerialized]
        const string NOT_AUTHORIZED_INSERT = "User not authorized to insert " + BUSINESS_OBJECT_NAME + ".";
        [NonSerialized]
        const string NOT_AUTHORIZED_VIEW = "User not authorized to view " + BUSINESS_OBJECT_NAME + ".";

        public static bool CanGetObject()
        {
            // TODO: customize to check user role
            //return ApplicationContext.User.IsInRole("");
            return true;
        }
        #endregion
        #region Factory Methods
        public static $safeitemrootname$ Get$safeitemrootname$( string filter )
        {
            return DataPortal.Fetch<$safeitemrootname$>( new Criteria( filter ) );
        }
        private $safeitemrootname$()
        { /* require use of factory methods */
        }
        #endregion
        #region Data Access
        [Serializable()]
        private class Criteria
        {
            private string _filter;
            public string Filter
            {
                get
                {
                    return _filter;
                }
            }
            public Criteria( string filter )
            {
                _filter = filter;
            }
        }
        private void DataPortal_Fetch( Criteria criteria )
        {
            _id = criteria.Id;

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
                            this.Add( ReadOnlyChild.GetReadOnlyChild( dr ) );
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