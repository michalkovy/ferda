using System;
using System.Collections.Generic;
using Ferda.Guha.Data;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public class Quantifiers
    {
        public const int InvokesBeforeNextReorderOfQuantifiers = 128;

        private readonly Dictionary<string, Quantifier> _quantifeirs;

        public Dictionary<string, Quantifier> Quantifeirs
        {
            get { return _quantifeirs; }
        }

        private Quantifiers(int quantifiersCount)
        {
            _quantifeirs = new Dictionary<string, Quantifier>(quantifiersCount);
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
                _quantifeirs.Add(quantifier.Setting.stringIceIdentity, quantifier);
            }
        }

        public Quantifiers(QuantifierBaseFunctionsPrx[] quantifiers, BitStringGeneratorProviderPrx taskFuncPrx)
            : this(quantifiers.Length)
        {
            foreach (QuantifierBaseFunctionsPrx prx in quantifiers)
            {
                Quantifier quantifier = new Quantifier(prx, taskFuncPrx);
                _quantifeirs.Add(quantifier.Setting.stringIceIdentity, quantifier);
            }
        }

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

            foreach (Quantifier value in _quantifeirs.Values)
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

        private ICollection<Quantifier> sortByActualEfficiency(ICollection<Quantifier> quantifiers)
        {
            if (quantifiers.Count <= 1)
                return quantifiers;

            SortedList<double, Quantifier> result = new SortedList<double, Quantifier>(quantifiers.Count);
            foreach (Quantifier quantifier in quantifiers)
            {
                double actualEfficiency = quantifier.ActualEfficiency;
                while (result.ContainsKey(actualEfficiency))
                    actualEfficiency *= 1.0000001d;
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

        private List<Quantifier> getQuantifiersSortedByEfficiency()
        {
            lock (this)
            {
                if (_invokes > InvokesBeforeNextReorderOfQuantifiers || _sortedByEfficiency == null)
                {
                    if (_sortedByEfficiency == null)
                    {
                        _sortedByEfficiency = new List<Quantifier>(_quantifeirs.Count);

                        #region Divide by performance difficulty

                        _qDifficult = new List<Quantifier>();
                        _qQuiteDifficult = new List<Quantifier>();
                        _qMedium = new List<Quantifier>();
                        _qQuiteEasy = new List<Quantifier>();
                        _qEasy = new List<Quantifier>();

                        foreach (KeyValuePair<string, Quantifier> pair in _quantifeirs)
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

        public bool Valid(ContingencyTableHelper contingencyTable)
        {
            foreach (Quantifier q in getQuantifiersSortedByEfficiency())
            {
                if (!q.Valid(contingencyTable))
                    return false;
                else
                    continue;
            }
            return true;
        }
    }
}