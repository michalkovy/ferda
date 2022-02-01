// Exceptions.cs - functions for defining exceptions concerning data
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
    /// Provides basic method to create BadParamsError exceptions with 
    /// localized message and with specified error type.
    /// </summary>
    public static class Exceptions
    {
        /// <summary>
        /// Gets the db provider invariant name error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// 	<see cref="T:Ferda.Modules.BadParamsError"/> exception.
        /// </returns>
        public static BadParamsError DbProviderInvariantNameError(Exception innnerException, string boxIdentity)
        {
            return Modules.Exceptions.BadParamsError(
                innnerException,
                boxIdentity,
                "Database provider invariant name error. Could not connect to database.",
                restrictionTypeEnum.DbProviderInvariantNameError
                );
        }

        /// <summary>
        /// Gets the db connection string error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// 	<see cref="T:Ferda.Modules.BadParamsError"/> exception.
        /// </returns>
        public static BadParamsError DbConnectionStringError(Exception innnerException, string boxIdentity)
        {
            return Modules.Exceptions.BadParamsError(
                innnerException,
                boxIdentity,
                "Bad connection string error. Could not connect to database.",
                restrictionTypeEnum.DbConnectionStringError
                );
        }

        /// <summary>
        /// Gets the db connection is broken error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// 	<see cref="T:Ferda.Modules.BadParamsError"/> exception.
        /// </returns>
        public static BadParamsError DbConnectionIsBrokenError(Exception innnerException, string boxIdentity)
        {
            return Modules.Exceptions.BadParamsError(
                innnerException,
                boxIdentity,
                "Database connection is broken. Could not connect to database.",
                restrictionTypeEnum.DbConnectionIsBrokenError
                );
        }

        /// <summary>
        /// Gets the db data table name error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// 	<see cref="T:Ferda.Modules.BadParamsError"/> exception.
        /// </returns>
        public static BadParamsError DbDataTableNameError(Exception innnerException, string boxIdentity)
        {
            return Modules.Exceptions.BadParamsError(
                innnerException,
                boxIdentity,
                "Specified data table name does not exist in current database.",
                restrictionTypeEnum.DbDataTableNameError
                );
        }

        /// <summary>
        /// Gets the db column name error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// 	<see cref="T:Ferda.Modules.BadParamsError"/> exception.
        /// </returns>
        public static BadParamsError DbColumnNameError(Exception innnerException, string boxIdentity)
        {
            return Modules.Exceptions.BadParamsError(
                innnerException,
                boxIdentity,
                "Specified column name does not exist in current data table.",
                restrictionTypeEnum.DbColumnNameError
                );
        }

        /// <summary>
        /// Gets the db unique columnName error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// 	<see cref="T:Ferda.Modules.BadValueError"/> exception.
        /// </returns>
        public static BadValueError DbUniqueKeyError(Exception innnerException, string boxIdentity)
        {
            return Modules.Exceptions.BadValueError(
                innnerException,
                boxIdentity,
                "Specified column(s) is not unique key.",
                null,
                restrictionTypeEnum.DbUniqueKeyError
                );
        }

        /// <summary>
        /// Gets the db unexpected error.
        /// </summary>
        /// <param name="innerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// 	<see cref="T:Ferda.Modules.BadParamsError"/> exception.
        /// </returns>
        public static BadParamsError DbUnexpectedError(Exception innerException, string boxIdentity)
        {
            return Modules.Exceptions.BadParamsError(
                innerException,
                boxIdentity,
                "Database unexpected error.",
                restrictionTypeEnum.DbUnexpectedError
                );
        }
    }
}