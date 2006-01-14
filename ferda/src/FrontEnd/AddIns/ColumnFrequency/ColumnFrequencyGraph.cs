using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.ColumnFrequency
{
    public partial class ColumnFrequency
    {
        #region Private variables

        private Steema.TeeChart.TChart ColumnFrequencyChart;

        #endregion

        #region Initialization

        /// <summary>
        /// Method to initialize Steema chart component for displaying contingency table
        /// </summary>
        private void InitializeGraph()
        {
            this.TabPageGraph.SuspendLayout();
            this.SuspendLayout();
            this.ColumnFrequencyChart = new Steema.TeeChart.TChart();

            this.ColumnFrequencyChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnFrequencyChart.Header.Lines = new string[] { resManager.GetString("ColumnFrequencyChart") };
            this.ColumnFrequencyChart.Location = new System.Drawing.Point(0, 0);

            this.ColumnFrequencyChart.Header.Visible = true;

            this.ColumnFrequencyChart.Aspect.View3D = false;
            this.ColumnFrequencyChart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;

       //     this.ColumnFrequencyChart.DoubleClick += new EventHandler(tChart1_DoubleClick);

            this.ColumnFrequencyChart.Size = new System.Drawing.Size(466, 286);

            this.TabPageGraph.Controls.Add(ColumnFrequencyChart);
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
        private void DrawBarsFromDataTable(DataTable dataTable, Steema.TeeChart.TChart chart)
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    double temp;
                    try
                    {
                        temp = Convert.ToDouble(dataRow[1]);
                    }
                    catch
                    {
                        temp = 0;
                    }
                    if (this.ToolStripMenuItemAbsolute.Checked)
                    {
                        barSeriesPie.Add(temp, dataRow[0].ToString());
                    }
                    else
                    {
                        if ((temp != 0) && (this.rowCount != 0))
                        {
                            temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                            barSeriesPie.Add(temp, dataRow[0].ToString());
                        }
                        else
                        {
                            barSeriesPie.Add(0, dataRow[0].ToString());
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    double temp;
                    try
                    {
                        temp = Convert.ToDouble(dataRow[1]);
                    }
                    catch
                    {
                        temp = 0;
                    }
                    if (this.ToolStripMenuItemAbsolute.Checked)
                    {
                        barSeriesBar.Add(temp, dataRow[0].ToString());
                    }
                    else
                    {
                        if ((temp != 0) && (this.rowCount != 0))
                        {
                            temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                            barSeriesBar.Add(temp, dataRow[0].ToString());
                        }
                        else
                        {
                            barSeriesBar.Add(0, dataRow[0].ToString());
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
