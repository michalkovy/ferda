// Functions.cs - Function objects for the SD4FT task box module
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
// Documented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský
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

#define ProgressBarDebug

using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.SDFourFold
{
    /// <summary>
    /// Class is providing ICE functionality of the SD4FT task
    /// box module
    /// </summary>
    public class Functions : MiningTaskFunctionsDisp_, IFunctions, ITask
    {
        #region Invariant code ... same for all tasks

        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        //protected IBoxInfo _boxInfo;

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

        #endregion

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
        /// Returns the proxies of quantifiers connected to the box. Every quantifier in
        /// Ferda implements the same interface and thus one function can retrieve proxies
        /// for quantifiers for all task types.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Proxies of quantifiers</returns>
        public override QuantifierBaseFunctionsPrx[] GetQuantifiers(Current current__)
        {
            return Common.GetQuantifierBaseFunctions(_boxModule, true).ToArray();
        }

        /// <summary>
        /// Gets string representing result of a task
        /// (see <see cref="Ferda.Guha.MiningProcessor.Results.SerializableResult"/>)
        /// of a given task box module.
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
        /// Names of sockets with Boolean attributes
        /// </summary>
        /// <returns>Boolean attribute socket names</returns>
        /// <example>
        /// The 4FT procedure has three Boolean attribute sockets
        /// named ANTECEDENT, SUCCEDENT and CONDITION.
        /// </example>
        public string[] GetBooleanAttributesSocketNames()
        {
            return new string[]
                {
                    Common.SockSuccedent,
                    Common.SockAntecedent,
                    Common.SockCondition,
                    Common.SockSDCedent1,
                    Common.SockSDCedent2
                };
        }

        /// <summary>
        /// Names of sockets with categorial attributes
        /// </summary>
        /// <returns>Categorial attribute socket names</returns>
        /// <example>
        /// The KL procedure has two categorial attribute sockets
        /// named ROW ATTRIBUTES and COLUMN ATTRIBUTES
        /// </example>
        public string[] GetCategorialAttributesSocketNames()
        {
            return new string[0];
        }

        /// <summary>
        /// Determines for a given socket name, if the socket requires
        /// minimally one box connected to the socket.
        /// </summary>
        /// <param name="socketName">Name of the socket</param>
        /// <returns>If there needs to be at least one box connected</returns>
        public bool IsRequiredOneAtMinimumAttributeInSocket(string socketName)
        {
            if (socketName == Common.SockSuccedent
                || socketName == Common.SockSDCedent1
                || socketName == Common.SockSDCedent2)
                return true;
            return false;
        }

        #endregion

        /// <summary>
        /// Generation of hypotheses
        /// </summary>
        public void Run()
        {
            // reset cache
            _cachedSerializableResultInfo = null;
            
            Common.RunTask(_boxModule, this, TaskTypeEnum.SDFourFold, ResultTypeEnum.Trace);
        }
    }
}