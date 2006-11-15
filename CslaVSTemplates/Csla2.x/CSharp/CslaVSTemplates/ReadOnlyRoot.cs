/***************************************************************************
 *   ReadOnlyRoot.cs
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
    public class $safeitemrootname$ : ReadOnlyBase<$safeitemrootname$>
    {
        //Friendly name for the business object.
        [NonSerialized]
        const string BUSINESS_OBJECT_NAME = "$safeitemrootname$";

        //replace "configkey" with your key attribute from the application configuration file.
        [NonSerialized]
        const string DATABASE_NAME = "configkey";

        #region Business Methods
        // TODO: add your own fields, properties and methods
        private int _id;
        public int Id
        {
            get
            {
                CanReadProperty( true );
                return _id;
            }
        }
        private string _SampleProperty;
        public string SampleProperty
        {
            get
            {
                CanReadProperty( true );
                return _SampleProperty;
            }
            set
            {
                if( _SampleProperty == value )
                    return;
                CanWriteProperty( true );
                _SampleProperty = value;
                PropertyHasChanged();
            }
        }  
        protected override object GetIdValue()
        {
            return _id;
        }
        #endregion
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

        protected override void AddAuthorizationRules()
        {
            // TODO: add authorization rules
            //AuthorizationRules.AllowRead("", "");
        }
        public static bool CanGetObject()
        {
            // TODO: customize to check user role
            //return ApplicationContext.User.IsInRole("");
            return true;
        }
        #endregion
        #region Factory Methods
        public static $safeitemrootname$ Get$safeitemrootname$( int id )
        {
            return DataPortal.Fetch<$safeitemrootname$>( new Criteria( id ) );
        }
        private $safeitemrootname$()
        { /* require use of factory methods */
        }
        #endregion
        #region Data Access
        [Serializable()]
        private class Criteria
        {
            private int _id;
            public int Id
            {
                get
                {
                    return _id;
                }
            }
            public Criteria( int id )
            {
                _id = id;
            }
        }
        private void DataPortal_Fetch( Criteria criteria )
        {
            // TODO: load values
            _id = criteria.Id;
        }
        #endregion
    }
}