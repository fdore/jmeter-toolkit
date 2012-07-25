using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using JMeter.Toolkit.Services.Spec;
using ZedGraph;

namespace JMeter.Toolkit.Engine.Charts
{
    public class TrendChartBuilder : BaseChartBuilder
    {
        /// <summary>
        /// Create trend chart
        /// </summary>
        public TrendChartBuilder()
        {
            // Set default values
            Width = 1920;
            Height = 1200;
            Dpi = 32f;
            BottomMargin = 40;
            LeftMargin = 40;
        }


        protected override void RenderBarLabel(GraphPane gp, RequestDataResults d, float x)
        {
            var dateText = new TextObj(d.Date.ToShortDateString() + "   .", x, 0);
            dateText.FontSpec.Fill.IsVisible = false;
            dateText.FontSpec.Border.IsVisible = false;
            dateText.FontSpec.FontColor = Color.Black;
            dateText.FontSpec.Size = 7;
            dateText.FontSpec.Angle = 45;
            dateText.Location.AlignH = AlignH.Right;
            gp.GraphObjList.Add(dateText);
        }

        protected override void RenderTitle(GraphPane gp, IEnumerable<RequestDataResults> dataResults)
        {
            gp.Title.Text = string.Format("Trend chart {0} - {1} {2}", dataResults.Min(x => x.Date).ToShortDateString(), dataResults.Max(x => x.Date).ToShortDateString(), dataResults.First().Request);
            gp.Title.FontSpec.Size = 10;
        }

        protected override void RenderLegend(GraphPane gp, IEnumerable<RequestDataResults> dataResults)
        {
            var hostName = new TextObj(dataResults.First().HostName, dataResults.Count() + 1, dataResults.Max(x => x.AverageResponseTime) * 1.1);
            hostName.FontSpec.Fill.IsVisible = false;
            hostName.FontSpec.Border.IsVisible = false;
            hostName.FontSpec.FontColor = Color.Black;
            hostName.FontSpec.Size = 7;
            gp.GraphObjList.Add(hostName);

            var date = new TextObj(DateTime.Now.ToString(), dataResults.Count() + 1, dataResults.Max(x => x.AverageResponseTime) * 1.05);
            date.FontSpec.Fill.IsVisible = false;
            date.FontSpec.Border.IsVisible = false;
            date.FontSpec.FontColor = Color.Black;
            date.FontSpec.Size = 7;
            gp.GraphObjList.Add(date);
        }

        protected override Color GenerateBarColor(IEnumerable<RequestDataResults> dataResults, int selectedIndex)
        {
            return GenerateColor(dataResults, selectedIndex);
        }

        /// <summary>
        /// Generate color based on response time of selected result
        /// </summary>
        /// <param name="dataResults"></param>
        /// <param name="selectedIndex"></param>
        /// <returns>Green if the current result is better than the previous one, or red if not</returns>
        private Color GenerateColor(IEnumerable<RequestDataResults> dataResults, int selectedIndex)
        {
            var lastReponseTime = double.MaxValue;
            if(selectedIndex > 0)
            {
                lastReponseTime = dataResults.ElementAt(selectedIndex - 1).AverageResponseTime;
            }
            var d = dataResults.ElementAt(selectedIndex);

            Color color = Color.FromArgb(75, Color.DarkGreen);
            if (d.AverageResponseTime > lastReponseTime)
                color = Color.FromArgb(75, Color.Red);

            return color;
        }

    }
}
