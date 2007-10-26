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
using Ice;
using System.Collections.Generic;

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
        /// Minimal node impurity
        /// </summary>
        public int MinimalNodeImpurity
        {
            get
            {
                return _boxModule.GetPropertyInt(SockMinimalNodeImpurity);
            }
        }

        /// <summary>
        /// Minimal node frequency
        /// </summary>
        public int MinimalNodeFrequency
        {
            get
            {
                return _boxModule.GetPropertyInt(SockMinimalNodeFrequency);
            }
        }

        /// <summary>
        /// Maximal tree depth
        /// </summary>
        public int MaximalTreeDepth
        {
            get
            {
                return _boxModule.GetPropertyInt(SockMaximalTreeDepth);
            }
        }

        #endregion

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
        /// Name of the socket defininng minimal node impurity
        /// </summary>
        public const string SockMinimalNodeImpurity = "MinimalNodeImpurity";

        /// <summary>
        /// Name of the socket defining minimal node frequency
        /// </summary>
        public const string SockMinimalNodeFrequency = "MinimalNodeFrequency";

        /// <summary>
        /// Name of the socket defining maximal tree depth
        /// </summary>
        public const string SockMaximalTreeDepth = "MaximalTreeDepth";

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
            //TODO az budu vedet, co tam budu davat, tak to dodelam
            statistics = string.Empty;
            return string.Empty;
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
        /// <param name="taskFunctions">Information about the task</param>
        /// <returns>Bit string generator</returns>
        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Current current__)
        {
            return Common.GetBitStringGenerator(_boxModule, attributeId, this);
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
        /// Returns information about the task results. The function should not be for ETree
        /// box, because the <see cref="Ferda.Guha.MiningProcessor.Results.SerializableResultInfo"/>
        /// in unsuitable for storing of decision trees.
        /// </summary>
        /// <returns>Information about a result of a task
        /// (can be serialized)</returns>
        public SerializableResultInfo GetResultInfo()
        {
            throw Exceptions.BoxRuntimeError(null, _boxModule.getDefaultUserLabel()[0],
                "The GetResultInfo function shouldn't be used for ETree box.");
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
            List<QuantifierBaseFunctionsPrx> quantifiers = 
                Common.GetQuantifierBaseFunctions(_boxModule, true);
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

            //string result =  + progress bar

        }
    }
}
