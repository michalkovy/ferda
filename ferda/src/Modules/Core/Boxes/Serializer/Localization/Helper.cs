using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.Serializer.Localization
{
    /// <summary>
    /// <para>
    /// This class provides more efficient representation of Box`s XML localization
    /// file than <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
    /// </para>
    /// <para>
    /// Instance of the <see cref="BoxLocalization"/> is actually deserealized XML localization
    /// file. The <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.Helper"/> creates 
    /// more efficient structures for futher working with the localization (e. g. 
    /// <see cref="T:System.Collections.Generic.Dictionary"> Dictionaries</see>).
    /// </para>
    /// </summary>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Reader"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.BoxInfo"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>
    [Serializable]
    public class Helper : Ferda.Modules.Boxes.Serializer.Localization.IHelper
    {
        private string localeId;
        /// <summary>
        /// Gets the locale id.
        /// </summary>
        /// <value>
        /// The culture names follow the RFC 1766 standard in the format 
        /// "&lt;languagecode2&gt;-&lt;country/regioncode2&gt;", where &lt;languagecode2&gt;
        /// is a lowercase two-letter code derived from ISO 639-1 and &lt;country/regioncode2&gt;
        /// is an uppercase two-letter code derived from ISO 3166. For example, U.S. English 
        /// is "en-US". In cases where a two-letter language code is not available, 
        /// the three-letter code derived from ISO 639-2 is used; for example, 
        /// the three-letter code "div" is used for cultures that use the Dhivehi language. 
        /// Some culture names have suffixes that specify the script; for example, 
        /// "-Cyrl" specifies the Cyrillic script, "-Latn" specifies the Latin script.
        /// </value>
        public string LocaleId
        {
            get { return localeId; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Helper"/> class.
        /// </summary>
        /// <param name="boxLocalization">Instance of the <see cref="BoxLocalization">localization of the box</see>.</param>
        /// <param name="localeId">The Id of current locale (e. g. "en-US", "cs-CZ", ...).</param>
        /// /// <exception cref="T:System.ArgumentNullException"><c>boxLocalization</c> is null reference.</exception>
        public Helper(BoxLocalization boxLocalization, string localeId)
        {
            if (boxLocalization == null)
            {
                System.Diagnostics.Debug.WriteLine("Ser05");
                throw new ArgumentNullException("boxLocalizations");
            }

            this.localeId = localeId;
            this.identifier = boxLocalization.Identifier;
            this.label = boxLocalization.Label;
            this.hint = boxLocalization.Hint;

            //prepare categories where the box belongs to
            if (boxLocalization.Categories != null)
                foreach (Category category in boxLocalization.Categories)
                {
                    this.categories.Add(category.Name, category.Label);
                }

            //prepare help files and paths towards them
            List<HelpFileInfo> helpFiles = new List<HelpFileInfo>();
            if (boxLocalization.HelpFiles != null)
                foreach (HelpFile helpFile in boxLocalization.HelpFiles)
                {
                    //help file identifier contains Id of the localization and help file`s identifier
                    string newHelpFileIdentifier = this.localeId + helpFile.Identifier;

                    //prepare path to the help file
                    this.helpFilesPaths.Add(newHelpFileIdentifier, helpFile.Path);

                    //prepare help files
                    HelpFileInfo item = new HelpFileInfo();
                    item.identifier = newHelpFileIdentifier;
                    item.label = helpFile.Label;
                    item.version = helpFile.Version;
                    helpFiles.Add(item);
                }
            this.helpFiles = helpFiles.ToArray();

            //prepare categories of box`s properties
            if (boxLocalization.PropertyCategories != null)
                foreach (PropertyCategory propertyCategory in boxLocalization.PropertyCategories)
                {
                    this.propertyCategories.Add(propertyCategory.Name, propertyCategory.Label);
                }

            //prepare sockets (i.e. also properties) and selectbox options
            if (boxLocalization.Sockets != null)
                foreach (Socket socket in boxLocalization.Sockets)
                {
                    //prepare sockets (i.e. also properties)
                    this.sockets.Add(socket.Name, socket);

                    //prepare selectbox options
                    if (socket.SelectOptions != null && socket.SelectOptions.Length > 0)
                    {
                        Dictionary<string, SelectOption> options = new Dictionary<string, SelectOption>();
                        foreach (SelectOption selectBoxParam in socket.SelectOptions)
                        {
                            options.Add(selectBoxParam.Name, selectBoxParam);
                        }
                        this.selectBoxOptions.Add(socket.Name, options);
                    }
                }

            //prepare localization for actions
            if (boxLocalization.Actions != null)
                foreach (Action action in boxLocalization.Actions)
                {
                    this.actions.Add(action.Name, action);
                }

            //prepare box`s dynamic help items
            this.dynamicHelpItems = this.getDynamicHelpItems(boxLocalization.DynamicHelpItems);

            //prepare box`s modules asking for creation
            if (boxLocalization.ModulesAskingForCreationSeq != null)
                foreach (ModulesAskingForCreation modulesAskingForCreation in boxLocalization.ModulesAskingForCreationSeq)
                {
                    Ferda.Modules.ModulesAskingForCreation modulesAfcItem = new Ferda.Modules.ModulesAskingForCreation();
                    modulesAfcItem.label = modulesAskingForCreation.Label;
                    modulesAfcItem.hint = modulesAskingForCreation.Hint;
                    modulesAfcItem.help = this.getDynamicHelpItems(modulesAskingForCreation.DynamicHelpItems);
                    this.modulesAskingForCreation.Add(modulesAskingForCreation.Name, modulesAfcItem);
                }

            //prepare box`s prases
            StringBuilder phrasesLoadError = new StringBuilder();
            if (boxLocalization.Phrases != null)
                foreach (Phrase phrase in boxLocalization.Phrases)
                {
                    try
                    {
                        this.phrases.Add(phrase.PhraseIdentifier, phrase.PhraseText);
                    }
                    catch (ArgumentNullException)
                    {
                        phrasesLoadError.AppendLine("Ser03: Empty identifier of phrase in box " + this.Identifier + "{" + this.LocaleId + "}");
                    }
                    catch (ArgumentException)
                    {
                        phrasesLoadError.AppendLine("Ser04: Phrase " + phrase.PhraseIdentifier + " in box " + this.Identifier + "{" + this.LocaleId + "} is more than once.");
                    }
                }
#if DEBUG
            if (phrasesLoadError.Length > 0)
            {
                string message = phrasesLoadError.ToString();
                System.Diagnostics.Debug.WriteLine(message);
                throw new Exception(message);
            }
#endif
        }

        /// <summary>
        /// The localized phrases.
        /// </summary>
        /// <value>The localized phrases.
        /// <para><c>Key</c> is name (if you like identifier) of the phrase.</para>
        /// <para><c>Value</c> is localized text of the phrase.</para>
        /// </value>
        private SortedList<string, string> phrases = new SortedList<string, string>();

        /// <summary>
        /// Tries to the get specified phrase (<c>phraseIdentifier</c>).
        /// </summary>
        /// <param name="phraseIdentifier">The phrase`s identifier.</param>
        /// <param name="phraseLocalizedText">The phrase`s localized text.</param>
        /// <returns>
        /// <c>true</c> if localization of specified phrase (<c>phraseIdentifier</c>)
        /// exists; otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetPhrase(string phraseIdentifier, out string phraseLocalizedText)
        {
            bool result = phrases.TryGetValue(phraseIdentifier, out phraseLocalizedText);
            if (!result)
                System.Diagnostics.Debug.WriteLine("Ser12: Phrase " + phraseIdentifier + " is not localized in " + this.Identifier + "{" + this.LocaleId + "}");
            return result;
        }

        private Dictionary<string, Ferda.Modules.ModulesAskingForCreation> modulesAskingForCreation = new Dictionary<string, Ferda.Modules.ModulesAskingForCreation>();
        /// <summary>
        /// Gets the modules asking for creation.
        /// </summary>
        /// <value>The modules asking for creation.
        /// <para><c>Key</c> is name of modules asking for creation.</para>
        /// <para><c>Value</c> is the array of <see cref="T:Ferda.Modules.DynamicHelpItem"/> i.e. dynamic help items. (label, hint and dynamic help items are specified)</para>
        /// </value>
        public Dictionary<string, Ferda.Modules.ModulesAskingForCreation> ModulesAskingForCreation
        {
            get { return modulesAskingForCreation; }
        }

        private Ferda.Modules.DynamicHelpItem[] dynamicHelpItems;
        /// <summary>
        /// Gets the localizec dynamic help items of the box.
        /// </summary>
        /// <value>The dynamic help items of the box.</value>
        public Ferda.Modules.DynamicHelpItem[] DynamicHelpItems
        {
            get { return dynamicHelpItems; }
        }

        private Dictionary<string, Action> actions = new Dictionary<string, Action>();
        /// <summary>
        /// Gets the actions of the box.
        /// </summary>
        /// <value>The actions of the box.</value>
        public Dictionary<string, Action> Actions
        {
            get { return actions; }
        }

        private string identifier;
        /// <summary>
        /// Gets the box`s identifier.
        /// </summary>
        /// <value>The box`s identifier.</value>
        public string Identifier
        { get { return identifier; } }

        private string label;
        /// <summary>
        /// Gets the box`s label.
        /// </summary>
        /// <value>The box`s label.</value>
        public string Label
        { get { return label; } }

        private string hint;
        /// <summary>
        /// Gets the box`s hint i.e. short tip.
        /// </summary>
        /// <value>The box`s hint.</value>
        public string Hint
        { get { return hint; } }

        private Dictionary<string, string> propertyCategories = new Dictionary<string, string>();
        /// <summary>
        /// Gets (localized) names of the categories of the property.
        /// </summary>
        /// <value>The property categories.
        /// <para><c>Key</c> is name the category where the property belongs to.</para>
        /// <para><c>Value</c> is localized name of the category.</para>
        /// </value>
        public Dictionary<string, string> PropertyCategories
        {
            get { return propertyCategories; }
        }

        private Dictionary<string, string> categories = new Dictionary<string, string>();
        /// <summary>
        /// Gets (localized) names of the categories of the box.
        /// </summary>
        /// <value>The box`s categories.
        /// <para><c>Key</c> is name the category where the box belongs to.</para>
        /// <para><c>Value</c> is localized name of the category.</para>
        /// </value>
        public Dictionary<string, string> Categories
        {
            get { return categories; }
        }

        private Dictionary<string, string> helpFilesPaths = new Dictionary<string, string>();
        /// <summary>
        /// Gets the help files paths.
        /// </summary>
        /// <remarks>Path is relative to the directory, where the config file is stored.</remarks>
        /// <value>The help files paths.
        /// <para><c>Key</c> is localeId + helpFileIdentifier.</para>
        /// <para><c>Value</c> is path to the help file.</para>
        /// </value>
        public Dictionary<string, string> HelpFilesPaths
        {
            get { return helpFilesPaths; }
        }

        private HelpFileInfo[] helpFiles;
        /// <summary>
        /// Gets the help files.
        /// </summary>
        /// <value>The help files.</value>
        public HelpFileInfo[] HelpFiles
        {
            get { return helpFiles; }
        }

        /// <summary>
        /// Gets the dynamic help items i.e. makes a conversion from array of 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.DynamicHelpItem"/>
        /// to array of <see cref="T:Ferda.Modules.DynamicHelpItem"/>.
        /// </summary>
        /// <param name="dynamicHelpItems">The dynamic help items.</param>
        /// <returns>Converted dynamic help items.</returns>
        private Ferda.Modules.DynamicHelpItem[] getDynamicHelpItems(DynamicHelpItem[] dynamicHelpItems)
        {
            if (dynamicHelpItems != null && dynamicHelpItems.Length > 0)
            {
                List<Ferda.Modules.DynamicHelpItem> result = new List<Ferda.Modules.DynamicHelpItem>();
                foreach (DynamicHelpItem dynamicHelpItem in dynamicHelpItems)
                {
                    Ferda.Modules.DynamicHelpItem item = new Ferda.Modules.DynamicHelpItem();
                    item.identifier = this.localeId + dynamicHelpItem.Identifier;
                    item.label = dynamicHelpItem.Label;
                    item.url = dynamicHelpItem.Url;
                    result.Add(item);
                }
                return result.ToArray();
            }
            else
                return new Ferda.Modules.DynamicHelpItem[0];
        }

        private Dictionary<string, Socket> sockets = new Dictionary<string, Socket>();
        /// <summary>
        /// Gets the localization of the sockets.
        /// </summary>
        /// <value>
        /// The sockets.
        /// <para><c>Key</c> is socket`s name.</para>
        /// 	<para><c>Value</c> is socket`s localization.</para>
        /// </value>
        /// <remarks>Localization of properties is included.</remarks>
        public Dictionary<string, Socket> Sockets
        {
            get { return sockets; }
        }

        /// <summary>
        /// Gets the localization of the socket of the specified name.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>
        /// 	<see cref="T:Ferda.Modules.Boxes.Serializer.Localization.Socket"/> if
        /// exists an element of specified <c>socketName</c>; otherwise, throws
        /// <see cref="T:Ferda.Modules.NameNotExistError"/>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">There is no socket with
        /// specified <c>socketName</c> in the box.</exception>
        public Socket GetSocket(string socketName)
        {
            try
            {
                return this.Sockets[socketName];
            }
            catch (Exception ex)
            {
                throw Ferda.Modules.Exceptions.NameNotExistError(ex, "", "Ser02: GetSocket(...): socketName (" + socketName + ") in box " + this.Identifier + "{" + this.LocaleId + "} is not localized.", socketName);
            }
        }

        private Dictionary<string, Dictionary<string, SelectOption>> selectBoxOptions = new Dictionary<string, Dictionary<string, SelectOption>>();
        /// <summary>
        /// Gets the the selectbox`s option of specified name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="fallOnError">If set to <c>true</c> and there is no
        /// localization of specified option in the property <see cref="T:System.Exception"/> is thrown.</param>
        /// <returns>
        /// Label and short label of the specified option of the property.
        /// </returns>
        public SelectOption GetSelectBoxOption(string propertyName, string optionName, bool fallOnError)
        {
            try
            {
                return this.selectBoxOptions[propertyName][optionName];
            }
            catch (Exception ex)
            {
                if (fallOnError)
                {
                    string message = "Ser01: GetSelectBoxOption(...): unknown propertyName (" + propertyName + ") or optionName (" + optionName + ")";
                    System.Diagnostics.Debug.WriteLine(message);
                    throw new System.Exception(message, ex);
                }
                else
                    return null;
            }
        }
    }
}
