using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.FFTTask
{
    class R : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            //a+b
            return
                quantifierSetting.firstContingencyTableRows[0][0] +
                quantifierSetting.firstContingencyTableRows[0][1];
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.FFTTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "R";
        }
    }
}
