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
        /// Default constructor of the class
        /// </summary>
        /// <param name="resManager">The resource manager</param>
        public MainWindow(ResourceManager resManager)
        {
            resourceManager = resManager;
            InitializeComponent();
            InitializeGraph();

            ChangeLocale();
        }

        /// <summary>
        /// Initializes the graph component
        /// </summary>
        private void InitializeGraph()
        {
            graph = new TChart();
            graph.Location = new System.Drawing.Point(12, 12);
            graph.Aspect.View3D = false;
            graph.Size = new System.Drawing.Size(822, 236);
            this.Controls.Add(graph);
            this.ResumeLayout();
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
