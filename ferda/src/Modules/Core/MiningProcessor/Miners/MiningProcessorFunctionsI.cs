// MuningProcessorFunctionsI.cs - The mining processor Ice interface
//
// Authors: Tom� Kucha� <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovsk� <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tom� Kucha�
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
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ice;
using Exception = System.Exception;

namespace Ferda.Guha.MiningProcessor.Miners
{
    /// <summary>
    /// Implementation of the Ice mining processor functions. This class is the
    /// entry point to the mining processor module. 
    /// </summary>
    public class MiningProcessorFunctionsI : MiningProcessorFunctionsDisp_
    {
        #region Private fields

        /// <summary>
        /// Identification of the virtual hypotheses attribute
        /// </summary>
        private static GuidStruct _attributeId;

        /// <summary>
        /// The count vector for the virtual hypotheses attribute
        /// </summary>
        private static int[] _countVector;

        /// <summary>
        /// Boolean attribute entity enumerators
        /// </summary>
        private IEnumerator<KeyValuePair<string, BitStringIce>> _booleanTraceEnumerator = null;
        
        /// <summary>
        /// Proxy of the progress task
        /// </summary>
        ProgressTaskPrx progressPrx = null;

        /// <summary>
        /// Proxy of the progress bar
        /// </summary>
        ProgressBarPrx progressBarPrx = null;

        /// <summary>
        /// Ice current information
        /// </summary>
        Ice.Current _current = null;

        #endregion

        /// <summary>
        /// Converts task type to string
        /// </summary>
        /// <param name="taskType">Task type enumeration</param>
        /// <returns>String representation of the task type</returns>
        private static string taskTypeToString(TaskTypeEnum taskType)
        {
            switch (taskType)
            {
                case TaskTypeEnum.CF:
                    return "CF";
                case TaskTypeEnum.FourFold:
                    return "4ft";
                case TaskTypeEnum.KL:
                    return "KL";
                case TaskTypeEnum.SDCF:
                    return "SDCF";
                case TaskTypeEnum.SDFourFold:
                    return "SD4ft";
                case TaskTypeEnum.SDKL:
                    return "SDKL";
                case TaskTypeEnum.ETree:
                    return "ETree";
                default:
                    throw new NotImplementedException();
            }
        }

        #region Ice functions

        /// <summary>
        /// Retrieves next bit string. This type of computation is used by the
        /// virtual hypotheses attribute boxes. They do not compute the whole task,
        /// they return bit strings corresponding to individual relevant questions
        /// instead. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Bit string</returns>
        public override BitStringIceWithCategoryId GetNextBitString(Current current__)
        {
            if (_booleanTraceEnumerator.MoveNext())
            {
                BitStringIceWithCategoryId _tmpTuple = new BitStringIceWithCategoryId();
                _tmpTuple.categoryId = _booleanTraceEnumerator.Current.Key;
                _tmpTuple.bitString = _booleanTraceEnumerator.Current.Value;
                return _tmpTuple;
            }
            else
            {
                if (progressBarPrx != null)
                {
                    progressBarPrx.done();
                    //System.Threading.Thread.Sleep(100);
                    ProgressTaskI.Destroy(_current.adapter, progressPrx);
                }
                return null;
            }
        }

