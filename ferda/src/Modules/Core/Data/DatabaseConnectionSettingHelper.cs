// DatabaseConectionSettingHelper.cs - some added functionality to the 
// database connection setting
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
using Ferda.Modules;

namespace Ferda.Guha.Data
{
    /// <summary>
    /// Encapsulates some new functionality to 
    /// <see cref="T:Ferda.Guha.Data.DatabaseConnectionSetting"/>.
    /// I.e. necessary methods for work with database connection setting
    /// is equality comparation because the setting is used as columnName
    /// in <see cref="T:Ferda.Guha.Data.GenericDataTable"/>). Please note that 
    /// two settings are equal iff provider invariant name and connection
    /// string are equal i.e. equality doesn`t depends on last reload request.
    /// </summary>
    public class DatabaseConnectionSettingHelper : IEquatable<DatabaseConnectionSettingHelper>, IComparable
    {
        private DatabaseConnectionSetting _databaseConnectionSetting;

        #region Properties

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get { return _databaseConnectionSetting.connectionString; }
        }

        /// <summary>
        /// Gets the provider invariant name.
        /// </summary>
        /// <value>The name of the provider invariant.</value>
        public string ProviderInvariantName
        {
            get { return _databaseConnectionSetting.providerInvariantName; }
        }

        /// <summary>
        /// Gets the last reload request.
        /// </summary>
        /// <value>The last reload request.</value>
        public DateTime LastReloadRequest
        {
            get
            {
                DateTime result;
                if (((DateTimeTI) _databaseConnectionSetting.lastReloadRequest).TryGetDateTime(out result))
                    return result;
                else return DateTime.MinValue;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Ferda.Guha.Data.DatabaseConnectionSettingHelper"/> class.
        /// </summary>
        /// <param name="databaseConnectionSetting">The database connection setting.</param>
        public DatabaseConnectionSettingHelper(DatabaseConnectionSetting databaseConnectionSetting)
        {
            _databaseConnectionSetting = databaseConnectionSetting;
        }

        #endregion

        #region Equals, GetHashCode ... for dictionary columnName purposes

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Two database connection settings are equal iff 
        /// the provider invariant name and connection string are equals.
        /// </remarks>
        public override bool Equals(object obj)
        {
            DatabaseConnectionSettingHelper other = obj as DatabaseConnectionSettingHelper;
            if (other != null)
                return ((ProviderInvariantName == other.ProviderInvariantName)
                        && (ConnectionString == other.ConnectionString));
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return ProviderInvariantName.GetHashCode() ^ ConnectionString.GetHashCode();
        }

        #endregion

        /// <summary>
        /// Determines whether the specified object to compare is equal to the current.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the specified object to compare is equal to the current; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Two database connection settings are equal iff
        /// the provider invariant name and connection string are equals.
        /// </remarks>
        public bool Equals(DatabaseConnectionSettingHelper other)
        {
            if (other != null)
                return ((ProviderInvariantName == other.ProviderInvariantName)
                        && (ConnectionString == other.ConnectionString));
            return false;
        }

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// Returns 0 if connection string and provider invariant name are equal.
        /// Last reload request is not considered for comparation. Otherwise return 1.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            DatabaseConnectionSettingHelper other = obj as DatabaseConnectionSettingHelper;
            if (other != null)
                if (Equals(other))
                    return 0;

            return 1;
        }

        #endregion
    }
}