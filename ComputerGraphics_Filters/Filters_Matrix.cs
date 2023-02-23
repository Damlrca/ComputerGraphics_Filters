using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Матричные фильтры

    public class MatrixFilter : Filter
    {
        protected float[,] kernel = null;

        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = 0;
            float resultG = 0;
            float resultB = 0;

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

    public class GaussianFilter : MatrixFilter
    {
        public GaussianFilter()
        {
            int radius = 3;
            float sigma = 2;
            int size = radius * 2 + 1;
            kernel = new float[size, size];
            float norm = 0;
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
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

    public class EmbossingFilter : MatrixFilter
    {
        public EmbossingFilter()
        {
            kernel = new float[3, 3] {
                { 0, 1, 0 },          //направление освещения можно менять, но в сумме должен получаться 0
                { 1, 0, -1 },
                { 0, -1, 0 } };
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = 128;
            float resultG = 128;
            float resultB = 128;

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

    // Фильтры выделения границы

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

        protected void calculatePartNewPixelColor(Bitmap sourceImage, int x, int y, float[,] kernel,
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