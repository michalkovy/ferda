using System;

using System.Collections;

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

            //TODO

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

        /// <returns></returns>

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

        /// <returns></returns>

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

        public String GetAntecedent(HypothesisStruct hypothese)

        {

            String returnString = "";

            bool firstRun = true;

            foreach (BooleanLiteralStruct literal in hypothese.boolenliterals)

            {

                if (literal.cedentType == CedentEnum.Antecedent)

                {

                    if (!firstRun)

                    {

                        returnString = returnString + ") & ";

                    }

                    if (literal.negation)

                    {

                        returnString = returnString + '\u00AC' + literal.literalName + "(";

                    }

                    else

                    {

                        returnString = returnString + literal.literalName + "(";

                    }

                    foreach (String category in literal.categoriesNames)

                    {

                        returnString = returnString + category.ToString();

                    }

                    firstRun = false;

                }

            }     



            if (returnString.Equals(""))

            {

                return this.resManager.GetString("Empty");

            }

            else

            {

                returnString = returnString + ")";

                return returnString;

            }

        }



        /// <summary>

        /// Function to get a string value for succedent

        /// </summary>

        /// <param name="hypothese">Hypothese to extract value from</param>

        /// <returns>String value with succedent</returns>

        public String GetSuccedent(HypothesisStruct hypothese)

        {



            String returnString = "";

            bool firstRun = true;

            foreach (BooleanLiteralStruct literal in hypothese.boolenliterals)

            {

                if (literal.cedentType == CedentEnum.Succedent)

                {

                    if (!firstRun)

                    {

                        returnString = returnString + ") & ";

                    }

                    if (literal.negation)

                    {

                        returnString = returnString + '\u00AC' + literal.literalName + "(";

                    }

                    else

                    {

                        returnString = returnString + literal.literalName + "(";

                    }

                    foreach (String category in literal.categoriesNames)

                    {

                        returnString = returnString + category.ToString();

                    }

                    firstRun = false;

                }

            }



            if (returnString.Equals(""))

            {

                return this.resManager.GetString("Empty");

            }

            else

            {

                returnString = returnString + ")";

                return returnString;

            }

        }



        /// <summary>

        /// Function to get a string value for condition

        /// </summary>

        /// <param name="hypothese">Hypothese to extract value from</param>

        /// <returns>String value with condition</returns>

        public String GetCondition(HypothesisStruct hypothese)

        {



            String returnString = "";

            bool firstRun = true;

            foreach (BooleanLiteralStruct literal in hypothese.boolenliterals)

            {

                if (literal.cedentType == CedentEnum.Condition)

                {

                    if (!firstRun)

                    {

                        returnString = returnString + ") & ";

                    }

                    if (literal.negation)

                    {

                        returnString = returnString + '\u00AC' + literal.literalName + "(";

                    }

                    else

                    {

                        returnString = returnString + literal.literalName + "(";

                    }

                    foreach (String category in literal.categoriesNames)

                    {

                        returnString = returnString + category.ToString();

                    }

                    firstRun = false;

                }

            }



            if (returnString.Equals(""))

            {

                return "";

            }

            else

            {

                returnString = returnString + ")";

                return returnString;

            }



        }



        public String GetHypothesisName(HypothesisStruct hypothese)

        {

            String returnString = "";



            returnString = returnString + this.GetAntecedent(hypothese) +

                '\u2022' + '\u2022' + this.GetSuccedent(hypothese);



            if (!this.GetCondition(hypothese).Equals(""))

            {

                returnString = returnString + " \\ " + this.GetCondition(hypothese);

            }



            return returnString;

        }



        public String GetHypothesisTable(HypothesisStruct hypothese)

        {

            String returnString = "";

            foreach (int[] arr in hypothese.quantifierSetting.firstContingencyTableRows)

            {

                foreach (int value in arr)

                {

                    returnString = returnString + value.ToString() + "\t";

                }

                returnString = returnString + "\n";

            }



            foreach (int[] arr in hypothese.quantifierSetting.secondContingencyTableRows)

            {

                foreach (int value in arr)

                {

                    returnString = returnString + value.ToString() + "\t";

                }

                returnString = returnString + "\n";

            }

            return returnString;

        }

        public String GetHypothesisData(int idHypothese)

        {

            String returnString;



            HypothesisStruct hypothese = (HypothesisStruct)this.Hypotheses[idHypothese];



            returnString = this.resManager.GetString("HypothesisId") + ": " + idHypothese + "\n\n";

            returnString = returnString + this.resManager.GetString("Antecedent") + ":\t";



            if (String.IsNullOrEmpty(this.GetAntecedent(hypothese)))

            {

                returnString = returnString + "(" + this.resManager.GetString("NoRestriction") + ")\n";

            }

            else

            {



                returnString = returnString + this.GetAntecedent(hypothese) + "\n";

            }



            returnString = returnString + this.resManager.GetString("Succedent") + "\t";



            if (String.IsNullOrEmpty(this.GetSuccedent(hypothese)))

            {

                returnString = returnString + "(" + this.resManager.GetString("NoRestriction") + ")\n\n";

            }

            else

            {

                returnString = returnString + this.GetSuccedent(hypothese) + "\n\n";

            }



            returnString = returnString + this.resManager.GetString("ContingencyTable") + ": \n";



            returnString = returnString + this.GetHypothesisTable(hypothese);



            returnString = returnString + "\n\n\n";



            returnString = returnString + this.resManager.GetString("AppliedQuantifiers") + ":\n\n";



            foreach (QuantifierProvider quantifier in this.UsedQuantifiers)

            {

                returnString = returnString + quantifier.localizedBoxLabel + " ("

                    + quantifier.userBoxLabel + "): " + "\t"

                + quantifier.functions.Value(hypothese.quantifierSetting) + "\n\n";

            }

            return returnString;



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

        /// Method which applies quantifiers on the 4-pole table data.

        /// </summary>

        /// <param name="table">4-pole table</param>

        /// <returns>List of values for each quantifier</returns>

        protected ArrayList ApplyQuantifiers(HypothesisStruct hypothese)

        {

            ArrayList returnArray = new ArrayList();

            // int [][] tempArray = table.Rows;



            for (int i = 0; i < this.AllQuantifiers.Length; i++)

            {

                if (this.SelectedQuantifiers[i] == 1)

                {

                    returnArray.Add(this.AllQuantifiers[i].functions.Value(hypothese.quantifierSetting));

                }

            }

            return returnArray;

        }



        /// <summary>

        /// Method which applies quantifiers on the hypothese fourpole table

        /// </summary>

        /// <param name="hypothese"></param>

        /// <returns>Arraylist of values</returns>

        public ArrayList QuantifierValues(HypothesisStruct hypothese)

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

