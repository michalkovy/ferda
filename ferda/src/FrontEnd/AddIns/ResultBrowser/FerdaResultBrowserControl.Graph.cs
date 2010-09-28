// FerdaResultBrowserControl.Graph.cs - Displaying charts part
//
// Author:   Alexander Kuzmin <alexander.kuzmin@gmail.com>
//           Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin, Martin Ralbovsky
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Modules;
using Steema.TeeChart;
using Steema.TeeChart.Styles;

namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    /// <summary>
    /// Part of the FerdaResultBrowserControl related to drawing
    /// contingency tables
    /// </summary>
    partial class FerdaResultBrowserControl
    {
        #region Private variables

        /// <summary>
        /// All the contingency table charts are drawn upon
        /// this graph
        /// </summary>
        private TChart ContingencyTableChart;

        #endregion

        #region Initialization

        /// <summary>
        /// Method to initialize Steema chart component for displaying contingency 
        /// table
        /// </summary>
        private void InitializeGraph()
        {
            ResultBrowserSplit.Panel2.SuspendLayout();
            ResultBrowserSplit.SuspendLayout();
            SuspendLayout();
            ContingencyTableChart = new Steema.TeeChart.TChart();
            ContingencyTableChart.Dock = System.Windows.Forms.DockStyle.Fill;
            ContingencyTableChart.Header.Lines = new string[] {"tChart1"};
            ContingencyTableChart.Location = new System.Drawing.Point(0, 0);
            ContingencyTableChart.Name = "tChart1";
            ContingencyTableChart.Header.Visible = true;
            ContingencyTableChart.Header.Text = resManager.GetString("ClickInto");

            ContingencyTableChart.Legend.Visible = false;
            ContingencyTableChart.Size = new System.Drawing.Size(466, 286);
            ContingencyTableChart.Axes.Depth.Visible = true;
            ContingencyTableChart.ContextMenuStrip = this.ContextMenuGraphRightClick;
            
            ToolStripShowGraphEdit.Click += new EventHandler(ToolStripShowGraphEdit_Click);
            ToolStripCopyChart.Click += new EventHandler(ToolStripCopyChart_Click);

            ResultBrowserSplit.Panel2.Controls.Add(ContingencyTableChart);
            ResultBrowserSplit.Panel2.ResumeLayout(false);
            ResultBrowserSplit.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        #region Chart option handlers (modifying graph view)

        /// <summary>
        /// Scrolls the graph in 3D
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void TrackBar3d_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.Chart3DPercent = 
                this.TrackBar3d.Value;
        }

        /// <summary>
        /// Zooms the graph
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void TrackBarZoom_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.Zoom = this.TrackBarZoom.Value;
        }

        /// <summary>
        /// Moves the graph horizontally
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>>
        private void TrackBarHOffset_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.HorizOffset = this.TrackBarHOffset.Value;
        }

        /// <summary>
        /// Moves the graph vertically
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void TrackBarVOffset_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.VertOffset = this.TrackBarVOffset.Value;
        }

        /// <summary>
        /// Shows the labels on bars of the graph
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void CheckBoxShowLabels_CheckedChanged(object sender, EventArgs e)
        {
            ShowLabels();
        }

        #endregion

        #region Other private methods

        /// <summary>
        /// Method for transposing a given array
        /// </summary>
        /// <param name="sourceArray">The source array</param>
        /// <returns>Transposed array</returns>
        private static double[][] Transpose(double[][] sourceArray)
        {
            int tmp = 0;
            if (sourceArray.GetLength(0) > 0)
            {
                tmp = sourceArray[0].Length;
            }
            else
            {
                return sourceArray;
            }

            double[][] returnArray = new double[tmp][];

            for (int i = 0; i < tmp; i++)
            {
                returnArray[i] = new double [sourceArray.GetLength(0)];
            }

            for (int i = 0; i < sourceArray.GetLength(0); i++)
            {
                for (int j = 0; j < tmp; j++)
                {
                    returnArray[j][i] = sourceArray[i][j];
                }
            }
            return returnArray;
        }

        /// <summary>
        /// Drawing first contingency table to chart
        /// </summary>
        /// <param name="hypothese">Hypothese to take contingency table from</param>
        private void DrawBars(Hypothesis hypothesis)
        {
            //clearing the graph
            ContingencyTableChart.Series.Clear();

            //for the 4FT task
            if (resultBrowser.TaskType == TaskTypeEnum.FourFold)
            {
                //drawing the title
                ContingencyTableChart.Header.Text =
                    "4FT " + resManager.GetString("ContingencyTable");
                DrawFFT(hypothesis);
            }

            //for the SD4FT task
            if (resultBrowser.TaskType == TaskTypeEnum.SDFourFold)
            {
                if (RadioFirstTable.Checked)
                {
                    ContingencyTableChart.Header.Text =
                        "SD4FT " + resManager.GetString("FirstContingencyTable");
                    DrawSDFFT(hypothesis, true);
                }
                else
                {
                    ContingencyTableChart.Header.Text =
                        "SD4FT " + resManager.GetString("SecondContingencyTable");
                    DrawSDFFT(hypothesis, false);
                }
            }

            //for the KL task
            if (resultBrowser.TaskType == TaskTypeEnum.KL)
            {
                //drawing the title
                ContingencyTableChart.Header.Text =
                    "KL " + resManager.GetString("ContingencyTable");
                DrawKL(hypothesis);
            }

            //for the CF task
            if (resultBrowser.TaskType == TaskTypeEnum.CF)
            {
                ContingencyTableChart.Header.Text =
                    "CF " + resManager.GetString("ContingencyTable");
                DrawCF(hypothesis);
            }
        }

        /// <summary>
        /// Draws the CF contingency table on the ContingencyTable chart
        /// </summary>
        /// <param name="hypothesis">Hypothesis to be drawn</param>
        private void DrawCF(Hypothesis hypothesis)
        {
            Bar bar = new Bar();
            Random random = new Random();
            bar.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
            bar.Marks.Style = MarksStyles.LabelValue;

            Formula f = hypothesis.GetFormula(MarkEnum.Attribute);
            bar.Title = f.ToString();

            //getting the names of the categories (should be in right order)
            string[] categoryNames = resultBrowser.GetCategoryNames(f);

            if (categoryNames == null)
            {
                return;
            }

            for (int i = 0; i < hypothesis.ContingencyTableA[0].Length; i++)
            {
                bar.Add(hypothesis.ContingencyTableA[0][i], categoryNames[i],
                    Color.FromArgb(random.Next(255), random.Next(255),
                    random.Next(255)));
            }

            ContingencyTableChart.Series.Add(bar);
            ShowLabels();
        }

        /// <summary>
        /// The method draws the KL contingency table on
        /// the ContingencyTable chart
        /// </summary>
        /// <param name="hypothesis">Hypothesis to be drawn</param>
        private void DrawKL(Hypothesis hypothesis)
        {
            double[][] transposed = Transpose(hypothesis.ContingencyTableA);

            //getting the names of the column attribute
            Formula f = hypothesis.GetFormula(MarkEnum.ColumnAttribute);
            string [] colAttrNames = resultBrowser.GetCategoryNames(f);

            //getting the names of the row attributes
            f = hypothesis.GetFormula(MarkEnum.RowAttribute);
            string[] rowAttrNames = resultBrowser.GetCategoryNames(f);

            if (colAttrNames == null || rowAttrNames == null)
            {
                return;
            }

            Bar bar;
            Random random = new Random();
            for (int i = transposed.GetUpperBound(0); i>= 0 ; i--)
            {
                bar = new Bar();
                bar.Color = Color.FromArgb(random.Next(255), 
                    random.Next(255), random.Next(255));
                bar.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
                bar.Marks.Style = MarksStyles.LabelValue;
                bar.Title = colAttrNames[i];

                //adding the columns to the row
                for (int j = 0; j < transposed[i].Length; j++)
                {
                    bar.Add(transposed[i][j], rowAttrNames[j]);
                }

                ContingencyTableChart.Series.Add(bar);
                ShowLabels();
            }
        }

        /// <summary>
        /// The method draws the FFT contingency table on the 
        /// ContingencyTableChart. 
        /// </summary>
        /// <param name="hypothesis">Hypothesis to be drawn</param>
        private void DrawFFT(Hypothesis hypothesis)
        {
            //initializing the antecedent bar (row)
            Bar ant = new Bar();
            Random random = new Random();
            ant.Color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            ant.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
            ant.Marks.Style = MarksStyles.LabelValue;
            ant.Title = hypothesis.GetFormula(MarkEnum.Antecedent).ToString();

            //initializing the not antecednet bar (row)
            Bar notant = new Bar();
            notant.Color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            notant.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
            notant.Marks.Style = MarksStyles.LabelValue;
            notant.Title = '\u00AC' + 
                hypothesis.GetFormula(MarkEnum.Antecedent).ToString();

            //If the preferences are set to show the labels
            if (this.CHBShowLabels.Checked)
            {
                ant.Marks.Visible = true;
                notant.Marks.Visible = true;
            }
            else
            {
                ant.Marks.Visible = false;
                notant.Marks.Visible = false;
            }

            //getting the right contingency table
            double[][] table = hypothesis.ContingencyTableA;

            //filling the contingency table according to the values in 
            //the contingency table where the actual numbers are
            //(I don't know why it is this way)
            ant.Add(table[0][0], hypothesis.GetFormula(MarkEnum.Succedent).ToString());
            ant.Add(table[0][2],
                '\u00AC' + hypothesis.GetFormula(MarkEnum.Succedent).ToString());
            notant.Add(table[2][0],
                hypothesis.GetFormula(MarkEnum.Succedent).ToString());
            notant.Add(table[2][2],
                '\u00AC' + hypothesis.GetFormula(MarkEnum.Succedent).ToString());

            ContingencyTableChart.Series.Add(ant);
            ContingencyTableChart.Series.Add(notant);
        }

        /// <summary>
        /// The method draws the first or second four-fold contingency
        /// table on the ContingencyTableChart depending on the parameter
        /// <paramref name="firstTable"/>
        /// </summary>
        /// <param name="hypothesis">Hypothesis to be drawn</param>
        /// <param name="firstTable">Determines if it is first or
        /// second contingency table of the SD4FT task</param>
        private void DrawSDFFT(Hypothesis hypothesis, bool firstTable)
        {
            //initializing the antecedent bar (row)
            Bar ant = new Bar();
            Random random = new Random();
            ant.Color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            ant.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
            ant.Marks.Style = MarksStyles.LabelValue;
            ant.Title = hypothesis.GetFormula(MarkEnum.Antecedent).ToString();

            //initializing the not antecednet bar (row)
            Bar notant = new Bar();
            notant.Color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            notant.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
            notant.Marks.Style = MarksStyles.LabelValue;
            notant.Title = '\u00AC' +
                hypothesis.GetFormula(MarkEnum.Antecedent).ToString();

            //If the preferences are set to show the labels
            if (this.CHBShowLabels.Checked)
            {
                ant.Marks.Visible = true;
                notant.Marks.Visible = true;
            }
            else
            {
                ant.Marks.Visible = false;
                notant.Marks.Visible = false;
            }

            //getting the right contingency table
            double[][] table;
            table = firstTable ? hypothesis.ContingencyTableA : 
                                 hypothesis.ContingencyTableB;

            //filling the contingency table according to the values in 
            //the contingency table where the actual numbers are
            //(I don't know why it is this way)
            ant.Add(table[0][0], hypothesis.GetFormula(MarkEnum.Succedent).ToString());
            ant.Add(table[0][1],
                '\u00AC' + hypothesis.GetFormula(MarkEnum.Succedent).ToString());
            notant.Add(table[1][0],
                hypothesis.GetFormula(MarkEnum.Succedent).ToString());
            notant.Add(table[1][1],
                '\u00AC' + hypothesis.GetFormula(MarkEnum.Succedent).ToString());

            ContingencyTableChart.Series.Add(ant);
            ContingencyTableChart.Series.Add(notant);
        }

        /// <summary>
        /// Method which toggles labels on/off for chart.
        /// </summary>
        private void ShowLabels()
        {
            foreach (Steema.TeeChart.Styles.Series serie 
                in ContingencyTableChart.Series)
            {
                if (this.CHBShowLabels.Checked)
                {
                    serie.Marks.Visible = true;
                }
                else
                {
                    serie.Marks.Visible = false;
                }
            }
        }

        /// <summary>
        /// Handles copying the chart to clipboard
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void ToolStripCopyChart_Click(object sender, EventArgs e)
        {
            Bitmap bitMap = new Bitmap(
                        this.ContingencyTableChart.Bitmap,
                        this.ContingencyTableChart.Size.Width,
                        this.ContingencyTableChart.Size.Height
                        );
            Clipboard.SetImage(bitMap);
        }

        #endregion
    }
}