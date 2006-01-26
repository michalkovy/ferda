using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Modules.Boxes.LISpMinerTasks
{
    public static class LISpMinerAbstractSDTask
    {
        public static WorkingWithCedentsFirstSetEnum GetFirstSet(BoxModuleI boxModule)
        {
            return (WorkingWithCedentsFirstSetEnum)Enum.Parse(typeof(WorkingWithCedentsFirstSetEnum),
                boxModule.GetPropertyString("FirstSet"),
                true);
        }

        public static WorkingWithCedentsSecondSetEnum GetSecondSet(BoxModuleI boxModule)
        {
            return (WorkingWithCedentsSecondSetEnum)Enum.Parse(typeof(WorkingWithCedentsSecondSetEnum),
                boxModule.GetPropertyString("SecondSet"),
                true);
        }
    }
}
