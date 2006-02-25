using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.LISpMinerTasks
{
    /// <summary>
    /// Interface which should be implemented by 
    /// all LISp-Miner task functionsI objects.
    /// </summary>
    public interface ILISpMinerAbstractTask
    {
        /// <summary>
        /// Runs the action <b>Run</b>.
        /// </summary>
        void RunActionRun();
    }
}
