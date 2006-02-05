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

        /// <summary>
        /// Bar chart
        /// </summary>
        private Steema.TeeChart.TChart AttributeFrequencyBarChart;

        /// <summary>
        /// Area chart
        /// </summary>
        private Steema.TeeChart.TChart AttributeFrequencyAreaChart;

        /// <summary>
        /// Pie chart
        /// </summary>
        private Steema.TeeChart.TChart AttributeFrequencyPieChart;

        #endregion


        #region Initialization


        /// <summary>
        /// Method to initialize Steema chart component for displaying contingency table
        /// </summary>
        private void InitializeGraph()
        {
            this.TabPageBarChart.SuspendLayout();
            this.SuspendLayout();
            this.AttributeFrequencyBarChart = new Steema.TeeChart.TChart();
            this.AttributeFrequencyBarChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeFrequencyBarChart.Header.Lines = new string[] { resManager.GetString("AttributeFrequencyBarChart") };
            this.AttributeFrequencyBarChart.Location = new System.Drawing.Point(0, 0);
            this.AttributeFrequencyBarChart.Header.Visible = true;
            this.AttributeFrequencyBarChart.Aspect.View3D = false;
            this.AttributeFrequencyBarChart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
            //     this.AttributeFrequencyChart.DoubleClick += new EventHandler(tChart1_DoubleClick);
            this.AttributeFrequencyBarChart.Size = new System.Drawing.Size(466, 286);
            this.TabPageBarChart.Controls.Add(AttributeFrequencyBarChart);

            this.AttributeFrequencyAreaChart = new Steema.TeeChart.TChart();
            this.AttributeFrequencyAreaChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeFrequencyAreaChart.Header.Lines = new string[] { resManager.GetString("AttributeFrequencyAreaChart") };
            this.AttributeFrequencyAreaChart.Location = new System.Drawing.Point(0, 0);
            this.AttributeFrequencyAreaChart.Header.Visible = true;
            this.AttributeFrequencyAreaChart.Aspect.View3D = false;
            this.AttributeFrequencyAreaChart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
            //     this.AttributeFrequencyChart.DoubleClick += new EventHandler(tChart1_DoubleClick);
            this.AttributeFrequencyAreaChart.Size = new System.Drawing.Size(466, 286);
            this.TabPageAreaChart.Controls.Add(AttributeFrequencyAreaChart);

            this.AttributeFrequencyPieChart = new Steema.TeeChart.TChart();
            this.AttributeFrequencyPieChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeFrequencyPieChart.Header.Lines = new string[] { resManager.GetString("AttributeFrequencyPieChart") };
            this.AttributeFrequencyPieChart.Location = new System.Drawing.Point(0, 0);
            this.AttributeFrequencyPieChart.Header.Visible = true;
            this.AttributeFrequencyPieChart.Aspect.View3D = false;
            this.AttributeFrequencyPieChart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
            //     this.AttributeFrequencyChart.DoubleClick += new EventHandler(tChart1_DoubleClick);
            this.AttributeFrequencyPieChart.Size = new System.Drawing.Size(466, 286);
            this.TabPagePieChart.Controls.Add(AttributeFrequencyPieChart);

            this.TabPageBarChart.ResumeLayout(false);
            this.ResumeLayout(false);
        }



        #endregion



        #region Other private methods

        /// <summary>
        /// Method to draw frequency bar graph from the datatable
        /// </summary>
        /// <param name="dataTable">DataTable with the source data</param>
        /// <param name="chart">Chart to insert bars into</param>
        private void DrawBarsFromDataTable(ArrayList categoriesFrequency, Steema.TeeChart.TChart chart)
        {
            chart.Series.RemoveAllSeries();
            Steema.TeeChart.Styles.HorizBar barSeriesBar;
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

        /// <summary>
        /// Method to draw frequency area graph from the datatable
        /// </summary>
        /// <param name="dataTable">DataTable with the source data</param>
        /// <param name="chart">Chart to insert area into</param>
        private void DrawAreaFromDataTable(ArrayList categoriesFrequency, Steema.TeeChart.TChart chart)
        {
            chart.Series.RemoveAllSeries();
            Steema.TeeChart.Styles.Area areaSeries;
            areaSeries = new Steema.TeeChart.Styles.Area();
            areaSeries.ColorEach = true;
            areaSeries.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;
            areaSeries.Marks.Visible = true;
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
                    areaSeries.Add(temp, catFrequency.key);
                }
                else
                {
                    if ((temp != 0) && (this.rowCount != 0))
                    {
                        temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                        areaSeries.Add(temp, catFrequency.key);
                    }
                    else
                    {
                        areaSeries.Add(0, catFrequency.key);
                    }
                }
                chart.Series.Add(areaSeries);
            }
        }


        /// <summary>
        /// Method to draw frequency pie graph from the datatable
        /// </summary>
        /// <param name="dataTable">DataTable with the source data</param>
        /// <param name="chart">Chart to insert pie into</param>
        private void DrawPieFromDataTable(ArrayList categoriesFrequency, Steema.TeeChart.TChart chart)
        {
            chart.Series.RemoveAllSeries();
            Steema.TeeChart.Styles.Pie pieSeries;
            pieSeries = new Steema.TeeChart.Styles.Pie();
            pieSeries.ColorEach = true;
            pieSeries.Marks.Style = Steema.TeeChart.Styles.MarksStyles.LabelValue;
            pieSeries.Marks.Visible = true;
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
                    pieSeries.Add(temp, catFrequency.key);
                }
                else
                {
                    if ((temp != 0) && (this.rowCount != 0))
                    {
                        temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                        pieSeries.Add(temp, catFrequency.key);
                    }
                    else
                    {
                        pieSeries.Add(0, catFrequency.key);
                    }
                }
                chart.Series.Add(pieSeries);
            }
        }

        /// <summary>
        /// Handles copying the chart to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItemCopyChart_Click(object sender, EventArgs e)
        {
            switch (TabControlAttributeFrequency.SelectedIndex)
            {
                case 1:
                    Bitmap bitMap = new Bitmap(
                        this.AttributeFrequencyAreaChart.Bitmap,
                        this.AttributeFrequencyAreaChart.Size.Width,
                        this.AttributeFrequencyAreaChart.Size.Height
                        );
                    Clipboard.SetImage(bitMap);
                    break;

                case 2:
                    Bitmap bitMap1 = new Bitmap(
                        this.AttributeFrequencyBarChart.Bitmap,
                        this.AttributeFrequencyBarChart.Size.Width,
                        this.AttributeFrequencyBarChart.Size.Height
                        );
                    Clipboard.SetImage(bitMap1);
                    break;

                case 3:
                    Bitmap bitMap2 = new Bitmap(
                         this.AttributeFrequencyPieChart.Bitmap,
                         this.AttributeFrequencyPieChart.Size.Width,
                         this.AttributeFrequencyPieChart.Size.Height
                         );
                    Clipboard.SetImage(bitMap2);
                    break;

                default:
                    break;
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