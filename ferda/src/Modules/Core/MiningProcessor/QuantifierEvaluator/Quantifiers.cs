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
            // pro 4ft-kvantifik�tory pou�it� v SD �loh�ch je (stejn� jako 
            // pro jin� ne� 4ft-kvantifik�tory) nutn�, aby byla nastavena 
            // vlastnost na Deleting.
            
            // OperationModeEnum operationMode;
            // --------------------------------
            // pro ne-SD tasky kontrolovat nastaven� FirstSetFrequencies 
            // mo�nosti pro OperationModeEnum

            // bool needsNumericValues;
            // ------------------------
            // p��slu�n� kategori�ln� atribut poskytuje numerick� hodnoty 

            // Ferda::Guha::Data::CardinalityEnum supportedData;
            // -------------------------------------------------
            // v�echny pou�it� kategori�ln� atributy jsou stejn� nebo 
            // vy��� (ordin�ln� < kardin�ln�) kardinality ne� vy�aduj� pou�it� 
            // kvantifik�tory.
            
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
