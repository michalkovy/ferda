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
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
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
    /// Struct for saving counted statistics
    /// </summary>
    public struct CountedValues
    {
        private string ValueName;
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
    }


    /// <summary>
    /// Structure for storing cached quantifiers and statistics
    /// </summary>
    public struct CachedHypothesis
    {
        public CountedValues[] StatisticsList;
        public CountedValues[] QuantifiersList;
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
        private HypothesisStruct[] Hypotheses;

        /// <summary>
        /// Quantifier is a function which takes in an integer array and returns an integer
        /// </summary>
        /// <param name="i">Integer array</param>
        /// <returns>Value of applied quantifier</returns>
        private delegate int Quantifier(int[][] i);

        /// <summary>
        /// Array of used quantifiers.
        /// </summary>
        private QuantifierProvider[] UsedQuantifiers;

        /// <summary>
        /// Array of actual quantifiers - user can choose to use more quantifiers that were used in the beginning.
        /// Represented by the array of boolean flags.
        /// </summary>
        private int[] SelectedQuantifiers;

        /// <summary>
        /// Array for cached hypotheses
        /// </summary>
        private CachedHypothesis[] cachedHypotheses;

        /// <summary>
        /// List of statistics function proxies
        /// </summary>
        List<Ferda.Statistics.StatisticsProviderPrx> statisticsProxies;

        /// <summary>
        /// Cached statistics names
        /// </summary>
        List<string> cachedStatisticsNames;

        /// <summary>
        /// List of quantifiers proxies
        /// </summary>
        private Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier.AbstractQuantifierFunctionsPrx[] proxy;

        /// <summary>
        /// Dictionary determining which antecedent values to display
        /// </summary>
        private Dictionary<string, LiteralFilter> antecedentFilter = new Dictionary<string, LiteralFilter>();

        /// <summary>
        /// Dictionary determining which succedent values to display
        /// </summary>
        private Dictionary<string, LiteralFilter> succedentFilter = new Dictionary<string, LiteralFilter>();

        /// <summary>
        /// Dictionary determining which condition values to display
        /// </summary>
        private Dictionary<string, LiteralFilter> conditionFilter = new Dictionary<string, LiteralFilter>();

        #endregion


        #region Properties

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
                for (int i = 0; i < Hypotheses.Length; i++)
                {
                    if (HypothesisIsValid(Hypotheses[i]))
                    {
                        returnDict.Add(i, Hypotheses[i]);
                    }
                }
                return returnDict;
            }
        }

        /// <summary>
        /// Gets all hypotheses
        /// </summary>
        /// <returns>Array with all hypotheses</returns>
        public HypothesisStruct[] AllHypotheses
        {
            get
            {
                return this.Hypotheses;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="rm">Resource manager</param>
        public FerdaResult(ResourceManager rm)
        {
            this.resManager = rm;
        }

        #endregion


        #region Initialization methods

        /// <summary>
        /// Method to initialize the ResultBrowser structure
        /// </summary>
        /// <param name="hypotheses"></param>
        public void Initialize(HypothesisStruct[] hypotheses, QuantifierProvider[] used_quantifiers, List<Ferda.Statistics.StatisticsProviderPrx> statisticsProxies)
        {
            this.Hypotheses = hypotheses;
            this.statisticsProxies = statisticsProxies;

            //working with quantifiers - need to obtain all quantifier, for now working only with used ones
            this.UsedQuantifiers = used_quantifiers;
            this.SelectedQuantifiers = new int[this.UsedQuantifiers.Length];
            proxy = new AbstractQuantifierFunctionsPrx[this.UsedQuantifiers.Length];
            for (int i = 0; i < this.UsedQuantifiers.Length; i++)
            {
                this.proxy[i] = this.UsedQuantifiers[i].functions;
            }

            #region Caching

            //caching statistics names
            this.cachedStatisticsNames = this.GetStatisticsNames();

            //initializing hypotheses cache
            this.cachedHypotheses = new CachedHypothesis[this.Hypotheses.Length];

            Thread IceCommunicationThread = new Thread(new ThreadStart(IceCommunication));
            IceCommunicationThread.Start();         

            #endregion
        }

        /// <summary>
        /// Method to be launched in separate thread, takes long time to complete
        /// </summary>
        private void IceCommunication()
        {
            //caching hypotheses quantifiers and statistics values
            for (int i = 0; i < this.Hypotheses.Length; i++)
            {
                this.cachedHypotheses[i].QuantifiersList = this.GetQuantifiers(i);
                this.cachedHypotheses[i].StatisticsList = this.GetStatistics(i);
                this.OnIceTick();
            }
            this.InitFilters();
            this.OnIceComplete();
        }

        #endregion


        #region Column methods

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

        #endregion


        #region Methods for composing various strings

        /// <summary>
        /// Function to get a string value for antecedent
        /// </summary>
        /// <param name="hypothese">Hypothese to extract value from</param>
        /// <returns>String value with antecedent</returns>
        public static String GetAntecedentString(HypothesisStruct hypothesis)
        {
            StringBuilder returnString = new StringBuilder();
            bool firstRun = true;
            bool boolCedent = false;
            if (hypothesis.literals.Length > 0)
            {
                foreach (LiteralStruct literal in hypothesis.literals)
                {
                    if (literal.cedentType == CedentEnum.Antecedent)
                    {
                        returnString.Append(literal.literalName);
                    }
                }
            }
            else
            {
                boolCedent = true;
                foreach (BooleanLiteralStruct literal in hypothesis.booleanLiterals)
                {
                    if (literal.cedentType == CedentEnum.Antecedent)
                    {
                        if (!firstRun)
                        {
                            returnString.Append(") & ");
                        }
                        if (literal.negation)
                        {
                            returnString.Append('\u00AC' + literal.literalName + "(");
                        }
                        else
                        {
                            returnString.Append(literal.literalName + "(");
                        }
                        foreach (String category in literal.categoriesNames)
                        {
                            returnString.Append(category.ToString());
                        }
                        firstRun = false;
                    }
                }
            }
            if (returnString.Length == 0)
            {
                return String.Empty;
            }
            else
            {
                if (boolCedent)
                    returnString.Append(")");

                return returnString.ToString();
            }
        }

        /// <summary>
        /// Function to get a string value for first set
        /// </summary>
        /// <param name="hypothese">Hypothese to extract value from</param>
        /// <returns>String value with antecedent</returns>
        public static String GetFirstSetString(HypothesisStruct hypothesis)
        {
            StringBuilder returnString = new StringBuilder();
            bool firstRun = true;
            bool boolCedent = false;
            if (hypothesis.literals.Length > 0)
            {
                foreach (LiteralStruct literal in hypothesis.literals)
                {
                    if (literal.cedentType == CedentEnum.FirstSet)
                    {
                        returnString.Append(literal.literalName);
                    }
                }
            }
            else
            {
                boolCedent = true;
                foreach (BooleanLiteralStruct literal in hypothesis.booleanLiterals)
                {
                    if (literal.cedentType == CedentEnum.FirstSet)
                    {
                        if (!firstRun)
                        {
                            returnString.Append(") & ");
                        }
                        if (literal.negation)
                        {
                            returnString.Append('\u00AC' + literal.literalName + "(");
                        }
                        else
                        {
                            returnString.Append(literal.literalName + "(");
                        }
                        foreach (String category in literal.categoriesNames)
                        {
                            returnString.Append(category.ToString());
                        }
                        firstRun = false;
                    }
                }
            }
            if (returnString.Length == 0)
            {
                return String.Empty;
            }
            else
            {
                if (boolCedent)
                    returnString.Append(")");

                return returnString.ToString();
            }
        }

        /// <summary>
        /// Function to get a string value for second set
        /// </summary>
        /// <param name="hypothese">Hypothese to extract value from</param>
        /// <returns>String value with antecedent</returns>
        public static String GetSecondSetString(HypothesisStruct hypothesis)
        {
            StringBuilder returnString = new StringBuilder();
            bool firstRun = true;
            bool boolCedent = false;
            if (hypothesis.literals.Length > 0)
            {
                foreach (LiteralStruct literal in hypothesis.literals)
                {
                    if (literal.cedentType == CedentEnum.SecondSet)
                    {
                        returnString.Append(literal.literalName);
                    }
                }
            }
            else
            {
                boolCedent = true;
                foreach (BooleanLiteralStruct literal in hypothesis.booleanLiterals)
                {
                    if (literal.cedentType == CedentEnum.SecondSet)
                    {
                        if (!firstRun)
                        {
                            returnString.Append(") & ");
                        }
                        if (literal.negation)
                        {
                            returnString.Append('\u00AC' + literal.literalName + "(");
                        }
                        else
                        {
                            returnString.Append(literal.literalName + "(");
                        }
                        foreach (String category in literal.categoriesNames)
                        {
                            returnString.Append(category.ToString());
                        }
                        firstRun = false;
                    }
                }
            }
            if (returnString.Length == 0)
            {
                return String.Empty;
            }
            else
            {
                if (boolCedent)
                    returnString.Append(")");

                return returnString.ToString();
            }
        }

        /// <summary>
        /// Function to get a string value for succedent
        /// </summary>
        /// <param name="hypothese">Hypothese to extract value from</param>
        /// <returns>String value with succedent</returns>
        public static String GetSuccedentString(HypothesisStruct hypothese)
        {
            StringBuilder returnString = new StringBuilder();
            bool firstRun = true;
            bool boolCedent = false;

            if (hypothese.literals.Length > 0)
            {
                foreach (LiteralStruct literal in hypothese.literals)
                {
                    if (literal.cedentType == CedentEnum.Succedent)
                    {
                        returnString.Append(literal.literalName);
                    }
                }
            }
            else
            {
                boolCedent = true;
                foreach (BooleanLiteralStruct literal in hypothese.booleanLiterals)
                {
                    if (literal.cedentType == CedentEnum.Succedent)
                    {
                        if (!firstRun)
                        {
                            returnString.Append(") & ");
                        }
                        if (literal.negation)
                        {
                            returnString.Append('\u00AC' + literal.literalName + "(");
                        }
                        else
                        {
                            returnString.Append(literal.literalName + "(");
                        }
                        foreach (String category in literal.categoriesNames)
                        {
                            returnString.Append(category.ToString());
                        }
                        firstRun = false;
                    }
                }
            }

            if (returnString.Length == 0)
            {
                return String.Empty;
            }

            else
            {
                if (boolCedent)
                    returnString.Append(")");
                return returnString.ToString();
            }
        }


        /// <summary>
        /// Function to get a string value for condition
        /// </summary>
        /// <param name="hypothese">Hypothese to extract value from</param>
        /// <returns>String value with condition</returns>
        public static String GetConditionString(HypothesisStruct hypothese)
        {
            StringBuilder returnString = new StringBuilder();
            bool firstRun = true;
            foreach (BooleanLiteralStruct literal in hypothese.booleanLiterals)
            {
                if (literal.cedentType == CedentEnum.Condition)
                {
                    if (!firstRun)
                    {
                        returnString.Append(") & ");
                    }
                    if (literal.negation)
                    {
                        returnString.Append('\u00AC' + literal.literalName + "(");
                    }
                    else
                    {
                        returnString.Append(literal.literalName + "(");
                    }
                    foreach (String category in literal.categoriesNames)
                    {
                        returnString.Append(category.ToString());
                    }
                    firstRun = false;
                }
            }

            if (returnString.Length == 0)
            {
                return "";
            }
            else
            {
                returnString.Append(")");
                return returnString.ToString();
            }
        }

        /// <summary>
        /// Method to compose hypothesis name
        /// </summary>
        /// <param name="hypothese">Hypothese </param>
        /// <returns></returns>
        public static String GetHypothesisName(HypothesisStruct hypothesis)
        {
            StringBuilder returnString = new StringBuilder();
            if (hypothesis.literals.Length > 0)
            {
                string temp, temp1;
                temp = temp1 = String.Empty;
                temp = FerdaResult.GetAntecedentString(hypothesis);
                temp1 = FerdaResult.GetSuccedentString(hypothesis);
                if (temp != String.Empty)
                {
                    if (temp1 != String.Empty)
                    {
                        returnString.Append(temp + " " + '\u00D7' + " " + temp1);
                    }
                    else
                    {
                        returnString.Append(temp);
                    }
                }
                else
                {
                    if (temp1 != String.Empty)
                    {
                        returnString.Append(temp1);
                    }
                    else
                    {
                        returnString.Append(String.Empty);
                    }
                }
            }
            else
            {
                returnString.Append(FerdaResult.GetAntecedentString(hypothesis) +
                   '\u2022' + '\u2022' + FerdaResult.GetSuccedentString(hypothesis));
            }

            if (!FerdaResult.GetConditionString(hypothesis).Equals(""))
            {
                returnString.Append(" \\ " + FerdaResult.GetConditionString(hypothesis));
            }
            return returnString.ToString();
        }

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
        public List<string> GetUsedQuantifiersNames()
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
        /// Method which finds out whether the quantifier is selected.
        /// </summary>
        /// <param name="quantifier"></param>
        /// <returns></returns>
        protected bool QuantifierIsSelected(QuantifierProvider quantifier)
        {
            for (int i = 0; i < this.UsedQuantifiers.Length; i++)
            {
                if (quantifier == this.UsedQuantifiers[i])
                {
                    if (this.SelectedQuantifiers[i] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Method which finds out whether the quantifier is used.
        /// </summary>
        /// <param name="quantifier"></param>
        /// <returns></returns>
        protected bool QuantifierIsUsed(QuantifierProvider quantifier)
        {
            for (int i = 0; i < this.UsedQuantifiers.Length; i++)
            {
                if (quantifier == this.UsedQuantifiers[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method for reading the pre-counted quantifier from cache
        /// </summary>
        /// <param name="hypid">Id of the hypothesis to get values for</param>
        /// <param name="precision">Number of decimal places of precision</param>
        /// <returns>List of counted values</returns>
        public double[] ReadSelectedQuantifiersFromCache(int hypid, int precision)
        {
            double[] quantifiers = new double[cachedHypotheses[hypid].QuantifiersList.Length];
            for (int i = 0; i < cachedHypotheses[hypid].QuantifiersList.Length; i++)
            {
                if (this.GetUsedQuantifiersNames().IndexOf(cachedHypotheses[hypid].QuantifiersList[i].Name) != -1)
                {
                    quantifiers[i] = Math.Round(cachedHypotheses[hypid].QuantifiersList[i].Value, precision);
                }
            }
            return quantifiers;
        }

        /// <summary>
        /// Method for reading the pre-counted quantifier from cache
        /// </summary>
        /// <param name="hypid">Id of the hypothesis to get values for</param>
        /// <param name="precision">Number of decimal places of precision</param>
        /// <returns>List of counted values</returns>
        public double[] ReadAllQuantifiersFromCache(int hypid, int precision)
        {
            double[] quantifiers = new double[cachedHypotheses[hypid].QuantifiersList.Length];
            for (int i = 0; i < cachedHypotheses[hypid].QuantifiersList.Length; i++)
            {
                quantifiers[i] = Math.Round(cachedHypotheses[hypid].QuantifiersList[i].Value, precision);
            }
            return quantifiers;
        }

        /// <summary>
        /// Method for reading statistics from cache
        /// </summary>
        /// <param name="hypid">Hypothesis id to read statistics for</param>
        /// <param name="precision">Number of decimal places of precision</param>
        /// <returns>List of counted values</returns>
        public List<CountedValues> ReadStatisticsFromCache(int hypid, int precision)
        {
            List<CountedValues> statistics = new List<CountedValues>();
            for (int i = 0; i < cachedHypotheses[hypid].StatisticsList.Length; i++)
            {
                CountedValues temp = new CountedValues();
                temp.Name = cachedHypotheses[hypid].StatisticsList[i].Name;
                temp.Value = Math.Round(cachedHypotheses[hypid].StatisticsList[i].Value, precision);
                statistics.Add(temp);
            }
            return statistics;
        }

        /// <summary>
        /// Method which applies all quantifiers on the contingency table data.
        /// </summary>
        /// <param name="hypothese">Hypothese to take tables from</param>
        /// <returns>List of values for each quantifier</returns>
        private List<double> CountAllQuantifiers(HypothesisStruct hypothese)
        {
            List<double> returnList = new List<double>();
            for (int i = 0; i < this.UsedQuantifiers.Length; i++)
            {
                returnList.Add(this.proxy[i].Value(hypothese.quantifierSetting));
            }
            return returnList;
        }

        /// <summary>
        /// Method which applies all quantifiers on the hypothese contingency table
        /// </summary>
        /// <param name="hypothese"></param>
        /// <returns>Arraylist of values</returns>
        public List<double> AllQuantifierValues(HypothesisStruct hypothese)
        {
            return this.CountAllQuantifiers(hypothese);
        }

        #endregion


        #region Hypotheses methods      

        /// <summary>
        /// Gets hypothesis at the required index
        /// </summary>
        /// <param name="index">Index of the hypothesis to get</param>
        /// <returns></returns>
        public HypothesisStruct GetHypothese(int index)
        {
            return this.Hypotheses[index];
        }

        #endregion


        #region Caching methods

        /// <summary>
        /// Method for counting quantifiers values for hypothese
        /// </summary>
        /// <param name="HypId">Hypothesis id to count values for</param>
        /// <returns>Counted quantifiers values</returns>
        private CountedValues[] GetQuantifiers(int HypId)
        {

            List<string> names = this.GetAllQuantifierNames();
            List<double> values = this.CountAllQuantifiers(this.Hypotheses[HypId]);
            CountedValues[] quantifiers = new CountedValues[values.Count];

            for (int i = 0; i < values.Count; i++)
            {
                quantifiers[i].Name = names[i];
                quantifiers[i].Value = values[i];
            }
            return quantifiers;
        }

        /// <summary>
        /// Method for counting statistics values for hypothese
        /// </summary>
        /// <param name="HypId">Hypothesis id to count values for</param>
        /// <returns>Counted statistics values</returns>
        private CountedValues[] GetStatistics(int HypId)
        {
            CountedValues[] returnList = new CountedValues[this.statisticsProxies.Count];
            for (int i = 0; i < this.statisticsProxies.Count; i++)
            {
                returnList[i].Name = this.cachedStatisticsNames[i];
                returnList[i].Value = this.statisticsProxies[i].getStatistics(this.Hypotheses[HypId].quantifierSetting);
            }
            return returnList;
        }

        /// <summary>
        /// Method for getting statistics names
        /// </summary>
        /// <returns>List of statistics names</returns>
        private List<string> GetStatisticsNames()
        {
            List<string> names = new List<string>();
            foreach (Ferda.Statistics.StatisticsProviderPrx prx in this.statisticsProxies)
            {
                names.Add(prx.getStatisticsName());
            }
            return names;
        }

        /// <summary>
        /// Method for getting statistics name from cache
        /// </summary>
        /// <returns>List of strings with statistics names</returns>
        public List<string> ReadStatisticsNamesFromCache()
        {
            return this.cachedStatisticsNames;
        }

        #endregion


        #region Other methods

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

        #endregion


        #region Filtering methods

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

        #endregion


        #region Localization

        public void ChangeRm(ResourceManager rm)
        {
            this.resManager = rm;
        }

        #endregion
    }
}

