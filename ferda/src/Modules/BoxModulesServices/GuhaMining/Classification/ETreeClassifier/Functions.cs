// Functions.cs - Function objects for the ETreeClassifier box module
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2008 Martin Ralbovský
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
using System.Data;
using Ferda.Guha.Data;
using Ferda.Guha.Attribute;
using Ferda.ModulesManager;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Guha.MiningProcessor.Miners;
using Ferda.Modules.Boxes.DataPreparation;

namespace Ferda.Modules.Boxes.GuhaMining.Classification.ETreeClassifier
{
    /// <summary>
    /// Class is providing ICE functionality of the SampleBoxModule
    /// box module
    /// </summary>
    public class Functions : ETreeClassifierFunctionsDisp_, Ferda.Modules.IFunctions
    {
        #region Private fields

        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI boxModule;

        //protected IBoxInfo _boxInfo;

        /// <summary>
        /// Examples that are true and classified correctly
        /// </summary>
        private int truePositive = 0;

        /// <summary>
        /// Examples that are true and classified incorrectly
        /// </summary>
        private int falseNegative = 0;
        
        /// <summary>
        /// Examples that are false and classified incorrectly
        /// </summary>
        private int falsePositive = 0;
        
        /// <summary>
        /// Examples that are false and classified correctly
        /// </summary>
        private int trueNegative = 0;

        /// <summary>
        /// Cached data table
        /// </summary>
        private DataTable dataTableCache = null;

        /// <summary>
        /// Ticks (precise time) of the last update of the 
        /// progress bar
        /// </summary>
        private long progressLastUpdateTicks = DateTime.Now.Ticks;

        /// <summary>
        /// Minimum change in number of ticks for the miner to publish its
        /// progress. 
        /// </summary>
        private const long progressMinCountOfTicksToPublish = 500000;
        // ticks:
        // 1 tick = 100-nanosecond 
        // nano is 0.000 000 001
        // mili is 0.001
        // 0.05 sec is ticks * 500 000

        /// <summary>
        /// Cache for attribute functions strings
        /// (in order not to ask Ice layer multiple times
        /// </summary>
        private Dictionary<string, AttributeSerializable<System.IComparable>>
            cachedAttributes = null;

        #endregion

        /// <summary>
        /// The decision trees to be used for classification
        /// </summary>
        public SerializableDecisionTree[] decisionTrees;

        #region Sockets

        /// <summary>
        /// Name of the socket containing ETree procedure input
        /// </summary>
        public const string SockETree = "ETree";

        /// <summary>
        /// Name of the socket containing data table with testing data
        /// </summary>
        public const string SockDataTable = "DataTable";

        /// <summary>
        /// Name of the socket determining the decision tree according to which
        /// the classification shloud be done.
        /// </summary>
        public const string SockTreeNumber = "TreeNumber";

        /// <summary>
        /// Name of the socket determining TruePositive count of confusion matrix:
        /// Examples that are true and classified correctly
        /// </summary>
        public const string SockTruePositive = "TruePositive";

        /// <summary>
        /// Name of the socket determining FalseNegative count of confusion matrix:
        /// Examples that are true and classified incorrectly
        /// </summary>
        public const string SockFalseNegative = "FalseNegative";
        
        /// <summary>
        /// Name of the socket determining FalsePositive count of confusion matrix:
        /// Examples that are false and classified incorrectly
        /// </summary>
        public const string SockFalsePositive = "FalsePositive";

        /// <summary>
        /// Name of the socket determining TruNegative count of confusion matrix:
        /// Examples that are false and classified correctly
        /// </summary>
        public const string SockTrueNegative = "TrueNegative";

        #endregion

        #region Properties

        /// <summary>
        /// Decision tree according to which the classification should be done
        /// (number in the hypotheses list). The property is counted from 0. 
        /// </summary>
        public int TreeNumber
        {
            get
            {
                return boxModule.GetPropertyInt(SockTreeNumber);
            }
        }
        
        /// <summary>
        /// Examples that are true and classified correctly
        /// </summary>
        public int TruePositive
        {
            get
            {
                return truePositive;
            }
        }

