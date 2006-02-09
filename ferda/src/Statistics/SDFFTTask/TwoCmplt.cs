using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.SDFFTTask
{
    class TwoCmplt : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            //a/(a+c)
            return
                (float)(
                quantifierSetting.secondContingencyTableRows[0][0]
                )
                /
                (float)(
                quantifierSetting.secondContingencyTableRows[0][0] +
                quantifierSetting.secondContingencyTableRows[1][0]
                );
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.SDFFTTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "2nd-Cmplt";
        }
    }
}
