// DatabaseFunctionsI.cs - functions object for database box module
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchař
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace Ferda.Modules.Boxes.DataMiningCommon.Database
{
    class DatabaseFunctionsI : DatabaseFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI boxModule;
        //protected IBoxInfo boxInfo;

        #region IFunctions Members
        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            this.boxModule = boxModule;
            //this.boxInfo = boxInfo;
        }
        #endregion

        #region Properties
        public string ConnectionString
        {
            get
            {
                return this.boxModule.GetPropertyString(DatabaseBoxInfo.OdbcConnectionStringPropertyName);
            }
        }

        public DateTimeTI LastReloadInfo
        {
            get
            {
                DateTimeTI result = (DateTimeTI)(this.boxModule.getProperty(DatabaseBoxInfo.LastReloadInfoPropertyName));
                if (result.year == 0 || result.month == 0 || result.day == 0)
                {
                    result = new DateTimeTI(DateTime.Now);
                    this.boxModule.setProperty(DatabaseBoxInfo.LastReloadInfoPropertyName, result);
                }
                return result;
                //return new DateTimeTI(this.boxModule.PropertiesDateTime[DatabaseBoxInfo.LastReloadInfoPropertyName]);
            }
            set
            {
                this.boxModule.setProperty(DatabaseBoxInfo.LastReloadInfoPropertyName, value);
            }
        }

        public string[] AcceptableTypesOfTables
        {
            get
            {
                //UNDONE
                //return Ferda.Modules.Helpers.Data.Database.GetAllPossibleTableTypes();
                return new string[0];
                //DatabaseBoxInfo.AcceptableTypesOfTablesPropertyName;
            }
        }
        #endregion

        #region Functions
        public override DatabaseInfo getDatabaseInfo(Ice.Current __current)
        {
            return new DatabaseInfo(ConnectionString, LastReloadInfo);
        }

        private ExplainDatabaseStructureCache explainDatabaseStructureCache = new ExplainDatabaseStructureCache();
        public override DataMatrixSchemaInfo[] explain(Ice.Current __current)
        {
            return explainDatabaseStructureCache.Value(boxModule.StringIceIdentity, LastReloadInfo, ConnectionString, AcceptableTypesOfTables);
        }

        private DataMatrixNamesCache dataMatrixNamesCached = new DataMatrixNamesCache();
        public override string[] getDataMatrixNames(Ice.Current __current)
        {
            return dataMatrixNamesCached.Value(boxModule.StringIceIdentity, LastReloadInfo, ConnectionString, AcceptableTypesOfTables);
        }

        private ConnectionInfoCache connectionInfoCache = new ConnectionInfoCache();
        public override ConnectionInfo getConnectionInfo(Ice.Current current__)
        {
            return connectionInfoCache.Value(boxModule.StringIceIdentity, LastReloadInfo, ConnectionString);
        }
        #endregion
    }
}
