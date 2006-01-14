using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.ProjectManager
{
    public struct ProjectManagerOptions
    {
        public bool StartIceGridLocaly;
        public bool StopIceGridLocaly;
        public bool IceGridAsService;
        public string SentenceForWait;
        public string[] ServersToStop;
        public string IceBinPath;
        public string[] LocalePrefs;
    }
}
