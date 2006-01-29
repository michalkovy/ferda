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
            this.ContingencyTableChart.DoubleClick += new EventHandler(tChart1_DoubleClick);
            this.ContingencyTableChart.ContextMenuStrip = this.ContextMenuGraphRightClick;
            this.ResultBrowserSplit.Panel2.Controls.Add(ContingencyTableChart);
            this.ResultBrowserSplit.Panel2.ResumeLayout(false);
            this.ResultBrowserSplit.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion


        #region TrackBar handlers (modifying graph view)

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

        private void TrackBarRotation_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.Rotation = this.TrackBarRotation.Value;
        }

        private void TrackBarPerspective_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.Perspective = this.TrackBarPerspective.Value;
        }

        private void TrackBarElevation_Scroll(object sender, EventArgs e)
        {
            this.ContingencyTableChart.Aspect.Elevation = this.TrackBarElevation.Value;
        }

        #endregion


        #region Debugging

        void tChart1_DoubleClick(object sender, EventArgs e)
        {
            ContingencyTableChart.ShowEditor();
        }

        #endregion


        #region Other private methods
        /// <summary>
        /// Drawing first contingency table to chart
        /// </summary>
        /// <param name="hypothese">Hypothese to take contingency table from</param>
        /// <param name="chart">Chart to draw bars into</param>
        private void DrawBarsFromFirstTable(HypothesisStruct hypothese,Steema.TeeChart.TChart chart )
        {
            chart.Series.Clear();
           Random random = new Random();
            foreach (int[] arr in hypothese.quantifierSetting.firstContingencyTableRows)
            {
                Steema.TeeChart.Styles.Bar barSeries = new Steema.TeeChart.Styles.Bar();
                barSeries.Color = System.Drawing.Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                barSeries.MultiBar = Steema.TeeChart.Styles.MultiBars.None;

                foreach (int number in arr)
                {
                    barSeries.Add(number);
                }
                chart.Series.Add(barSeries);
            }
        }
        #endregion
    }
}

