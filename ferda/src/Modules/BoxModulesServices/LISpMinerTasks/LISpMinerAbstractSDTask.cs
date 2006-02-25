using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Modules.Boxes.LISpMinerTasks
{
    /// <summary>
    /// Static class for working with SD-* tasks.
    /// </summary>
    public static class LISpMinerAbstractSDTask
    {
        /// <summary>
        /// Gets the first set.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <returns>First set setting for SD-* task.</returns>
        public static WorkingWithCedentsFirstSetEnum GetFirstSet(BoxModuleI boxModule)
        {
            return (WorkingWithCedentsFirstSetEnum)Enum.Parse(typeof(WorkingWithCedentsFirstSetEnum),
                boxModule.GetPropertyString("FirstSet"),
                true);
        }

        /// <summary>
        /// Gets the second set.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <returns>Second set setting for SD-* task.</returns>
        public static WorkingWithCedentsSecondSetEnum GetSecondSet(BoxModuleI boxModule)
        {
            return (WorkingWithCedentsSecondSetEnum)Enum.Parse(typeof(WorkingWithCedentsSecondSetEnum),
                boxModule.GetPropertyString("SecondSet"),
                true);
        }
    }
}
