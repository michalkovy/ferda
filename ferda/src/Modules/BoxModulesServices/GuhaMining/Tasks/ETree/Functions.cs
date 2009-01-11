// Functions.cs - Function objects for the ETree task box module
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovský
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

using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.ModulesManager;
using Ferda.Modules.Boxes.DataPreparation;
using Ice;
using System.Collections.Generic;
using System;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.ETree
{
    /// <summary>
    /// Class is providing ICE functionality of the ETree task
    /// box module
    /// </summary>
    public class Functions : MiningTaskFunctionsDisp_, IFunctions, ITask
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
            //_boxInfo = boxInfo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Minimal leaf purity (algorithm parameter). Node purity = number of
        /// right classifications/number of all items in the node. Minimal leaf purity
        /// means that if the leaf purity is lower than minimal, the node branches,
        /// otherwise it stops branching.
        /// </summary>
        public float MinimalLeafPurity
        {
            get
            {
                return _boxModule.GetPropertyFloat(SockMinimalLeafPurity);
            }
        }

        /// <summary>
        /// Minimal node frequency (algorithm parameter). Minimal node frequency is
        /// a condition for stopping growth of a tree. When a node does not contain
        /// minimal number of items (determined by this parameter), the three is returned
        /// in output and stops growing. 
        /// </summary>
        public int MinimalNodeFrequency
        {
            get
            {
                return _boxModule.GetPropertyInt(SockMinimalNodeFrequency);
            }
        }

        /// <summary>
        /// Maximal tree depth (algorithm parameter). The total depth of the tree
        /// cannot exceed this value. 
        /// </summary>
        public int MaximalTreeDepth
        {
            get
            {
                return _boxModule.GetPropertyInt(SockMaximalTreeDepth);
            }
        }

        /// <summary>
        /// Number of attributes for branching (algorithm parameter). When determining
        /// the most suitable for tree branching, the remaining attributes are sorted
        /// by a criterion (here, chi squared) and the best N(determined by this 
        /// parameter) are used for branching.
        /// </summary>
        public int NoAttributesForBranching
        {
            get
            {
                return _boxModule.GetPropertyInt(SockNoAttributesForBranching);
            }
        }

        /// <summary>
        /// Maximal number of hypotheses to be generated by the miner. This parameter
        /// is present mainly because of the fact, that total number of relevant questions
        /// is not a good sign of progress of the task (in present way of approximating the
        /// number, it can easily reach infinity). 
        /// </summary>
        public long MaxNumberOfHypotheses
        {
            get
            {
                return _boxModule.GetPropertyLong(SockMaxNumberOfHypotheses);
            }
        }

        /// <summary>
        /// If output should contain only trees of desired
        /// length, or also shorter subtrees.
        /// </summary>
        public bool OnlyFullTree
        {
            get
            {
                return _boxModule.GetPropertyBool(SockOnlyFullTree);
            }
        }

        /// <summary>
        /// Determines, if the branching should be carried out for each node suitable for
        /// branching separately, or at once for all nodes suitable for branching.
        /// </summary>
        public bool IndividualNodesBranching
        {
            get
            {
                return _boxModule.GetPropertyBool(SockIndividualNodesBranching);
            }
        }

        /// <summary>
        /// Defines criterion for stopping of branching of a node
        /// in the ETree mining procedure.
        /// </summary>
        public BranchingStoppingCriterionEnum BranchingStoppingCriterion
        {
            get
            {
                return (BranchingStoppingCriterionEnum)Enum.Parse(
                    typeof(BranchingStoppingCriterionEnum),
                    _boxModule.GetPropertyString(SockBranchingStoppingCriterion));
            }
        }

        #endregion

        #region Sockets

        /// <summary>
        /// Name of quantifier's socket. In the ETree box, the quantifiers
        /// determine quality of the individual decision trees.
        /// </summary>
        public const string SockQuantifiers = "TreeQuality";

        /// <summary>
        /// Name of the socket containing target classification
        /// attribute
        /// </summary>
        public const string SockTargetClassificationAttribute = "TargetClassificationAttribute";

        /// <summary>
        /// Name of the socket containing branching attributes
        /// </summary>
        public const string SockBranchingAttributes = "BranchingAttributes";

        /// <summary>
        /// Name of the socket defininng minimal leaf purity
        /// </summary>
        public const string SockMinimalLeafPurity = "MinimalLeafPurity";

        /// <summary>
        /// Name of the socket defining minimal node frequency
        /// </summary>
        public const string SockMinimalNodeFrequency = "MinimalNodeFrequency";

        /// <summary>
        /// Name of the socket defining maximal tree depth
        /// </summary>
        public const string SockMaximalTreeDepth = "MaximalTreeDepth";

        /// <summary>
        /// Name of the socket defining number of branching categories
        /// </summary>
        public const string SockNoAttributesForBranching = "NoAttributesForBranching";

        /// <summary>
        /// Name of the socket defining maximal number of hypotheses for the output
        /// </summary>
        public const string SockMaxNumberOfHypotheses = "MaxNumberOfHypotheses";

        /// <summary>
        /// Name of the socket defining if output should contain only trees of desired
        /// length, or also shorter subtrees.
        /// </summary>
        public const string SockOnlyFullTree = "OnlyFullTree";

        /// <summary>
        /// Name of the socket defining the criterion for stopping of node branching
        /// </summary>
        public const string SockBranchingStoppingCriterion = "BranchingStoppingCriterion";

        /// <summary>
        /// Name of the socket defining if the branching should be carried out for each 
        /// node suitable for branching separately, or at once for all nodes suitable for
        /// branching.
        /// </summary>
        public const string SockIndividualNodesBranching = "IndividualNodesBranching";

        #endregion

        #region MiningTaskFunctions overrides

        /// <summary>
        /// Returns the proxies of quantifiers connected to the box. Every quantifier in
        /// Ferda implements the same interface and thus one function can retrieve proxies
        /// for quantifiers for all task types. For the ETree task box, the quantifiers are
        /// used for evaluating the resulting decision trees. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Proxies of quantifiers</returns>
        public override QuantifierBaseFunctionsPrx[] GetQuantifiers(Current current__)
        {
            List<QuantifierBaseFunctionsPrx> quant = SocketConnections.GetPrxs<QuantifierBaseFunctionsPrx>(
                _boxModule,
                SockQuantifiers,
                QuantifierBaseFunctionsPrxHelper.checkedCast,
                true,
                true);

            return quant.ToArray();
        }

        /// <summary>
        /// Gets string representing result of a task. Unlike the other GUHA procedures,
        /// this result contains decision trees
        /// </summary>
        /// <param name="statistics">Result statistics, see
        /// <see cref="Ferda.Guha.MiningProcessor.Results.SerializableResultInfo"/>
        /// </param>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Result of a task</returns>
        public override string GetResult(out string statistics, Current current__)
        {
            statistics = Common.GetResultInfo(_boxModule);
            return Common.GetResult(_boxModule);
        }

        /// <summary>
        /// Returns pairs of attributes and their identification for a given task
        /// box module. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Pairs of attributes and their identification</returns>
        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return Common.GetAttributeNames(_boxModule, this);
        }

        /// <summary>
        /// Gets the bit string generator for a given attribute.
        /// see <see cref="T:Ferda.Guha.MiningProcessor.BitStringGenerator"/>
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <param name="attributeId">Id of the attribute</param>
        /// <returns>Bit string generator</returns>
        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Current current__)
        {
            return Common.GetBitStringGenerator(_boxModule, attributeId, this);
        }

        /// <summary>
        /// Returns the proxies of all the bit string generators of this box. In case
        /// of GUHA task, it the generators of all the Boolean and categorial attributes
        /// together. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Proxy of all the bit string generators</returns>
        public override BitStringGeneratorPrx[] GetBitStringGenerators(Current current__)
        {
            return Common.GetBitStringGenerators(_boxModule, this);
        }

        /// <summary>
        /// Gets the ID of the table that is beeing mined for a box module
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>ID of the mined table</returns>
        public override string GetSourceDataTableId(Current current__)
        {
            return Common.GetSourceDataTableId(_boxModule, this);
        }

        #endregion

        #region ITask Members

        /// <summary>
        /// Cached serializible result info
        /// </summary>
        private SerializableResultInfo _cachedSerializableResultInfo = null;

        /// <summary>
        /// Returns information about the task results
        /// </summary>
        /// <returns>Information about a result of a task
        /// (can be serialized)</returns>
        public SerializableResultInfo GetResultInfo()
        {
            if (_cachedSerializableResultInfo == null)
                _cachedSerializableResultInfo = Common.GetResultInfoDeserealized(_boxModule);
            return _cachedSerializableResultInfo;
        }

        /// <summary>
        /// Names of sockets with Boolean attributes. The ETree box does not
        /// contain any boolean attribute socket names.
        /// </summary>
        /// <returns>Boolean attribute socket names</returns>
        /// <example>
        /// The 4FT procedure has three Boolean attribute sockets
        /// named ANTECEDENT, SUCCEDENT and CONDITION.
        /// </example>
        public string[] GetBooleanAttributesSocketNames()
        {
            return new string[0];
        }

        /// <summary>
        /// Names of sockets with categorial attributes. The ETree box contains
        /// sockets <c>TargetClassificationAttribute</c> and <c>BranchingAttributes</c>
        /// as categorial attributes.
        /// </summary>
        /// <returns>Categorial attribute socket names</returns>
        /// <example>
        /// The KL procedure has two categorial attribute sockets
        /// named ROW ATTRIBUTES and COLUMN ATTRIBUTES
        /// </example>
        public string[] GetCategorialAttributesSocketNames()
        {
            return new string[]
                {
                    SockTargetClassificationAttribute,
                    SockBranchingAttributes
                };
        }

        /// <summary>
        /// Determines for a given socket name, if the socket requires
        /// minimally one box connected to the socket.
        /// </summary>
        /// <param name="socketName">Name of the socket</param>
        /// <returns>If there needs to be at least one box connected</returns>
        public bool IsRequiredOneAtMinimumAttributeInSocket(string socketName)
        {
            //there has to be exactly one classification attribute and 
            //at least one branching attribute
            return true;
        }

        #endregion

        #region Other private methods

        /// <summary>
        /// The <see cref="Ferda.Guha.MiningProcessor.Results.SerializableDecisionTree.AttributeLaterColumnName"/>
        /// needs to be changed from the name of the attribute (property 
        /// "Name in Boolean attributes") of the attribute box to select expression of the
        /// column connected to the attribute, which is used in classification
        /// to access the data table. 
        /// </summary>
        /// <param name="serializedResult">
        /// Previous serialized result
        /// (attribute names in the nodes correspond to attribute
        /// "Name in Boolean attributes")
        /// </param>
        /// <returns>
        /// Result where attribute names correspond to select expression
        /// of the column connected to the attribute
        /// </returns>
        private string ChangeAttribute2ColumnNames(string serializedResult)
        {
            BoxModulePrx[] attrBoxPrxs = _boxModule.GetConnections(SockBranchingAttributes);
            GuidAttributeNamePair[] pairs = GetAttributeNames();
            //the conversion dictionary, key is attribute name,
            //value is select expression
            Dictionary<string, string> conversion = new Dictionary<string,string>();

            foreach (BoxModulePrx attrBoxPrx in attrBoxPrxs)
            {
                //getting the attribute functions
                BitStringGeneratorPrx attrPrx =
                    BitStringGeneratorPrxHelper.checkedCast(attrBoxPrx.getFunctions());

                //getting the attribute guid
                GuidStruct guid = attrPrx.GetAttributeId();

                //finding the name of the attribute (property "Name in Boolean attributes")
                string attrName = string.Empty;
                foreach (GuidAttributeNamePair tmp in pairs)
                {
                    if (tmp.id == guid)
                    {
                        attrName = tmp.attributeName;
                    }
                }

                //finding the select expression of the column
                BoxModulePrx colPrx = attrBoxPrx.getConnections("Column")[0];
                ColumnFunctionsPrx colFnc =
                    ColumnFunctionsPrxHelper.checkedCast(colPrx.getFunctions());
                string colSelectExpr = colFnc.getColumnInfo().columnSelectExpression;

                conversion.Add(attrName, colSelectExpr);
            }

            //getting the serialized result
            DecisionTreeResult res = DecisionTreeResult.Deserialize(serializedResult);

            foreach (SerializableDecisionTree tree in res.decisionTrees)
            {
                tree.RootNode.ChangeAttributeToColumnNames(conversion);
            }

            return DecisionTreeResult.Serialize(res);
        }

        #endregion

        /// <summary>
        /// Generation of hypotheses
        /// </summary>
        public void Run()
        {
            MiningProcessorFunctionsPrx miningProcessor = 
                Common.GetMiningProcessorFunctionsPrx(_boxModule);
            BitStringGeneratorProviderPrx bsProvider = 
                Common.GetBitStringGeneratorProviderPrx(_boxModule);

            //retrieving and checking the quantifiers
            //because of other name of the socket that contain quantifiers,
            //we cannot use Common.GetQuantifierBaseFunctions
            List<QuantifierBaseFunctionsPrx> quantifiers = 
                SocketConnections.GetPrxs<QuantifierBaseFunctionsPrx>(
                _boxModule, 
                SockQuantifiers,
                QuantifierBaseFunctionsPrxHelper.checkedCast,
                true, true);
            if (quantifiers == null || quantifiers.Count == 0)
            {
                throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                                Common.QuantifiersErrorMessage,
                                                new string[] { SockQuantifiers },
                                                restrictionTypeEnum.OtherReason);
            }

            //getting the categorial attributes
            CategorialAttribute[] branchingAttributes =
                Common.GetCategorialAttributesBySockets(_boxModule,
                    new string[] { SockBranchingAttributes }, this);
            CategorialAttribute classificationAttribute =
                Common.GetCategorialAttributesBySockets(_boxModule,
                    new string[] { SockTargetClassificationAttribute }, this)[0];

            if (classificationAttribute == null)
            {
                throw Exceptions.NoConnectionInSocketError(null, _boxModule.StringIceIdentity,
                    new string[] { SockTargetClassificationAttribute });
            }
            if (branchingAttributes.Length == 0)
            {
                throw Exceptions.NoConnectionInSocketError(null, _boxModule.StringIceIdentity,
                    new string[] { SockBranchingAttributes });
            }

            //filling the task params structure
            ETreeTaskRunParams par = new ETreeTaskRunParams();

            par.branchingAttributes = branchingAttributes;
            par.targetClassificationAttribute = classificationAttribute;
            par.quantifiers = quantifiers.ToArray();
            par.minimalLeafPurity = MinimalLeafPurity;
            par.minimalNodeFrequency = MinimalNodeFrequency;
            par.branchingStoppingCriterion = BranchingStoppingCriterion;
            par.maximalTreeDepth = MaximalTreeDepth;
            par.noAttributesForBranching = NoAttributesForBranching;
            par.maxNumberOfHypotheses = MaxNumberOfHypotheses;
            par.onlyFullTree = OnlyFullTree;
            par.individualNodesBranching = IndividualNodesBranching;

            string resultInfo = string.Empty;
            string result = string.Empty;
            result = miningProcessor.ETreeRun(
                _boxModule.MyProxy,
                par,
                _boxModule.Output,
                out resultInfo);


            //processing of the results
            Common.SetResultInfo(_boxModule, resultInfo);

            _cachedSerializableResultInfo = SerializableResultInfo.Deserialize(resultInfo);
            result = ChangeAttribute2ColumnNames(result);
            Common.SetResult(_boxModule, result);
        }
    }
}
