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

        /// <summary>
        /// Area chart
        /// </summary>
        private Steema.TeeChart.TChart ColumnFrequencyAreaChart;

        /// <summary>
        /// Bar chart
        /// </summary>
        private Steema.TeeChart.TChart ColumnFrequencyBarChart;

        /// <summary>
        /// Pie chart
        /// </summary>
        private Steema.TeeChart.TChart ColumnFrequencyPieChart;

        #endregion


        #region Initialization



        /// <summary>
        /// Method to initialize Steema chart component for displaying contingency table
        /// </summary>
        private void InitializeGraph()
        {
            this.TabPageBarChart.SuspendLayout();
            this.SuspendLayout();
            this.ColumnFrequencyBarChart = new Steema.TeeChart.TChart();
            this.ColumnFrequencyBarChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnFrequencyBarChart.Header.Lines = new string[] { resManager.GetString("ColumnFrequencyBarChart") };
            this.ColumnFrequencyBarChart.Location = new System.Drawing.Point(0, 0);
            this.ColumnFrequencyBarChart.Header.Visible = true;
            this.ColumnFrequencyBarChart.Aspect.View3D = false;
            this.ColumnFrequencyBarChart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
            //     this.ColumnFrequencyChart.DoubleClick += new EventHandler(tChart1_DoubleClick);
            this.ColumnFrequencyBarChart.Size = new System.Drawing.Size(466, 286);
            this.TabPageBarChart.Controls.Add(ColumnFrequencyBarChart);
            this.TabPageBarChart.ResumeLayout(false);

            this.ColumnFrequencyAreaChart = new Steema.TeeChart.TChart();
            this.ColumnFrequencyAreaChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnFrequencyAreaChart.Header.Lines = new string[] { resManager.GetString("ColumnFrequencyAreaChart") };
            this.ColumnFrequencyAreaChart.Location = new System.Drawing.Point(0, 0);
            this.ColumnFrequencyAreaChart.Header.Visible = true;
            this.ColumnFrequencyAreaChart.Aspect.View3D = false;
            this.ColumnFrequencyAreaChart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
            this.ColumnFrequencyAreaChart.Size = new System.Drawing.Size(466, 286);
            this.TabPageAreaChart.Controls.Add(ColumnFrequencyAreaChart);

            this.ColumnFrequencyPieChart = new Steema.TeeChart.TChart();
            this.ColumnFrequencyPieChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnFrequencyPieChart.Header.Lines = new string[] { resManager.GetString("ColumnFrequencyPieChart") };
            this.ColumnFrequencyPieChart.Location = new System.Drawing.Point(0, 0);
            this.ColumnFrequencyPieChart.Header.Visible = true;
            this.ColumnFrequencyPieChart.Aspect.View3D = false;
            this.ColumnFrequencyPieChart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
            this.ColumnFrequencyPieChart.Size = new System.Drawing.Size(466, 286);
            this.TabPagePieChart.Controls.Add(ColumnFrequencyPieChart);
            this.TabPagePieChart.ResumeLayout(false);
            this.ResumeLayout(false);
        }


        #endregion


        #region Other private methods

        /// <summary>
        /// Method to draw frequency area chart from the datatable
        /// </summary>
        /// <param name="dataTable">DataTable with the source data</param>
        /// <param name="chart">Chart to insert area into</param>
        private void DrawAreaFromDataTable(DataTable dataTable, Steema.TeeChart.TChart chart)
        {
            chart.Series.RemoveAllSeries();
            Steema.TeeChart.Styles.Area areaSeries = new Steema.TeeChart.Styles.Area();
            areaSeries.ColorEach = true;
            areaSeries.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;
            areaSeries.Marks.Visible = true;
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
                    areaSeries.Add(temp, dataRow[0].ToString());
                }
                else
                {
                    if ((temp != 0) && (this.rowCount != 0))
                    {
                        temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                        areaSeries.Add(temp, dataRow[0].ToString());
                    }
                    else
                    {
                        areaSeries.Add(0, dataRow[0].ToString());
                    }
                }
                chart.Series.Add(areaSeries);
            }
        }

        /// <summary>
        /// Method to draw frequency bar chart from the datatable
        /// </summary>
        /// <param name="dataTable">DataTable with the source data</param>
        /// <param name="chart">Chart to insert bars into</param>
        private void DrawBarsFromDataTable(DataTable dataTable, Steema.TeeChart.TChart chart)
        {
            chart.Series.RemoveAllSeries();
            Steema.TeeChart.Styles.HorizBar barSeriesBar;
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

        /// <summary>
        /// Method to draw frequency pie chart from the datatable
        /// </summary>
        /// <param name="dataTable">DataTable with the source data</param>
        /// <param name="chart">Chart to insert pie into</param>
        private void DrawPieFromDataTable(DataTable dataTable, Steema.TeeChart.TChart chart)
        {
            chart.Series.RemoveAllSeries();
            Steema.TeeChart.Styles.Pie pieSeries = new Steema.TeeChart.Styles.Pie();

            pieSeries.ColorEach = true;
            pieSeries.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;
            pieSeries.Marks.Visible = true;
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
                    pieSeries.Add(temp, dataRow[0].ToString());
                }
                else
                {
                    if ((temp != 0) && (this.rowCount != 0))
                    {
                        temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                        pieSeries.Add(temp, dataRow[0].ToString());
                    }
                    else
                    {
                        pieSeries.Add(0, dataRow[0].ToString());
                    }
                }
                chart.Series.Add(pieSeries);
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

