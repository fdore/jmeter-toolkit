using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using JMeter.Toolkit.Engine.Extensions;
using JMeter.Toolkit.Services.Spec;
using ZedGraph;

namespace JMeter.Toolkit.Engine.Charts
{
    public abstract class BaseChartBuilder : IChartBuilder
    {
        public int BottomMargin { get; set; }
        public int LeftMargin { get; set; }

        /// <summary>
        /// Chart title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Width of the generated chart
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the generated chart
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Resolution of the generated chart
        /// Default to 32dpi
        /// </summary>
        public float Dpi { get; set; }

        /// <summary>
        /// Render individual bar
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="d"></param>
        /// <param name="dataResults"></param>
        /// <param name="selectedIndex"></param>
        /// <param name="x"></param>
        /// <param name="func">Function to retrieve color to apply to the bar</param>
        protected virtual void RenderBar(GraphPane gp, IEnumerable<RequestDataResults> dataResults, int selectedIndex, float x, Func<IEnumerable<RequestDataResults>, int, Color> func)
        {
            var color = Color.DarkGray;
            if(func != null)
            {
                color = func(dataResults, selectedIndex);
            }
            var d = dataResults.ElementAt(selectedIndex);
            var box = new BoxObj(x, d.AverageResponseTime, 1, d.AverageResponseTime)
            {
                Fill = { Color = color }
            };
            gp.GraphObjList.Add(box);
        }

        /// <summary>
        /// Render min/max line
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="d"></param>
        /// <param name="x"></param>
        /// <param name="maxY"></param>
        protected virtual void RenderMinMaxLine(GraphPane gp, RequestDataResults d, int x, int maxY)
        {
            double max = Math.Min(maxY, d.MaxResponseTime);
            int factor = 60;
            var line = new LineObj(Color.FromArgb(75, factor, factor, factor), x, max, x, d.MinResponseTime);
            gp.GraphObjList.Add(line);
        }

        /// <summary>
        /// Render min/max response time excluding the 10% fastest and 10% slowest requests
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="d"></param>
        /// <param name="x"></param>
        /// <param name="maxY"></param>
        protected virtual void RenderMinMaxExcludingExtremesLine(GraphPane gp, RequestDataResults d, int x, int maxY)
        {
            double max = Math.Min(maxY, d.MaxResponseTimeExcludingBottomDecile);
             int factor = 60;
            var line = new LineObj(Color.FromArgb(75, factor, factor, factor), x, max, x, d.MinResponseTimeExcludingTopDecile);
            line.Line.Width = 3;
            gp.GraphObjList.Add(line);
        }

        /// <summary>
        /// Render average response time excluding the 10% fastest and 10% slowest requests
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="d"></param>
        /// <param name="x"></param>
        protected virtual void RenderAverageLine(GraphPane gp, RequestDataResults d, int x)
        {
            int factor = 60;
            var line = new LineObj(Color.FromArgb(100, factor, factor, factor), x-0.3, d.AverageResponseTimeExcludingTopAndBottomDeciles, x+0.3, d.AverageResponseTimeExcludingTopAndBottomDeciles);
            line.Line.Width = 3;
            gp.GraphObjList.Add(line);
        }

        /// <summary>
        /// Render text inside bar
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="d"></param>
        /// <param name="x"></param>
        protected virtual void RenderBarContent(GraphPane gp, RequestDataResults d, float x)
        {
            var avgResponseTimeText = new TextObj(d.AverageResponseTime.ToString(), x, d.AverageResponseTime / 2).Style(Color.White, 7);
            gp.GraphObjList.Add(avgResponseTimeText);
        }

        /// <summary>
        /// Render text at the bottom of a bar
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="d"></param>
        /// <param name="x"></param>
        protected virtual void RenderBarLabel(GraphPane gp, RequestDataResults d, float x)
        {
           // Do nothing
        }

        /// <summary>
        /// Render chart title
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="dataResults"></param>
        protected virtual void RenderTitle(GraphPane gp, IEnumerable<RequestDataResults> dataResults)
        {
           // Do nothing
        }

        /// <summary>
        /// Render legend
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="dataResults"></param>
        protected virtual void RenderLegend(GraphPane gp, IEnumerable<RequestDataResults> dataResults)
        {
            // Do nothing
        }

        /// <summary>
        /// Render X Axis
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="title"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        protected virtual void RenderXAxis(GraphPane gp, string title, int min, int max)
        {
            gp.XAxis.Title.Text = title;
            gp.XAxis.Title.FontSpec.Size = 10;
            gp.XAxis.MajorTic.IsBetweenLabels = false;
            gp.XAxis.MajorTic.IsAllTics = false;
            gp.XAxis.MinorTic.IsAllTics = false;
            gp.XAxis.Type = AxisType.Text;
            gp.XAxis.Scale.FontSpec.Size = 10;
            gp.XAxis.Scale.Min = min;
            gp.XAxis.Scale.Max = max;
        }

