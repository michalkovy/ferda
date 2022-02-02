using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferda.ModulesManager
{
    public interface IBackgroundActionCallback
    {
        public void OnException(System.Exception ex);
        public void OnResponse();
    }
}
