// BodyMassIndexFunctionsI.cs - functions object for body mass index box module
//
// Author: TomĂˇĹˇ KuchaĹ™ <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 TomĂˇĹˇ KuchaĹ™
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
using Ferda.Guha.Data;
using Ferda.Modules.Boxes.DataPreparation;
using Ice;
using Exception=System.Exception;

namespace Ferda.Modules.Boxes.Sample.BodyMassIndex
{
    /// <summary>
    /// This is an implementation of slice design of Body Mass Indes box module.
    /// </summary>
    /// <remarks>
    /// To implement the BMI slice design you have to inherit the abstract class
    /// <see cref="T:Ferda.Modules.Boxes.Sample.BodyMassIndex.BodyMassIndexFunctionsDisp_"/>
    /// which was generated from slice design.
    /// To implement the functions object you has to implement 
    /// the <see cref="T:Ferda.Modules.IFunctions"/> interface.
    /// Now the implementation of box module`s functios is complete.
    /// </remarks>
    public class BodyMassIndexFunctionsI : BodyMassIndexFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The <see cref="T:Ferda.Modules.BoxModuleI"/> class. This
        /// represents current instance of the BMI box module. I.e. all
        /// box module settings like sockets connections or properties values
        /// are holded by the _boxModule.
        /// </summary>
        protected BoxModuleI _boxModule;

        /// <summary>
        /// The <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>
        /// class.
        /// </summary>
        /// <remarks>
        /// Please note that all instances of the Body Mass Index 
        /// box module (all BMI box instances) share one instance 
        /// of the BoxInfo.
        /// </remarks>
        protected IBoxInfo boxInfo;

        #region IFunctions Members

