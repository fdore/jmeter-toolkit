using System.Drawing;
using ZedGraph;

namespace JMeter.Toolkit.Engine.Extensions
{
    public static class TextObjExtensions
    {
        /// <summary>
        /// Apply style to Zedgraph text object
        /// </summary>
        /// <param name="textObj"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        public static TextObj Style(this TextObj textObj, Color color, int size)
        {
            textObj.FontSpec.Fill.IsVisible = false;
            textObj.FontSpec.Border.IsVisible = false;
            textObj.FontSpec.FontColor = color;
            textObj.FontSpec.Size = size;
            textObj.FontSpec.Size = size;
            return textObj;
        }

        /// <summary>
        /// Apply style to Zedgraph text object
        /// </summary>
        /// <param name="textObj"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="alignH"></param>
        public static TextObj Style(this TextObj textObj, Color color,  int size, AlignH alignH)
        {
            textObj.Style(color, size);
            textObj.Location.AlignH = alignH;
            return textObj;
        }
    }
}
