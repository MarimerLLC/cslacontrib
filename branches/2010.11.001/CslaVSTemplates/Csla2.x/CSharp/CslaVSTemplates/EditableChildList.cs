/***************************************************************************
 *   EditableChildList.cs
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
    BusinessListBase<$safeitemrootname$, EditableChild>
    {
        //Friendly name for the business object.
        [NonSerialized]
        const string BUSINESS_OBJECT_NAME = "$safeitemrootname$";

        //replace "configkey" with your key attribute from the application configuration file.
        [NonSerialized]
        const string DATABASE_NAME = "configkey";
        #region Factory Methods
        internal static $safeitemrootname$ New$safeitemrootname$()
        {
            return new $safeitemrootname$();
        }
        internal static $safeitemrootname$ Get$safeitemrootname$(
        SqlDataReader dr )
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
        private void Fetch( SqlDataReader dr )
        {
            RaiseListChangedEvents = false;
            while( dr.Read() )
            {
                this.Add( EditableChild.GetEditableChild( dr ) );
            }
            RaiseListChangedEvents = true;
        }
        internal void Update( object parent )
        {
            RaiseListChangedEvents = false;
            foreach( EditableChild item in DeletedList )
                item.DeleteSelf();
            DeletedList.Clear();
            foreach( EditableChild item in this )
                if( item.IsNew )
                    item.Insert( parent );
                else
                    item.Update( parent );
            RaiseListChangedEvents = true;
        }
        #endregion
    }
}