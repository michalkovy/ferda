using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// My implementation of clipboart, it remembers list of nodes
    /// </summary>
    public interface IFerdaClipboard
    {
        /// <summary>
        /// Nodes contained in the clipboard
        /// </summary>
        List<ModulesManager.IBoxModule> Nodes
        {
            set;
            get;
        }

        /// <summary>
        /// Determines if there is something in the clipboard
        /// </summary>
        bool IsEmpty
        {
            get;
        }
    }
}
