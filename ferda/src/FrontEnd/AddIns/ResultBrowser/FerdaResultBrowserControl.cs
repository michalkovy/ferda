using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Text;

using System.Windows.Forms;

using FrontEnd.AddIns.ResultBrowser.NonGUIClasses;

using Ferda;

using Ferda.Modules;

using Ferda.FrontEnd.AddIns;

using Ferda.FrontEnd.External;

using Ferda.FrontEnd.Properties;

using Ferda.Modules.Boxes.AbstractLMTask;

using System.Resources;

using System.Reflection;

using Ferda.ModulesManager;



namespace Ferda.FrontEnd.AddIns.ResultBrowser

{

    public partial class FerdaResultBrowserControl : UserControl

    {

        #region Private variables



        /// <summary>

        /// Ferda result browser class, will be displayed in the listview

        /// </summary>

        FerdaResult resultBrowser;



        /// <summary>

        /// Resource manager

        /// </summary>

        private ResourceManager resManager;



        /// <summary>

        /// Localization string, en-US or cs-CZ for now.

        /// </summary>

        private string localizationString;



        /// <summary>

        /// Implementation of propertygrid

        /// </summary>

        IOtherObjectDisplayer displayer;



        /// <summary>

        /// Implementation of propertygrid

        /// </summary>

        IBoxModule selectedBox;

        #endregion





        #region Constructor



        public FerdaResultBrowserControl(string[] localePrefs, HypothesisStruct[] hypotheses, QuantifierProvider[] used_quantifiers, IOtherObjectDisplayer Displayer)

        {

            //setting the ResManager resource manager and localization string

            string locale;

            try

            {

                locale = localePrefs[0];



                localizationString = locale;



                locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;



                resManager = new ResourceManager(locale,

            Assembly.GetExecutingAssembly());



            }



            catch

            {

                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ResultBrowser.Localization_en-US",

            Assembly.GetExecutingAssembly());

                localizationString = "en-US";

            }







            InitializeComponent();

            InitializeGraph();

            resultBrowser = new FerdaResult(resManager);

            resultBrowser.IceRun(hypotheses, used_quantifiers);

            this.displayer = Displayer;

           // selectedBox = (IBoxModule)new object();

           // selectedBox.SetPropertyString("neco", "Hodnota neco");

            //this.displayer.SelectedBox = selectedBox;

            this.displayer.Reset();

          //  this.displayer.SelectedBox.SetPropertyString("Neco", "Hodnota neco");

            this.Initialize();

        }



        #endregion





        #region Private methods

        /// <summary>

        /// Method to refresh resultbrowser

        /// </summary>

        private void RefreshBrowser()

        {

            //disabling the column click handler (sorter)

            this.HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);



            //clearing all the items

            this.HypothesesListView.Items.Clear();



            //clearing all the columns

            this.HypothesesListView.Columns.Clear();



            //adding 4 main columns

            this.HypothesesListView.Columns.Add(this.ColumnHypotheseName);

            this.HypothesesListView.Columns.Add(this.ColumnAntecedent);

            this.HypothesesListView.Columns.Add(this.ColumnSuccedent);

            this.HypothesesListView.Columns.Add(this.ColumnCondition);



            //setting locale

            this.ChangeLocale(this.resManager);



            //adding handler to display hypothesis details in richtextbox

            HypothesesListView.ItemActivate += new EventHandler(ItemSelectHandler);

            HypothesesListView.ItemCheck += new ItemCheckEventHandler(ItemSelectHandler);

            HypothesesListView.MouseClick += new MouseEventHandler(ItemSelectHandler);

            HypothesesListView.KeyDown += new KeyEventHandler(ItemSelectHandler);

            HypothesesListView.KeyUp += new KeyEventHandler(ItemSelectHandler);



            //adding the sorter

            HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);



            //adding the column names selected in the context menu

            foreach (String name in resultBrowser.GetSelectedColumnNames())

            {

                ColumnHeader header = new ColumnHeader();

                header.Text = name;

                header.Width = 100;

                this.HypothesesListView.Columns.Add(header);

            }



            //adding hypotheses

            int i = 0;

            foreach (HypothesisStruct hypothese in resultBrowser.GetAllHypotheses())

            {

                ListViewItem item = new ListViewItem(resultBrowser.GetHypothesisName(hypothese));



                //antecedent

                item.SubItems.Add(resultBrowser.GetAntecedent(hypothese));



                //succedent

                item.SubItems.Add(resultBrowser.GetSuccedent(hypothese));



                //Condition

                item.SubItems.Add(resultBrowser.GetCondition(hypothese));



                //quantifiers

                foreach (object value in resultBrowser.QuantifierValues(hypothese))

                {

                    item.SubItems.Add(value.ToString());

                }



                item.Tag = i;

                HypothesesListView.Items.Add(item);

                i++;

            }



        }



        /// <summary>

        /// Method for Result listview initialization

        /// </summary>

        private void Initialize()

        {

            //setting locale

            this.ChangeLocale(this.resManager);



            //   this.HypothesesDetailTextBox.ReadOnly = true;





            //adding unused quantifiers in the context menu

            ToolStripMenuItem tempitem;

            foreach (String item in resultBrowser.GetUsedColumnNames())

            {

                tempitem = (ToolStripMenuItem)this.QuantifiersListContextMenu.Items.Add(item);

                tempitem.Click += new EventHandler(ItemClick);

                tempitem.CheckOnClick = true;

            }



            foreach (String item in resultBrowser.GetUnusedColumnNames())

            {

                tempitem = (ToolStripMenuItem)this.QuantifiersListContextMenu.Items.Add(item);

                tempitem.Click += new EventHandler(ItemClick);

                tempitem.CheckOnClick = true;

            }



            this.RefreshBrowser();



            this.ToolStripShowGraphEdit.Click += new EventHandler(ToolStripShowGraphEdit_Click);

        }



        void ToolStripShowGraphEdit_Click(object sender, EventArgs e)

        {

            if (!ToolStripShowGraphEdit.Checked)

            {

                this.GroupBoxChangeGraph.Visible = true;

                this.GroupBoxChangeGraph.BringToFront();

                this.ToolStripShowGraphEdit.Checked = true;

            }



            else

            {

                this.GroupBoxChangeGraph.Visible = false;

                this.ToolStripShowGraphEdit.Checked = false;

            }

        }



        #endregion





        #region Handlers

        /// <summary>

        /// Function handling the selection of the quantifiers from the dropdown context menu.

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        private void ItemClick(object sender, EventArgs e)

        {

            ToolStripMenuItem item = (ToolStripMenuItem)sender;



            //opposite logic: when the control is clicked on, it is firstly selected

            if (item.Checked)

            {

                this.resultBrowser.AddColumn(item.Text);

            }



            else

            {

                this.resultBrowser.RemoveColumn(item.Text);

                this.HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);

                this.HypothesesListView.Columns.RemoveByKey(item.Text);

            }



            this.RefreshBrowser();



        }



        /// <summary>

        /// Method for filling the graph with the hypothese data.

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        private void ItemSelectHandler(object sender, EventArgs e)

        {

            ListView view = (ListView)sender;

            int index = 0;

            try

            {

                index = (int)view.SelectedItems[0].Tag;

                this.DrawBarsFromFirstTable(this.resultBrowser.GetHypothese(index), this.ContingencyTableChart);

            }



            catch

            {

            }



        }



        /// <summary>

        /// Method for sorting by the column

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        private void ClickOnColumn(object sender, System.Windows.Forms.ColumnClickEventArgs e)

        {

            Sorter columnSorter = new Sorter();

            columnSorter.column = e.Column;



            if ((columnSorter.bAscending = (HypothesesListView.Sorting == SortOrder.Ascending)))

                HypothesesListView.Sorting = SortOrder.Descending;

            else

                HypothesesListView.Sorting = SortOrder.Ascending;



            HypothesesListView.ListViewItemSorter = columnSorter;

        }



        #endregion





        #region Localization

        /// <summary>

        /// Resource manager of the application, it is filled according to the

        /// current localization

        /// </summary>

        public ResourceManager ResManager

        {

            get

            {

                return resManager;

            }

        }



        /// <summary>

        /// Localization string of the application, possible values are "en-US" and "cs-CZ"

        /// </summary>

        public string LocalizationString

        {

            get

            {

                return localizationString;

            }

        }



        /// <summary>

        /// Method to change l10n.

        /// </summary>

        /// <param name="rm">Resource manager to handle new l10n resource</param>

        private void ChangeLocale(ResourceManager rm)

        {

            this.ColumnAntecedent.Text = rm.GetString("ColumnAntecedent");

            this.ColumnCondition.Text = rm.GetString("ColumnCondition");

            this.ColumnHypotheseName.Text = rm.GetString("ColumnHypotheseName");

            this.ColumnSuccedent.Text = rm.GetString("ColumnSuccedent");

            this.GroupBoxChangeGraph.Text = rm.GetString("GraphViewOptions");

            this.ToolStripShowGraphEdit.Text = rm.GetString("GraphViewOptions");

            this.Label3dpercent.Text = rm.GetString("3dpercent");

            this.ContingencyTableChart.Header.Lines = new string[] { rm.GetString("ContingencyTable") };

            this.LabelHOffset.Text = rm.GetString("LabelHOffset");

            this.LabelVOffset.Text = rm.GetString("LabelVOffset");

            this.LabelZoom.Text = rm.GetString("LabelZoom");

            this.LabelRotation.Text = rm.GetString("LabelRotation");



            //   this.TextTabPage.Text = rm.GetString("Text");

            //   this.GraphTabPage.Text = rm.GetString("Graph");

        }



        #endregion





        #region Property Grid implementation

        

        public Ferda.ModulesManager.IBoxModule SelectedBox

        {

            set

            {

                selectedBox = value;

            }

            get

            {

                return selectedBox;

            }

        }



        #endregion



        

    }

}