        /// <summary>
        /// Render Y Axis
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="title"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        protected virtual void RenderYAxis(GraphPane gp, string title, int min, int max)
        {
            gp.YAxis.Title.Text = title;
            gp.YAxis.Title.FontSpec.Size = 7;
            gp.YAxis.AxisGap = 10;
            gp.YAxis.Scale.Min = min;
            gp.YAxis.Scale.Max = max;
            gp.YAxis.Scale.FontSpec.Size = 7;
            gp.YAxis.MajorGrid.IsVisible = true;
        }

        /// <summary>
        /// Generate color of the bars
        /// </summary>
        /// <param name="dataResults"></param>
        /// <param name="selectedIndex"></param>
        /// <returns></returns>
        protected virtual Color GenerateBarColor(IEnumerable<RequestDataResults> dataResults, int selectedIndex)
        {
            return Color.DarkGray;
        }

        /// <summary>
        /// Render background
        /// </summary>
        /// <param name="gp"></param>
        protected virtual void RenderBackground(GraphPane gp)
        {
            gp.Chart.Fill = new Fill(Color.White, Color.DimGray, 45F);
        }

        /// <summary>
        /// Render time at which the graph has been generated
        /// </summary>
        /// <param name="gp"></param>
        protected virtual void RenderTimeTag(GraphPane gp)
        {            
            var textObj = new TextObj(DateTime.Now.ToString(), gp.XAxis.Scale.Max, gp.YAxis.Scale.Max/100).Style(Color.White, 5, AlignH.Right);
            gp.GraphObjList.Add(textObj);
        }

        /// <summary>
        /// Render curve representing the distribution of response time
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="dataResults"></param>
        /// <param name="index"></param>
        /// <param name="max"></param>
        protected virtual void RenderResponseTimeDistributionCurve(GraphPane gp, RequestDataResults dataResults, int index, int max)
        {
            if (dataResults.ResponseTimeDistribution != null && dataResults.ResponseTimeDistribution.Count() > 0)
            {
                int pivot = dataResults.ResponseTimeDistribution.Count()/2;
                var points = new PointPairList();

                float x = index;
                points.Add(x, dataResults.MinResponseTime);
                for (int i = 0; i <dataResults.ResponseTimeDistribution.Count(); i++)
                {
                    float offset;
                    if (i < pivot)
                    {
                        offset = 0.1f;
                    }
                    else
                    {
                        offset = -0.1f;
                    }
                    x += offset;
                    points.Add(new PointPair(x, Math.Min(dataResults.ResponseTimeDistribution[i], max)));
                }
                int factor = 40;
                for (int i = 0; i < dataResults.ResponseTimeDistribution.Count(); i++)
                {
                    var line = new LineObj(Color.FromArgb(100, factor, factor, factor), points[i].X, points[i].Y,
                                           points[i + 1].X, points[i + 1].Y);
                    line.Line.Width = 1;
                    gp.GraphObjList.Add(line);
                }
            }
        }

        /// <summary>
        /// Generate image, and save it to the output stream
        /// </summary>
        /// <param name="outputStream"></param>
        /// <param name="dataResults"></param>
        public void Generate(Stream outputStream, IEnumerable<RequestDataResults> dataResults)
        {
            var gp = new GraphPane();

            int index = 1;
            var maxY = (int)(dataResults.Max(x => x.AverageResponseTime) * 1.2);
            var orderedResults = dataResults.OrderBy(x => x.Date);
            foreach (var d in orderedResults)
            {
                float offset = 0.5f;

                // Create bar
                RenderBar(gp, orderedResults, index - 1, index - offset, GenerateBarColor);

                // Add average response time on top of the bar
                RenderBarContent(gp, d, index);

                // Add request at the bottom of the bar
                RenderBarLabel(gp, d, index);

                // Add min/max point to list
                RenderMinMaxLine(gp, d, index, maxY);

                RenderMinMaxExcludingExtremesLine(gp, d, index, maxY);

                RenderAverageLine(gp, d, index);

                RenderResponseTimeDistributionCurve(gp, d, index, maxY);

                index++;
            }
            
            // Title
            RenderTitle(gp, orderedResults);

            // X Axis
            RenderXAxis(gp, string.Empty, 0, index + 1);
           
            // Y Axis
            RenderYAxis(gp, "Response time (in ms)", 0, maxY);

            // Add legend
            RenderLegend(gp, orderedResults);

            // Fill background
            RenderBackground(gp);

            // Add time tag
            RenderTimeTag(gp);

            // Add space at the bottom
            gp.Margin.Bottom = BottomMargin;
            gp.Margin.Left = LeftMargin;

            // Refresh panel
            gp.AxisChange();

            // Render image
            var bitmap = gp.GetImage(Width, Height, Dpi);
            bitmap.Save(outputStream, ImageFormat.Jpeg);
        }

       

    }


}
