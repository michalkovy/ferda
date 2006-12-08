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
            this.ResultBrowserSplit.Panel2.SuspendLayout();
            this.ResultBrowserSplit.SuspendLayout();
            this.SuspendLayout();
            this.ContingencyTableChart = new Steema.TeeChart.TChart();
            this.ContingencyTableChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContingencyTableChart.Header.Lines = new string[] {"tChart1"};
            this.ContingencyTableChart.Location = new System.Drawing.Point(0, 0);
            this.ContingencyTableChart.Name = "tChart1";
            this.ContingencyTableChart.Header.Visible = true;
            
            this.ContingencyTableChart.Size = new System.Drawing.Size(466, 286);
            this.ContingencyTableChart.Axes.Depth.Visible = true;
            this.ContingencyTableChart.ContextMenuStrip = this.ContextMenuGraphRightClick;
            //this.ContingencyTableChart.Page.MaxPointsPerPage = 8;

            this.ToolStripShowGraphEdit.Click += new EventHandler(ToolStripShowGraphEdit_Click);

            this.ResultBrowserSplit.Panel2.Controls.Add(ContingencyTableChart);
            this.ResultBrowserSplit.Panel2.ResumeLayout(false);
            this.ResultBrowserSplit.ResumeLayout(false);
            this.ResumeLayout(false);
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
            ShowLabels(this.ContingencyTableChart);
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
        private void DrawBarsFromSecondTable(Hypothesis hypothese)
        {
            /*
            chart.Series.Clear();
            Random random = new Random();
            bool fft = false;

            this.ContingencyTableChart.Header.Text = this.resManager.GetString("SecondContingencyTable");

            //for miners with boolean antecedents and succedents - for now only 4ft
            foreach (BooleanLiteralStruct booleanLiteral in hypothese.booleanLiterals)
            {
                if ((booleanLiteral.cedentType == CedentEnum.Antecedent) || (booleanLiteral.cedentType == CedentEnum.Succedent))
                {
                    fft = true;
                    break;
                }
            }
            int i = 0;
            int j = 0;

            /*
            int[][] transpondedTable = FerdaResultBrowserControl.Transpose(hypothese.quantifierSetting.secondContingencyTableRows);

            for (int k = transpondedTable.GetUpperBound(0); k >=0; k--)
            {
                Steema.TeeChart.Styles.Bar ant = new Steema.TeeChart.Styles.Bar();
                ant.Color = System.Drawing.Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                ant.MultiBar = Steema.TeeChart.Styles.MultiBars.None;

                foreach (int number in transpondedTable[k])
                {
                    if ((fft) && (j == 0))
                    {
                        ant.Add(number, this.resManager.GetString("ColumnAntecedent"));
                    }
                    else
                    {
                        if ((fft) && (j == 1))
                        {
                            ant.Add(number, '\u00AC' + this.resManager.GetString("ColumnAntecedent"));
                        }
                        else
                        {
                            string seriesTitle = String.Empty;
                            foreach (LiteralStruct literal in hypothese.literals)
                            {
                                if (literal.cedentType == CedentEnum.Antecedent)
                                {
                                    if (literal.categoriesNames.Length > j)
                                    {
                                        seriesTitle = literal.categoriesNames[j];
                                    }
                                }
                            }
                            ant.Add(number, seriesTitle);
                        }
                    }
                    j++;
                }

                if (this.CheckBoxShowLabels.Checked)
                {
                    ant.Marks.Visible = true;
                }
                else
                {
                    ant.Marks.Visible = false;
                }
                ant.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;
                if (fft)
                {
                    if (k == 0)
                    {
                        ant.Title = this.resManager.GetString("ColumnSuccedent");
                    }
                    else
                    {
                        ant.Title = '\u00AC' + this.resManager.GetString("ColumnSuccedent");
                    }
                }
                else
                {
                    string seriesTitle = String.Empty;
                    foreach (LiteralStruct literal in hypothese.literals)
                    {
                        if (literal.cedentType == CedentEnum.Succedent)
                        {
                            if (literal.categoriesNames.Length > k)
                            {
                                seriesTitle = literal.categoriesNames[k];
                            }
                            break;
                        }
                    }
                    if (seriesTitle == String.Empty)
                    {
                        foreach (LiteralStruct literal in hypothese.literals)
                        {
                            if (literal.cedentType == CedentEnum.Antecedent)
                            {
                                if (literal.categoriesNames.Length > k)
                                {
                                    seriesTitle = literal.categoriesNames[k];
                                }
                            }
                        }
                    }

                    ant.Title = seriesTitle;
                }
                chart.Series.Add(ant);
             //   i++;
                j = 0;
            }
            */
        }

        /// <summary>
        /// Drawing first contingency table to chart
        /// </summary>
        /// <param name="hypothese">Hypothese to take contingency table from</param>
        private void DrawBarsFromFirstTable(Hypothesis hypothesis)
        {
            //clearing the graph
            ContingencyTableChart.Series.Clear();

            //for miners with boolean antecedents and succedents - for now only 4ft
            Formula antecedent = hypothesis.GetFormula(MarkEnum.Antecedent);
            if (antecedent != null)
            {
                //drawing the titles, name of the table, antecedents and
                //succedents names
                ContingencyTableChart.Header.Text =
                    "4FT " + resManager.GetString("ContingencyTable");
                ContingencyTableChart.SubFooter.Text = antecedent.ToString();
                DrawFFT(hypothesis);
            }

            Formula rowAttributes = hypothesis.GetFormula(MarkEnum.ColumnAttribute);

            if (rowAttributes != null)
            {
                ContingencyTableChart.Header.Text =
                    "KL " + resManager.GetString("ContingencyTable");
                DrawKL(hypothesis, rowAttributes);
            }
        }

        /// <summary>
        /// The method draws the KL contingency table on
        /// the ContingencyTable chart
        /// </summary>
        /// <param name="hypothesis">Hypothesis to be drawn</param>
        private void DrawKL(Hypothesis hypothesis, Formula f)
        {
            double[][] transposed = Transpose(hypothesis.ContingencyTableA);

            Bar bar;
            Random random;
            for (int i = 0; i <= transposed.GetUpperBound(0); i++)
            {
                bar = new Bar();
                random = new Random();
                bar.Color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                bar.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
                bar.Marks.Style = MarksStyles.LabelValue;

                //TODO name of the category
                bar.Title = i.ToString();

                //adding the columns to the row
                foreach (int value in transposed[i])
                {
                    bar.Add(value, value.ToString());
                }

                ContingencyTableChart.Series.Add(bar);
            }
        }

        /// <summary>
        /// The method draws the FFT contingency table on the 
        /// ContingencyTableCart
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
            ant.Title = resManager.GetString("ColumnAntecedent");

            //initializing the not antecednet bar (row)
            Bar notant = new Bar();
            notant.Color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            notant.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
            notant.Marks.Style = MarksStyles.LabelValue;
            notant.Title = '\u00AC' + resManager.GetString("ColumnAntecedent");

            //If the preferences are set to show the labels
            if (this.CheckBoxShowLabels.Checked)
            {
                ant.Marks.Visible = true;
                notant.Marks.Visible = true;
            }
            else
            {
                ant.Marks.Visible = false;
                notant.Marks.Visible = false;
            }

            //filling the contingency table according to the values in 
            //the contingency table where the actual numbers are
            //(I don't know why it is this way)
            ant.Add(hypothesis.ContingencyTableA[0][0],
                resManager.GetString("ColumnSuccedent"));
            ant.Add(hypothesis.ContingencyTableA[0][2],
                '\u00AC' + resManager.GetString("ColumnSuccedent"));
            notant.Add(hypothesis.ContingencyTableA[2][0],
                resManager.GetString("ColumnSuccedent"));
            notant.Add(hypothesis.ContingencyTableA[2][2],
                '\u00AC' + resManager.GetString("ColumnSuccedent"));

            ContingencyTableChart.Series.Add(ant);
            ContingencyTableChart.Series.Add(notant);
        }

        /// <summary>
        /// Method which toggles labels on/off for chart.
        /// </summary>
        /// <param name="chart">Chart to toggle lables for</param>
        private void ShowLabels(TChart chart)
        {
            foreach (Steema.TeeChart.Styles.Series serie in chart.Series)
            {
                if (this.CheckBoxShowLabels.Checked)
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