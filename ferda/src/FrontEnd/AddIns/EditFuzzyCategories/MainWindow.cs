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
        /// Default constructor of the class
        /// </summary>
        /// <param name="resManager">The resource manager</param>
        /// <param name="minimum">Minimal value of the attribute</param>
        /// <param name="maximum">Maximal value of the attribute</param>
        public MainWindow(ResourceManager resManager, double minimum, double maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
            resourceManager = resManager;
            InitializeComponent();
            InitializeGraph();

            ChangeLocale();
            List<TrapezoidalFuzzySet> list = new List<TrapezoidalFuzzySet>();
            TrapezoidalFuzzySet f = new TrapezoidalFuzzySet();
            f.Name = "name";
            f.A = 10000;
            f.D = 10500;
            f.C = 11000;
            f.B = 11500;
            list.Add(f);
            FillGraph(list);
        }

        /// <summary>
        /// Initializes the graph component
        /// </summary>
        private void InitializeGraph()
        {
            graph = new TChart();
            graph.Location = new System.Drawing.Point(12, 12);
            graph.Aspect.View3D = false;
            graph.Header.Visible = false;
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
        private void FillGraph(IList<TrapezoidalFuzzySet> fuzzySets)
        {
            Random r = new Random();
            graph.Series.RemoveAllSeries();
            foreach (TrapezoidalFuzzySet fuzzySet in fuzzySets)
            {
                //System.Drawing.Color c =
                //    System.Drawing.Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));

                Line l = new Line();
                l.Title = fuzzySet.Name;
                //l.Brush.Color = c;
                l.Brush.Color = System.Drawing.Color.Red;
                l.Add(fuzzySet.A,0);
                l.Add(fuzzySet.D,1);
                l.Add(fuzzySet.C,1);
                l.Add(fuzzySet.B,0);
                graph.Series.Add(l);
            }


            //graph.Series.RemoveAllSeries();
            //Line l = new Line();
            //l.Title = "lajna2";
            //l.Brush.Color = System.Drawing.Color.Red;
            //l.Add(10000, 0);
            //l.Add(10500, 1);
            //l.Add(11000, 1);
            //l.Add(11500, 0);
            //graph.Series.Add(l);
            
            //l = new Line();
            //l.Title = "lajna1";
            //l.Brush.Color = System.Drawing.Color.Blue;
            //l.Add(9000, 0);
            //l.Add(9500, 1);
            //l.Add(10000, 1);
            //l.Add(10500, 0);
            //graph.Series.Add(l);
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
            ValueBLabel.Text = resourceManager.GetString("DTB");
            ValueCLabel.Text = resourceManager.GetString("CTB");
            ValueDLabel.Text = resourceManager.GetString("BTB");
            EditFCButton.Text = resourceManager.GetString("EditFC");
            RemoveFCButton.Text = resourceManager.GetString("RemoveFC");
            CancelButton.Text = resourceManager.GetString("Cancel");
            AddFCButton.Text = resourceManager.GetString("AddFC");
        }
    }
}
