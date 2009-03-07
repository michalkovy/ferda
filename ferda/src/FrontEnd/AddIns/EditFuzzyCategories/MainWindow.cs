// MainWindow.cs - the EditFuzzyCategories dialog window
//
// Author: Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2009 Martin Ralbovsky
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Ferda.Guha.Attribute;

namespace Ferda.FrontEnd.AddIns.EditFuzzyCategories
{
    /// <summary>
    /// The EditFuzzyCategories setting module main dialog window
    /// </summary>
    public partial class MainWindow : Form
    {
        /// <summary>
        /// The graphing component
        /// </summary>
        private TChart graph;

        /// <summary>
        /// The resource manager for localization
        /// </summary>
        private ResourceManager resourceManager;

        /// <summary>
        /// Minimal value of the attribute
        /// </summary>
        private double minimum;

        /// <summary>
        /// Maximal value of the attribute
        /// </summary>
        private double maximum;

        /// <summary>
        /// The trapezoidal fuzzy sets
        /// </summary>
        private Dictionary<string, TrapezoidalFuzzySet> fuzzySets = new Dictionary<string, TrapezoidalFuzzySet>();

        /// <summary>
        /// The set, that has been edited.
        /// </summary>
        private TrapezoidalFuzzySet editedSet = null;

        /// <summary>
        /// The owner of add in (FrontEnd component)
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        #region Constructor

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="resManager">The resource manager</param>
        /// <param name="minimum">Minimal value of the attribute</param>
        /// <param name="maximum">Maximal value of the attribute</param>
        public MainWindow(ResourceManager resManager, double minimum, double maximum, IOwnerOfAddIn ownerOfAddIn,
            TrapezoidalFuzzySets fSets)
        {
            foreach (TrapezoidalFuzzySet set in fSets.fuzzySets)
            {
                this.fuzzySets.Add(set.Name, set);
            }

            this.ownerOfAddIn = ownerOfAddIn;
            this.minimum = minimum;
            this.maximum = maximum;
            resourceManager = resManager;
            InitializeComponent();
            InitializeGraph();

            ChangeLocale();

            //tady se to bude jeste menit az budem ziskavat informace z krabicky
            FillGraph(fuzzySets);
            FillList(fuzzySets);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the graph component
        /// </summary>
        private void InitializeGraph()
        {
            graph = new TChart();
            graph.Location = new System.Drawing.Point(12, 12);
            //graph.Aspect.View3D = false;
            //graph.Header.Visible = false;
            graph.Header.Text = "0 fuzzy categories, minimum = " + minimum.ToString() + 
                ", maximum = " + maximum.ToString();
            graph.Zoom.Allow = false;
            graph.Panel.MarginBottom = 2;
            graph.Panel.MarginLeft = 2;
            graph.Panel.MarginRight = 2;
            graph.Panel.MarginTop = 2;
            graph.Size = new System.Drawing.Size(822, 236);
            graph.Axes.Left.Automatic = false;
            graph.Axes.Left.Minimum = 0;
            graph.Axes.Left.Maximum = 1.1;
            graph.Axes.Bottom.Automatic = false;
            graph.Axes.Bottom.Minimum = minimum;
            graph.Axes.Bottom.Maximum = maximum;
            graph.Axes.Visible = true;
            this.Controls.Add(graph);
            graph.ResumeLayout();
            this.ResumeLayout();
        }

        /// <summary>
        /// Fills the graph with fuzzy sets. 
        /// </summary>
        /// <param name="fuzzySets">Fuzzy sets in trapeziodal form</param>
        private void FillGraph(Dictionary<string,TrapezoidalFuzzySet> fuzzySets)
        {
            graph.Header.Text = fuzzySets.Count.ToString() + " fuzzy categories, minimum = " 
                + minimum.ToString() + ", maximum = " + maximum.ToString();

            if (fuzzySets.Count == 1)
            {
                graph.Legend.Visible = false;
            }
            else
            {
                graph.Legend.Visible = true;
            }
            Random r = new Random();
            graph.Series.RemoveAllSeries();
            foreach (TrapezoidalFuzzySet fuzzySet in fuzzySets.Values)
            {
                System.Drawing.Color c =
                    System.Drawing.Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));

                Line l = new Line();
                l.Title = fuzzySet.Name;
                l.Brush.Color = c;
                l.Add(fuzzySet.A,0);
                l.Add(fuzzySet.D,1);
                l.Add(fuzzySet.C,1);
                l.Add(fuzzySet.B,0);
                graph.Series.Add(l);
            }
        }

        /// <summary>
        /// Changes the localization according to the Resource manager passed
        /// to the class by its constructor
        /// </summary>
        private void ChangeLocale()
        {
            this.Text = resourceManager.GetString("EditFuzzyCategoriesAbout");
            NewFCLabel.Text = resourceManager.GetString("NewFuzzyCategory");
            ExistingFCLabel.Text = resourceManager.GetString("ExistingFuzzyCategories");
            NewFCNameLabel.Text = resourceManager.GetString("NewFCName");
            ValueALabel.Text = resourceManager.GetString("ATB");
            ValueBLabel.Text = resourceManager.GetString("BTB");
            ValueCLabel.Text = resourceManager.GetString("CTB");
            ValueDLabel.Text = resourceManager.GetString("DTB");
            EditFCButton.Text = resourceManager.GetString("EditFC");
            RemoveFCButton.Text = resourceManager.GetString("RemoveFC");
            CancelButton.Text = resourceManager.GetString("Cancel");
            AddFCButton.Text = resourceManager.GetString("AddFC");
            SaveFCButton.Text = resourceManager.GetString("SaveFC");
            HelpButton.Text = resourceManager.GetString("Help");
        }

