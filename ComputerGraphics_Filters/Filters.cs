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

        public virtual Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker,int MaxPercent = 100, int add = 0)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * MaxPercent) + add);
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
        protected readonly Random random = new Random();

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

    public abstract class GlobalFilter : Filter
    {
        public int GetBrightness(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100)
        {
            long brightness = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * MaxPercent));
                if (worker.CancellationPending)
                    return 0;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    long pix = 0;
                    Color color = sourceImage.GetPixel(i, j);
                    pix += color.R;
                    pix += color.G;
                    pix += color.B;
                    pix /= 3;
                    brightness += pix;
                }
            }
            brightness /= sourceImage.Width * sourceImage.Height;
            return (int)brightness;
        }

        public void GetMax(Bitmap sourceImage, out int R, out int G, out int B, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            R = G = B = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * MaxPercent) + add);
                if (worker.CancellationPending)
                    return;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color color = sourceImage.GetPixel(i, j);
                    R = Math.Max(R, color.R);
                    G = Math.Max(G, color.G);
                    B = Math.Max(B, color.B);
                }
            }
        }

        public void GetMin(Bitmap sourceImage, out int R, out int G, out int B, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            R = G = B = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * MaxPercent) + add);
                if (worker.CancellationPending)
                    return;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color color = sourceImage.GetPixel(i, j);
                    R = Math.Min(R, color.R);
                    G = Math.Min(G, color.G);
                    B = Math.Min(B, color.B);
                }
            }
        }

        public void GetAvarageColors(Bitmap sourceImage, out int R, out int G, out int B, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            //Даёт среднюю яркость по каждому каналу
            R = G = B = 0;
            long tR = 0; long tG = 0; long tB = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * MaxPercent) + add);
                if (worker.CancellationPending)
                    return;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color color = sourceImage.GetPixel(i, j);
                    tR += color.R;
                    tG += color.G;
                    tB += color.B;
                }
            }
            R = (int)tR / (sourceImage.Width * sourceImage.Height);
            G = (int)tG / (sourceImage.Width * sourceImage.Height);
            B = (int)tB / (sourceImage.Width * sourceImage.Height);
        }
    }

    public class ContrastFilter : GlobalFilter
    {
        protected int brightness = 0;

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            brightness = GetBrightness(sourceImage, worker, 50);

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

    public class AutolevelsFilter : GlobalFilter
    {
        int minR, maxR;
        int minG, maxG;
        int minB, maxB;

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            GetMax(sourceImage, out maxR, out maxG, out maxB, worker, 33, 0);
            GetMin(sourceImage, out minR, out minG, out minB, worker, 33, 33);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * (100 - 33 - 33)) + (33 + 33));
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
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(minR == maxR ? minR : (int)((sourceColor.R - minR) * 255.0 / (maxR - minR)),
                                  minG == maxG ? minG : (int)((sourceColor.G - minG) * 255.0 / (maxG - minG)),
                                  minB == maxB ? minB : (int)((sourceColor.B - minB) * 255.0 / (maxB - minB)));
        }
    }

    public class GrayWorldFilter : GlobalFilter
    {
        int Avg;
        int AvgR;
        int AvgG;
        int AvgB;

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            GetAvarageColors(sourceImage, out AvgR, out AvgG, out AvgB, worker, 50, 0);
            Avg = (int)(AvgR + AvgG + AvgB) / 3;
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
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(Clamp((int)((float)(sourceColor.R * Avg / AvgR)),0,255),
                                  Clamp((int)((float)(sourceColor.G * Avg / AvgG)),0,255),
                                  Clamp((int)((float)(sourceColor.B * Avg / AvgB)),0,255));
        }
    }

    public class PerfectReflectorFilter : GlobalFilter
    {
        int maxR;
        int maxG;
        int maxB;

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker,int MaxPercent = 100, int add = 0)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            GetMax(sourceImage, out maxR, out maxG, out maxB, worker, 50, 0);
           
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
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(Clamp((int)((float)(sourceColor.R * 255 / maxR)), 0, 255),
                                  Clamp((int)((float)(sourceColor.G * 255 / maxG)), 0, 255),
                                  Clamp((int)((float)(sourceColor.B * 255 / maxB)), 0, 255));
        }
    }

    // Шумы

    public class SaltAndPepperFilter : Filter
    {
        protected readonly Random random = new Random();
        protected double p_white = 0.05f;
        protected double p_black = 0.05f;

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            double p = random.NextDouble();
            if (p < p_white)
                return Color.White;
            else if (p + p_black > 1)
                return Color.Black;
            else
                return sourceImage.GetPixel(x, y);
        }
    }

    // Дизеринг

    public class QuantizationFilter : Filter
    {
        protected int k = 2;

        protected int getQuantized(int x)
        {
            int j = Math.Max(1, (x * k + 254) / 255); // j == 1...k
            return (j - 1) * 255 / (k - 1); // j - 1 == 0 .. k - 1
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(getQuantized(sourceColor.R),
                                  getQuantized(sourceColor.G),
                                  getQuantized(sourceColor.B));
        }
    }

    public class FloydSteinbergDitheringFilter : QuantizationFilter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color newColor = base.calculateNewPixelColor(sourceImage, x, y);
            int quant_errorR = sourceColor.R - newColor.R;
            int quant_errorG = sourceColor.G - newColor.G;
            int quant_errorB = sourceColor.B - newColor.B;
            if (x + 1 < sourceImage.Width)
            {
                Color color = sourceImage.GetPixel(x + 1, y);
                sourceImage.SetPixel(x + 1, y, Color.FromArgb(
                    Clamp(color.R + quant_errorR * 7 / 16, 0, 255),
                    Clamp(color.G + quant_errorG * 7 / 16, 0, 255),
                    Clamp(color.B + quant_errorB * 7 / 16, 0, 255)));
            }
            if (x - 1 >= 0 && y + 1 < sourceImage.Height)
            {
                Color color = sourceImage.GetPixel(x - 1, y + 1);
                sourceImage.SetPixel(x - 1, y + 1, Color.FromArgb(
                    Clamp(color.R + quant_errorR * 3 / 16, 0, 255),
                    Clamp(color.G + quant_errorG * 3 / 16, 0, 255),
                    Clamp(color.B + quant_errorB * 3 / 16, 0, 255)));
            }
            if (y + 1 < sourceImage.Height)
            {
                Color color = sourceImage.GetPixel(x, y + 1);
                sourceImage.SetPixel(x, y + 1, Color.FromArgb(
                    Clamp(color.R + quant_errorR * 5 / 16, 0, 255),
                    Clamp(color.G + quant_errorG * 5 / 16, 0, 255),
                    Clamp(color.B + quant_errorB * 5 / 16, 0, 255)));
            }
            if (x + 1 < sourceImage.Width && y + 1 < sourceImage.Height)
            {
                Color color = sourceImage.GetPixel(x + 1, y + 1);
                sourceImage.SetPixel(x + 1, y + 1, Color.FromArgb(
                    Clamp(color.R + quant_errorR * 1 / 16, 0, 255),
                    Clamp(color.G + quant_errorG * 1 / 16, 0, 255),
                    Clamp(color.B + quant_errorB * 1 / 16, 0, 255)));
            }
            return newColor;
        }
    }

    //Морфологические
    public class MorphologicalFilters : Filter
    {
        protected float[,] mask = null;
        protected int n;
        protected int m;
        public MorphologicalFilters()
        {
            n = 2;m = 2;
            mask = new float[5, 5]{
            {1,1,1,1,1},
            {1,1,1,1,1},
            {1,1,1,1,1},
            {1,1,1,1,1},
            {1,1,1,1,1}};
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return Color.FromArgb(0, 0, 0);
        }
        
    }

    public class MorphologicalDilationFilter : MorphologicalFilters
    {
       
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int maxR = 0;
            int maxG = 0;
            int maxB = 0;
            for(int i = -n; i <= n; i++)
            {
                for(int j = -m; j <= m; j++)
                {
                    int xx = x + i;
                    int yy = y + j;
                    if(mask[i+n,j+m] == 1 && xx >=0 && yy >=0 && xx < sourceImage.Width && yy < sourceImage.Height)
                    {
                        Color color = sourceImage.GetPixel(xx, yy);
                        maxR = Math.Max(maxR, color.R);
                        maxG = Math.Max(maxG, color.G);
                        maxB = Math.Max(maxB, color.B);
                    }
                }
            }
            return Color.FromArgb(maxR, maxG, maxB);
        }
    }

    public class MorphologicalErosionFilter : MorphologicalFilters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int minR = 255;
            int minG = 255;
            int minB = 255;
            for (int i = -n; i <= n; i++)
            {
                for (int j = -m; j <= m; j++)
                {
                    int xx = x + i;
                    int yy = y + j;
                    if (mask[i + n, j + m] == 1 && xx >= 0 && yy >= 0 && xx < sourceImage.Width && yy < sourceImage.Height)
                    {
                        Color color = sourceImage.GetPixel(xx, yy);
                        minR = Math.Min(minR, color.R);
                        minG = Math.Min(minG, color.G);
                        minB = Math.Min(minB, color.B);
                    }
                }
            }
            return Color.FromArgb(minR, minG, minB);
        }
    }

    public class MorphologicalOpenFilter : MorphologicalFilters
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            Filter tmp = new MorphologicalErosionFilter();
            Filter tmp2 = new MorphologicalDilationFilter();
            return tmp2.processImage(tmp.processImage(sourceImage,worker,50,0),worker,50,50);
        }
    }

    public class MorphologicalCloseFilter : MorphologicalFilters
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            Filter tmp = new MorphologicalDilationFilter();
            Filter tmp2 = new MorphologicalErosionFilter();
            return tmp2.processImage(tmp.processImage(sourceImage, worker,50,0), worker,50,50);
        }
    }

    public class MorphologicalTopHatFilter:MorphologicalFilters
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filter filter = new MorphologicalCloseFilter();
            Bitmap closingImage = filter.processImage(sourceImage,worker,50,0);
            
            for(int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 50) + 50);
                if (worker.CancellationPending)
                    return null;
                for (int j = 0;j< sourceImage.Height; j++)
                {
                    Color color = sourceImage.GetPixel(i,j);
                    Color color_closing = closingImage.GetPixel(i,j);
                    resultImage.SetPixel(i, j, Color.FromArgb(
                        Clamp(-color.R + color_closing.R, 0, 255),
                        Clamp(-color.G + color_closing.G, 0, 255),
                        Clamp(-color.B + color_closing.B, 0, 255)));
                }
            }
            return resultImage;
        }
    }

    public class MorphologicalBlackHatFilter : MorphologicalFilters
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filter filter = new MorphologicalErosionFilter();
            Bitmap openImage = filter.processImage(sourceImage, worker, 50, 0);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 50) + 50);
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color color = sourceImage.GetPixel(i, j);
                    Color color_open = openImage.GetPixel(i, j);
                    resultImage.SetPixel(i, j, Color.FromArgb(
                        Clamp(color.R - color_open.R, 0, 255),
                        Clamp(color.G - color_open.G, 0, 255),
                        Clamp(color.B - color_open.B, 0, 255)));
                }
            }
            return resultImage;
        }
    }

    public class MorphologicalGradFilter : MorphologicalFilters
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker, int MaxPercent = 100, int add = 0)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            Filter Erosion = new MorphologicalErosionFilter();
            Bitmap ErosionImage = Erosion.processImage(sourceImage, worker, 33, 0);

            Filter Dilation = new MorphologicalDilationFilter();
            Bitmap DilationImage = Dilation.processImage(sourceImage, worker, 33, 33);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 33) + 66);
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color colorErosion = ErosionImage.GetPixel(i, j);
                    Color colorDilation = DilationImage.GetPixel(i, j);
                    resultImage.SetPixel(i, j, Color.FromArgb(
                        Clamp(-colorErosion.R + colorDilation.R, 0, 255),
                        Clamp(-colorErosion.G + colorDilation.G, 0, 255),
                        Clamp(-colorErosion.B + colorDilation.B, 0, 255)));
                }
            }
            return resultImage;
        }
    }

}
