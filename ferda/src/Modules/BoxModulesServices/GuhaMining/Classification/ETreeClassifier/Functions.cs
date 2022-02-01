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
        private Dictionary<string, Attribute<IComparable>>
            cachedAttributes = null;

        /// <summary>
        /// Cache for types of columns accessible by the columns name.
        /// Initialized in the 
        /// <see cref="GetAttribute"/> method.
        /// </summary>
        //private Dictionary<string, DbDataTypeEnum> cachedAttributeColumnTypes
        //    = null;

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

            //computing the confusion matrix
            try
            {
                progressBarPrx.setValue(1, "Computing confusion matrix...");
            }
            catch { }
            ComputeConfusionMatrix(resultCategories);

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
        /// Computes confusion matrix
        /// </summary>
        /// <param name="resultCategories">Result of classification by
        /// a tree</param>
        private void ComputeConfusionMatrix(string[] resultCategories)
        {
            //reseting the counters
            truePositive = 0;
            falseNegative = 0;
            falsePositive = 0;
            trueNegative = 0;
            //intermediate results - testing
            int TP;
            int FN;
            int FP;
            int TN;

            //getting the classification attribute
            string classAttrName;
            Attribute<IComparable> classAtribute =
                GetClassificationAttribute(out classAttrName);
            
            //getting the data column
            DataColumn column = TestingData.Columns[classAttrName];

            foreach (string category in classAtribute.Keys)
            {
                TP = 0;
                FN = 0;
                FP = 0;
                TN = 0;

                //iterating through all the records
                for (int i = 0; i < resultCategories.Length; i++)
                {
                    //if it is the counted category
                    if (category == TestingData.Rows[i][column].ToString())
                    {
                        //counting the confusion matrix
                        if (resultCategories[i] == category)
                        {
                            TP++;
                        }
                        else
                        {
                            FN++;
                        }
                    }
                    else
                    {
                        if (resultCategories[i] == category)
                        {
                            FP++;
                        }
                        else
                        {
                            TN++;
                        }
                    }
                }

                truePositive += TP;
                falseNegative += FN;
                falsePositive += FP;
                trueNegative += TN;
            }
        }

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
            //getting the attribute and the data entry
            Attribute<IComparable> attribute = GetAttribute(node.AttributeLaterColumnName);
            IComparable item = (IComparable) row[node.AttributeLaterColumnName];

            //searching in categories (direct classification)
            for (int i = 0; i < node.NodeCategories.Length; i++)
            {
                Category<IComparable> category = attribute[node.NodeCategories[i]];
                if (category.BelongsToCategory(item))
                {
                    return node.ClassificationCategories[i];
                }
            }

            //searching in subnodes
            if (node.SubNodes != null)
            {
                for (int i = 0; i < node.SubNodeCategories.Length; i++)
                {
                    Category<IComparable> category = attribute[node.SubNodeCategories[i]];
                    if (category.BelongsToCategory(item))
                    {
                        return ClassifyRow(row, node.SubNodes[i]);
                    }
                }
            }

            //here throwing an exception - this situation occurs iff the attribute
            //does not fully cover the column domain
            BoxRuntimeError error = new BoxRuntimeError(null,
                    "The attribute does not fully cover the column domain, categorization impossible");
            throw error;
        }

        /// <summary>
        /// Gets the classification attribute of the ETree task
        /// </summary>
        /// <param name="selectExpression">
        /// The select expression of the classification attribute
        /// </param>
        /// <returns>Classification attribute</returns>
        private Attribute<IComparable> GetClassificationAttribute(out string selectExpression)
        {
            BoxModulePrx eTree = boxModule.getConnections(SockETree)[0];
            BoxModulePrx clasAtrPrx = 
                eTree.getConnections(Tasks.ETree.Functions.SockTargetClassificationAttribute)[0];
            BoxModulePrx colPrx = 
                clasAtrPrx.getConnections("Column")[0];
            ColumnFunctionsPrx colFnc = 
                ColumnFunctionsPrxHelper.checkedCast(colPrx.getFunctions());

            selectExpression = colFnc.getColumnInfo().columnSelectExpression;
            return GetAttribute(selectExpression);
        }

        /// <summary>
        /// Returns attribute connected to the ETree procedure by its name (
        /// which corresponds to select expression of the column). The method
        /// uses and fills two caches, <see cref="cachedAttributes"/> and
        /// <see cref="cachedAttributeColumnTypes"/>.
        /// </summary>
        /// <param name="columnSelect">Name of the column select expression
        /// for which the attribute is retrieved</param>
        /// <returns>Attribute</returns>
        private Attribute<IComparable> GetAttribute(string columnSelect)
        {
            //loading attributes into cache
            if (cachedAttributes == null)
            {
                cachedAttributes = new Dictionary<string, Attribute<IComparable>>();
                //cachedAttributeColumnTypes = new Dictionary<string, DbDataTypeEnum>();

                List<BoxModulePrx> attributePrxs =
                    GetAttributeBoxModulePrxs();

                //deserializing attribute info
                foreach (BoxModulePrx prx in attributePrxs)
                {
                    //the attribute functions proxy
                    AttributeFunctionsPrx attrPrx =
                        AttributeFunctionsPrxHelper.checkedCast(prx.getFunctions());

                    //the column functions proxy
                    BoxModulePrx col = prx.getConnections("Column")[0];
                    ColumnFunctionsPrx colPrx = ColumnFunctionsPrxHelper.checkedCast(col.getFunctions());

                    //column name and type
                    DbDataTypeEnum colType = colPrx.getColumnInfo().dataType;
                    string colSelect = colPrx.getColumnInfo().columnSelectExpression;

                    //deserializing attribute
                    Attribute<IComparable> attr =
                        Ferda.Guha.Attribute.Serializer.RetypeAttributeSerializable(
                            attrPrx.getAttribute(), colType);

                    //adding to the cache
                    cachedAttributes.Add(colSelect, attr);
                }
            }

            return cachedAttributes[columnSelect];
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
