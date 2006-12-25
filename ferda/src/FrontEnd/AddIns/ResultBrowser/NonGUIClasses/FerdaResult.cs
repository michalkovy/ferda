// FerdaResult.cs - class for working with results of the taskrun
//
// Authors:   Alexander Kuzmin <alexander.kuzmin@gmail.com>
//            Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin, Martin Ralbovsky
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
using Ferda.Guha.MiningProcessor.Formulas;
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
    /// Struct for saving counted values of the quantifiers.
    /// </summary>
    public struct CountedValues
    {
        /// <summary>
        /// Name of the quantifier
        /// </summary>
        public string Name;
        /// <summary>
        /// User (localized) name of the quantifier
        /// </summary>
        public string UserName;
        /// <summary>
        /// Value of the quantifier
        /// </summary>
        public double Value;
        /// <summary>
        /// If the quantifier provides numerical value
        /// </summary>
        public bool ProvidesValue;
    };

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

        /// <summary>
        /// Provider of the bit strings to identify the names of categories
        /// for the KL and CF tasks
        /// </summary>
        private BitStringGeneratorProviderPrx bitStringProvider;

        /// <summary>
        /// The owner of addin to display the box runtime errors
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        #endregion

        #region Properties

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
                    return result.Hypotheses.Count;
                }
                catch
                {
                    return 0;
                }
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

        /// <summary>
        /// True when hypotheses in the result containt two contingency tables
        /// </summary>
        public bool TwoContingencyTables
        {
            get
            {
                return this.result.TwoContingencyTables;
            }
        }

        /// <summary>
        /// Determines the type of the task
        /// </summary>
        public TaskTypeEnum TaskType
        {
            get
            {
                return result.TaskTypeEnum;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default class constructor
        /// </summary>
        /// <param name="quantifiers">Applied quantifiers</param>
        /// <param name="result">String identifier of the result</param>
        /// <param name="bitStringProvider">
        /// Provider of the bit strings to identify the names of categories
        /// for the KL and CF tasks
        /// </param>
        /// <param name="owner">
        /// The owner of addin to display the box runtime errors
        /// </param>
        public FerdaResult(string result, Quantifiers quantifiers, 
            BitStringGeneratorProviderPrx bitStringProvider, 
            IOwnerOfAddIn owner)
        {
            this.quantifiers = quantifiers;
            this.result = SerializableResult.Deserialize(result);
            this.bitStringProvider = bitStringProvider;
            this.ownerOfAddIn = owner;

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
        /// Method to initialize the ResultBrowser structure. It 
        /// starts a separate thread that fills the <code>cachedHypotheses</code>
        /// structure with the information about the calculated hypotheses of the
        /// current task.
        /// </summary>
        public void Initialize()
        {
            //initializing hypotheses cache
            this.cachedHypotheses = new CachedHypothesisValues[this.result.Hypotheses.Count];

            //starting the thread
            Thread IceCommunicationThread = new Thread(new ThreadStart(IceCommunication));
            IceCommunicationThread.Start();         
        }

        /// <summary>
        /// Method to be launched in separate thread, takes long time to complete.
        /// It loads the calculated hypotheses of the task.
        /// </summary>
        private void IceCommunication()
        {
            //caching hypotheses quantifiers
            for (int i = 0; i < this.result.Hypotheses.Count; i++)
            {
                this.cachedHypotheses[i].Quantifiers = this.QuantifiersValues(i);
                //this.UnfoldHypothesis(i);
                this.OnIceTick();
            }
            //This is a little work around... 
            //The problem is that the thread loading the hypotheses can be much 
            //quicker (when there is only little hypotheses) then the main thread
            //doing all the initializations of the ResultBrowser control. The the
            //loading thread loads the hypotheses and displays them to the
            //hypotheses list, but they are erased by the main thread doing initilaizations
            //That is why we chose for this thread to sleep :)
            Thread.Sleep(50);
            this.OnIceComplete();

            //TODO: Sasa...Implement callback functions not to access datastructure from a separate thread
            //Formally not thread-safe, but I know what I am doing :-)
        }

        #endregion

        #region Events

        public event LongRunTick IceTicked;
        /// <summary>
        /// Delegate to deal with the Ice tick
        /// </summary>
        public void OnIceTick()
        {
            if (IceTicked != null)
            {
                IceTicked();
            }
        }

        public event LongRunCompleted IceComplete;
        /// <summary>
        /// Delegate to deal with the Ice complete
        /// </summary>
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
                CountedValues countedValue = new CountedValues();
                countedValue.Name = quantifier.LocalizedLabel;
                countedValue.UserName = quantifier.LocalizedUserLabel;

                //division between the quantifiers that provide 
                //numerical values a those that do not
                if (quantifier.ProvidesValues)
                {
                    countedValue.ProvidesValue = true;
                    countedValue.Value =
                        quantifier.Value(result.Hypotheses[index], 
                        result.AllObjectsCount);
                }
                else
                {
                    countedValue.ProvidesValue = false;
                }
                returnValues.Add(countedValue);
            }
            return returnValues.ToArray();
        }

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
                if (cachedHypotheses[hypid].Quantifiers[i].ProvidesValue)
                {
                    returnQuantifiers.Add(Math.Round(cachedHypotheses[hypid].Quantifiers[i].Value, precision));
                }
                else
                {
                    returnQuantifiers.Add(Double.NaN);
                }
            }
            return returnQuantifiers.ToArray();
        }

        #endregion

        #region Hypotheses methods

        /// <summary>
        /// Method to get string value of MarkEnums for the given hypothesis
        /// </summary>
        /// <param name="mark">MarkEnum to get String value for</param>
        /// <param name="hypothesis">The hypothesis</param>
        /// <returns>The string value of MarkEnums for hypothesis</returns>
        public string GetFormulaString(MarkEnum mark, Hypothesis hypothesis)
        {
            return hypothesis.GetFormula(mark).ToString();
        }

        /// <summary>
        /// Method to get string value of MarkEnums for the given hypothesis
        /// </summary>
        /// <param name="mark">MarkEnum to get String value for</param>
        /// <param name="hypothesisId">Hypothesis id</param>
        /// <returns>The string value of MarkEnums for hypothesis</returns>
        public string GetFormulaString(MarkEnum mark, int hypothesisId)
        {
            try
            {
                return this.result.Hypotheses[hypothesisId].GetFormula(mark).ToString();
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Returns names of categories of an attribute. To be used when displaying
        /// the KL and CF contingency tables
        /// </summary>
        /// <param name="formula">Formula to retrieve information from</param>
        /// <returns></returns>
        public string[] GetCategoryNames(Formula formula)
        {
            string[] result = null;

            //The formula should be a categorial attribute formula
            CategorialAttributeFormula f = formula as CategorialAttributeFormula;
            if (f == null)
            {
                return null;
            }

            //getting the BitStringGenerator
            GuidStruct guid = new GuidStruct(f.AttributeGuid);
            BitStringGeneratorPrx categoriesGenerator = 
                bitStringProvider.GetBitStringGenerator(guid);

            //getting the categories names - must be in a try block
            //(i.e. bad connection string)
            try
            {
                result = categoriesGenerator.GetCategoriesIds();
            }
            catch (Ferda.Modules.BoxRuntimeError e)
            {
                ownerOfAddIn.ShowBoxException(e);
            }

            return result;
        }

        #endregion
    }
}