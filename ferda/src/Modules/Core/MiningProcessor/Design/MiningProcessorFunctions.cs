// MiingProcessorFunctions.cs - Functionality of the mining processor
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
using System.Text;
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor.Design
{
    /// <summary>
    /// The interface for Ferda GUHA mining processor. There is only
    /// one method, that gathers all the needeed settings of the GUHA
    /// tasks and then executes the run. All the GUHA procedures
    /// in Ferda call this function when calculating their tasks. 
    /// </summary>
    /// <remarks>
    /// The original slice design can be found in 
    /// <c>slice/Modules/Guha.MiningProcessor.ice</c>.
    /// The class diagram representing dependencies of the designed entities
    /// can be found in
    /// <c>slice/Modules/GUha.MiningProcessor.png(csd)</c>.
    /// The csd file stands for class diagram, that can be edited with the 
    /// <c>NClass</c> tool, see <see cref="http://nclass.sourceforge.net"/>.
    /// </remarks>
    interface MiningProcessorFunctions
    {
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
        /// <returns>Serialized result of the task in form
        /// <see cref="Ferda.Guha.MiningProcessor.SerializableResult"/> is returned.
        /// </returns>
        string Run(BoxModule taskBoxModule,
            BooleanAttribute[] booleanAttributes,
            CategorialAttribute[] categorialAttributes,
            Ferda.Guha.Math.Quantifiers.QuantifiersBaseFunctionsPrx[] quantifiers,
            TaskRunParams taskParams,
            BitStringGeneratorProvider bitStringGenerator,
            Ferda.ModulesManager.Output output,
            Guid attributeId,
            int[] countVector,
            out string resultInfo);

        /// <summary>
        /// Retrieves next bit string. This type of computation is used by the
        /// virtual hypotheses attribute boxes. They do not compute the whole task,
        /// they return bit strings corresponding to individual relevant questions
        /// instead. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </summary>
        /// <returns>Bit string</returns>
        BitStringWithCategoryId GetNextBitString();

        /// <summary>
        /// This function runs the ETree task in the mining processor.
        /// Unlike other GUHA procedures implemented in Ferda, the proceduer 
        /// does not mine for patterns or association rules, but for decision trees.
        /// Therefore it was not suitable to use existing <c>Run</c> function
        /// for running the other 6 procedures. Therefore a new functionality of the
        /// Mining processor was added in form of this function.
        /// </summary>
        /// <param name="taskBoxModule">The task box module</param>
        /// <param name="output">Where the progress of the task should be written</param>
        /// <param name="resultInfo">Information about the task run are stored
        /// in this parameter</param>
        /// <param name="taskParams">
        /// Task parameters in <see cref="Ferda.Guha.MiningProcessor.ETreeTaskRunParams"/>
        /// structure.
        /// </param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Decision trees serialized to a string</returns>        
        string ETreeRun(BoxModulePrx taskBoxModule,
            ETreeTaskRunParams taskParams,
            OutputPrx output,
            out string resultInfo, Current current__);
    }
}