        /// <summary>
        /// Executes a run of a GUHA task. All the needed information are
        /// in the parameters. It executes the
        /// <see cref="T:Ferda.Guha.MiningProcessor.Miners.MiningProcessorBase.Trace"/>
        /// method to get results of a run.
        /// </summary>
        /// <param name="boxModule">The task box module</param>
        /// <param name="booleanAttributes">Boolean attributes connected to the task</param>
        /// <param name="categorialAttributes">Categorial attributes connected to the task</param>
        /// <param name="quantifiers">Quantifiers connected to the task</param>
        /// <param name="taskParams">Task parameters</param>
        /// <param name="bitStringGenerator">The bit string generator provider
        /// (should be only one for the whole task)</param>
        /// <param name="output">Where the progress of the task should be written</param>
        /// <param name="attributeId">Id of the task attribute - valid only for
        /// virtual hypotheses attributes.</param>
        /// <param name="countVector">Count vector - valid only for the
        /// virtual hypotheses attributes.</param>
        /// <param name="resultInfo">In this string serialized informations
        /// about the task run will be stored in from
        /// <see cref="Ferda.Guha.MiningProcessor.SerializableResultInfo"/></param>
        /// <param name="current__">Ice current information</param>
        /// <returns>Serialized result of the task in form
        /// <see cref="Ferda.Guha.MiningProcessor.SerializableResult"/> is returned.
        /// </returns>
        public override string Run(
            BoxModulePrx boxModule,
            BooleanAttribute[] booleanAttributes,
            CategorialAttribute[] categorialAttributes,
            QuantifierBaseFunctionsPrx[] quantifiers,
            TaskRunParams taskParams,
            BitStringGeneratorProviderPrx bitStringGenerator,
            OutputPrx output,
            GuidStruct attributeId,
            int[] countVector,
            out string resultInfo,
            Current current__
            )
        {
            // Ice peformance
            BitStringCache.IceCalls = 0;
            BitStringCache.IceTicks = 0;
            Quantifier.IceCalls = 0;
            Quantifier.IceTicks = 0;
            long before = DateTime.Now.Ticks;

            ProgressTaskListener progressListener = null;

            string label = String.Empty;
            string result = String.Empty;

            _current = current__;

            //Writing initialization information to the progress bar 
            switch (taskParams.resultType)
            {
                case ResultTypeEnum.Trace:
                    label = taskTypeToString(taskParams.taskType) + "-Task";
                    progressListener = new ProgressTaskListener();
                    progressPrx =
                        ProgressTaskI.Create(_current.adapter, progressListener);
                    progressBarPrx =
                        output.startProgress(progressPrx, label, label + " running...");
                    progressBarPrx.setValue(-1, "Loading ...");
                    break;

                case ResultTypeEnum.TraceBoolean:
                    label = taskTypeToString(taskParams.taskType) + "-SubTask";
                    progressListener = new ProgressTaskListener();
                    progressPrx =
                        ProgressTaskI.Create(_current.adapter, progressListener);
                    progressBarPrx =
                        output.startProgress(progressPrx, label, label + " running...");
                    progressBarPrx.setValue(-1, "Loading ...");
                    break;

                case ResultTypeEnum.TraceReal:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }

            //Creating mining processor of the right kind
            MiningProcessorBase miningProcessor = null;
            try
            {
                switch (taskParams.taskType)
                {
                    case TaskTypeEnum.FourFold:
                        miningProcessor = new FourFoldMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.CF:
                        miningProcessor = new CFMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.KL:
                        miningProcessor = new KLMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.SDCF:
                        miningProcessor = new SDCFMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.SDFourFold:
                        miningProcessor = new SDFourFoldMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.SDKL:
                        miningProcessor = new SDKLMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                //executing the trace method according to the result type of the procedure
                switch (taskParams.resultType)
                {
                    case ResultTypeEnum.Trace:
                        miningProcessor.Trace();
                        miningProcessor.ProgressSetValue(1, "Completing result.");
                        miningProcessor.ResultInfo.OtherInfo = string.Format("BitStringCache.IceCalls: {0}; BitStringCache.IceTicks: {1}; Quantifier.IceCalls: {2}; Quantifier.IceTicks: {3}; All.Ticks: {4}", BitStringCache.IceCalls, BitStringCache.IceTicks, Quantifier.IceCalls, Quantifier.IceTicks, DateTime.Now.Ticks - before);
                        resultInfo = SerializableResultInfo.Serialize(miningProcessor.ResultInfo);
                        result = SerializableResult.Serialize(miningProcessor.Result);
                        break;

                    case ResultTypeEnum.TraceBoolean:
                        resultInfo = String.Empty;
                        _attributeId = attributeId;
                        _countVector = countVector;
                        AttributeNameProviderPrx nameProvider = 
                            AttributeNameProviderPrxHelper.checkedCast(boxModule.getFunctions());
                        Ferda.Guha.MiningProcessor.Formulas.AttributeNameInLiteralsProvider.Init(nameProvider);

                        _booleanTraceEnumerator = miningProcessor.TraceBoolean(
                            _countVector, _attributeId, taskParams.skipFirstN).GetEnumerator();
                        break;

                    case ResultTypeEnum.TraceReal:
                        throw new NotImplementedException();

                    default:
                        throw new NotImplementedException();
                }
                // performance ICE

                return result;
            }
            catch (BoxRuntimeError)
            {
                if (miningProcessor != null)
                    miningProcessor.ProgressSetValue(-1, "Generation failed");
                else
                    progressBarPrx.setValue(-1, "Generation failed");
                //System.Threading.Thread.Sleep(1500);
                throw;
            }
            catch (Exception e)
            {
                if (miningProcessor != null)
                    miningProcessor.ProgressSetValue(-1, "Generation failed");
                else
                    progressBarPrx.setValue(-1, "Generation failed");
                //System.Threading.Thread.Sleep(1500);
                throw Modules.Exceptions.BoxRuntimeError(e, null, "An error ocured in mining processor: " + e.Message + "\n" + e.ToString());
            }
            finally
            {
                //System.Threading.Thread.Sleep(200);
                if ((progressBarPrx != null) && (_booleanTraceEnumerator == null))
                {
                    progressBarPrx.done();
                    //System.Threading.Thread.Sleep(100);
                    ProgressTaskI.Destroy(_current.adapter, progressPrx);
                }
            }
        }

        /// <summary>
        /// This function runs the ETree task in the mining processor.
        /// Unlike other GUHA procedures implemented in Ferda, the proceduer 
        /// does not mine for patterns or association rules, but for decision trees.
        /// Therefore it was not suitable to use existing <c>Run</c> function
        /// for running the other 6 procedures. Therefore a new functionality of the
        /// Mining processor was added in form of this function.
        /// </summary>
        /// <param name="taskBoxModule">The task box module</param>
        /// <param name="branchingAttributes">Attributes used for branching 
        /// decision trees</param>
        /// <param name="targetClassificationAttribute">Attribute used for 
        /// target classification</param>
        /// <param name="minimalNodeImpurity">
        /// Minimal node impurity (algorithm parameter). Minimal node impurity is
        /// a condition for stopping growth of a tree. When sufficient amount 
        /// (determined by this parameter) of cases (items) belongs to one classification
        /// class in one node, the three is returned in output and stops growing. 
        /// </param>
        /// <param name="minimalNodeFrequency">
        /// Minimal node frequency (algorithm parameter). Minimal node frequency is
        /// a condition for stopping growth of a tree. When a node does not contain
        /// minimal number of items (determined by this parameter), the three is returned
        /// in output and stops growing. 
        /// </param>
        /// <param name="maximalTreeDepth">
        /// Maximal tree depth (algorithm parameter). The total depth of the tree
        /// cannot exceed this value.
        /// </param>
        /// <param name="noAttributesForBranching">
        /// Number of attributes for branching (algorithm parameter). When determining
        /// the most suitable for tree branching, the remaining attributes are sorted
        /// by a criterion (here, chi squared) and the best N(determined by this 
        /// parameter) are used for branching.
        /// </param>
        /// <param name="maxNumberOfHypotheses">
        /// Maximal number of hypotheses to be generated by the miner. This parameter
        /// is present mainly because of the fact, that total number of relevant questions
        /// is not a good sign of progress of the task (in present way of approximating the
        /// number, it can easily reach infinity). 
        /// </param>
        /// <param name="output">Where the progress of the task should be written</param>
        /// <param name="resultInfo">Information about the task run are stored
        /// in this parameter</param>
        /// <param name="quantifiers">Quantifiers to evaluate quality of the
        /// generated trees</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Decision trees serialized to a string</returns>       
        public override string ETreeRun(BoxModulePrx taskBoxModule, 
            CategorialAttribute[] branchingAttributes, 
            CategorialAttribute targetClassificationAttribute,
            QuantifierBaseFunctionsPrx[] quantifiers, 
            int minimalNodeImpurity, 
            int minimalNodeFrequency, 
            int maximalTreeDepth, 
            int noAttributesForBranching,
            long maxNumberOfHypotheses,
            OutputPrx output, 
            out string resultInfo, Current current__)
        {
            ProgressTaskListener progressListener = null;
            string label = String.Empty;
            _current = current__;


            label = taskTypeToString(TaskTypeEnum.ETree) + "-Task";
            progressListener = new ProgressTaskListener();
            progressPrx =
                ProgressTaskI.Create(_current.adapter, progressListener);
            progressBarPrx =
                output.startProgress(progressPrx, label, label + " running...");
            progressBarPrx.setValue(-1, "Loading ...");

            ETreeMiningProcessor miningProcessor = new ETreeMiningProcessor(
                branchingAttributes,
                targetClassificationAttribute,
                quantifiers,
                minimalNodeImpurity,
                minimalNodeFrequency,
                maximalTreeDepth,
                noAttributesForBranching,
                maxNumberOfHypotheses,
                progressListener,
                progressBarPrx);

            try
            {
                AttributeNameProviderPrx nameProvider =
                    AttributeNameProviderPrxHelper.checkedCast(taskBoxModule.getFunctions());
                Ferda.Guha.MiningProcessor.Formulas.AttributeNameInLiteralsProvider.Init(nameProvider);
                miningProcessor.Trace();
            }
            catch (BoxRuntimeError)
            {
                if (miningProcessor != null)
                    miningProcessor.ProgressSetValue(-1, "Generation failed");
                else
                    progressBarPrx.setValue(-1, "Generation failed");
                //System.Threading.Thread.Sleep(1500);
                throw;
            }
            catch (Exception e)
            {
                if (miningProcessor != null)
                    miningProcessor.ProgressSetValue(-1, "Generation failed");
                else
                    progressBarPrx.setValue(-1, "Generation failed");
                throw Modules.Exceptions.BoxRuntimeError(e, null, "An error ocured in mining processor: " + e.Message + "\n" + e.ToString());
            }
            finally
            {
                if ((progressBarPrx != null) && (_booleanTraceEnumerator == null))
                {
                    progressBarPrx.done();
                    ProgressTaskI.Destroy(_current.adapter, progressPrx);
                }
            }

            resultInfo = SerializableResultInfo.Serialize(miningProcessor.ResultInfo);
            return string.Empty;
        }

        #endregion
    }
}
