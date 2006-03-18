using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FrontEnd.AddIns.ResultBrowser;
using Ferda.Modules;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.ModulesManager;
using System.Resources;
using System.Reflection;


namespace FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
    public struct Tuple : IComparable
    {
        int HypothesisId;
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
    /// Structure for storing cached quantifiers and statistics
    /// </summary>
    public struct CachedHypothesis
    {
        public CountedValues[] StatisticsList;
        public CountedValues[] QuantifiersList;
    }


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
        /// <param name="i">Integer array (4-pole table)</param>
        /// <returns>A return value.</returns>
        private delegate int Quantifier(int[][] i);


        /// <summary>
        /// Array of all quantifiers.
        /// </summary>
        //  private QuantifierProvider[] AllQuantifiers;


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

        #endregion


        #region Constructor

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

            //caching hypotheses quantifiers and statistics values
            for (int i = 0; i < this.Hypotheses.Length; i++)
            {
                this.cachedHypotheses[i].QuantifiersList = this.GetQuantifiers(i);
                this.cachedHypotheses[i].StatisticsList = this.GetStatistics(i);
                this.OnIceTick();
            }

            #endregion
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
        public static String GetAntecedent(HypothesisStruct hypothesis)
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
        /// Function to get a string value for succedent
        /// </summary>
        /// <param name="hypothese">Hypothese to extract value from</param>
        /// <returns>String value with succedent</returns>
        public static String GetSuccedent(HypothesisStruct hypothese)
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
        public static String GetCondition(HypothesisStruct hypothese)
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
                temp = FerdaResult.GetAntecedent(hypothesis);
                temp1 = FerdaResult.GetSuccedent(hypothesis);
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
                returnString.Append(FerdaResult.GetAntecedent(hypothesis) +
                   '\u2022' + '\u2022' + FerdaResult.GetSuccedent(hypothesis));
            }

            if (!FerdaResult.GetCondition(hypothesis).Equals(""))
            {
                returnString.Append(" \\ " + FerdaResult.GetCondition(hypothesis));
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

        public event IceTick IceTicked;
        public void OnIceTick()
        {
            if (IceTicked != null)
            {
                IceTicked();
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

        /*
        /// <summary>
        /// Method which applies selected quantifiers on the contingency table data.
        /// </summary>
        /// <param name="hypothese">Hypothese to take tables from</param>
        /// <returns>List of values for each quantifier</returns>
        protected List<double> CountSelectedQuantifiers(HypothesisStruct hypothese)
        {
            List<double> returnList = new List<double>();
            for (int i = 0; i < this.UsedQuantifiers.Length; i++)
            {
                if (this.SelectedQuantifiers[i] == 1)
                {
                    returnList.Add(this.proxy[i].Value(hypothese.quantifierSetting));
                }
            }
            return returnList;
        }*/

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



        /*
        /// <summary>
        /// Method which applies selected quantifiers on the hypothese fourpole table
        /// </summary>
        /// <param name="hypothese"></param>
        /// <returns>Arraylist of values</returns>
        public List<double> SelectedQuantifierValues(HypothesisStruct hypothese, int precision)
        {
            return this.CountSelectedQuantifiers(hypothese, precision);
        }*/

        /// <summary>
        /// Method which applies all quantifiers on the hypothese fourpole table
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
        /// Method which gets all the hypotheses in the result
        /// </summary>
        /// <returns>Arraylist with all hypotheses</returns>
        public HypothesisStruct[] GetAllHypotheses()
        {
            return this.Hypotheses;
        }

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


        #region Localization

        public void ChangeRm(ResourceManager rm)
        {
            this.resManager = rm;
        }

        #endregion
    }
}

