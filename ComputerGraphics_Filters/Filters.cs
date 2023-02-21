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

        public Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
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

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }

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
    public class MovingFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            if (x + 50 < sourceImage.Width)
            {
                return sourceImage.GetPixel(x + 50, y);
            }
            else
            {
                return Color.FromArgb(0 , 0, 0);
            }
        }
    }
    public class TurnFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int x0 = (int)sourceImage.Width / 2;
            int y0 =(int)sourceImage.Height / 2;
            //x0,y0 - центр поворота
            double alpha = Math.Acos(-1) / (double)6;
            int new_x =(int)((x-x0)*Math.Cos(alpha)) - (int)((y-y0)*Math.Sin(alpha)) + x0;
            int new_y = (int)((x - x0) * Math.Sin(alpha)) + (int)((y - y0)*Math.Cos(alpha)) + y0;
            if (new_x >=0 && new_x < sourceImage.Width && new_y >=0 && new_y < sourceImage.Height)
            {
                return sourceImage.GetPixel(new_x, new_y);
            }
            else
            {
                return Color.FromArgb(0, 0, 0);
            }
        }
    }
    public class Wave1Filter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {

            double pi = Math.Acos(-1);
            int new_x = x + (int)(20 * Math.Sin(2 * pi * x / (double)60));
            int new_y = y;
            new_x = Clamp(new_x, 0, sourceImage.Width - 1);
            new_y = Clamp(new_y, 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(new_x, new_y);
        }
    }
    public class Wave2Filter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {

            double pi = Math.Acos(-1);
            int new_x = x + (int)(20 * Math.Sin(2 * pi * y / (double)30));
            int new_y = y;
            new_x = Clamp(new_x, 0, sourceImage.Width - 1);
            new_y = Clamp(new_y, 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(new_x, new_y);
        }
    }

    public class GlassFilter : Filter
    {
        private Random random = new Random();
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int new_x = x + (int)((random.NextDouble() - 0.5) * 10);
            int new_y = y + (int)((random.NextDouble() - 0.5) * 10);
            new_x = Clamp(new_x, 0, sourceImage.Width - 1);
            new_y = Clamp(new_y, 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(new_x, new_y);
        }
    }

    public class MedianFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            List<int> all_r = new List<int>();
            List<int> all_g = new List<int>();
            List<int> all_b = new List<int>();
            int radiusX = 2;
            int radiusY = 2;
            for(int i = -radiusX; i <= radiusX; i++)
            {
                for(int j = -radiusY;j<= radiusY; j++)
                {
                    int xx = x + i;
                    int yy = y + j;
                    if(xx >=0 && xx < sourceImage.Width && yy >=0 && yy < sourceImage.Height)
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

    public class IncreaseBrightnessFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(Clamp(sourceColor.R + 50, 0, 255), Clamp(sourceColor.G + 50, 0, 255), Clamp(sourceColor.B + 50, 0, 255));
        }
    }

    public class MatrixFilter : Filter
    {
        protected float[,] kernel = null;

        protected float basicR = 0;
        protected float basicG = 0;
        protected float basicB = 0;
        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = basicR;
            float resultG = basicG;
            float resultB = basicB;

            for (int l = -radiusX; l <= radiusX; l++)
            {
                for (int k = -radiusY; k <= radiusY; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighbourColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighbourColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighbourColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighbourColor.B * kernel[k + radiusX, l + radiusY];
                }
            }

            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255)
                );
        }
    }

    public class BlurFilter : MatrixFilter
    {
        public BlurFilter()
        {
            int sizeX = 9;
            int sizeY = 9;
            kernel = new float[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    kernel[i, j] = 1.0f / (sizeX * sizeY);
        }
    }
    

    public class MotionBlurFilter : MatrixFilter
    {
        public MotionBlurFilter()
        {
            int n = 9;
            kernel = new float[n, n];
            for (int i = 0; i < n; i++)
                kernel[i, i] = 1.0f / n;
        }
    }

    public class Sharpness1Filter : MatrixFilter
    {
        public Sharpness1Filter()
        {
            kernel = new float[3, 3] {
                { 0, -1, 0 },
                { -1, 5, -1 },
                { 0, -1, 0 } };
        }
    }

    public class Sharpness2Filter : MatrixFilter
    {
        public Sharpness2Filter()
        {
            kernel = new float[3, 3] {
                { -1, -1, -1 },
                { -1, 9, -1 },
                { -1, -1, -1 } };
        }
    }

    public class GaussianFilter : MatrixFilter
    {
        public GaussianFilter()
        {
            int radius = 3;
            float sigma = 2;
            int size = radius * 2 + 1;
            kernel = new float[size, size];
            float norm = 0;
            for (int i=-radius; i<=radius; i++)
            {
                for(int j=-radius; j<=radius; j++)
                {
                    kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / (sigma * sigma)));
                    norm += kernel[i + radius, j + radius];
                }
            }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= norm;
        }
    }

    public class EmbossingFilter : MatrixFilter
    {
        public EmbossingFilter()
        {
            kernel = new float[3, 3] {
                { 0, 1, 0 },          //направление освещения можно менять, но в сумме должен получаться 0
                { 1, 0, -1 },
                { 0, -1, 0 } };
            basicR = 128;
            basicG = 128;
            basicB = 128;
        }
    }

    public class GradientMatrixFilter : Filter
    {
        protected float[,] kernelX = null;
        protected float[,] kernelY = null;
        protected GradientMatrixFilter() { }
        public GradientMatrixFilter(float[,] kernelX, float[,] kernelY)
        {
            this.kernelX = kernelX;
            this.kernelY = kernelY;
        }
        private void calculatePartNewPixelColor(Bitmap sourceImage, int x, int y, float[,] kernel,
            out float resultR, out float resultG, out float resultB)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            resultR = 0;
            resultG = 0;
            resultB = 0;
            for (int l = -radiusX; l <= radiusX; l++)
            {
                for (int k = -radiusY; k <= radiusY; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighbourColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighbourColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighbourColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighbourColor.B * kernel[k + radiusX, l + radiusY];
                }
            }
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            float resRX, resGX, resBX;
            calculatePartNewPixelColor(sourceImage, x, y, kernelX, out resRX, out resGX, out resBX);
            float resRY, resGY, resBY;
            calculatePartNewPixelColor(sourceImage, x, y, kernelY, out resRY, out resGY, out resBY);

            return Color.FromArgb(
                Clamp((int)Math.Sqrt(resRX * resRX + resRY * resRY), 0, 255),
                Clamp((int)Math.Sqrt(resGX * resGX + resGY * resGY), 0, 255),
                Clamp((int)Math.Sqrt(resBX * resBX + resBY * resBY), 0, 255)
            );
        }
    }

    public class PrewittFilter : GradientMatrixFilter
    {
        public PrewittFilter()
        {
            kernelX = new float[3, 3] {
                { -1, 0, 1 },
                { -1, 0, 1 },
                { -1, 0, 1 }};
            kernelY = new float[3, 3] {
                { -1, -1, -1 },
                { 0, 0, 0 },
                { 1, 1, 1 }};
        }
    }

    public class SobelFilter : GradientMatrixFilter
    {
        public SobelFilter()
        {
            kernelX = new float[3, 3] {
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 1 }};
            kernelY = new float[3, 3] {
                { -1, -2, -1 },
                { 0, 0, 0 },
                { 1, 2, 1 }};
        }
    }

    public class ScharrFilter : GradientMatrixFilter
    {
        public ScharrFilter()
        {
            kernelX = new float[3, 3] {
                { 3, 0, -3 },
                { 10, 0, -10 },
                { 3, 0, -3 }};
            kernelY = new float[3, 3] {
                { 3, 10, 3 },
                { 0, 0, 0 },
                { -3, -10, -3 }};
        }
    }
}