        void IFunctions.setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
            this.boxInfo = boxInfo;
        }

        #endregion

        /// <summary>
        /// Gets the height units.
        /// </summary>
        /// <value>The height units.</value>
        public string HeightUnits
        {
            get { return _boxModule.GetPropertyString("HeightUnits"); }
        }

        /// <summary>
        /// Gets the weight units.
        /// </summary>
        /// <value>The weight units.</value>
        public string WeightUnits
        {
            get { return _boxModule.GetPropertyString("WeightUnits"); }
        }

        /// <summary>
        /// Gets the column functions proxy from specified <c>socketName</c>.
        /// </summary>
        /// <param name="fallOnError">if set to <c>true</c> exception is thrown on error otherwise null is returned.</param>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>
        /// Proxy of box module providing column functions connected
        /// to the specified <c>socketName</c>.
        /// </returns>
        private ColumnFunctionsPrx GetColumnFunctionsPrx(bool fallOnError, string socketName)
        {
            // gets column functionsObjectI if any connected; 
            // otherwise, null or throws Ferda.Modules.NoConnectionInSocketError
            return SocketConnections.GetPrx<ColumnFunctionsPrx>(
                _boxModule,
                socketName,
                ColumnFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        private void getColumnSelectExpression(bool fallOnError, out string weightInKg, out string heightInM)
        {
            string heightInMeters = null;
            string weightInKilograms = null;
            ExceptionsHandler.GetResult<int>(
                fallOnError,
                delegate
                    {
                        ColumnFunctionsPrx heightColumnPrx = GetColumnFunctionsPrx(fallOnError, "Height");
                        ColumnFunctionsPrx weightColumnPrx = GetColumnFunctionsPrx(fallOnError, "Weight");
                        if (heightColumnPrx != null && weightColumnPrx != null)
                        {
                            ColumnInfo weightColumn = weightColumnPrx.getColumnInfo();
                            ColumnInfo heightColumn = heightColumnPrx.getColumnInfo();

                            // test if both columns come from the same datamatrix
                            if (
                                testColumnsAreFromSameDataMatrix(fallOnError, heightColumn, weightColumn)
                                )
                            {
                                // build the select expression
                                // BMI equals a person's weight in kilograms divided by height in meters squared (BMI=kg/m^2).

                                heightInMeters = convertToMeters(heightColumn.columnSelectExpression, HeightUnits);
                                weightInKilograms = convertToKilograms(weightColumn.columnSelectExpression, WeightUnits);
                            }
                        }
                        return 0;
                    },
                delegate
                    {
                        return 0;
                    },
                _boxModule.StringIceIdentity
                );
            heightInM = heightInMeters;
            weightInKg = weightInKilograms;
            return;
        }

        /// <summary>
        /// Gets the default user label.
        /// </summary>
        /// <returns>Pseudoformula introducing BMI theorem.</returns>
        public string GetDefaultUserLabel(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string>(
                fallOnError,
                delegate
                    {
                        string weightInKilograms;
                        string heightInMeters;
                        getColumnSelectExpression(fallOnError, out weightInKilograms, out heightInMeters);
                        if (weightInKilograms != null && heightInMeters != null)
                            return "(" + weightInKilograms + ") / (" + heightInMeters + ")^2";
                        return String.Empty;
                    },
                delegate
                    {
                        return String.Empty;
                    },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Tests if the columns are from the same data matrix.
        /// </summary>
        /// <param name="fallOnError">if set to <c>true</c> exception is thrown on error otherwise null is returned.</param>
        /// <param name="firstColumn">The first column.</param>
        /// <param name="secondColumn">The second column.</param>
        /// <returns></returns>
        private bool testColumnsAreFromSameDataMatrix(bool fallOnError, ColumnInfo firstColumn, ColumnInfo secondColumn)
        {
            DatabaseConnectionSettingHelper tmp1 =
                new DatabaseConnectionSettingHelper(firstColumn.dataTable.databaseConnectionSetting);
            DatabaseConnectionSettingHelper tmp2 =
                new DatabaseConnectionSettingHelper(secondColumn.dataTable.databaseConnectionSetting);
            if (
                !tmp1.Equals(tmp2)
                || firstColumn.dataTable.dataTableName != secondColumn.dataTable.dataTableName
                )
                if (fallOnError)
                    throw Exceptions.BadValueError(
                        null,
                        _boxModule.StringIceIdentity,
                        "Both input columns has to come from the same datamatrix",
                        new string[] {"HeightColumn", "WeightColumn"},
                        restrictionTypeEnum.OtherReason
                        );
                else
                    return false;
            return true;
        }

        /// <summary>
        /// Converts <see cref="P:Ferda.Modules.Boxes.Sample.BodyMassIndex.BodyMassIndexFunctionsI.GetHeightColumn"/> 
        /// <see cref="P:Ferda.Modules.Boxes.Sample.BodyMassIndex.BodyMassIndexFunctionsI.HeightUnits">units</see> to meters.
        /// </summary>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="columnUnits">The column units.</param>
        /// <returns>Height (in meters) column select expression.</returns>
        private string convertToMeters(string columnSelectExpression, string columnUnits)
        {
            switch (columnUnits)
            {
                case "Meter":
                    return columnSelectExpression;
                case "Centimeter":
                    return columnSelectExpression + "/100";
                case "Milimeter":
                    return columnSelectExpression + "/1000";
                case "Mile":
                    {
                        double multiplicator = 1609.344;
                        return columnSelectExpression + "*" + multiplicator.ToString();
                    }
                case "Yard":
                    {
                        double multiplicator = 0.9144;
                        return columnSelectExpression + "*" + multiplicator.ToString();
                    }
                case "Foot":
                    {
                        double multiplicator = 0.3048;
                        return columnSelectExpression + "*" + multiplicator.ToString();
                    }
                case "Inch":
                    {
                        double multiplicator = 0.0254;
                        return columnSelectExpression + "*" + multiplicator.ToString();
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Converts <see cref="P:Ferda.Modules.Boxes.Sample.BodyMassIndex.BodyMassIndexFunctionsI.WeightColumn"/> 
        /// <see cref="P:Ferda.Modules.Boxes.Sample.BodyMassIndex.BodyMassIndexFunctionsI.WeightUnits">units</see> to kilograms.
        /// </summary>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="columnUnits">The column units.</param>
        /// <returns>Weight (in kilograms) column select expression.</returns>
        private string convertToKilograms(string columnSelectExpression, string columnUnits)
        {
            switch (columnUnits)
            {
                case "Kilogram":
                    return columnSelectExpression;
                case "Gram":
                    return columnSelectExpression + "/1000";
                case "Pound":
                    {
                        double multiplicator = 0.45359237;
                        return columnSelectExpression + "*" + multiplicator.ToString();
                    }
                case "Ounce":
                    {
                        double multiplicator = 0.028349523125;
                        return columnSelectExpression + "*" + multiplicator.ToString();
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        /*
        /// <summary>
        /// Provides functionality of manually prepared attribute box, where is connected
        /// BMI column. There are created basic BMI categories (see below) in the result.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        /// <term>[kg/m^2]</term>
        /// <description><b>en-US</b>; cs-CZ</description>
        /// </listheader>
        /// <item>
        /// <term>&lt; 15 </term>
        /// <description><b>Extremely underweight</b>; vyzĂˇblĂˇ postava</description>
        /// </item>
        /// <item>
        /// <term>15 - 20</term>
        /// <description><b>Underweight</b>; hubenĂˇ postava</description>
        /// </item>
        /// <item>
        /// <term>20 - 25</term>
        /// <description><b>Normal</b>; normĂˇlnĂ­ postava</description>
        /// </item>
        /// <item>
        /// <term>25 - 30</term>
        /// <description><b>Overweight</b>; postava s nadvĂˇhou</description>
        /// </item>
        /// <item>
        /// <term>&gt; 30</term>
        /// <description><b>Obese</b>; obĂ©znĂ­ postava</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override string getAttribute(Current current__)
        {
            AbstractAttributeStruct result = new AbstractAttributeStruct();

            CategoriesStruct categories = new CategoriesStruct();
            categories.floatIntervals = new FloatIntervalCategorySeq();
            bool isLocalized;
            categories.floatIntervals.Add(
                _boxModule.GetPhrase("ExtremelyUnderweight", out isLocalized),
                new FloatIntervalStruct[]
                    {
                        new FloatIntervalStruct(
                            BoundaryEnum.Infinity,
                            BoundaryEnum.Round,
                            0,
                            15)
                    }
                );
            categories.floatIntervals.Add(
                _boxModule.GetPhrase("Underweight", out isLocalized),
                new FloatIntervalStruct[]
                    {
                        new FloatIntervalStruct(
                            BoundaryEnum.Sharp,
                            BoundaryEnum.Round,
                            15,
                            20)
                    }
                );
            categories.floatIntervals.Add(
                _boxModule.GetPhrase("Normal", out isLocalized),
                new FloatIntervalStruct[]
                    {
                        new FloatIntervalStruct(
                            BoundaryEnum.Sharp,
                            BoundaryEnum.Round,
                            20,
                            25)
                    }
                );
            categories.floatIntervals.Add(
                _boxModule.GetPhrase("Overweight", out isLocalized),
                new FloatIntervalStruct[]
                    {
                        new FloatIntervalStruct(
                            BoundaryEnum.Sharp,
                            BoundaryEnum.Round,
                            25,
                            30)
                    }
                );
            categories.floatIntervals.Add(
                _boxModule.GetPhrase("Obese", out isLocalized),
                new FloatIntervalStruct[]
                    {
                        new FloatIntervalStruct(
                            BoundaryEnum.Sharp,
                            BoundaryEnum.Infinity,
                            30,
                            float.MaxValue)
                    }
                );

            result.categories = categories;
            result.column = getColumnInfo();
            result.countOfCategories = result.categories.floatIntervals.Count;
            result.identifier = _boxModule.PersistentIdentity;
            result.includeNullCategory = "";
            result.nameInLiterals = "BodyMassIndex";
            result.xCategory = "";
            return result;
        }
        */

        /// <summary>
        /// Gets the column info.
        /// </summary>
        /// <param name="current__">The current__.</param>
        /// <returns>Basic information about the column.</returns>
        public override ColumnInfo getColumnInfo(Current current__)
        {
            ColumnInfo result = new ColumnInfo();

            // locks all sockets and properties of current and (recursively) all souce boxes
            _boxModule.Manager.getBoxModuleLocker().lockBoxModule(_boxModule.StringIceIdentity);

            try
            {
                string weightInKilograms;
                string heightInMeters;
                getColumnSelectExpression(true, out weightInKilograms, out heightInMeters);
                return new ColumnInfo(
                    GetColumnFunctionsPrx(true, "Height").getColumnInfo().dataTable,
                    "(" + weightInKilograms + ") / (" + heightInMeters + " * " + heightInMeters + ")",
                    DbDataTypeEnum.DoubleType,
                    Guha.Data.CardinalityEnum.Cardinal
                    );
            }
            finally
            {
                // unlock (recursively) locked boxes
                _boxModule.Manager.getBoxModuleLocker().unlockBoxModule(_boxModule.StringIceIdentity);
            }
        }

        /// <summary>
        /// Gets the column statistics.
        /// </summary>
        /// <param name="current__">The current__.</param>
        /// <returns></returns>
        public override ColumnStatistics getColumnStatistics(Current current__)
        {
            //TODO
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets distinct values of the BMI column.
        /// </summary>
        /// <param name="current__">The ICE current.</param>
        /// <returns>Distinct values of the BMI column.</returns>
        public override ValuesAndFrequencies getDistinctsAndFrequencies(Current current__)
        {
            //TODO
            throw new Exception("The method or operation is not implemented.");
        }
    }
}