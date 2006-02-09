using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.SDFFTTask
{
    class TwoD : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            return (float)quantifierSetting.secondContingencyTableRows[1][1];
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.SDFFTTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "2nd-D";
        }
    }
}
