using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;

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
    public class BodyMassIndexFunctionsI : BodyMassIndexFunctionsDisp_, Ferda.Modules.IFunctions
    {
        /// <summary>
        /// The <see cref="T:Ferda.Modules.BoxModuleI"/> class. This
        /// represents current instance of the BMI box module. I.e. all
        /// box module settings like sockets connections or properties values
        /// are holded by the boxModule.
        /// </summary>
        protected Ferda.Modules.BoxModuleI boxModule;

        /// <summary>
        /// The <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>
        /// class.
        /// </summary>
        /// <remarks>
        /// Please note that all instances of the Body Mass Index 
        /// box module (all BMI box instances) share one instance 
        /// of the BoxInfo.
        /// </remarks>
        protected Ferda.Modules.Boxes.IBoxInfo boxInfo;

        #region IFunctions Members

        void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
        {
            this.boxModule = boxModule;
            this.boxInfo = boxInfo;
        }

        #endregion


        /// <summary>
        /// Gets the height units.
        /// </summary>
        /// <value>The height units.</value>
        public string HeightUnits
        {
            get
            {
                return boxModule.GetPropertyString("HeightUnits");
            }
        }

        /// <summary>
        /// Gets the weight units.
        /// </summary>
        /// <value>The weight units.</value>
        public string WeightUnits
        {
            get
            {
                return boxModule.GetPropertyString("WeightUnits");
            }
        }

        /// <summary>
        /// Gets the height column.
        /// </summary>
        /// <value>The height column.</value>
        public ColumnFunctionsPrx HeightColumn
        {
            get
            {
                return getColumnFunctionsPrx("Height");
            }
        }

        /// <summary>
        /// Gets the weight column.
        /// </summary>
        /// <value>The weight column.</value>
        public ColumnFunctionsPrx WeightColumn
        {
            get
            {
                return getColumnFunctionsPrx("Weight");
            }
        }

        /// <summary>
        /// Gets the column functions proxy from specified <c>socketName</c>.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>
        /// Proxy of box module providing column functions connected 
        /// to the specified <c>socketName</c>.
        /// </returns>
        private ColumnFunctionsPrx getColumnFunctionsPrx(string socketName)
        {
            //gets column functionsObjectI if any connected; otherwise, throws Ferda.Modules.NoConnectionInSocketError
            Ice.ObjectPrx otherObjectPrx = Ferda.Modules.Boxes.SocketConnections.GetObjectPrx(boxModule, socketName);
            return ColumnFunctionsPrxHelper.checkedCast(otherObjectPrx);
        }

        /// <summary>
        /// Gets the default user label.
        /// </summary>
        /// <returns>Pseudoformula introducing BMI theorem.</returns>
        public string GetDefaultUserLabel()
        {
            try
            {
                ColumnInfo heightColumn = HeightColumn.getColumnInfo();
                ColumnInfo weightColumn = WeightColumn.getColumnInfo();

                // test if both columns come from the same datamatrix
                testColumnsAreFromSameDataMatrix(heightColumn, weightColumn);

                // build the select expression
                // BMI equals a person's weight in kilograms divided by height in meters squared (BMI=kg/m^2).

                string heightInMeters = convertToMeters(heightColumn.columnSelectExpression, HeightUnits);
                string weightInKilograms = convertToKilograms(weightColumn.columnSelectExpression, WeightUnits);

                return "(" + weightInKilograms + ") / (" + heightInMeters + ")^2";
            }
            catch { }
            return String.Empty;
        }

        /// <summary>
        /// Tests if the columns are from the same data matrix.
        /// </summary>
        /// <param name="firstColumn">The first column.</param>
        /// <param name="secondColumn">The second column.</param>
        private void testColumnsAreFromSameDataMatrix(ColumnInfo firstColumn, ColumnInfo secondColumn)
        {
            if (firstColumn.dataMatrix.database.odbcConnectionString != secondColumn.dataMatrix.database.odbcConnectionString
                || firstColumn.dataMatrix.dataMatrixName != secondColumn.dataMatrix.dataMatrixName)
                throw Ferda.Modules.Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "both input columns has to come from the same datamatrix",
                    new string[] { "HeightColumn", "WeightColumn" },
                    Ferda.Modules.restrictionTypeEnum.Other
                    );
        }

        /// <summary>
        /// Gets the column info.
        /// </summary>
        /// <param name="current__">The current__.</param>
        /// <returns>Basic information about the column.</returns>
        public override Ferda.Modules.Boxes.DataMiningCommon.Column.ColumnInfo getColumnInfo(Ice.Current current__)
        {
            ColumnInfo result = new ColumnInfo();
            Exception e = null;

            // locks all sockets and properties of current and (recursively) all souce boxes
            boxModule.Manager.getBoxModuleLocker().lockBoxModule(boxModule.StringIceIdentity);

            try
            {

                ColumnInfo heightColumn = HeightColumn.getColumnInfo();
                ColumnInfo weightColumn = WeightColumn.getColumnInfo();

                // test if both columns come from the same datamatrix
                testColumnsAreFromSameDataMatrix(heightColumn, weightColumn);

                // build the select expression
                // BMI equals a person's weight in kilograms divided by height in meters squared (BMI=kg/m^2).

                string heightInMeters = convertToMeters(heightColumn.columnSelectExpression, HeightUnits);
                string weightInKilograms = convertToKilograms(weightColumn.columnSelectExpression, WeightUnits);

                result.columnSelectExpression = "(" + weightInKilograms + ") / ((" + heightInMeters + ") * (" + heightInMeters + "))";
                result.columnSubType = Ferda.Modules.ValueSubTypeEnum.FloatType;
                result.columnType = ColumnTypeEnum.Derived;
                result.dataMatrix = heightColumn.dataMatrix; // or equivalent weightColumn.dataMatrix
                
                //for fully implementation there should be full intialization of statistics
                /*
                result.statistics = Ferda.Modules.Helpers.Data.Column.GetStatistics(
                    result.dataMatrix.database.odbcConnectionString, 
                    result.dataMatrix.dataMatrixName, 
                    result.columnSelectExpression, 
                    ValueSubTypeEnum.FloatType, 
                    boxModule.StringIceIdentity
                    );
                 */
                //but getting this information is quite slow, so it should be cached
                //please see: Ferda.Modules.Helpers.Caching.Cache
                result.statistics = new StatisticsInfo();
            }
            catch (Ferda.Modules.NoConnectionInSocketError ex)
            {
                e = ex;
            }
            catch (Ferda.Modules.BoxRuntimeError ex)
            {
                e = ex;
            }
            catch (Exception ex)
            {
                e = Ferda.Modules.Exceptions.BoxRuntimeError(ex, this.boxModule.StringIceIdentity, null);
            }
            finally
            {
                // unlock (recursively) locked boxes
                boxModule.Manager.getBoxModuleLocker().unlockBoxModule(boxModule.StringIceIdentity);
            }

            if (e != null)
                throw e;

            return result;
        }

        /// <summary>
        /// Converts <see cref="P:Ferda.Modules.Boxes.Sample.BodyMassIndex.BodyMassIndexFunctionsI.HeightColumn"/> 
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
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(columnUnits);
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
                        double multiplicator = 0.45359237; //TODO
                        return columnSelectExpression + "*" + multiplicator.ToString();
                    }
                case "Ounce":
                    {
                        double multiplicator = 0.028349523125;
                        return columnSelectExpression + "*" + multiplicator.ToString();
                    }
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(columnUnits);
            }
        }

        /// <summary>
        /// Gets distinct values of the BMI column.
        /// </summary>
        /// <param name="current__">The ICE current.</param>
        /// <returns>Distinct values of the BMI column.</returns>
        public override string[] getDistinctValues(Ice.Current current__)
        {
            ColumnInfo bmiColumn = this.getColumnInfo();
            return Ferda.Modules.Helpers.Data.Column.GetDistinctsStringSeq(
                bmiColumn.dataMatrix.database.odbcConnectionString,
                bmiColumn.dataMatrix.dataMatrixName,
                bmiColumn.columnSelectExpression,
                boxModule.StringIceIdentity);
        }

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
        /// <description><b>Extremely underweight</b>; vyzáblá postava</description>
        /// </item>
        /// <item>
        /// <term>15 - 20</term>
        /// <description><b>Underweight</b>; hubená postava</description>
        /// </item>
        /// <item>
        /// <term>20 - 25</term>
        /// <description><b>Normal</b>; normální postava</description>
        /// </item>
        /// <item>
        /// <term>25 - 30</term>
        /// <description><b>Overweight</b>; postava s nadváhou</description>
        /// </item>
        /// <item>
        /// <term>&gt; 30</term>
        /// <description><b>Obese</b>; obézní postava</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeStruct getAbstractAttribute(Ice.Current current__)
        {
            AbstractAttributeStruct result = new AbstractAttributeStruct();

            Ferda.Modules.CategoriesStruct categories = new Ferda.Modules.CategoriesStruct();
            categories.floatIntervals = new Ferda.Modules.FloatIntervalCategorySeq();
            bool isLocalized;
            categories.floatIntervals.Add(
                boxModule.GetPhrase("ExtremelyUnderweight", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(
                        Ferda.Modules.BoundaryEnum.Infinity, 
                        Ferda.Modules.BoundaryEnum.Round, 
                        0, 
                        15)
                    }
                );
            categories.floatIntervals.Add(
                boxModule.GetPhrase("Underweight", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(
                        Ferda.Modules.BoundaryEnum.Sharp, 
                        Ferda.Modules.BoundaryEnum.Round, 
                        15, 
                        20)
                    }
                );
            categories.floatIntervals.Add(
                boxModule.GetPhrase("Normal", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(
                        Ferda.Modules.BoundaryEnum.Sharp, 
                        Ferda.Modules.BoundaryEnum.Round, 
                        20, 
                        25)
                    }
                );
            categories.floatIntervals.Add(
                boxModule.GetPhrase("Overweight", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(
                        Ferda.Modules.BoundaryEnum.Sharp, 
                        Ferda.Modules.BoundaryEnum.Round, 
                        25, 
                        30)
                    }
                );
            categories.floatIntervals.Add(
                boxModule.GetPhrase("Obese", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(
                        Ferda.Modules.BoundaryEnum.Sharp, 
                        Ferda.Modules.BoundaryEnum.Infinity, 
                        30, 
                        float.MaxValue)
                    }
                );

            result.categories = categories;
            result.column = this.getColumnInfo();
            result.countOfCategories = result.categories.floatIntervals.Count;
            result.identifier = boxModule.PersistentIdentity;
            result.includeNullCategory = "";
            result.nameInLiterals = "BodyMassIndex";
            result.xCategory = "";
            return result;
        }
    }
}
