/***************************************************************************
 *   EditableRoot.cs
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
            //AuthorizationRules.AllowWrite("", "");
        }
        public static bool CanAddObject()
        {
            // TODO: customize to check user role
            //return ApplicationContext.User.IsInRole("");
            return true;
        }
        public static bool CanGetObject()
        {
            // TODO: customize to check user role
            //return ApplicationContext.User.IsInRole("");
            return true;
        }
        public static bool CanEditObject()
        {
            // TODO: customize to check user role
            //return ApplicationContext.User.IsInRole("");
            return true;
        }
        public static bool CanDeleteObject()
        {
            // TODO: customize to check user role
            //return ApplicationContext.User.IsInRole("");
            return true;
        }
        #endregion
        #region Factory Methods
        public static $safeitemrootname$ New$safeitemrootname$()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException( NOT_AUTHORIZED_INSERT );
            return DataPortal.Create<$safeitemrootname$>();
        }

        public static $safeitemrootname$ Get$safeitemrootname$(int id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException(NOT_AUTHORIZED_VIEW);

            return DataPortal.Fetch<$safeitemrootname$>(new Criteria(id));
        }

        public static void Delete$safeitemrootname$(int id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException(NOT_AUTHORIZED_DELETE);
            DataPortal.Delete(new Criteria(id));
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private $safeitemrootname$()
        { /* require use of factory methods */ }

        public override $safeitemrootname$ Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException( NOT_AUTHORIZED_DELETE );
                  
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException( NOT_AUTHORIZED_INSERT );

            else if (!CanEditObject())
                throw new System.Security.SecurityException( NOT_AUTHORIZED_UPDATE );

            return base.Save();
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
        private void DataPortal_Create( Criteria criteria )
        {
            base.DataPortal_Create(criteria);
        }
        private void DataPortal_Fetch( Criteria criteria )
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString))
            {
                cn.Open();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "uspGet$safeitemrootname$ByID";
                    cm.Parameters.AddWithValue("@$safeitemrootname$ID", criteria.Id);

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        dr.Read();
                        this._Id = criteria.Id;
                        //this._SpecialServicerFee = dr.GetFloat("SpecialServicerFee");
                        //this._PrincipalRecoveryFee = dr.GetFloat("PrincipalRecoveryFee");
                        //this._LiquidationFee = dr.GetFloat("LiquidationFee");
                        //this._CorrectedMLFee = dr.GetFloat("CorrectedMLFee");
                        //this._OtherFee = dr.GetFloat("OtherFee");
                        //this._SSFeeAccrualMethod = dr.GetString("SSFeeAccrualMethod");
                    }
                }
            }
            MarkOld();
        }
        protected override void DataPortal_Insert()
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString))
            {
                cn.Open();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = "usp$safeitemrootname$Insert";
                    DoInsertUpdate(cm);
                }
            }
        }
        protected override void DataPortal_Update()
        {
            if (base.IsDirty)
            {
                using (SqlConnection cn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "usp$safeitemrootname$Update";
                        cm.Parameters.AddWithValue("@$safeitemrootname$ID", _id);
                        DoInsertUpdate(cm);
                    }
                }
            }
        }
        private void DoInsertUpdate(SqlCommand cm)
        {
            cm.CommandType = CommandType.StoredProcedure;
            //cm.Parameters.AddWithValue("@SpecialServicerFee", _SpecialServicerFee);
            //cm.Parameters.AddWithValue("@PrincipalRecoveryFee", _PrincipalRecoveryFee);
            //cm.Parameters.AddWithValue("@LiquidationFee", _LiquidationFee);
            //cm.Parameters.AddWithValue("@CorrectedMLFee", _CorrectedMLFee);
            //cm.Parameters.AddWithValue("@OtherFee", _OtherFee);
            //cm.Parameters.AddWithValue("@SSFeeAccrualMethod", _SSFeeAccrualMethod);
            cm.ExecuteNonQuery();
            //_timestamp = (byte[])cm.Parameters["@newLastChanged"].Value;
        }
        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete( new Criteria( _id ) );
        }
        private void DataPortal_Delete( Criteria criteria )
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString))
            {
                cn.Open();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "usp$safeitemrootname$Delete";
                    cm.Parameters.AddWithValue("@$safeitemrootname$ID", criteria.Id);
                    cm.ExecuteNonQuery();
                }
            }
        }
        #endregion
    }
}