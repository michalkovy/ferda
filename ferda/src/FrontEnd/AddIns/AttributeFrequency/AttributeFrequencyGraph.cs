using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.FrontEnd.AddIns.AttributeFrequency.NonGUIClasses;

namespace Ferda.FrontEnd.AddIns.AttributeFrequency
{
    public partial class AttributeFrequency
    {
        #region Private variables

        private Steema.TeeChart.TChart AttributeFrequencyChart;

        #endregion

        #region Initialization

        /// <summary>
        /// Method to initialize Steema chart component for displaying contingency table
        /// </summary>
        private void InitializeGraph()
        {
            this.TabPageGraph.SuspendLayout();
            this.SuspendLayout();
            this.AttributeFrequencyChart = new Steema.TeeChart.TChart();

            this.AttributeFrequencyChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeFrequencyChart.Header.Lines = new string[] { resManager.GetString("AttributeFrequencyChart") };
            this.AttributeFrequencyChart.Location = new System.Drawing.Point(0, 0);

            this.AttributeFrequencyChart.Header.Visible = true;

            this.AttributeFrequencyChart.Aspect.View3D = false;
            this.AttributeFrequencyChart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
            //     this.AttributeFrequencyChart.DoubleClick += new EventHandler(tChart1_DoubleClick);
            this.AttributeFrequencyChart.Size = new System.Drawing.Size(466, 286);

            this.TabPageGraph.Controls.Add(AttributeFrequencyChart);
            this.TabPageGraph.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        #region Other private methods
        /*
        void ColumnFrequencyChart_GetAxisLabel(object sender, Steema.TeeChart.GetAxisLabelEventArgs e)
        {
            if (((Steema.TeeChart.Axis)sender).Equals(this.ColumnFrequencyChart.Axes.Bottom))
            {
                
                marks.Style = Steema.TeeChart.Styles.MarksStyles.Legend;
               e.LabelText = Convert.ToString(e.Series.LegendString(e.ValueIndex,style));
            } 
        }*/


        /// <summary>
        /// Method to draw frequency graph from the datatable
        /// </summary>
        /// <param name="dataTable">DataTable with the source data</param>
        /// <param name="chart">Chart to insert bars into</param>
        private void DrawBarsFromDataTable(ArrayList categoriesFrequency, Steema.TeeChart.TChart chart)
        {
            chart.Series.RemoveAllSeries();
            Steema.TeeChart.Styles.HorizBar barSeriesBar;
            Steema.TeeChart.Styles.Pie barSeriesPie;
            if (this.ToolStripMenuItemPie.Checked)
            {
                barSeriesPie = new Steema.TeeChart.Styles.Pie();
                barSeriesPie.ColorEach = true;
                barSeriesPie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;

                barSeriesPie.Marks.Visible = true;
                foreach (CategoryFrequency catFrequency in categoriesFrequency)
                {
                    double temp;
                    try
                    {
                        temp = Convert.ToDouble(catFrequency.count);
                    }
                    catch
                    {
                        temp = 0;
                    }
                    if (this.ToolStripMenuItemAbsolute.Checked)
                    {
                        barSeriesPie.Add(temp, catFrequency.key);
                    }
                    else
                    {
                        if ((temp != 0) && (this.rowCount != 0))
                        {
                            temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                            barSeriesPie.Add(temp, catFrequency.key);
                        }
                        else
                        {
                            barSeriesPie.Add(0, catFrequency.key);
                        }
                    }
                    chart.Series.Add(barSeriesPie);
                }
            }
            else
            {
                barSeriesBar = new Steema.TeeChart.Styles.HorizBar();
                barSeriesBar.MultiBar = Steema.TeeChart.Styles.MultiBars.SideAll;

                barSeriesBar.ColorEach = true;
                barSeriesBar.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;

                barSeriesBar.Marks.Visible = true;
                foreach (CategoryFrequency catFrequency in categoriesFrequency)
                {
                    double temp;
                    try
                    {
                        temp = Convert.ToDouble(catFrequency.count);
                    }
                    catch
                    {
                        temp = 0;
                    }
                    if (this.ToolStripMenuItemAbsolute.Checked)
                    {
                        barSeriesBar.Add(temp, catFrequency.key);
                    }
                    else
                    {
                        if ((temp != 0) && (this.rowCount != 0))
                        {
                            temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                            barSeriesBar.Add(temp, catFrequency.key);
                        }
                        else
                        {
                            barSeriesBar.Add(0, catFrequency.key);
                        }
                    }
                    chart.Series.Add(barSeriesBar);
                }
            }
        }

        
        #endregion

        #region Debugging

        /*
        void tChart1_DoubleClick(object sender, EventArgs e)
        {
            ColumnFrequencyChart.ShowEditor();
        }
        */
        #endregion
        }
    }
