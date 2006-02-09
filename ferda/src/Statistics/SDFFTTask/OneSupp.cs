using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.SDFFTTask
{
    class OneSupp : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            //a/(a+b+c+d)
            return
                (
                quantifierSetting.firstContingencyTableRows[0][0]
                )
                /
                (
                quantifierSetting.firstContingencyTableRows[0][0] +
                quantifierSetting.firstContingencyTableRows[0][1] +
                quantifierSetting.firstContingencyTableRows[1][0] +
                quantifierSetting.firstContingencyTableRows[1][1]
                );
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.SDFFTTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "1st-Supp";
        }
    }
}
