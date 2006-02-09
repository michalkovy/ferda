using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.SDFFTTask
{
    class DfAvg : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            //Differences between average difference a(a+b+c+d)/((a+b)(a+c))- 1
            return
                ((float)(
                     quantifierSetting.firstContingencyTableRows[0][0] *
                    (
                        quantifierSetting.firstContingencyTableRows[0][0] +
                        quantifierSetting.firstContingencyTableRows[0][1] +
                        quantifierSetting.firstContingencyTableRows[1][0] +
                        quantifierSetting.firstContingencyTableRows[1][1]
                    )
                )
                /
                (float)((
                    (
                    quantifierSetting.firstContingencyTableRows[0][0] +
                    quantifierSetting.firstContingencyTableRows[0][1]
                    )
                    *
                    (
                    quantifierSetting.firstContingencyTableRows[0][0] +
                    quantifierSetting.firstContingencyTableRows[1][0]
                    )
                ) - 1))
                -
                ((float)(
                     quantifierSetting.secondContingencyTableRows[0][0] *
                    (
                        quantifierSetting.secondContingencyTableRows[0][0] +
                        quantifierSetting.secondContingencyTableRows[0][1] +
                        quantifierSetting.secondContingencyTableRows[1][0] +
                        quantifierSetting.secondContingencyTableRows[1][1]
                    )
                )
                /
                (float)((
                    (
                    quantifierSetting.secondContingencyTableRows[0][0] +
                    quantifierSetting.secondContingencyTableRows[0][1]
                    )
                    *
                    (
                    quantifierSetting.secondContingencyTableRows[0][0] +
                    quantifierSetting.secondContingencyTableRows[1][0]
                    )
                ) - 1))
                ;
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.SDFFTTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "DfAvg";
        }
    }
}
