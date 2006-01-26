using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;

namespace Ferda.Modules.Boxes.Sample.BodyMassIndex
{
    public class BodyMassIndexFunctionsI : BodyMassIndexFunctionsDisp_, Ferda.Modules.IFunctions
    {
        protected Ferda.Modules.BoxModuleI boxModule;
        protected Ferda.Modules.Boxes.IBoxInfo boxInfo;

        #region IFunctions Members

        void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
        {
            this.boxModule = boxModule;
            this.boxInfo = boxInfo;
        }

        #endregion


        public string HeightUnits
        {
            get
            {
                return boxModule.GetPropertyString("HeightUnits");
            }
        }

        public string WeightUnits
        {
            get
            {
                return boxModule.GetPropertyString("WeightUnits");
            }
        }

        public ColumnFunctionsPrx HeightColumn
        {
            get
            {
                return getColumnFunctionsPrx("Height");
            }
        }

        public ColumnFunctionsPrx WeightColumn
        {
            get
            {
                return getColumnFunctionsPrx("Weight");
            }
        }

        private ColumnFunctionsPrx getColumnFunctionsPrx(string socketName)
        {
            Ice.ObjectPrx otherObjectPrx;
            if (!Ferda.Modules.Boxes.SocketConnections.TryGetObjectPrx(boxModule, socketName, out otherObjectPrx))
                return null;
            else
                return ColumnFunctionsPrxHelper.checkedCast(otherObjectPrx);
        }

        public override Ferda.Modules.Boxes.DataMiningCommon.Column.ColumnStruct getColumn(Ice.Current current__)
        {
            // locks all sockets and properties of current and (recursively) all souce boxes
            boxModule.Manager.getBoxModuleLocker().lockBoxModule(boxModule.StringIceIdentity);

            ColumnStruct heightColumn = HeightColumn.getColumn();
            ColumnStruct weightColumn = WeightColumn.getColumn();

            // test if both columns come from the same datamatrix
            if (heightColumn.dataMatrix.database.connectionString != weightColumn.dataMatrix.database.connectionString
                || heightColumn.dataMatrix.dataMatrixName != weightColumn.dataMatrix.dataMatrixName)
                throw Ferda.Modules.Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "both input columns has to come from the same datamatrix",
                    new string[] { "Height", "Weight" },
                    Ferda.Modules.restrictionTypeEnum.Other
                    );

            // build the select expression
            // BMI equals a person's weight in kilograms divided by height in meters squared (BMI=kg/m^2).

            string heightInMeters = convertToMeters(heightColumn.columnSelectExpression, HeightUnits);
            string weightInKilograms = convertToKilograms(weightColumn.columnSelectExpression, WeightUnits);

            ColumnStruct result = new ColumnStruct();
            result.columnSelectExpression = "(" + weightInKilograms + ") / ((" + heightInMeters + ") * (" + heightInMeters + "))";
            result.columnSubType = Ferda.Modules.ValueSubTypeEnum.FloatType;
            result.columnType = ColumnTypeEnum.Derived;
            result.dataMatrix = heightColumn.dataMatrix; // or equivalent weightColumn.dataMatrix

            // unlock (recursively) locked boxes
            boxModule.Manager.getBoxModuleLocker().unlockBoxModule(boxModule.StringIceIdentity);

            return result;
        }

        private string convertToMeters(string columnSelectExpression, string columnUnits)
        {
            switch (columnUnits)
            {
                case "Meter":
                    return columnSelectExpression;
                case "Cenimeter":
                    return columnSelectExpression + "/100";
                case "Milimeter":
                    return columnSelectExpression + "/1000";
                case "Foot":
                    return columnSelectExpression + "/1000"; //TODO
                case "Inch":
                    return columnSelectExpression + "/1000"; //TODO
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(columnUnits);
            }
        }

        private string convertToKilograms(string columnSelectExpression, string columnUnits)
        {
            switch (columnUnits)
            {
                case "Kilogram":
                    return columnSelectExpression;
                case "Gram":
                    return columnSelectExpression + "/1000";
                case "Pound":
                    return columnSelectExpression + "/1000"; //TODO
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(columnUnits);
            }
        }

        public override string[] getDistinctValues(Ice.Current current__)
        {
            ColumnStruct bmiColumn = this.getColumn();
            return Ferda.Modules.Helpers.Data.Column.GetDistinctsStringSeq(
                bmiColumn.dataMatrix.database.connectionString,
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
                    new Ferda.Modules.FloatIntervalStruct(Ferda.Modules.BoundaryEnum.Infinity, Ferda.Modules.BoundaryEnum.Round, 0, 15)
                    }
                );
            categories.floatIntervals.Add(
                boxModule.GetPhrase("Underweight", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(Ferda.Modules.BoundaryEnum.Sharp, Ferda.Modules.BoundaryEnum.Round, 15, 20)
                    }
                );
            categories.floatIntervals.Add(
                boxModule.GetPhrase("Normal", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(Ferda.Modules.BoundaryEnum.Sharp, Ferda.Modules.BoundaryEnum.Round, 20, 25)
                    }
                );
            categories.floatIntervals.Add(
                boxModule.GetPhrase("Overweight", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(Ferda.Modules.BoundaryEnum.Sharp, Ferda.Modules.BoundaryEnum.Round, 25, 30)
                    }
                );
            categories.floatIntervals.Add(
                boxModule.GetPhrase("Obese", out isLocalized),
                new Ferda.Modules.FloatIntervalStruct[] {
                    new Ferda.Modules.FloatIntervalStruct(Ferda.Modules.BoundaryEnum.Sharp, Ferda.Modules.BoundaryEnum.Infinity, 30, float.MaxValue)
                    }
                );

            result.categories = categories;
            result.column = this.getColumn();
            result.countOfCategories = result.categories.floatIntervals.Count;
            result.identifier = boxModule.PersistentIdentity;
            result.includeNullCategory = "";
            result.nameInLiterals = "BodyMassIndex";
            result.xCategory = "";
            return result;
        }
    }
}
