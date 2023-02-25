using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Filters;

namespace ComputerGraphics_Filters
{
    public partial class Form1 : Form
    {
        Bitmap previous_image = null;
        Bitmap image = null;
        Filter lastFilter = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Open_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                previous_image = image;
                image = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
        }

        private void Save_as_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                image.Save(saveFileDialog1.FileName);
            }
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Bitmap resultImage = ((Filter)e.Argument).processImage(image, backgroundWorker1);
            Bitmap resultImage = ((Filter)e.Argument).processImage(new Bitmap(image), backgroundWorker1);

            if (!backgroundWorker1.CancellationPending)
            {
                previous_image = image;
                lastFilter = (Filter)e.Argument;
                image = resultImage;
            }
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void undo_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = previous_image;
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        private void repeat_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(lastFilter);
        }

        // Точечные фильтры

        private void Inverse_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new InvertFilter());
        }

        private void GrayScale_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new GrayScaleFilter());
        }

        private void Sepia_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new SepiaFilter());
        }

        private void IncreaseBrightness_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new IncreaseBrightnessFilter());
        }

        // Геометрический фильтры

        private void Move_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MoveFilter());
        }

        private void Rotate_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new RotateFilter());
        }

        private void Waves1_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new Waves1Filter());
        }

        private void Waves2_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new Waves2Filter());
        }

        private void Glass_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new GlassFilter());
        }

        // Нелинейные фильтры

        private void Median_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MedianFilter());
        }

        private void Maximum_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MaximumFilter());
        }

        private void Minimum_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MinimumFilter());
        }

        // Матричные фильтры

        private void Blur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new BlurFilter());
        }

        private void MotionBlur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MotionBlurFilter());
        }

        private void GaussianBlur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new GaussianFilter());
        }

        private void Sharpness1_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new Sharpness1Filter());
        }

        private void Sharpness2_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new Sharpness2Filter());
        }

        private void Embossing_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new EmbossingFilter());
        }

        // Фильтры выделения границы

        private void Prewitt_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new PrewittFilter());
        }

        private void Sobel_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new SobelFilter());
        }

        private void Scharr_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new ScharrFilter());
        }

        // Глобальные фильтры
        
        private void IncreaseContrast_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new ContrastFilter());
        }

        private void autolevels_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new AutolevelsFilter());
        }

        // Шумы

        private void saltAndPepper_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new SaltAndPepperFilter());
        }

        // Каналы

        private void rRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new rRGBFilter());
        }

        private void gRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new gRGBFilter());
        }

        private void bRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new bRGBFilter());
        }

        private void rgRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new rgRGBFilter());
        }

        private void rbRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new rbRGBFilter());
        }

        private void gbRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new gbRGBFilter());
        }

        // Дизеринг

        private void Quantization_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new QuantizationFilter());
        }

        private void FloydSteinbergDithering_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new FloydSteinbergDitheringFilter());
        }

        private void GrayWorld_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new GrayWorldFilter());
        }

        private void PerfectReflector_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new PerfectReflectorFilter());
        }

        private void Dilation_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MorphologicalDilationFilter());
        }

        private void Erosion_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MorphologicalErosionFilter());
        }

        private void OpenFilter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MorphologicalOpenFilter());
        }

        private void CloseFilter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MorphologicalCloseFilter());
        }

        private void TopHat_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MorphologicalTopHatFilter());
        }

        private void BlackHat_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MorphologicalBlackHatFilter());
        }

        private void Grad_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MorphologicalGradFilter());
        }

        private void ReferenceColorCorrectiom_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new ReferenceColorFilter());
        }
    }
}
