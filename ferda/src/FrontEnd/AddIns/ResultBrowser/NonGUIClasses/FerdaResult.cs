using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FrontEnd.AddIns.ResultBrowser;
using Ferda.Modules;
using Ferda.Modules.Boxes.AbstractLMTask;
using Ferda.ModulesManager;
using System.Resources;
using System.Reflection;


namespace FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
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
        private QuantifierProvider[] AllQuantifiers;


        /// <summary>
        /// Array of used quantifiers.
        /// </summary>
        private QuantifierProvider[] UsedQuantifiers;


        /// <summary>
        /// Array of actual quantifiers - user can choose to use more quantifiers that were used in the beginning.
        /// Represented by the array of boolean flags.
        /// </summary>
        private int[] SelectedQuantifiers;

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
        public void IceRun(HypothesisStruct[] hypotheses, QuantifierProvider[] used_quantifiers)
        {
            this.Hypotheses = hypotheses;

            //working with quantifiers - need to obtain all quantifier, for now working only with used ones

            this.UsedQuantifiers = used_quantifiers;
            this.AllQuantifiers = this.UsedQuantifiers;
            this.SelectedQuantifiers = new int[this.AllQuantifiers.Length];
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

            foreach (QuantifierProvider quantifier in this.AllQuantifiers)
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
                if (this.GetColumnName(this.AllQuantifiers[i]) == columnName)
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
            for (int i = 0; i < this.AllQuantifiers.Length; i++)
            {
                if (this.GetColumnName(this.AllQuantifiers[i]) == columnName)
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
                    returnArray.Add(this.AllQuantifiers[i].localizedBoxLabel.ToString());
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
            foreach (BooleanLiteralStruct literal in hypothesis.boolenliterals)
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

            if (returnString.Length == 0)
            {
                return "Empty";
            }
            else
            {
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
            foreach (BooleanLiteralStruct literal in hypothese.boolenliterals)
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

            if (returnString.Length == 0)
            {
                return "Empty";
            }

            else
            {
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
            foreach (BooleanLiteralStruct literal in hypothese.boolenliterals)
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
            returnString.Append(FerdaResult.GetAntecedent(hypothesis) +

                '\u2022' + '\u2022' + FerdaResult.GetSuccedent(hypothesis));

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

        /*
        /// <summary>
        /// Method for getting hypothesis data
        /// </summary>
        /// <param name="hypothesis">Hypothesis to get data from</param>
        /// <returns>Formatted string with hypothesis data</returns>
        public String GetHypothesisData(HypothesisStruct hypothesis)
        {
            StringBuilder returnString = new StringBuilder();
            //HypothesisStruct hypothesis = (HypothesisStruct)this.Hypotheses[idHypothese];
           // returnString.Append(this.resManager.GetString("HypothesisId") + ": "
          //      + idHypothese + "\n\n" + this.resManager.GetString("Antecedent") + ":\t");

            if (String.IsNullOrEmpty(FerdaResult.GetAntecedent(hypothesis)))
            {
                returnString.Append("(" + this.resManager.GetString("NoRestriction") + ")\n");
            }
            else
            {
                returnString.Append(FerdaResult.GetAntecedent(hypothesis) + "\n");
            }

            returnString.Append(this.resManager.GetString("Succedent") + "\t");
            if (String.IsNullOrEmpty(FerdaResult.GetSuccedent(hypothesis)))
            {
                returnString.Append("(" + this.resManager.GetString("NoRestriction") + ")\n\n");
            }

            else
            {
                returnString.Append(FerdaResult.GetSuccedent(hypothesis) + "\n\n");
            }
            returnString.Append(this.resManager.GetString("ContingencyTable") + ": \n" +
            this.GetHypothesisTable(hypothesis) +
            "\n\n\n" +
            this.resManager.GetString("AppliedQuantifiers") + ":\n\n");

            foreach (QuantifierProvider quantifier in this.UsedQuantifiers)
            {
                returnString.Append(quantifier.localizedBoxLabel + " ("
                    + quantifier.userBoxLabel + "): " + "\t"
                + quantifier.functions.Value(hypothesis.quantifierSetting) + "\n\n");
            }
            return returnString.ToString();
        }
        */
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


        #region Quantifier functions

        /// <summary>
        /// Method which finds out whether the quantifier is selected.
        /// </summary>
        /// <param name="quantifier"></param>
        /// <returns></returns>
        protected bool QuantifierIsSelected(QuantifierProvider quantifier)
        {
            for (int i = 0; i < this.AllQuantifiers.Length; i++)
            {
                if (quantifier == this.AllQuantifiers[i])
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
        /// Method which applies quantifiers on the contingency table data.
        /// </summary>
        /// <param name="hypothese">hypothese to take table from</param>
        /// <returns>List of values for each quantifier</returns>
        protected List<double> ApplyQuantifiers(HypothesisStruct hypothese)
        {
            List<double> returnList = new List<double>();
            for (int i = 0; i < this.AllQuantifiers.Length; i++)
            {
                if (this.SelectedQuantifiers[i] == 1)
                {
                    returnList.Add(this.AllQuantifiers[i].functions.Value(hypothese.quantifierSetting));
                }
            }
            return returnList;
        }



        /// <summary>
        /// Method which applies quantifiers on the hypothese fourpole table
        /// </summary>
        /// <param name="hypothese"></param>
        /// <returns>Arraylist of values</returns>
        public List<double> QuantifierValues(HypothesisStruct hypothese)
        {
            return this.ApplyQuantifiers(hypothese);
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

        public HypothesisStruct GetHypothese(int index)
        {
            return this.Hypotheses[index];
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

