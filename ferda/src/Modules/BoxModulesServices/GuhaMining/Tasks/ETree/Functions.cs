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

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.ETree
{
    /// <summary>
    /// Class is providing ICE functionality of the ETree task
    /// box module
    /// </summary>
    public class Functions : MiningTaskFunctionsDisp_, IFunctions
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
            //TODO dodelat
            return null;
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
            statistics = string.Empty;
            //TODO dodelat
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
            //TODO dodelat
            return null;
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
            //TODO dodelat
            //return Common.GetBitStringGenerator(_boxModule, attributeId, this);
            return null;
        }

        /// <summary>
        /// Gets the ID of the table that is beeing mined for a box module
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>ID of the mined table</returns>
        public override string GetSourceDataTableId(Current current__)
        {
            //TODO nejak to resit
            //return Common.GetSourceDataTableId(_boxModule, this);
            return null;
        }
    }
}
