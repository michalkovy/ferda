using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.FFTTask
{
    class UBound : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.FFTTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "UBound";
        }
    }
}
