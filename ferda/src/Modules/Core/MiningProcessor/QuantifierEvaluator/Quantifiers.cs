// Quantifiers.cs - represents set of quantifiers
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
using System.Diagnostics;
using Ferda.Guha.Data;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    /// <summary>
    /// Represents set of quantifiers for validation of
    /// relevant questions traced by a miner.
    /// </summary>
    public class Quantifiers
    {
        #region Private fields

        /// <summary>
        /// Number of invokes before next dynamic reorder of
        /// quantifiers
        /// </summary>
        private const int invokesBeforeNextReorderOfQuantifiers = 128;

        /// <summary>
        /// Set of quantifiers in the class
        /// </summary>
        private readonly Dictionary<string, Quantifier> _quantifiers;

        #endregion

        #region Properties

        /// <summary>
        /// Set of quantifiers in the class
        /// </summary>
        public Dictionary<string, Quantifier> Quantifeirs
        {
            get { return _quantifiers; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor initializing number of quantifiers
        /// </summary>
        /// <param name="quantifiersCount">Number of quantifiers</param>
        private Quantifiers(int quantifiersCount)
        {
            _quantifiers = new Dictionary<string, Quantifier>(quantifiersCount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Quantifiers"/> class.
        /// </summary>
        /// <param name="quantifiers">The quantifiers.</param>
        /// <param name="taskFuncPrx">The "numeric values providers" provider.</param>
        /// <param name="localePrefs">The locale prefs.</param>
        public Quantifiers(QuantifierBaseFunctionsPrx[] quantifiers, BitStringGeneratorProviderPrx taskFuncPrx,
                           string[] localePrefs)
            : this(quantifiers.Length)
        {
            foreach (QuantifierBaseFunctionsPrx prx in quantifiers)
            {
                Quantifier quantifier = new Quantifier(prx, taskFuncPrx, localePrefs);
                _quantifiers.Add(quantifier.Setting.stringIceIdentity, quantifier);
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Quantifiers"/> class.
        /// </summary>
        /// <param name="quantifiers">The quantifiers.</param>
        /// <param name="taskFuncPrx">The "numeric values providers" provider.</param>
        public Quantifiers(QuantifierBaseFunctionsPrx[] quantifiers, BitStringGeneratorProviderPrx taskFuncPrx)
            : this(quantifiers.Length)
        {
            foreach (QuantifierBaseFunctionsPrx prx in quantifiers)
            {
                Quantifier quantifier = new Quantifier(prx, taskFuncPrx);
                _quantifiers.Add(quantifier.Setting.stringIceIdentity, quantifier);
            }
        }

        #endregion

        /// <summary>
        /// Gets the setting of the base quantifier, if there is a base
        /// quantifier connected. Otherwise returns null
        /// </summary>
        /// <returns>Base quantifier setting or null</returns>
        public QuantifierSetting GetBaseQuantifierSetting()
        {
            //UNDONE this should return array of all used Base quantifiers (if more than one)
            foreach (Quantifier value in _quantifiers.Values)
            {
                if (value.Setting.boxTypeIdentifier == "GuhaMining.Quantifiers.FourFold.Others.Base")
                    return value.Setting;
            }
            return null;
        }

        /// <summary>
        /// Determines which request are valid for this set of
        /// quantifiers. Returns the information to the parameters.
        /// </summary>
        /// <param name="notOnlyFirstSetOperationMode">
        /// If there exist operation mode different than 
        /// <c>FirstSetFrequencies</c>. For SD tasks only.
        /// </param>
        /// <param name="needsNumericValues">
        /// If there is a quantifier that needs numeric values.
        /// </param>
        /// <param name="notOnlyDeletingMissingInformation">
        /// If there is a quantifier that requires information
        /// handling different then <c>Deleting</c>
        /// </param>
        /// <param name="maximalRequestedCardinality">
        /// What is the maximal cardinality requested by 
        /// quantifiers
        /// </param>
        public void ValidRequests(
            out bool notOnlyFirstSetOperationMode,
            out bool needsNumericValues,
            out bool notOnlyDeletingMissingInformation,
            out CardinalityEnum maximalRequestedCardinality
            )
        {
            // OperationModeEnum operationMode;
            // --------------------------------
            // pro ne-SD tasky kontrolovat nastavení FirstSetFrequencies 
            // možnosti pro OperationModeEnum

            // bool needsNumericValues;
            // ------------------------
            // pøíslušný kategoriální atribut poskytuje numerické hodnoty 

            // MissingInformationHandlingEnum missingInformationHandling;
            // ----------------------------------------------------------
            // pro 4ft-kvantifikátory použité v SD úlohách je (stejnì jako 
            // pro jiné než 4ft-kvantifikátory) nutné, aby byla nastavena 
            // vlastnost na Deleting.

            // Ferda::Guha::Data::CardinalityEnum supportedData;
            // -------------------------------------------------
            // všechny použité kategoriální atributy jsou stejné nebo 
            // vyšší (ordinální < kardinální) kardinality než vyžadují použité 
            // kvantifikátory.

            // UNDONE
            // bool supportsFloatContingencyTable;

            notOnlyFirstSetOperationMode = false;
            needsNumericValues = false;
            notOnlyDeletingMissingInformation = false;
            maximalRequestedCardinality = CardinalityEnum.Nominal;

            foreach (Quantifier value in _quantifiers.Values)
            {
                if (value.Setting.operationMode != OperationModeEnum.FirstSetFrequencies)
                {
                    notOnlyFirstSetOperationMode = true;
                }
                if (value.Setting.needsNumericValues)
                {
                    needsNumericValues = true;
                }
                if (value.Setting.missingInformationHandling != MissingInformationHandlingEnum.Deleting)
                {
                    notOnlyDeletingMissingInformation = true;
                }
                maximalRequestedCardinality = Common.GreaterCardinalityEnums(
                    value.Setting.supportedData,
                    maximalRequestedCardinality
                    );
            }
        }

        #region Sort quantifiers by efficiency

        /// <summary>
        /// Sorts quantifiers according to actual efficiency. Acual efficiency
        /// of the quantifier is used.
        /// </summary>
        /// <param name="quantifiers">Quantifiers to be sorted</param>
        /// <returns>Sorted quantifiers</returns>
        private ICollection<Quantifier> sortByActualEfficiency(ICollection<Quantifier> quantifiers)
        {
            if (quantifiers.Count <= 1)
                return quantifiers;

            SortedList<double, Quantifier> result = new SortedList<double, Quantifier>(quantifiers.Count);
            foreach (Quantifier quantifier in quantifiers)
            {
                double actualEfficiency = quantifier.ActualEfficiency;
                if (actualEfficiency <= 0.000000001d)
                    actualEfficiency = 0.00000001d; //UNDONE
                while (result.ContainsKey(actualEfficiency))
                    actualEfficiency *= 1.1d; //UNDONE
                result.Add(actualEfficiency, quantifier);
            }
            return result.Values;
        }

        private List<Quantifier> _sortedByEfficiency = null;
        private List<Quantifier> _qDifficult;
        private List<Quantifier> _qQuiteDifficult;
        private List<Quantifier> _qMedium;
        private List<Quantifier> _qQuiteEasy;
        private List<Quantifier> _qEasy;
        private int _invokes = 0;

        /// <summary>
        /// Gets quantifiers sorted by efficiency. The sorting is done
        /// according to the difficulty of quantifier and according to
        /// actual efficiency.
        /// </summary>
        /// <returns></returns>
        private List<Quantifier> getQuantifiersSortedByEfficiency()
        {
            lock (this)
            {
                if (_invokes > invokesBeforeNextReorderOfQuantifiers || _sortedByEfficiency == null)
                {
                    if (_sortedByEfficiency == null)
                    {
                        _sortedByEfficiency = new List<Quantifier>(_quantifiers.Count);

                        #region Divide by performance difficulty

                        _qDifficult = new List<Quantifier>();
                        _qQuiteDifficult = new List<Quantifier>();
                        _qMedium = new List<Quantifier>();
                        _qQuiteEasy = new List<Quantifier>();
                        _qEasy = new List<Quantifier>();

                        foreach (KeyValuePair<string, Quantifier> pair in _quantifiers)
                        {
                            switch (pair.Value.PerformanceDifficulty)
                            {
                                case PerformanceDifficultyEnum.Difficult:
                                    _qDifficult.Add(pair.Value);
                                    break;
                                case PerformanceDifficultyEnum.QuiteDifficult:
                                    _qQuiteDifficult.Add(pair.Value);
                                    break;
                                case PerformanceDifficultyEnum.Medium:
                                    _qMedium.Add(pair.Value);
                                    break;
                                case PerformanceDifficultyEnum.QuiteEasy:
                                    _qQuiteEasy.Add(pair.Value);
                                    break;
                                case PerformanceDifficultyEnum.Easy:
                                    _qEasy.Add(pair.Value);
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }

                        #endregion
                    }

                    // make new sort
                    _sortedByEfficiency.Clear();
                    _sortedByEfficiency.AddRange(sortByActualEfficiency(_qEasy));
                    _sortedByEfficiency.AddRange(sortByActualEfficiency(_qQuiteEasy));
                    _sortedByEfficiency.AddRange(sortByActualEfficiency(_qMedium));
                    _sortedByEfficiency.AddRange(sortByActualEfficiency(_qQuiteDifficult));
                    _sortedByEfficiency.AddRange(sortByActualEfficiency(_qDifficult));
                }
                return _sortedByEfficiency;
            }
        }

        #endregion

        /// <summary>
        /// The method takes out of the list of contingency tables
        /// the tables that are 'good', which is determined by
        /// a list of good flags.
        /// </summary>
        /// <param name="contingencyTables">List of contingency tables</param>
        /// <param name="goodsFlags">List of good flags</param>
        /// <param name="onlyGoodsIndexes">Indexes of contingency tables,
        /// that are good.</param>
        /// <returns>List of good contingency tables.</returns>
        private List<ContingencyTableHelper> onlyGoods(List<ContingencyTableHelper> contingencyTables, List<bool> goodsFlags, out List<int> onlyGoodsIndexes)
        {
            List<ContingencyTableHelper> result = new List<ContingencyTableHelper>();
            onlyGoodsIndexes = new List<int>();

            for (int i = 0; i < goodsFlags.Count; i++ )
            {
                if (goodsFlags[i])
                {
                    onlyGoodsIndexes.Add(i);
                    result.Add(contingencyTables[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// The method takes out of the list of contingency tables
        /// the tables that are 'good', which is determined by
        /// an array of good flags.
        /// </summary>
        /// <param name="contingencyTables">List of contingency tables</param>
        /// <param name="goodsFlags">Array of good flags</param>
        /// <param name="onlyGoodsIndexes">Indexes of contingency tables,
        /// that are good.</param>
        /// <returns>List of good contingency tables.</returns>
        private List<ContingencyTableHelper> onlyGoods(ContingencyTableHelper [] contingencyTables, bool [] goodsFlags, out List<int> onlyGoodsIndexes)
        {
            List<ContingencyTableHelper> result = new List<ContingencyTableHelper>();
            onlyGoodsIndexes = new List<int>();
            
            for (int i = 0; i < goodsFlags.Length; i++)
            {
                if (goodsFlags[i])
                {
                    onlyGoodsIndexes.Add(i);
                    result.Add(contingencyTables[i]);
                }
            }
            return result;
        }
        
        /// <summary>
        /// Updates the current list of good flags with new a new list.
        /// </summary>
        /// <param name="goodsFlags">The good flags list</param>
        /// <param name="updateFlags">The update flags array</param>
        /// <param name="indexes">The indexes to orginal field</param>
        private void updateGoods(List<bool> goodsFlags, bool[] updateFlags, List<int> indexes)
        {
            for (int i = 0; i < updateFlags.Length; i++)
            {
                if (!updateFlags[i])
                    goodsFlags[indexes[i]] = false;
            }
        }

        /// <summary>
        /// Updates the current array of good flags with new a new list.
        /// </summary>
        /// <param name="goodsFlags">The good flags array</param>
        /// <param name="updateFlags">The update flags array</param>
        /// <param name="indexes">The indexes to orginal field</param>
        private void updateGoods(bool [] goodsFlags, bool[] updateFlags, List<int> indexes)
        {
            for (int i = 0; i < updateFlags.Length; i++)
            {
                if (!updateFlags[i])
                    goodsFlags[indexes[i]] = false;
            }
        }

        /// <summary>
        /// If a contigency table is valid for all the
        /// quantifiers in this class.
        /// </summary>
        /// <param name="contingencyTable">Contingency table</param>
        /// <returns>Iff it is valid</returns>
        public bool Valid(ContingencyTableHelper contingencyTable)
        {
            if (contingencyTable.IsEmpty)
                return false;
            foreach (Quantifier q in getQuantifiersSortedByEfficiency())
            {
                if (!q.Valid(contingencyTable))
                    return false;
                else
                    continue;
            }
            return true;
        }

        /// <summary>
        /// Validates a list of contingency table agains all the containing 
        /// quantifiers
        /// </summary>
        /// <param name="contingencyTables">List of contingency tables</param>
        /// <returns>A boolean vectors saying which contingency table
        /// was valid</returns>
        public List<bool> Valid(List<ContingencyTableHelper> contingencyTables)
        {
            List<bool> result = new List<bool>(contingencyTables.Count);
            List<ContingencyTableHelper> onlyGoods;
            List<int> onlyGoodsIndexes;

            for (int i = 0; i < contingencyTables.Count; i++)
            {
                    result.Insert(i, !(contingencyTables[i].IsEmpty));
            }
                
            foreach (Quantifier q in getQuantifiersSortedByEfficiency())
            {
                onlyGoods = this.onlyGoods(contingencyTables, result, out onlyGoodsIndexes);
                if (onlyGoods.Count == 0)
                    return result;
                updateGoods(result, q.Valid(onlyGoods), onlyGoodsIndexes);
            }
            return result;
        }

        /// <summary>
        /// Validates a list of contingency table agains all the containing 
        /// quantifiers
        /// </summary>
        /// <param name="contingencyTables">List of contingency tables</param>
        /// <returns>A boolean vectors saying which contingency table
        /// was valid</returns>
        public bool [] Valid(ContingencyTableHelper [] contingencyTables)
        {
            bool [] result = new bool[contingencyTables.Length];
            List<ContingencyTableHelper> onlyGoods;
            List<int> onlyGoodsIndexes;

            for (int i = 0; i < contingencyTables.Length; i++)
            {
                result[i]=!(contingencyTables[i].IsEmpty);
            }

            foreach (Quantifier q in getQuantifiersSortedByEfficiency())
            {
                onlyGoods = this.onlyGoods(contingencyTables, result, out onlyGoodsIndexes);
                if (onlyGoods.Count == 0)
                    return result;
                updateGoods(result, q.Valid(onlyGoods), onlyGoodsIndexes);
            }
            return result;
        }

        /// <summary>
        /// Computes values of containing quantifiers for a given 
        /// contingency table.
        /// </summary>
        /// <param name="contingencyTable">The contingency table</param>
        /// <returns>Array of validity values</returns>
        public double[] Values(ContingencyTableHelper contingencyTable)
        {
            double[] result = new double[_quantifiers.Count];

            //UNDONE mozna kontrolovat zda vsechny pouzite kvantifikatory poskytuji ciselne hodnoty
            if (contingencyTable.IsEmpty)
                return result;
            int i = 0;
            foreach (Quantifier q in _quantifiers.Values)
            {
                result[i] = q.Value(contingencyTable);
                i++;
            }
            return result;
        }

        /// <summary>
        /// Verifies the difference of quantifiers for the SD 
        /// <c>Difference of quantifiers</c> mode (between the first set and the second set).
        /// </summary>
        /// <param name="sDFirstSetValues">Values of quantifiers for the first set</param>
        /// <param name="sDSecondSetValues">Values of quantifiers for the second set</param>
        /// <returns>Iff valid (all differences are in given relation)</returns>
        public bool VerifySdDifferenceOfQuantifierValues(double[] sDFirstSetValues, double[] sDSecondSetValues)
        {
            int i = 0;
            Debug.Assert(sDFirstSetValues.Length == sDSecondSetValues.Length);
            Debug.Assert(sDSecondSetValues.Length > 0);
            foreach (Quantifier q in _quantifiers.Values)
            {
                if (!
                Ferda.Guha.Math.Common.Compare(
                    q.Setting.relation,
                    sDFirstSetValues[i] - sDSecondSetValues[i],
                    q.Setting.treshold
                    )
                )
                    return false;
                i++;
            }
            return true;
        }

        #region Used SD operation modes in quantifiers

        //public class UsedSDQuantifiersOpeartionModesClass
        //{
        //    public bool DifferenceOfFrequencies = false;
        //    public List<Quantifier> QDifferenceOfFrequencies = new List<Quantifier>();

        //    public bool DifferenceOfQuantifierValues = false;
        //    public List<Quantifier> QDifferenceOfQuantifierValues = new List<Quantifier>();

        //    public bool FirstSetFrequencies = false;
        //    public List<Quantifier> QFirstSetFrequencies = new List<Quantifier>();

        //    public bool SecondSetFrequencies = false;
        //    public List<Quantifier> QSecondSetFrequencies = new List<Quantifier>();
        //}

        //private UsedSDQuantifiersOpeartionModesClass _usedSDQuantifiersOpeartionModes = null;
        //public UsedSDQuantifiersOpeartionModesClass UsedSDQuantifiersOpeartionModes
        //{
        //    get
        //    {
        //        if (_usedSDQuantifiersOpeartionModes == null)
        //        {
        //            _usedSDQuantifiersOpeartionModes = usedSDQuantifiersOperationModes();
        //        }
        //        return _usedSDQuantifiersOpeartionModes;
        //    }
        //}

        //private UsedSDQuantifiersOpeartionModesClass usedSDQuantifiersOperationModes()
        //{
        //    UsedSDQuantifiersOpeartionModesClass result = new UsedSDQuantifiersOpeartionModesClass();
        //    foreach (Quantifier value in _quantifeirs.Values)
        //    {
        //        switch (value.Setting.operationMode)
        //        {
        //            case OperationModeEnum.DifferenceOfFrequencies:
        //                result.DifferenceOfFrequencies = true;
        //                result.QDifferenceOfFrequencies.Add(value);
        //                break;
        //            case OperationModeEnum.DifferenceOfQuantifierValues:
        //                result.DifferenceOfQuantifierValues = true;
        //                result.QDifferenceOfQuantifierValues.Add(value);
        //                break;
        //            case OperationModeEnum.FirstSetFrequencies:
        //                result.FirstSetFrequencies = true;
        //                result.QFirstSetFrequencies.Add(value);
        //                break;
        //            case OperationModeEnum.SecondSetFrequencies:
        //                result.SecondSetFrequencies = true;
        //                result.QSecondSetFrequencies.Add(value);
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    return result;
        //} 

        #endregion
    }
}