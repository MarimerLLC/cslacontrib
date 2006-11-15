/***************************************************************************
 *   CommandObject.cs
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
    public class $safeitemrootname$ : CommandBase
    {
        //Friendly name for the business object.
        [NonSerialized]
        const string BUSINESS_OBJECT_NAME = "$safeitemrootname$";

        //replace "configkey" with your key attribute from the application configuration file.
        [NonSerialized]
        const string DATABASE_NAME = "configkey";

        #region Authorization Methods
        //Can't use string.Format() on a const.
        [NonSerialized]
        const string NOT_AUTHORIZED_UPDATE = "User not authorized to update " + BUSINESS_OBJECT_NAME + ".";
        [NonSerialized]
        const string NOT_AUTHORIZED_DELETE = "User not authorized to delete " + BUSINESS_OBJECT_NAME + ".";
        [NonSerialized]
        const string NOT_AUTHORIZED_INSERT = "User not authorized to insert " + BUSINESS_OBJECT_NAME + ".";
        [NonSerialized]
        const string NOT_AUTHORIZED_VIEW = "User not authorized to view " + BUSINESS_OBJECT_NAME + ".";

        public static bool CanExecuteCommand()
        {
            // TODO: customize to check user role
            //return ApplicationContext.User.IsInRole("");
            return true;
        }
        #endregion
        #region Client-side Code
        // TODO: add your own fields and properties
        bool _result;
        public bool Result
        {
            get
            {
                return _result;
            }
        }
        private void BeforeServer()
        {
            // TODO: implement code to run on client
            // before server is called
        }
        private void AfterServer()
        {
            // TODO: implement code to run on client
            // after server is called
        }
        #endregion
        #region Factory Methods
        public static bool Execute()
        {
            $safeitemrootname$ cmd = new $safeitemrootname$();
            cmd.BeforeServer();
            cmd = DataPortal.Execute<$safeitemrootname$>( cmd );
            cmd.AfterServer();
            return cmd.Result;
        }
        private $rootnamespace$()
        { /* require use of factory methods */
        }
        #endregion
        #region Server-side Code
        protected override void DataPortal_Execute()
        {
            // TODO: implement code to run on server
            // and set result value(s)
            _result = true;
        }
        #endregion
    }
}