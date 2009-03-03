// Functions.cs - functionality for the Automatic Attribute with fuzzy categories box
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.cz>
//
// Copyright (c) 2009 Martin Ralbovský
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
using System.Text;
using Ferda.Guha.Data;
using Ice;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.AutomaticAttributeWithFuzzyCategories
{
    /// <summary>
    /// Class is providing ICE functionality of the SampleBoxModule
    /// box module
    /// </summary>
    public class Functions : FuzzyAttributeFunctionsDisp_, Ferda.Modules.IFunctions
    {
        #region Fields

        protected Ferda.Modules.BoxModuleI _boxModule;
        protected Ferda.Modules.Boxes.IBoxInfo _boxInfo;

        #endregion

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
        {
            this._boxModule = boxModule;
            this._boxInfo = boxInfo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The GUID (unique identifier) of the attribute
        /// </summary>
        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the proxy of the connected column
        /// </summary>
        /// <param name="fallOnError">If the function should throw an exception on error</param>
        /// <returns>Proxy of the connected column</returns>
        public ColumnFunctionsPrx GetColumnFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<ColumnFunctionsPrx>(
                _boxModule,
                Public.SockColumn,
                ColumnFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        #endregion

        #region ICE functions

        /// <summary>
        /// Gets the name of the column from which the attribute and bit string generator
        /// is created. Added for PMML purposes. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns></returns>
        public string GetColumnName(Current current__)
        {
            return GetColumnFunctionsPrx(true).getColumnInfo().columnSelectExpression;
        }

        /// <summary>
        /// Returns information from the column about the values and frequencies
        /// of the column. This fucntion was added to the Slice desing for
        /// the PMML support.
        /// </summary>
        /// <returns>ValuesAndFrequencies structure</returns>
        public ValuesAndFrequencies GetColumnValuesAndFrequencies(Current current__)
        {
            return GetColumnFunctionsPrx(true).getDistinctsAndFrequencies();
        }

        /// <summary>
        /// Returns the identification of the attribute.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Identification of the attribute</returns>
        public GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        public override string HelloWorld(Ice.Current __current)
        {
            return "Hello World!";
        }

        #endregion
    }
}
