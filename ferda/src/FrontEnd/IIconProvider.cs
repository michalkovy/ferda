using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// This interface provides icons used in Ferda to other controls
    /// </summary>
    public interface IIconProvider
    {
        /// <summary>
        /// Gets the icon specified by icons string identifier
        /// </summary>
        /// <param name="IconName">Name of the icon</param>
        /// <returns>Icon that is connected to this name</returns>
        Icon GetIcon(string IconName);
    }
}
