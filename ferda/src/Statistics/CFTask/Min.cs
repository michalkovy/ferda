using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Statistics.Common;

namespace Ferda.Statistics.CFTask
{
    class Min : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            return (float)Common.Functions.Min(quantifierSetting.firstContingencyTableRows);
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.CFTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "Min";
        }
    }
}
