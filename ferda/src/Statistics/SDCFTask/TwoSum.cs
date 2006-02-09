using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.SDCFTask
{
    class TwoSum : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            return Common.Functions.Sum(quantifierSetting.secondContingencyTableRows);
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.SDCFTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "2nd-Sum";
        }
    }
}