        /// <summary>
        /// Fills the list containing names of fuzzy categories
        /// </summary>
        /// <param name="fuzzySets">The fuzzy sets which are being
        /// filled</param>
        private void FillList(Dictionary<string,TrapezoidalFuzzySet> fuzzySets)
        {
            ExistingFCLB.Items.Clear();
            foreach (string fuzzySet in fuzzySets.Keys)
            {
                ExistingFCLB.Items.Add(fuzzySet);
            }
        }

        /// <summary>
        /// Determines, if a fuzzy set of the same four values of the trapezoid
        /// already exists
        /// </summary>
        /// <param name="a">Value A of the trapezoid</param>
        /// <param name="b">Value B of the trapezoid</param>
        /// <param name="c">Value C of the trapezoid</param>
        /// <param name="d">Value D of the trapezoid</param>
        /// <returns></returns>
        private bool ContainsFuzzySet(double a, double b, double c, double d)
        {
            foreach (TrapezoidalFuzzySet set in fuzzySets.Values)
            {
                if (set.A == a && set.B == b && set.C == c && set.D == d)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a <see cref="Ferda.Guha.Attribute.TrapezoidalFuzzySets"/>
        /// object consisting of fuzzy categories to be serialized. 
        /// </summary>
        /// <returns><see cref="Ferda.Guha.Attribute.TrapezoidalFuzzySets"/>
        /// object</returns>
        public TrapezoidalFuzzySets GetSets()
        {
            TrapezoidalFuzzySets sets = new TrapezoidalFuzzySets();
            List<TrapezoidalFuzzySet> field = new List<TrapezoidalFuzzySet>();
            foreach(TrapezoidalFuzzySet set in fuzzySets.Values)
            {
                field.Add(set);
            }
            sets.fuzzySets = field.ToArray();
            return sets;
        }

        #endregion

        #region Events

        /// <summary>
        /// The event should add a fuzzy category.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void AddFCButton_Click(object sender, EventArgs e)
        {
            double a, b, c, d;
            try
            {
                //checking the numbers, if they exist
                a = Convert.ToDouble(ATB.Text);
                b = Convert.ToDouble(BTB.Text);
                c = Convert.ToDouble(CTB.Text);
                d = Convert.ToDouble(DTB.Text);
            }
            catch
            {
                MessageBox.Show(resourceManager.GetString("TrapezoidInvalidFormat"), 
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Checking the name of the new fuzzy category
            if (string.IsNullOrEmpty(NameTB.Text))
            {
                MessageBox.Show(resourceManager.GetString("NoName"),
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Checking the trapezoidal shape
            if (a < d && d < c && c < b)
            {
            }
            else
            {
                MessageBox.Show(resourceManager.GetString("ADCB"),
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (a > maximum)
            {
                MessageBox.Show(resourceManager.GetString("ABiggerThanMax"),
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (b < minimum)
            {
                MessageBox.Show(resourceManager.GetString("BLowerThanMin"),
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //filling a new trapeziodal fuzzy set structure
            TrapezoidalFuzzySet fs = new TrapezoidalFuzzySet();
            fs.A = a;
            fs.B = b;
            fs.C = c;
            fs.D = d;
            fs.Name = NameTB.Text;

            Button s = sender as Button;
            if (s == SaveFCButton)
            {
                fuzzySets.Remove(editedSet.Name);
            }

            if (fuzzySets.ContainsKey(NameTB.Text))
            {
                MessageBox.Show(resourceManager.GetString("AlreadyContainsName"),
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (ContainsFuzzySet(a, b, c, d))
            {
                MessageBox.Show(resourceManager.GetString("AlreadyContainsValues"),
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            editedSet = null;

            fuzzySets.Add(fs.Name, fs);

            FillGraph(fuzzySets);
            FillList(fuzzySets);

            //deleting the text boxes
            NameTB.Text = string.Empty;
            ATB.Text = string.Empty;
            BTB.Text = string.Empty;
            CTB.Text = string.Empty;
            DTB.Text = string.Empty;
        }

        /// <summary>
        /// The event should remove a fuzzy category.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void RemoveFCButton_Click(object sender, EventArgs e)
        {
            if (ExistingFCLB.SelectedIndex == -1)
            {
                MessageBox.Show(resourceManager.GetString("ItemNotSelected"),
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string item = ExistingFCLB.SelectedItem as string;
            fuzzySets.Remove(item);
            FillGraph(fuzzySets);
            FillList(fuzzySets);
            ExistingFCLB.SelectedItem = -1;
            editedSet = null;
            NameTB.Text = string.Empty;
            ATB.Text = string.Empty;
            BTB.Text = string.Empty;
            CTB.Text = string.Empty;
            DTB.Text = string.Empty;
        }

        /// <summary>
        /// The event should edit a fuzzy category.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void EditFCButton_Click(object sender, EventArgs e)
        {
            if (ExistingFCLB.SelectedIndex == -1)
            {
                MessageBox.Show(resourceManager.GetString("ItemNotSelected"),
                    resourceManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string item = ExistingFCLB.SelectedItem as string;
            editedSet = fuzzySets[item];
            NameTB.Text = fuzzySets[item].Name;
            ATB.Text = fuzzySets[item].A.ToString();
            BTB.Text = fuzzySets[item].B.ToString();
            CTB.Text = fuzzySets[item].C.ToString();
            DTB.Text = fuzzySets[item].D.ToString();
        }

        /// <summary>
        /// The event should load and display the module help file
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void HelpButton_Click(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
