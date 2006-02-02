using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;

namespace Ferda.FrontEnd.Menu
{
    /// <summary>
    /// Interface presents functionality for all the controls to be able
    /// to change their localizations
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        /// Method that changes the localication of the whole application
        /// </summary>
        void ChangeGlobalLocalization(string locstring);

        /// <summary>
        /// Current and valid resourceManager that holds the resources
        /// </summary>
        ResourceManager ResManager
        {
            get;
        }

        /// <summary>
        /// Localization string that is selected by the application
        /// </summary>
        string[] LocalePrefs
        {
            get;
            set;
        }
    }
}
