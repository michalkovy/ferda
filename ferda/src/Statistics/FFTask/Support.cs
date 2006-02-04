using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.FFT
{
    class Support : Ferda.Statistics.StatisticsProviderDisp_
    {
        public override float getStatistics(Ferda.Modules.AbstractQuantifierSetting quantifierSetting, Ice.Current current__)
        {
            return 123.456F;
        }

        public override string getTaskType(Ice.Current current__)
        {
            return "LISpMinerTasks.FFTask";
        }

        public override string getStatisticsName(Ice.Current current__)
        {
            return "Support";
        }
    }
}
