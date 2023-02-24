using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Каналы

    // RGB

    public class rRGBFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int r = sourceImage.GetPixel(x, y).R;
            return Color.FromArgb(r, r, r);
        }
    }

    public class gRGBFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int g = sourceImage.GetPixel(x, y).G;
            return Color.FromArgb(g, g, g);
        }
    }

    public class bRGBFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int b = sourceImage.GetPixel(x, y).B;
            return Color.FromArgb(b, b, b);
        }
    }

    public class rgRGBFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            return Color.FromArgb(color.R, color.G, 0);
        }
    }

    public class rbRGBFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            return Color.FromArgb(color.R, 0, color.B);
        }
    }

    public class gbRGBFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            return Color.FromArgb(0, color.G, color.B);
        }
    }
}
