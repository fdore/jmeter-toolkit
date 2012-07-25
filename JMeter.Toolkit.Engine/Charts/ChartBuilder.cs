using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using JMeter.Toolkit.Engine.Extensions;
using JMeter.Toolkit.Services.Spec;
using ZedGraph;

namespace JMeter.Toolkit.Engine.Charts
{
    public class ChartBuilder : BaseChartBuilder
    {
        public ChartBuilder()
        {
            // Set default values
            Width = 1920;
            Height = 1200;
            Dpi = 32f;
            BottomMargin = 100;
            LeftMargin = 100;
        }

        /// <summary>
        /// Render chart title
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="dataResults"></param>
        protected override void RenderTitle(GraphPane gp, IEnumerable<RequestDataResults> dataResults)
        {
            gp.Title.Text = string.Format("{0} - {1}", dataResults.First().HostName, dataResults.First().Date.ToShortDateString());
            gp.Title.FontSpec.Size = 10;
        }


        /// <summary>
        /// Render text at the bottom of a bar
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="d"></param>
        /// <param name="x"></param>
        protected override void RenderBarLabel(GraphPane gp, RequestDataResults d, float x)
        {
            var requestText = new TextObj(d.Request + "   -", x, 0).Style(Color.Black, 7);
            requestText.FontSpec.Angle = 25;
            requestText.Location.AlignH = AlignH.Right;
            gp.GraphObjList.Add(requestText);
        }

        /// <summary>
        /// Generate color of the bars
        /// </summary>
        /// <param name="dataResults"></param>
        /// <param name="selectedIndex"></param>
        /// <returns></returns>
        protected override Color GenerateBarColor(IEnumerable<RequestDataResults> dataResults, int selectedIndex)
        {
            return GenerateColor(dataResults, selectedIndex);
        }

        /// <summary>
        /// Generate random color based on selected result's average response time
        /// </summary>
        /// <param name="dataResults"></param>
        /// <param name="selectedIndex"></param>
        /// <returns></returns>
        private Color GenerateColor(IEnumerable<RequestDataResults> dataResults, int selectedIndex)
        {
            var d = dataResults.ElementAt(selectedIndex);
            return GetRandomColor((int)d.AverageResponseTime / 2);
        }

        /// <summary>
        /// Generate random color
        /// </summary>
        /// <param name="salt"></param>
        /// <returns></returns>
        private Color GetRandomColor(int salt)
        {
            var random = new Random(salt);
            return Color.FromArgb(100, random.Next(120, 255), random.Next(60, 255), random.Next(0, 255));
        }


        /// <summary>
        /// Render legend
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="dataResults"></param>
        protected override void RenderLegend(GraphPane gp, IEnumerable<RequestDataResults> dataResults)
        {
            int fontSize = 6;
            int offset = 0;
            int unit = (int) gp.YAxis.Scale.Max/10;
            // Average time
            var avgLegendBox = new BoxObj(-4, unit, 1, unit)
            {
                Fill = new Fill(Color.FromArgb(75, Color.YellowGreen))
            };
            gp.GraphObjList.Add(avgLegendBox);

            var avgLegendText = new TextObj("125", -3.5, (double)unit / 2).Style(Color.Black, fontSize);
            gp.GraphObjList.Add(avgLegendText);

            var avgLegendLabel = new TextObj("Average time", -4, (double)unit / 2).Style(Color.Black, fontSize, AlignH.Right);
            gp.GraphObjList.Add(avgLegendLabel);

            offset += unit*2;

            // Min/Max
            var minMaxLine = new LineObj(Color.Black, -3.5, offset, -3.5, offset + 2*unit);
            gp.GraphObjList.Add(minMaxLine);

            var minMaxLabel = new TextObj("Min/Max", -4, offset + unit).Style(Color.Black, fontSize, AlignH.Right);
            gp.GraphObjList.Add(minMaxLabel);

            offset += 3*unit;

            // Min/Max excluding extreme deciles
            var minMaxExclLine = new LineObj(Color.Black, -3.5, offset, -3.5, offset + 2*unit);
            minMaxExclLine.Line.Width = 3;            
            minMaxExclLine.Line.Color = Color.DarkGray;
            gp.GraphObjList.Add(minMaxExclLine);

            var avgExclLine = new LineObj(Color.Black, -3.8, offset + unit, -3.2, offset + unit);
            avgExclLine.Line.Width = 3;
            avgExclLine.Line.Color = Color.DarkGray;
            gp.GraphObjList.Add(avgExclLine);

            var avgExclLabel1 = new TextObj("Min/Max/Avg", -4, offset + 1.2 *unit).Style(Color.Black, fontSize, AlignH.Right);
            gp.GraphObjList.Add(avgExclLabel1);
            var avgExclLabel2 = new TextObj("excluding extremes", -4, offset + 0.8 * unit).Style(Color.Black, fontSize, AlignH.Right);
            gp.GraphObjList.Add(avgExclLabel2);
           
        }
    }
}
