using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    public abstract class Filter
    {
        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y);

        public virtual Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            return resultImage;
        }

        protected int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }

    // Точечные фильтры

    public class InvertFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(255 - sourceColor.R, 255 - sourceColor.G, 255 - sourceColor.B);
        }
    }

    public class GrayScaleFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)(0.36 * sourceColor.R + 0.53 * sourceColor.G + 0.11 * sourceColor.B);
            Intensity = Clamp(Intensity, 0, 255);
            return Color.FromArgb(Intensity, Intensity, Intensity);
        }
    }

    public class SepiaFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)(0.36 * sourceColor.R + 0.53 * sourceColor.G + 0.11 * sourceColor.B);
            Intensity = Clamp(Intensity, 0, 255);
            int k = 20;
            return Color.FromArgb(Clamp(Intensity + 2 * k, 0, 255),
                Clamp((int)(Intensity + 0.5 * k), 0, 255),
                Clamp(Intensity - k, 0, 255));
        }
    }

    public class IncreaseBrightnessFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int k = 50;
            return Color.FromArgb(Clamp(sourceColor.R + k, 0, 255),
                Clamp(sourceColor.G + k, 0, 255),
                Clamp(sourceColor.B + k, 0, 255));
        }
    }

    // Геометрический фильтры

    public class MoveFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            if (x + 50 < sourceImage.Width && y + 20 < sourceImage.Height)
            {
                return sourceImage.GetPixel(x + 50, y + 20);
            }
            else
            {
                return Color.FromArgb(0, 0, 0);
            }
        }
    }

    public class RotateFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            // x0, y0 - центр поворота
            int x0 = sourceImage.Width / 2;
            int y0 = sourceImage.Height / 2;
            double alpha = Math.PI / 6;
            int new_x = (int)((x - x0) * Math.Cos(alpha)) - (int)((y - y0) * Math.Sin(alpha)) + x0;
            int new_y = (int)((x - x0) * Math.Sin(alpha)) + (int)((y - y0) * Math.Cos(alpha)) + y0;
            if (new_x >= 0 && new_x < sourceImage.Width && new_y >= 0 && new_y < sourceImage.Height)
            {
                return sourceImage.GetPixel(new_x, new_y);
            }
            else
            {
                return Color.FromArgb(0, 0, 0);
            }
        }
    }

    public class Waves1Filter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int new_x = x + (int)(20 * Math.Sin(2 * Math.PI * x / 60));
            int new_y = y;
            new_x = Clamp(new_x, 0, sourceImage.Width - 1);
            new_y = Clamp(new_y, 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(new_x, new_y);
        }
    }

    public class Waves2Filter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int new_x = x + (int)(20 * Math.Sin(2 * Math.PI * y / 30));
            int new_y = y;
            new_x = Clamp(new_x, 0, sourceImage.Width - 1);
            new_y = Clamp(new_y, 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(new_x, new_y);
        }
    }

    public class GlassFilter : Filter
    {
        private readonly Random random = new Random();

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int new_x = x + (int)((random.NextDouble() - 0.5) * 10);
            int new_y = y + (int)((random.NextDouble() - 0.5) * 10);
            new_x = Clamp(new_x, 0, sourceImage.Width - 1);
            new_y = Clamp(new_y, 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(new_x, new_y);
        }
    }

    // Нелинейные фильтры

    public class MedianFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            List<int> all_r = new List<int>();
            List<int> all_g = new List<int>();
            List<int> all_b = new List<int>();
            int radiusX = 1, radiusY = 1;
            for (int i = -radiusX; i <= radiusX; i++)
            {
                for (int j = -radiusY; j <= radiusY; j++)
                {
                    int xx = x + i;
                    int yy = y + j;
                    if (xx >= 0 && xx < sourceImage.Width && yy >= 0 && yy < sourceImage.Height)
                    {
                        Color color = sourceImage.GetPixel(xx, yy);
                        all_r.Add(color.R);
                        all_g.Add(color.G);
                        all_b.Add(color.B);
                    }
                }
            }
            all_r.Sort();
            all_g.Sort();
            all_b.Sort();
            return Color.FromArgb(all_r[all_r.Count() / 2], all_g[all_g.Count() / 2], all_b[all_b.Count() / 2]);
        }
    }

    public class MaximumFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int max_r = 0, max_g = 0, max_b = 0;
            int radiusX = 1, radiusY = 1;
            for (int i = -radiusX; i <= radiusX; i++)
            {
                for (int j = -radiusY; j <= radiusY; j++)
                {
                    int xx = x + i;
                    int yy = y + j;
                    if (xx >= 0 && xx < sourceImage.Width && yy >= 0 && yy < sourceImage.Height)
                    {
                        Color color = sourceImage.GetPixel(xx, yy);
                        max_r = Math.Max(max_r, color.R);
                        max_g = Math.Max(max_g, color.G);
                        max_b = Math.Max(max_b, color.B);
                    }
                }
            }
            return Color.FromArgb(max_r, max_g, max_b);
        }
    }

    public class MinimumFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int min_r = 255, min_g = 255, min_b = 255;
            int radiusX = 1, radiusY = 1;
            for (int i = -radiusX; i <= radiusX; i++)
            {
                for (int j = -radiusY; j <= radiusY; j++)
                {
                    int xx = x + i;
                    int yy = y + j;
                    if (xx >= 0 && xx < sourceImage.Width && yy >= 0 && yy < sourceImage.Height)
                    {
                        Color color = sourceImage.GetPixel(xx, yy);
                        min_r = Math.Min(min_r, color.R);
                        min_g = Math.Min(min_g, color.G);
                        min_b = Math.Min(min_b, color.B);
                    }
                }
            }
            return Color.FromArgb(min_r, min_g, min_b);
        }
    }

    // Глобальные фильтры

    public class ContrastFilter : Filter
    {
        protected long brightness = 0;

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 50));
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    long pix = 0;
                    pix += sourceImage.GetPixel(i, j).R;
                    pix += sourceImage.GetPixel(i, j).G;
                    pix += sourceImage.GetPixel(i, j).B;
                    pix /= 3;
                    brightness += pix;
                }
            }
            brightness /= sourceImage.Width * sourceImage.Height;

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 50) + 50);
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            return resultImage;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            float c = 1.2f;
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(Clamp((int)(brightness + (sourceColor.R - brightness) * c), 0, 255),
                                  Clamp((int)(brightness + (sourceColor.G - brightness) * c), 0, 255),
                                  Clamp((int)(brightness + (sourceColor.B - brightness) * c), 0, 255));
        }
    }
}