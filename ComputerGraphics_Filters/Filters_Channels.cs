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

    // YIQ

    public class yYIQFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color c = sourceImage.GetPixel(x, y);
            int Intensity = (int)(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
            return Color.FromArgb(Intensity, Intensity, Intensity);
        }
    }

    public class iYIQFilter : Filter
    {
        double min_Intensity = -0.274 * 255 - 0.322 * 255;
        double max_Intensity = 0.596 * 255;
        Color l = Color.FromArgb(0, 255, 255);
        Color r = Color.FromArgb(255, 0, 0);

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color c = sourceImage.GetPixel(x, y);
            double Intensity = 0.596 * c.R - 0.274 * c.G - 0.322 * c.B;
            int R = (int)((Intensity - min_Intensity) / (max_Intensity - min_Intensity) * (r.R - l.R) + l.R);
            int G = (int)((Intensity - min_Intensity) / (max_Intensity - min_Intensity) * (r.G - l.G) + l.G);
            int B = (int)((Intensity - min_Intensity) / (max_Intensity - min_Intensity) * (r.B - l.B) + l.B);
            return Color.FromArgb(R, G, B);
        }
    }

    public class qYIQFilter : Filter
    {
        double min_Intensity = -0.522 * 255;
        double max_Intensity = 0.211 * 255 + 0.311 * 255;
        Color l = Color.FromArgb(0, 255, 0);
        Color r = Color.FromArgb(255, 0, 255);

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color c = sourceImage.GetPixel(x, y);
            double Intensity = 0.211 * c.R - 0.522 * c.G + 0.311 * c.B;
            int R = (int)((Intensity - min_Intensity) / (max_Intensity - min_Intensity) * (r.R - l.R) + l.R);
            int G = (int)((Intensity - min_Intensity) / (max_Intensity - min_Intensity) * (r.G - l.G) + l.G);
            int B = (int)((Intensity - min_Intensity) / (max_Intensity - min_Intensity) * (r.B - l.B) + l.B);
            R = Clamp(R, 0, 255);
            G = Clamp(G, 0, 255);
            B = Clamp(B, 0, 255);
            return Color.FromArgb(R, G, B);
        }
    }

    // CMY

    public class cCMYFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            int c = 255 - color.R;
            return Color.FromArgb(255 - c, 255, 255);
        }
    }

    public class mCMYFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            int m = 255 - color.G;
            return Color.FromArgb(255, 255 - m, 255);
        }
    }

    public class yCMYFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            int _y = 255 - color.B;
            return Color.FromArgb(255, 255, 255 - _y);
        }
    }

    // CMYK

    public class cCMYKFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            int k = 255 - Math.Max(Math.Max(color.R, color.G), color.B);
            int c = (255 - color.R) - k;
            return Color.FromArgb(255 - c, 255, 255);
        }
    }

    public class mCMYKFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            int k = 255 - Math.Max(Math.Max(color.R, color.G), color.B);
            int m = (255 - color.G) - k;
            return Color.FromArgb(255, 255 - m, 255);
        }
    }

    public class yCMYKFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            int k = 255 - Math.Max(Math.Max(color.R, color.G), color.B);
            int _y = (255 - color.B) - k;
            return Color.FromArgb(255, 255, 255 - _y);
        }
    }

    public class kCMYKFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            int k = 255 - Math.Max(Math.Max(color.R, color.G), color.B);
            return Color.FromArgb(255 - k, 255 - k, 255 - k);
        }
    }
}
