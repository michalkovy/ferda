using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.CFTask
{
    class Sum : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            return (float)Common.Functions.Sum(quantifierSetting.firstContingencyTableRows);
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.CFTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "Sum";
        }
    }
}