        /// <summary>
        /// Examples that are true and classified incorrectly
        /// </summary>
        public int FalseNegative
        {
            get
            {
                return falseNegative;
            }
        }

        /// <summary>
        /// Examples that are false and classified incorrectly
        /// </summary>
        public int FalsePositive
        {
            get
            {
                return falsePositive;
            }
        }

        /// <summary>
        /// Examples that are false and classified correctly
        /// </summary>
        public int TrueNegative
        {
            get
            {
                return trueNegative;
            }
        }

        /// <summary>
        /// Gets actual data of the testing data table. 
        /// </summary>
        /// <returns>Data in a structure</returns>
        public DataTable TestingData
        {
            get
            {
                if (dataTableCache == null)
                {
                    DataTableFunctionsPrx dtPrx =
                        SocketConnections.GetPrx<DataTableFunctionsPrx>(
                        boxModule,
                        SockDataTable,
                        DataTableFunctionsPrxHelper.checkedCast,
                        true);

                    DataTableInfo dbInfo = dtPrx.getDataTableInfo();

                    DatabaseConnectionSettingHelper helper =
                        new DatabaseConnectionSettingHelper(dbInfo.databaseConnectionSetting);

                    //testing the database
                    GenericDatabase db = GenericDatabaseCache.GetGenericDatabase(helper);

                    //getting the result
                    string tableName = dbInfo.dataTableName;
                    GenericDataTable table = db[dbInfo.dataTableName];
                    dataTableCache = table.Select();
                }
                return dataTableCache;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets column names of the data table
        /// </summary>
        /// <returns>column names</returns>
        public string[] GetDatatableColumnNames()
        {
            DataTableFunctionsPrx dtPrx =
                SocketConnections.GetPrx<DataTableFunctionsPrx>(
                boxModule,
                SockDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                true);

            return dtPrx.getColumnsNames();
        }

        /// <summary>
        /// Gets column names of attributes connected to ETree task
        /// </summary>
        /// <returns>Column names</returns>
        public string[] GetAttributeColumnNames()
        {
            List<ColumnFunctionsPrx> columnPrxs = GetETreeColumnFunctionPrxs();

            List<string> columnNames = new List<string>();
            foreach (ColumnFunctionsPrx clPrx in columnPrxs)
            {
                columnNames.Add(clPrx.getColumnInfo().columnSelectExpression);
            }

            return columnNames.ToArray();
        }

        /// <summary>
        /// Stores the decision trees needed for classification to
        /// private field
        /// </summary>
        public void RetrieveDecisionTrees()
        {
            string stat;

            MiningTaskFunctionsPrx taskPrx =
                SocketConnections.GetPrx<MiningTaskFunctionsPrx>(
                    boxModule,
                    SockETree,
                    MiningTaskFunctionsPrxHelper.checkedCast,
                    true);
            string result = taskPrx.GetResult(out stat);

            //deserializing the result
            DecisionTreeResult res;
            try
            {
                res = DecisionTreeResult.Deserialize(result);
                decisionTrees = res.decisionTrees;
            }
            catch
            {
                BoxRuntimeError error = new BoxRuntimeError(null,
                    "Either a different task than ETree or the ETree procedure wasn't run.");
                throw error;
            }
        }

        /// <summary>
        /// Classifies the data according to the selected tree
        /// </summary>
        public void Classify()
        {
            //starting the progress bar
            int count = 0;
            string label = String.Empty;
            ProgressTaskListener progressListener = new ProgressTaskListener();
            ProgressTaskPrx progressPrx =
                ProgressTaskI.Create(boxModule.Adapter, progressListener);
            ProgressBarPrx progressBarPrx =
                boxModule.Output.startProgress(progressPrx, label, label + " running...");
            progressBarPrx.setValue(-1, "Loading ...");

            string[] resultCategories = new string[TestingData.Rows.Count];

            foreach (DataRow row in TestingData.Rows)
            {
                //set progress
                long actTicks = DateTime.Now.Ticks;
                if (System.Math.Abs(progressLastUpdateTicks - actTicks) > progressMinCountOfTicksToPublish)
                {
                    progressLastUpdateTicks = actTicks;
                    try
                    {
                        progressBarPrx.setValue((float)count/TestingData.Rows.Count, "Classifying rows...");
                    }
                    catch { }
                }

                //classification of one row
                resultCategories[count] = ClassifyRow(row, decisionTrees[TreeNumber].RootNode);

                count++;
            }

            //finishing the progress bar
            progressBarPrx.setValue(-1, "Finished ...");
            if (progressBarPrx != null)
            {
                progressBarPrx.done();
                ProgressTaskI.Destroy(boxModule.Adapter, progressPrx);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns a string representation of category classified by
        /// a node in the parameter
        /// </summary>
        /// <param name="row">Row to be classified</param>
        /// <param name="node">Decision tree node where the
        /// classification takes part</param>
        /// <returns>String representation of the classification category</returns>.
        private string ClassifyRow(DataRow row, SerializableNode node)
        {
            AttributeSerializable<IComparable> attribute = GetAttribute(node.AttributeName);

            return string.Empty;
        }

        private AttributeSerializable<IComparable> GetAttribute(string attributeName)
        {
            //loading attributes into cache
            if (cachedAttributes == null)
            {
                List<BoxModulePrx> attributePrxs =
                    GetAttributeBoxModulePrxs();

                List<AttributeFunctionsPrx> attributeFncs =
                    new List<AttributeFunctionsPrx>();

                //loading attribute function proxies
                foreach (BoxModulePrx prx in attributePrxs)
                {
                    attributeFncs.Add(
                        AttributeFunctionsPrxHelper.checkedCast(prx.getFunctions()));
                }

                //deserializing the attribute info
                foreach (AttributeFunctionsPrx attrPrx in attributeFncs)
                {
                    AttributeSerializable<IComparable> tmp =
                        Ferda.Guha.Attribute.Serializer.Deserialize<IComparable>
                            (attrPrx.getAttribute());
                    cachedAttributes.Add(attributeName, tmp);
                }
            }

            return cachedAttributes[attributeName];
        }

        /// <summary>
        /// Gets box module proxies of the attributes connected to the ETree
        /// box.
        /// </summary>
        /// <returns>Box module proxies of the attributes</returns>
        private List<BoxModulePrx> GetAttributeBoxModulePrxs()
        {
            BoxModulePrx eTree = boxModule.getConnections(SockETree)[0];

            //getting the attributes
            List<BoxModulePrx> attributes = new List<BoxModulePrx>();
            attributes.Add(
                eTree.getConnections(Tasks.ETree.Functions.SockTargetClassificationAttribute)[0]);
            attributes.AddRange(
                eTree.getConnections(Tasks.ETree.Functions.SockBranchingAttributes));
            return attributes;
        }

        /// <summary>
        /// Get column function proxies, which are connected to the ETree
        /// attributes.
        /// </summary>
        /// <returns>Column functions proxies</returns>
        private List<ColumnFunctionsPrx> GetETreeColumnFunctionPrxs()
        {
            List<BoxModulePrx> attributes = GetAttributeBoxModulePrxs();

            //getting the columns box modules
            List<BoxModulePrx> columns = new List<BoxModulePrx>();
            foreach (BoxModulePrx atr in attributes)
            {
                columns.Add(atr.getConnections("Column")[0]);
            }

            //getting the columns functions
            List<ColumnFunctionsPrx> columnPrxs = new List<ColumnFunctionsPrx>();
            foreach (BoxModulePrx col in columns)
            {
                ColumnFunctionsPrx tmp =
                    ColumnFunctionsPrxHelper.checkedCast(col.getFunctions());
                columnPrxs.Add(tmp);
            }
            return columnPrxs;
        }

        #endregion

        #region ICE functions

        public override string HelloWorld(Ice.Current __current)
        {
            return "Hello World!";
        }

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
            this.boxModule = boxModule;
            //this.boxInfo = boxInfo;
        }

        #endregion
    }
}