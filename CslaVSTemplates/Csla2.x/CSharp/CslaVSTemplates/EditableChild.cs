/***************************************************************************
 *   EditableChild.cs
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
    public class $safeitemrootname$ : BusinessBase<$safeitemrootname$>
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
        public int id
        {
            get
            {
                CanReadProperty( true );
                return _id;
            }
            set
            {
                CanWriteProperty( true );
                if( _id != value )
                {
                    _id = value;
                    PropertyHasChanged();
                }
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
        #region Validation Rules
        protected override void AddBusinessRules()
        {
            //Sample String requirement and lenght Rule.
            //ValidationRules.AddRule(Csla.Validation.CommonRules.StringRequired, "Name");
            //ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs("Name", 50));

            //TemplateTodo: Add sample RegEx code.
        }
        #endregion
        #region Authorization Rules
        protected override void AddAuthorizationRules()
        {
            // TODO: add authorization rules
            //AuthorizationRules.AllowWrite("", "");
        }
        #endregion
        #region Factory Methods
        internal static $safeitemrootname$ New$safeitemrootname$()
        {
            // TODO: change to use new keyword if not loading defaults
            return DataPortal.Create<$safeitemrootname$>();
        }
        internal static $safeitemrootname$ Get$safeitemrootname$( SqlDataReader dr )
        {
            return new $safeitemrootname$( dr );
        }
        private $safeitemrootname$()
        {
            MarkAsChild();
        }
        private $safeitemrootname$( SqlDataReader dr )
        {
            MarkAsChild();
            Fetch( dr );
        }
        #endregion
        #region Data Access
        private void Fetch( SafeDataReader dr )
        {
            _$safeitemrootname$ID = dr.GetInt32( "$safeitemrootname$ID" );
            //... todo add your data access here
            MarkOld();
        }
        internal void Insert($safeitemrootname$List parent)
        {
            using( SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString ) )
            {
                cn.Open();
                using( SqlCommand cm = cn.CreateCommand() )
                {
                    cm.CommandText = "usp$safeitemrootname$Update";
                    DoInsertUpdate( cm );
                }
            }
            MarkOld();
        }
        internal void Update( $safeitemrootname$List parent )
        {
            using( SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString ) )
            {
                cn.Open();
                using( SqlCommand cm = cn.CreateCommand() )
                {
                    cm.CommandText = "usp$safeitemrootname$Insert";
                    cm.Parameters.AddWithValue("@$safeitemrootname$ID", _Id);
                    DoInsertUpdate( cm );
                }
            }
            MarkOld();
        }
        internal void DeleteSelf()
        {
            using( SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString ) )
            {
                cn.Open();
                using( SqlCommand cm = cn.CreateCommand() )
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "usp$safeitemrootname$Delete";
                    cm.Parameters.AddWithValue( "@$safeitemrootname$ID", _TransactionServicerID );
                    cm.ExecuteNonQuery();
                }
            }
            MarkNew();
        }

        private void DoInsertUpdate( SqlCommand cm )
        {
            cm.CommandType = CommandType.StoredProcedure;
            //cm.Parameters.AddWithValue( "@$safeitemrootname$ListID", _TransactionServicerID );
            //todo:
            cm.ExecuteNonQuery();
            //_timestamp = (byte[])cm.Parameters["@newLastChanged"].Value;
        }
        #endregion
    }
}