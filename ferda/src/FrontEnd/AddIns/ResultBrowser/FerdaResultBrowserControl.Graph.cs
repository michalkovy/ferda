// FerdaResultBrowserControl.Graph.cs - Displaying charts part
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using Ferda.Modules;

namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    /// <summary>
    /// User Control - charts
    /// </summary>
    partial class FerdaResultBrowserControl
    {
        #region Private variables



        private Steema.TeeChart.TChart ContingencyTableChart;



        #endregion


        #region Initialization

        /// <summary>
        /// Method to initialize Steema chart component for displaying contingency table
        /// </summary>
        private void InitializeGraph()
        {
            this.ResultBrowserSplit.Panel2.SuspendLayout();
            this.ResultBrowserSplit.SuspendLayout();
            this.SuspendLayout();
            this.ContingencyTableChart = new Steema.TeeChart.TChart();
            this.ContingencyTableChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContingencyTableChart.Header.Lines = new string[] {
        "tChart1"};
            this.ContingencyTableChart.Location = new System.Drawing.Point(0, 0);
            this.ContingencyTableChart.Name = "tChart1";
            this.ContingencyTableChart.Header.Visible = true;
            
            this.ContingencyTableChart.Size = new System.Drawing.Size(466, 286);
            this.ContingencyTableChart.GetAxisLabel += new Steema.TeeChart.GetAxisLabelEventHandler(ContingencyTableChart_GetAxisLabel);
            this.ContingencyTableChart.Axes.Depth.Visible = true;
            this.ContingencyTableChart.ContextMenuStrip = this.ContextMenuGraphRightClick;
            //   this.ContingencyTableChart.Page.MaxPointsPerPage = 8;

            this.ResultBrowserSplit.Panel2.Controls.Add(ContingencyTableChart);
            this.ResultBrowserSplit.Panel2.ResumeLayout(false);
            this.ResultBrowserSplit.ResumeLayout(false);
            this.ResumeLayout(false);
        }



        #endregion


        #region Chart option handlers (modifying graph view)

        private void TrackBar3d_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.Chart3DPercent = this.TrackBar3d.Value;
        }

        private void TrackBarZoom_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.Zoom = this.TrackBarZoom.Value;
        }


        private void TrackBarHOffset_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.HorizOffset = this.TrackBarHOffset.Value;
        }

        private void TrackBarVOffset_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.VertOffset = this.TrackBarVOffset.Value;
        }

        private void CheckBoxShowLabels_CheckedChanged(object sender, EventArgs e)
        {
            ShowLabels(this.ContingencyTableChart);
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Method for transposing a given array
        /// </summary>
        /// <param name="sourceArray"></param>
        /// <returns></returns>
        private static int[][] Transpose(int[][] sourceArray)
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

            int[][] returnArray = new int[tmp][];

            for (int i = 0; i < tmp; i++)
            {
                returnArray[i] = new int[sourceArray.GetLength(0)];
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
        /// <param name="chart">Chart to draw bars into</param>
        private void DrawBarsFromSecondTable(HypothesisStruct hypothese, Steema.TeeChart.TChart chart)
        {
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
           // int i = 0;
            int j = 0;

            int[][] transpondedTable = FerdaResultBrowserControl.Transpose(hypothese.quantifierSetting.secondContingencyTableRows);

            for (int k = transpondedTable.GetUpperBound(0); k >=0; k--)
            {
                Steema.TeeChart.Styles.Bar barSeries = new Steema.TeeChart.Styles.Bar();
                barSeries.Color = System.Drawing.Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                barSeries.MultiBar = Steema.TeeChart.Styles.MultiBars.None;

                foreach (int number in transpondedTable[k])
                {
                    if ((fft) && (j == 0))
                    {
                        barSeries.Add(number, this.resManager.GetString("ColumnAntecedent"));
                    }
                    else
                    {
                        if ((fft) && (j == 1))
                        {
                            barSeries.Add(number, '\u00AC' + this.resManager.GetString("ColumnAntecedent"));
                        }
                        else
                        {
                            string seriesTitle = String.Empty;
                            foreach (LiteralStruct literal in hypothese.literals)
                            {
                                if (literal.cedentType == CedentEnum.Antecedent)
                                {
                                    seriesTitle = literal.categoriesNames[j];
                                }
                            }
                            barSeries.Add(number, seriesTitle);
                        }
                    }
                    j++;
                }

                if (this.CheckBoxShowLabels.Checked)
                {
                    barSeries.Marks.Visible = true;
                }
                else
                {
                    barSeries.Marks.Visible = false;
                }
                barSeries.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;
                if (fft)
                {
                    if (k == 0)
                    {
                        barSeries.Title = this.resManager.GetString("ColumnSuccedent");
                    }
                    else
                    {
                        barSeries.Title = '\u00AC' + this.resManager.GetString("ColumnSuccedent");
                    }
                }
                else
                {
                    string seriesTitle = String.Empty;
                    foreach (LiteralStruct literal in hypothese.literals)
                    {
                        if (literal.cedentType == CedentEnum.Succedent)
                        {
                            seriesTitle = literal.categoriesNames[k];
                            break;
                        }
                    }
                    if (seriesTitle == String.Empty)
                    {
                        foreach (LiteralStruct literal in hypothese.literals)
                        {
                            if (literal.cedentType == CedentEnum.Antecedent)
                            {
                                seriesTitle = literal.categoriesNames[k];
                            }
                        }
                    }

                    barSeries.Title = seriesTitle;
                }
                chart.Series.Add(barSeries);
             //   i++;
                j = 0;
            }
        }

        /// <summary>
        /// Drawing first contingency table to chart
        /// </summary>
        /// <param name="hypothese">Hypothese to take contingency table from</param>
        /// <param name="chart">Chart to draw bars into</param>
        private void DrawBarsFromFirstTable(HypothesisStruct hypothese, Steema.TeeChart.TChart chart)
        {
            chart.Series.Clear();
            Random random = new Random();
            bool fft = false;
            this.ContingencyTableChart.Header.Text = this.resManager.GetString("FirstContingencyTable");

            //for miners with boolean antecedents and succedents - for now only 4ft
            foreach (BooleanLiteralStruct booleanLiteral in hypothese.booleanLiterals)
            {
                if ((booleanLiteral.cedentType == CedentEnum.Antecedent) || (booleanLiteral.cedentType == CedentEnum.Succedent))
                {
                    fft = true;
                    break;
                }
            }
            // int i = 0;
            int j = 0;

            int[][] transpondedTable = FerdaResultBrowserControl.Transpose(hypothese.quantifierSetting.firstContingencyTableRows);

            for (int k = transpondedTable.GetUpperBound(0); k >= 0; k--)
            {
                Steema.TeeChart.Styles.Bar barSeries = new Steema.TeeChart.Styles.Bar();
                barSeries.Color = System.Drawing.Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                barSeries.MultiBar = Steema.TeeChart.Styles.MultiBars.None;

                foreach (int number in transpondedTable[k])
                {
                    if ((fft) && (j == 0))
                    {
                        barSeries.Add(number, this.resManager.GetString("ColumnAntecedent"));
                    }
                    else
                    {
                        if ((fft) && (j == 1))
                        {
                            barSeries.Add(number, '\u00AC' + this.resManager.GetString("ColumnAntecedent"));
                        }
                        else
                        {
                            string seriesTitle = String.Empty;
                            foreach (LiteralStruct literal in hypothese.literals)
                            {
                                if (literal.cedentType == CedentEnum.Antecedent)
                                {
                                    seriesTitle = literal.categoriesNames[j];
                                }
                            }
                            barSeries.Add(number, seriesTitle);
                        }
                    }
                    j++;
                }

                if (this.CheckBoxShowLabels.Checked)
                {
                    barSeries.Marks.Visible = true;
                }
                else
                {
                    barSeries.Marks.Visible = false;
                }
                barSeries.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;
                if (fft)
                {
                    if (k == 0)
                    {
                        barSeries.Title = this.resManager.GetString("ColumnSuccedent");
                    }
                    else
                    {
                        barSeries.Title = '\u00AC' + this.resManager.GetString("ColumnSuccedent");
                    }
                }
                else
                {
                    string seriesTitle = String.Empty;
                    foreach (LiteralStruct literal in hypothese.literals)
                    {
                        if (literal.cedentType == CedentEnum.Succedent)
                        {
                            seriesTitle = literal.categoriesNames[k];
                            break;
                        }
                    }
                    if (seriesTitle == String.Empty)
                    {
                        foreach (LiteralStruct literal in hypothese.literals)
                        {
                            if (literal.cedentType == CedentEnum.Antecedent)
                            {
                                seriesTitle = literal.categoriesNames[k];
                            }
                        }
                    }

                    barSeries.Title = seriesTitle;
                }
                chart.Series.Add(barSeries);
                //   i++;
                j = 0;
            }
        }

        /// <summary>
        /// Method which toggles labels on/off for chart.
        /// </summary>
        /// <param name="chart">Chart to toggle lables for</param>
        private void ShowLabels(Steema.TeeChart.TChart chart)
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripCopyChart_Click(object sender, EventArgs e)
        {
            Bitmap bitMap = new Bitmap(
                        this.ContingencyTableChart.Bitmap,
                        this.ContingencyTableChart.Size.Width,
                        this.ContingencyTableChart.Size.Height
                        );
            Clipboard.SetImage(bitMap);
        }

        void ContingencyTableChart_GetAxisLabel(object sender, Steema.TeeChart.GetAxisLabelEventArgs e)
        {

            Steema.TeeChart.Axis axis = (Steema.TeeChart.Axis)sender;
            if (axis.Equals(ContingencyTableChart.Axes.Bottom))
            {
                // e.LabelText = "WOW" + e.ValueIndex + " " + e.LabelText;


                // e.Series[0].Label = "1Wow";

                /*
                switch (e.LabelIndex)
                { 
                    case 0:
                        e.LabelValue = 5;
                        break;
                    case 1:
                        e.LabelValue = 13;
                        break;
                    case 2:
                        e.LabelValue = 19;
                        break;
                    default:
                        e.Stop = true;
                        break; 
                } */
            }
        }

        #endregion
    }
}

