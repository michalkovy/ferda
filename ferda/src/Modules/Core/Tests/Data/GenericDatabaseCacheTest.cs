﻿// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
using System;
using System.Collections.Generic;
using Ferda.Guha.Data;
using Ferda.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardinalityEnum=Ferda.Guha.Data.CardinalityEnum;

namespace Tests.Data
{
    /// <summary>
    ///This is a test class for Ferda.Guha.Data.GenericDatabaseCache and is intended
    ///to contain all Ferda.Guha.Data.GenericDatabaseCache Unit Tests
    ///</summary>
    [TestClass()]
    public class GenericDatabaseCacheTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        /// <summary>
        /// A test for (nearly) whole namespace Ferda.Guha.Data.
        ///</summary>
        [TestMethod()]
        public void NamespaceDataTest()
        {
            // testing data provider helper

            DataProviderHelper.FactoryClassesInvariantNames.Contains(DataProviderHelper.OdbcInvariantName);


            // db connection setup

            string mdbDriver = "Microsoft Access Driver (*.mdb)";
            //string mdbDriver = "Driver do Microsoft Access (*.mdb)";

            string fileName = @"E:\Saves\Projekt\DB\Test.mdb";
            string connectionString = "Driver={" + mdbDriver + "};Dbq=" + fileName + ";Uid=admin;";
            //string connectionString = "DSN=Testovací DB";

            string providerInvariantName = DataProviderHelper.OdbcInvariantName;

            DatabaseConnectionSetting connectionSetting = new DatabaseConnectionSetting(
                providerInvariantName,
                connectionString,
                new DateTimeTI(DateTime.Now)
                );

            DatabaseConnectionSettingHelper connectionSettingHelper =
                new DatabaseConnectionSettingHelper(connectionSetting);

            // testing generic database

            GenericDatabase db = GenericDatabaseCache.GetGenericDatabase(connectionSettingHelper);

            Assert.AreEqual(connectionString, db.ConnectionInfo.connectionString);
            Assert.AreEqual(providerInvariantName, db.ProviderInvariantName);
            Assert.IsTrue(db.DataTablesNames.Length > 1); //test db contains also some system tables

            string[] tmp = db.GetAcceptableDataTablesNames(new List<string>(new string[] {"TABLE"}));
            Assert.IsTrue(Array.IndexOf<string>(tmp, "CABG") >= 0);

            // testing generic data table

            string tableName = "CABG";
            int numberOfRowsInTheTable = 2976;
            GenericDataTable table = db[tableName];
            Assert.AreEqual(30, table.BasicColumnsNames.Length); // number of basic columns in the table
            Assert.AreEqual(tableName, table.Explain.name);
            Assert.AreEqual((long) numberOfRowsInTheTable, table.Explain.recordsCount);
            table.TestUniqueKey(new string[] {"ID"});

            {
                bool failedAsExpected = false;
                try
                {
                    table.TestUniqueKey(new string[] {"AGE", "LV_FUNCT"});
                }
                catch (BadParamsError e)
                {
                    if (e.restrictionType == restrictionTypeEnum.DbUniqueKeyError)
                        failedAsExpected = true;
                }
                Assert.IsTrue(failedAsExpected);
            }

            {
                bool failedAsExpected = false;
                try
                {
                    table.TestUniqueKey(new string[] {"ColumnWitchIsNotInDatabase"});
                }
                catch (BadParamsError e)
                {
                    if (e.restrictionType == restrictionTypeEnum.DbUnexpectedError)
                        failedAsExpected = true;
                }
                Assert.IsTrue(failedAsExpected);
            }

            Assert.IsFalse(
                table.IsAcceptable(new List<string>(new string[] {"SYSTEM TABLE", "VIEW", "GLOBAL TEMPORARY"})));

            // testing columns

            GenericColumn IDcolumn = table["ID"]; //int auto increment
            Assert.IsTrue(IDcolumn.DbSimpleDataType == DbSimpleDataTypeEnum.IntegerSimpleType
                          || IDcolumn.DbSimpleDataType == DbSimpleDataTypeEnum.LongSimpleType);
            Assert.IsTrue(IDcolumn.IsNumericDataType);
            Assert.IsTrue(IDcolumn.PotentiallyCardinality == CardinalityEnum.Cardinal);
            Assert.IsFalse(IDcolumn.IsDerived);
            Assert.IsTrue(IDcolumn.Explain.isAutoIncrement);
            Equals(0, IDcolumn.Explain.columnOrdinal);
            Assert.IsFalse(IDcolumn.Explain.allowDBNull);
            Equals(numberOfRowsInTheTable, IDcolumn.Statistics.valueDistincts);
            Equals(1, IDcolumn.Statistics.valueMin);
            Equals(numberOfRowsInTheTable, IDcolumn.Statistics.valueMax);
            Assert.IsTrue(IDcolumn.Statistics.valueStandardDeviation > 0);
            Assert.IsTrue(IDcolumn.Statistics.valueVariability > 0);
            Assert.IsTrue((Convert.ToDouble(IDcolumn.Statistics.valueMin) +
                           Convert.ToDouble(IDcolumn.Statistics.valueMax))/2 ==
                          Convert.ToDouble(IDcolumn.Statistics.valueAverage));

            GenericColumn AGEcolumn = table["AGE"]; //float nullable
            Assert.IsTrue(AGEcolumn.DbSimpleDataType == DbSimpleDataTypeEnum.FloatSimpleType
                          || AGEcolumn.DbSimpleDataType == DbSimpleDataTypeEnum.DoubleSimpleType);
            Assert.IsTrue(AGEcolumn.IsNumericDataType);
            Assert.IsTrue(AGEcolumn.PotentiallyCardinality == CardinalityEnum.Cardinal);
            Assert.IsFalse(AGEcolumn.IsDerived);
            Equals(1, AGEcolumn.Explain.columnOrdinal);
            Assert.IsTrue(AGEcolumn.Explain.allowDBNull);
            Assert.IsTrue(Convert.ToDouble(AGEcolumn.Statistics.valueMin) > 18);
            Assert.IsTrue(Convert.ToDouble(AGEcolumn.Statistics.valueMax) <= 85);
            Assert.IsTrue(AGEcolumn.Statistics.valueStandardDeviation > 0);
            Assert.IsTrue(AGEcolumn.Statistics.valueVariability > 0);

            {
                bool failedAsExpected = false;
                try
                {
                    AGEcolumn.ChangeDbDataType(DbDataTypeEnum.BooleanType);
                }
                catch (InvalidOperationException)
                {
                    failedAsExpected = true;
                }
                Assert.IsTrue(failedAsExpected);
            }

            GenericColumn BOOLCOLUMNcolumn = table["Bool Column"]; //bool 
            Assert.IsTrue(BOOLCOLUMNcolumn.DbSimpleDataType == DbSimpleDataTypeEnum.BooleanSimpleType);
            Assert.IsFalse(BOOLCOLUMNcolumn.IsNumericDataType);
            Assert.IsTrue(BOOLCOLUMNcolumn.PotentiallyCardinality == CardinalityEnum.Nominal);
            Assert.IsFalse(BOOLCOLUMNcolumn.IsDerived);
            Assert.IsFalse(BOOLCOLUMNcolumn.Explain.allowDBNull);
            Assert.IsTrue(Convert.ToDouble(BOOLCOLUMNcolumn.Statistics.valueMin) == -1); //UNDONE??
            Assert.IsTrue(Convert.ToDouble(BOOLCOLUMNcolumn.Statistics.valueMax) == 0); //UNDONE??
            Assert.AreEqual(Double.NaN, BOOLCOLUMNcolumn.Statistics.valueStandardDeviation);
            Assert.AreEqual(Double.NaN, BOOLCOLUMNcolumn.Statistics.valueVariability);

            GenericColumn DATECOLUMNcolumn = table["DATE Column"]; //datetime
            Assert.IsTrue(DATECOLUMNcolumn.DbSimpleDataType == DbSimpleDataTypeEnum.DateTimeSimpleType);
            Assert.IsFalse(DATECOLUMNcolumn.IsNumericDataType);
            Assert.IsTrue(DATECOLUMNcolumn.PotentiallyCardinality == CardinalityEnum.Cardinal);
            Assert.IsFalse(DATECOLUMNcolumn.IsDerived);
            //Assert.IsTrue(DATECOLUMNcolumn.Statistics.ValueMin == -1); //UNDONE??
            //Assert.IsTrue(DATECOLUMNcolumn.Statistics.ValueMax == 0); //UNDONE??
            Assert.AreEqual(Double.NaN, DATECOLUMNcolumn.Statistics.valueVariability);
            Assert.AreEqual(Double.NaN, DATECOLUMNcolumn.Statistics.valueStandardDeviation);

            GenericColumn AOQUALcoumn = table["AOQUAL"]; //string nullable
            Assert.IsTrue(AOQUALcoumn.DbSimpleDataType == DbSimpleDataTypeEnum.StringSimpleType);
            Assert.IsFalse(AOQUALcoumn.IsNumericDataType);
            Assert.IsTrue(AOQUALcoumn.PotentiallyCardinality == CardinalityEnum.OrdinalCyclic);
            Assert.IsFalse(AOQUALcoumn.IsDerived);
            Assert.IsTrue(AOQUALcoumn.Explain.allowDBNull);
            Assert.AreEqual("NORMAL", AOQUALcoumn.Statistics.valueMin);
            Assert.AreEqual("SLIGHTLY CHANGED", AOQUALcoumn.Statistics.valueMax);
            Assert.AreEqual(Double.NaN, AOQUALcoumn.Statistics.valueVariability);
            Assert.AreEqual(Double.NaN, AOQUALcoumn.Statistics.valueStandardDeviation);

            // testing derived columns

            {
                string derivedColumnName = "NVESSELS + AGE";
                table.AddDerivedColumn(derivedColumnName);
                GenericColumn derived = table[derivedColumnName];
                Assert.AreEqual(DbDataTypeEnum.DoubleType, derived.Explain.dataType);
                Assert.IsTrue(derived.Statistics.valueDistincts > 0);
                Assert.IsTrue(derived.IsDerived);
                derived.ChangeDbDataType(DbDataTypeEnum.DoubleType);
                object dummy = derived.GetDistinctsAndFrequencies("");
                //derived.TestDerivedColumnDbDataType(DbDataTypeEnum.FloatType);
            }

            {
                string derivedColumnName = "LV_FUNCT + `REOPBLEED`";
                table.AddDerivedColumn(derivedColumnName);
                GenericColumn derived = table[derivedColumnName];
                Assert.IsTrue(derived.IsDerived);
                object dummy = derived.GetDistinctsAndFrequencies("");
                table.RemoveDerivedColumn(derivedColumnName);

                {
                    bool failedAsExpected = false;
                    try
                    {
                        GenericColumn dummy2 = table[derivedColumnName];
                    }
                    catch (BadParamsError e)
                    {
                        if (e.restrictionType == restrictionTypeEnum.DbColumnNameError)
                            failedAsExpected = true;
                    }
                    Assert.IsTrue(failedAsExpected);
                }
            }

            // testing enumerators (tests nearly everything)
            {
                foreach (GenericDataTable vTable in db)
                {
                    try
                    {
                        foreach (GenericColumn column in vTable)
                        {
                            object dummy = null;
                            dummy = column.DbSimpleDataType;
                            dummy = column.Explain;
                            dummy = column.IsDerived;
                            dummy = column.IsNumericDataType;
                            dummy = column.PotentiallyCardinality;
                            try
                            {
                                dummy = column.Statistics;
                                dummy = column.GetDistinctsAndFrequencies("");
                            }
                            catch (BadParamsError e)
                            {
                                if (!
                                    (
                                        e.restrictionType == restrictionTypeEnum.DbUnexpectedError
                                        &&
                                        column.Explain.dataType == DbDataTypeEnum.UnknownType
                                    )
                                    )
                                    throw;
                            }
                        }
                        {
                            object dummy = vTable;
                            dummy = vTable.BasicColumnsNames;
                            dummy = vTable.Explain;
                        }
                    }
                    catch (BadParamsError e)
                    {
                        if (e.restrictionType != restrictionTypeEnum.DbUnexpectedError)
                            throw;
                        if (vTable.DataTableType == "SYSTEM")
                            throw;
                    }
                }
            }

            // testing generic database cache
            GenericDatabase dbFromCache = GenericDatabaseCache.GetGenericDatabase(connectionSettingHelper);
            GenericDatabaseCache.RemoveGenericDatabase(connectionSettingHelper);
        }
    }
}