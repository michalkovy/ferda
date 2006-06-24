using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public class Quantifiers
    {
        private readonly Dictionary<string, Quantifier> _quantifeirs;
        
        private Quantifiers(int quantifiersCount)
        {
            _quantifeirs = new Dictionary<string, Quantifier>(quantifiersCount);
        }

        public Quantifiers(QuantifierBaseFunctionsPrx[] quantifiers, BitStringGeneratorProvider taskFuncPrx, string[] localePrefs)
            : this(quantifiers.Length)
        {
            foreach (QuantifierBaseFunctionsPrx prx in quantifiers)
            {
                Quantifier quantifier = new Quantifier(prx, taskFuncPrx, localePrefs);
                _quantifeirs.Add(quantifier.Setting.stringIceIdentity, quantifier);
            }
        }

        public Quantifiers(QuantifierBaseFunctionsPrx[] quantifiers, BitStringGeneratorProvider taskFuncPrx)
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
            // MissingInformationHandlingEnum missingInformationHandling;
            // ----------------------------------------------------------
            // pro 4ft-kvantifikátory použité v SD úlohách je (stejnì jako 
            // pro jiné než 4ft-kvantifikátory) nutné, aby byla nastavena 
            // vlastnost na Deleting.
            
            // OperationModeEnum operationMode;
            // --------------------------------
            // pro ne-SD tasky kontrolovat nastavení FirstSetFrequencies 
            // možnosti pro OperationModeEnum

            // bool needsNumericValues;
            // ------------------------
            // pøíslušný kategoriální atribut poskytuje numerické hodnoty 

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
    }
}
