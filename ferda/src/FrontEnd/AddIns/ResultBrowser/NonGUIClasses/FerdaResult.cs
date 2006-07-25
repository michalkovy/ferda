// FerdaResult.cs - class for working with results of the taskrun
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Ferda.FrontEnd.AddIns.ResultBrowser;
using Ferda.Modules;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.ModulesManager;
using System.Resources;
using System.Reflection;
using System.Threading;

namespace Ferda.FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
    #region Data structures

    /// <summary>
    /// Struct for sorting items
    /// </summary>
    public struct Tuple : IComparable
    {
        /// <summary>
        /// Hypothesis id
        /// </summary>
        int HypothesisId;

        /// <summary>
        /// Counted statistics
        /// </summary>
        double CountedStatistics;

        public int HypId
        {
            get
            {
                return HypothesisId;
            }
            set
            {
                HypothesisId = value;
            }
        }

        public double Value
        {
            get
            {
                return CountedStatistics;
            }

            set
            {
                CountedStatistics = value;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is Tuple)
            {
                Tuple tuple = (Tuple)obj;
                return CountedStatistics.CompareTo(tuple.CountedStatistics);
            }
            else
            {
                throw new ArgumentException("NotTuple");
            }
        }
    };

    
    /// <summary>
    /// Struct for saving counted values
    /// </summary>
    public struct CountedValues
    {
        private string ValueName;
        private string UserValueName;
        private Double ValueValue;

        public string Name
        {
            get
            {
                return ValueName;
            }
            set
            {
                ValueName = value;
            }
        }

        public string UserName
        {
            get
            {
                return UserValueName;
            }
            set
            {
                UserValueName = value;
            }
        }

        public Double Value
        {
            get
            {
                return ValueValue;
            }

            set
            {
                ValueValue = value;
            }
        }
    };

    #region Commented
    /*
    
    /// <summary>
    /// Struct for literal filters
    /// </summary>
    public struct LiteralFilter
    {
        private GaceTypeEnum gace;
        private int lowLength;
        private int highLength;
        private bool selected;

        public GaceTypeEnum Gace
        {
            get
            {
                return gace;
            }
            set
            {
                gace = value;
            }
        }

        public int LowLength
        {
            get
            {
                return lowLength;
            }
            set
            {
                lowLength = value;
            }
        }

        public int HighLength
        {
            get
            {
                return highLength;
            }
            set
            {
                highLength = value;
            }
        }

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }
    }*/

    #endregion


    /// <summary>
    /// Structure for storing cached quantifiers
    /// </summary>
    public struct CachedHypothesisValues
    {
        public CountedValues[] Quantifiers;
    }

    #endregion

    /// <summary>
    /// Class for storing and proceeding the results
    /// </summary>
    public class FerdaResult
    {
        #region Private class variables

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Localization string, en-US or cs-CZ for now.
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Hypotheses to display
        /// </summary>
  //      private HypothesisStruct[] Hypotheses;

        /// <summary>
        /// Quantifier is a function which takes in an integer array and returns an integer
        /// </summary>
        /// <param name="i">Integer array</param>
        /// <returns>Value of applied quantifier</returns>
        private delegate int Quantifier(int[][] i);

        /// <summary>
        /// Array of used quantifiers.
        /// </summary>
        private Quantifiers quantifiers;

        /// <summary>
        /// Array for cached hypotheses
        /// </summary>
        private CachedHypothesisValues[] cachedHypotheses;

        /// <summary>
        /// Array of quantifiers labels
        /// </summary>
        private string [] quantifiersLabels;

        /// <summary>
        /// Array of quantifiers uerlabels
        /// </summary>
        private string[] quantifiersUserLabels = new string [0];


        /// <summary>
        /// Deserialized result
        /// </summary>
        private Result result;

        #region Commented
        /// <summary>
        /// List of quantifiers proxies
        /// </summary>
     //   private Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier.AbstractQuantifierFunctionsPrx[] proxy;

        /// <summary>
        /// Dictionary determining which antecedent values to display
        /// </summary>
    //    private Dictionary<string, LiteralFilter> antecedentFilter = new Dictionary<string, LiteralFilter>();

        /// <summary>
        /// Dictionary determining which succedent values to display
        /// </summary>
    //    private Dictionary<string, LiteralFilter> succedentFilter = new Dictionary<string, LiteralFilter>();

        /// <summary>
        /// Dictionary determining which condition values to display
        /// </summary>
        //   private Dictionary<string, LiteralFilter> conditionFilter = new Dictionary<string, LiteralFilter>();

        #endregion
        #endregion


        #region Properties

        #region Commented
        /*
        /// <summary>
        /// Gets or sets antecedent filter
        /// </summary>
        public Dictionary<string, LiteralFilter> AntecedentFilter
        {
            get
            {
                return antecedentFilter;
            }
            set
            {
                antecedentFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets succedent filter
        /// </summary>
        public Dictionary<string, LiteralFilter> SuccedentFilter
        {
            get
            {
                return succedentFilter;
            }
            set
            {
                succedentFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets condition filter
        /// </summary>
        public Dictionary<string, LiteralFilter> ConditionFilter
        {
            get
            {
                return conditionFilter;
            }
            set
            {
                conditionFilter = value;
            }
        }
        
        /// <summary>
        /// Gets all filtered hypotheses in the result
        /// </summary>
        /// <returns>Dictionary with all filtered hypotheses</returns>
        public Dictionary<int, HypothesisStruct> AllFilteredHypotheses
        {           
            get
            {
                Dictionary<int, HypothesisStruct> returnDict = new Dictionary<int, HypothesisStruct>();
                /*for (int i = 0; i < Hypotheses.Length; i++)
                {
                    if (HypothesisIsValid(Hypotheses[i]))
                    {
                        returnDict.Add(i, Hypotheses[i]);
                    }
                }
                return returnDict;
            }
        }
         * */
        #endregion

        /// <summary>
        /// Gets all hypotheses
        /// </summary>
        /// <returns>Array with all hypotheses</returns>
        public List<Hypothesis> AllHypotheses
        {
            get
            {
                return this.result.Hypotheses;
            }
        }

        /// <summary>
        /// Count of hypotheses
        /// </summary>
        public long AllHypothesesCount
        {
            get
            {
                try
                {
                    return result.AllObjectsCount;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Method to get string value of MarkEnums for the given hypothesis
        /// </summary>
        /// <param name="mark">MarkEnum to get String value for</param>
        /// <param name="hypothesisId">Hypothesis id</param>
        /// <returns></returns>
        public string GetFormula(MarkEnum mark, int hypothesisId)
        {
            try
            {
                return this.result.Hypotheses[hypothesisId].GetFormula(mark).ToString();
            }
            catch
            {
                return "GetFormula failed";
            }
        }

        /// <summary>
        /// Gets semantic marks of the hypotheses in result
        /// </summary>
        public MarkEnum[] SemanticMarks
        {
            get
            {
                try
                {
                    return this.result.GetSemanticMarks();
                }
                catch
                {
                    return new MarkEnum[0];
                }
            }
        }

        /// <summary>
        /// Quantifiers labels
        /// </summary>
        public string[] QuantifiersLabels
        {
            get
            {
                return this.quantifiersLabels;
            }
        }

        /// <summary>
        /// Quantifiers userlabels
        /// </summary>
        public string[] QuantifiersUserLabels
        {
            get
            {
                return this.quantifiersUserLabels;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="rm">Resource manager</param>
        public FerdaResult(ResourceManager rm, string result, Quantifiers quantifiers)
        {
            this.resManager = rm;
            this.quantifiers = quantifiers;
            this.result = SerializableResult.Deserialize(result);

            List<string> temp = new List<string>();
            List<string> temp1 = new List<string>();
            foreach (Ferda.Guha.MiningProcessor.QuantifierEvaluator.Quantifier quantifier in quantifiers.Quantifeirs.Values)
            {
                temp.Add(quantifier.LocalizedLabel);
                temp1.Add(quantifier.LocalizedUserLabel);
            }
            this.quantifiersLabels = temp.ToArray();
            this.quantifiersUserLabels = temp1.ToArray();
        }

        #endregion


        #region Initialization methods

        /// <summary>
        /// Method to initialize the ResultBrowser structure
        /// </summary>
        /// <param name="hypotheses"></param>
        public void Initialize()
        {

            #region Caching

            //initializing hypotheses cache
            this.cachedHypotheses = new CachedHypothesisValues[this.result.Hypotheses.Count];

            Thread IceCommunicationThread = new Thread(new ThreadStart(IceCommunication));
            IceCommunicationThread.Start();         

            #endregion
        }

        /// <summary>
        /// Method to be launched in separate thread, takes long time to complete
        /// </summary>
        private void IceCommunication()
        {          
            //caching hypotheses quantifiers
            for (int i = 0; i < this.result.Hypotheses.Count; i++)
            {
                this.cachedHypotheses[i].Quantifiers = this.QuantifiersValues(i);
                this.OnIceTick();
            }
        //    this.InitFilters();
            this.OnIceComplete();
        }

        #endregion


        #region Column methods

        #region commented
        /*
        /// <summary>
        /// Method for retrieving used column names.
        /// </summary>
        /// <returns>Arraylist of used column names</returns>
        public ArrayList GetUsedColumnNames()
        {
            ArrayList returnArray = new ArrayList();
            foreach (QuantifierProvider quantifier in this.UsedQuantifiers)
            {
                returnArray.Add(quantifier.localizedBoxLabel.ToString());
            }
            return returnArray;
        }


        /// <summary>
        /// Method for retrieving unused column names.
        /// </summary>
        /// <returns>Arraylist of unused column names</returns>
        public ArrayList GetUnusedColumnNames()
        {
            ArrayList returnArray = new ArrayList();

            foreach (QuantifierProvider quantifier in this.UsedQuantifiers)
            {
                if (!this.QuantifierIsUsed(quantifier))
                {
                    returnArray.Add(quantifier.localizedBoxLabel.ToString());
                }
            }
            return returnArray;
        }


        /// <summary>
        /// Method to remove the column from view (=value of applied quantifier)
        /// </summary>
        /// <param name="columnName">Column to remove</param>
        public void RemoveColumn(String columnName)
        {
            for (int i = 0; i < this.SelectedQuantifiers.Length; i++)
            {
                if (this.GetColumnName(this.UsedQuantifiers[i]) == columnName)
                {
                    this.SelectedQuantifiers[i] = 0;
                    return;
                }
            }
        }

        /// <summary>
        /// Method to add column to the current view.
        /// </summary>
        /// <param name="columnName">Column name to add.</param>
        public void AddColumn(String columnName)
        {
            for (int i = 0; i < this.UsedQuantifiers.Length; i++)
            {
                if (this.GetColumnName(this.UsedQuantifiers[i]) == columnName)
                {
                    this.SelectedQuantifiers[i] = 1;
                    return;
                }
            }
        }

        /// <summary>
        /// Method which gets column names which are selected to be displayed.
        /// </summary>
        /// <returns>An arraylist with column names.</returns>
        public ArrayList GetSelectedColumnNames()
        {
            ArrayList returnArray = new ArrayList();

            for (int i = 0; i < this.SelectedQuantifiers.Length; i++)
            {
                if (this.SelectedQuantifiers[i] == 1)
                {
                    returnArray.Add(this.UsedQuantifiers[i].localizedBoxLabel.ToString());
                }
            }
            return returnArray;
        }
         * */
        #endregion

        #endregion


        #region Methods for composing various strings

        #region commented

        /*
        /// <summary>
        /// Method to get hypothesis contingency table
        /// </summary>
        /// <param name="hypothese">Hypothese to take the table from</param>
        /// <returns>Formatted string containing contingency table</returns>
        public String GetHypothesisTable(HypothesisStruct hypothese)
        {
            StringBuilder returnString = new StringBuilder();
            foreach (int[] arr in hypothese.quantifierSetting.firstContingencyTableRows)
            {
                foreach (int value in arr)
                {
                    returnString.Append(value.ToString() + "\t");
                }
                returnString.Append("\n");
            }

            foreach (int[] arr in hypothese.quantifierSetting.secondContingencyTableRows)
            {
                foreach (int value in arr)
                {
                    returnString.Append(value.ToString() + "\t");
                }
                returnString.Append("\n");
            }
            return returnString.ToString();
        }
        
        /// <summary>
        /// Method which gets used quantifiers names
        /// </summary>
        /// <returns>List of used quantifiers names</returns>
        public List<string> GetSelectedQuantifiersNames()
        {
            List<string> returnList = new List<string>();
            foreach (QuantifierProvider quantifier in this.UsedQuantifiers)
            {
                returnList.Add(quantifier.localizedBoxLabel + " ("
                    + quantifier.userBoxLabel + ")" + "\t");
            }
            return returnList;
        }

        public List<string> GetAllQuantifierNames()
        {
            List<string> returnList = new List<string>();
            foreach (QuantifierProvider quantifier in this.UsedQuantifiers)
            {
                returnList.Add(quantifier.localizedBoxLabel + " ("
                    + quantifier.userBoxLabel + ")" + "\t");
            }
            return returnList;
        }

        /// <summary>
        /// Method which gets column name based on function name.
        /// </summary>
        /// <returns></returns>
        protected String GetColumnName(QuantifierProvider quantifier)
        {
            String returnString;
            returnString = quantifier.localizedBoxLabel.ToString();
            return returnString;
        }

         * */
        #endregion
        #endregion


        #region Events

        public event LongRunTick IceTicked;
        public void OnIceTick()
        {
            if (IceTicked != null)
            {
                IceTicked();
            }
        }

        public event LongRunCompleted IceComplete;
        public void OnIceComplete()
        {
            if (IceComplete != null)
            {
                IceComplete();
            }
        }

        #endregion


        #region Quantifier functions

        /// <summary>
        /// Counting all of the quantifiers for the first contingency table of the chosen hypothesis
        /// </summary>
        /// <param name="index">Hypothesis index</param>
        /// <returns></returns>
        private CountedValues[] QuantifiersValues(int index)
        {
            List<CountedValues> returnValues = new List<CountedValues>();

            foreach (Ferda.Guha.MiningProcessor.QuantifierEvaluator.Quantifier quantifier in quantifiers.Quantifeirs.Values)
            {
                if (quantifier.ProvidesValues)
                {
                    CountedValues countedValue = new CountedValues();
                    countedValue.Name = quantifier.LocalizedLabel;
                    countedValue.UserName = quantifier.LocalizedUserLabel;
                    countedValue.Value = 
                        quantifier.Value(result.Hypotheses[index],result.AllObjectsCount);
                }
            }
            return returnValues.ToArray();
        }

        /*
        /// <summary>
        /// Counting all of the quantifiers for the second contingency table of the chosen hypothesis
        /// </summary>
        /// <param name="index">Hypothesis index</param>
        /// <returns></returns>
        private CountedValues[] QuantifiersValuesSecondTable(int index)
        {
            List<CountedValues> returnValues = new List<CountedValues>();

            foreach (Ferda.Guha.MiningProcessor.QuantifierEvaluator.Quantifier quantifier in quantifiers.Quantifeirs.Values)
            {
                if (quantifier.ProvidesValues)
                {
                    CountedValues countedValue = new CountedValues();
                    countedValue.Name = quantifier.LocalizedLabel;
                    countedValue.UserName = quantifier.LocalizedUserLabel;
                    ContingencyTableHelper helper = new ContingencyTableHelper(
                        result.Hypotheses[index].ContingencyTableB, result.AllObjectsCount);
                    countedValue.Value = quantifier.Value(helper);
                }
            }
            return returnValues.ToArray();
        }*/

        /// <summary>
        /// Method for reading the pre-counted quantifiers for first table from cache
        /// </summary>
        /// <param name="hypid">Id of the hypothesis to get values for</param>
        /// <param name="precision">Number of decimal places of precision</param>
        /// <returns>List of counted values</returns>
        public double[] ReadQuantifiersFromCache(int hypid, int precision)
        {
            List<double> returnQuantifiers = new List<double>();
            double[] quantifiers = new double[cachedHypotheses[hypid].Quantifiers.Length];
            for (int i = 0; i < cachedHypotheses[hypid].Quantifiers.Length; i++)
            {
                returnQuantifiers.Add(Math.Round(cachedHypotheses[hypid].Quantifiers[i].Value, precision));
            }
            return returnQuantifiers.ToArray();
        }
        /*
        /// <summary>
        /// Method for reading the pre-counted quantifiers for second table from cache
        /// </summary>
        /// <param name="hypid">Id of the hypothesis to get values for</param>
        /// <param name="precision">Number of decimal places of precision</param>
        /// <returns>List of counted values</returns>
        public double[] ReadQuantifiersFromCacheSecondTable(int hypid, int precision)
        {
            List<double> returnQuantifiers = new List<double>();
            double[] quantifiers = new double[cachedHypotheses[hypid].QuantifiersSecondTable.Length];
            for (int i = 0; i < cachedHypotheses[hypid].QuantifiersSecondTable.Length; i++)
            {
                returnQuantifiers.Add(Math.Round(cachedHypotheses[hypid].QuantifiersSecondTable[i].Value, precision));
            }
            return returnQuantifiers.ToArray();
        }
        */
        #endregion


        #region Hypotheses methods

        /// <summary>
        /// Gets hypothesis at the required index
        /// </summary>
        /// <param name="index">Index of the hypothesis to get</param>
        /// <returns></returns>
        public Hypothesis GetHypothese(int index)
        {
            return this.result.Hypotheses[index];
        }

        #endregion


        #region Caching methods
        #region commented
        /*
        /// <summary>
        /// Method for counting quantifiers values for hypothese
        /// </summary>
        /// <param name="HypId">Hypothesis id to count values for</param>
        /// <returns>Counted quantifiers values</returns>
        private CountedValues[] GetQuantifiers(int HypId)
        {
            
          //  List<string> names = this.GetAllQuantifierNames();
          //  List<double> values = this.CountAllQuantifiers(this.Hypotheses[HypId]);
            CountedValues[] quantifiers = new CountedValues[values.Count];

            for (int i = 0; i < values.Count; i++)
            {
                quantifiers[i].Name = names[i];
                quantifiers[i].Value = values[i];
            }
            return quantifiers;
        }
         * */
        #endregion

        #endregion


        #region Other methods

        /*
        /// <summary>
        /// Method to decide whether the hypothesis is of 4-ft miner
        /// </summary>
        /// <param name="hypothesis">Hypothesis to check</param>
        /// <returns>True if hypothesis is from 4ft</returns>
        public static bool IsFFT(HypothesisStruct hypothesis)
        {
            foreach (BooleanLiteralStruct booleanLiteral in hypothesis.booleanLiterals)
            {
                if ((booleanLiteral.cedentType == CedentEnum.Antecedent) || (booleanLiteral.cedentType == CedentEnum.Succedent))
                {
                    return true;
                }
            }
            return false;
        }
        */
        #endregion


        #region Filtering methods
        #region commented
        /*
        /// <summary>
        /// Method which initializes the filters
        /// </summary>
        private void InitFilters()
        {
            foreach (HypothesisStruct hypothese in Hypotheses)
            {
                foreach (BooleanLiteralStruct booleanLiteral in hypothese.booleanLiterals)
                {
                    LiteralFilter temp = new LiteralFilter();
                    temp.Gace = GaceTypeEnum.Both;
                    temp.LowLength = 0;
                    temp.HighLength = 99;
                    temp.Selected = true;
                    switch (booleanLiteral.cedentType)
                    {
                        case CedentEnum.Antecedent:
                            if (!this.antecedentFilter.ContainsKey(booleanLiteral.literalName))
                            {
                                this.antecedentFilter.Add(booleanLiteral.literalName, temp);
                            }
                            break;

                        case CedentEnum.Succedent:
                            if (!this.succedentFilter.ContainsKey(booleanLiteral.literalName))
                            {
                                this.succedentFilter.Add(booleanLiteral.literalName, temp);
                            }
                            break;

                        case CedentEnum.Condition:
                            if (!this.conditionFilter.ContainsKey(booleanLiteral.literalName))
                            {
                                this.conditionFilter.Add(booleanLiteral.literalName, temp);
                            }
                            break;

                        default:
                            break;
                            //throw new Exception("SwitchBranchNotImplemented");
                    }
                }

                foreach (LiteralStruct literal in hypothese.literals)
                {
                    LiteralFilter temp = new LiteralFilter();
                    temp.Gace = GaceTypeEnum.Both;
                    temp.LowLength = 0;
                    temp.HighLength = 99;
                    temp.Selected = true;
                    switch (literal.cedentType)
                    {
                        case CedentEnum.Antecedent:
                            if (!this.antecedentFilter.ContainsKey(literal.literalName))
                            {
                                this.antecedentFilter.Add(literal.literalName, temp);
                            }
                            break;

                        case CedentEnum.Succedent:
                            if (!this.succedentFilter.ContainsKey(literal.literalName))
                            {
                                this.succedentFilter.Add(literal.literalName, temp);
                            }
                            break;

                        case CedentEnum.Condition:
                            if (!this.conditionFilter.ContainsKey(literal.literalName))
                            {
                                this.conditionFilter.Add(literal.literalName, temp);
                            }
                            break;

                        default:
                            //break;
                            throw new Exception("SwitchBranchNotImplemented");
                    }
                }
            }
        }

        /// <summary>
        /// Method which check whether the given hypothesis matches current filter settings
        /// </summary>
        /// <param name="hypothesis">Hypothesis to check</param>
        /// <returns>True if hypothesis matches</returns>
        private bool HypothesisIsValid(HypothesisStruct hypothesis)
        {
            LiteralFilter filter;

            //initialize structure
            List<string> antecedentLiteralsList = new List<string>();
            List<string> succedentLiteralsList = new List<string>();
            List<string> conditionLiteralsList = new List<string>();
            foreach (BooleanLiteralStruct booleanLiteral in hypothesis.booleanLiterals)
            {
                switch (booleanLiteral.cedentType)
                {
                    case CedentEnum.Antecedent:
                        if (!antecedentLiteralsList.Contains(booleanLiteral.literalName))
                        {
                            antecedentLiteralsList.Add(booleanLiteral.literalName);
                        }
                        break;

                    case CedentEnum.Succedent:
                        if (!succedentLiteralsList.Contains(booleanLiteral.literalName))
                        {
                            succedentLiteralsList.Add(booleanLiteral.literalName);
                        }
                        break;

                    case CedentEnum.Condition:
                        if (!conditionLiteralsList.Contains(booleanLiteral.literalName))
                        {
                            conditionLiteralsList.Add(booleanLiteral.literalName);
                        }
                        break;

                    default:
                        break;
                }
            }

            foreach (LiteralStruct literal in hypothesis.literals)
            {
                switch (literal.cedentType)
                {
                    case CedentEnum.Antecedent:
                        if (!antecedentLiteralsList.Contains(literal.literalName))
                        {
                            antecedentLiteralsList.Add(literal.literalName);
                        }
                        break;

                    case CedentEnum.Succedent:
                        if (!succedentLiteralsList.Contains(literal.literalName))
                        {
                            succedentLiteralsList.Add(literal.literalName);
                        }
                        break;

                    case CedentEnum.Condition:
                        if (!conditionLiteralsList.Contains(literal.literalName))
                        {
                            conditionLiteralsList.Add(literal.literalName);
                        }
                        break;

                    default:
                        break;
                }
            }

            //check antecedent
            foreach (string literal in antecedentLiteralsList)
            {
                if (antecedentFilter.TryGetValue(literal, out filter))
                {
                    if (!filter.Selected)
                    {
                        return false;
                    }
                }
            }

            //check succedent
            foreach (string literal in succedentLiteralsList)
            {
                if (succedentFilter.TryGetValue(literal, out filter))
                {
                    if (!filter.Selected)
                    {
                        return false;
                    }
                }
            }

            //check condition
            foreach (string literal in conditionLiteralsList)
            {
                if (conditionFilter.TryGetValue(literal, out filter))
                {
                    if (!filter.Selected)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        */
        #endregion
        #endregion


        #region Localization

        public void ChangeRm(ResourceManager rm)
        {
            this.resManager = rm;
        }

        #endregion
    }
}

