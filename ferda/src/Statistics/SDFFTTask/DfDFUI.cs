using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.SDFFTTask
{
    class DfDFUI : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            //Differences between DConfidence a/(a+b+c)
            return
                ((float)quantifierSetting.firstContingencyTableRows[0][0] /
                (float)(
                quantifierSetting.firstContingencyTableRows[0][0] +
                quantifierSetting.firstContingencyTableRows[0][1] +
                quantifierSetting.firstContingencyTableRows[1][0]
                ))
                -
                ((float)quantifierSetting.secondContingencyTableRows[0][0] /
                (float)(
                quantifierSetting.secondContingencyTableRows[0][0] +
                quantifierSetting.secondContingencyTableRows[0][1] +
                quantifierSetting.secondContingencyTableRows[1][0]
                ))
                ;
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.SDFFTTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "DfFUI";
        }
    }
}
