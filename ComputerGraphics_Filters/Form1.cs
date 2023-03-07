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

        // Файл

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

        // Отмена

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        // Правка

        private void Undo_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = previous_image;
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        private void Repeat_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(lastFilter);
        }

        // BackgroundWorker1

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

        // StartFilter

        private void StartFilter(Filter filter)
        {
            if (backgroundWorker1.IsBusy == false)
                backgroundWorker1.RunWorkerAsync(filter);
        }

        // Точечные фильтры

        private void Inverse_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new InvertFilter());
        }

        private void GrayScale_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new GrayScaleFilter());
        }

        private void Sepia_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new SepiaFilter());
        }

        private void IncreaseBrightness_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new IncreaseBrightnessFilter());
        }

        // Геометрический фильтры

        private void Move_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MoveFilter());
        }

        private void Rotate_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new RotateFilter());
        }

        private void Waves1_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new Waves1Filter());
        }

        private void Waves2_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new Waves2Filter());
        }

        private void Glass_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new GlassFilter());
        }

        // Нелинейные фильтры

        private void Median_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MedianFilter());
        }

        private void Maximum_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MaximumFilter());
        }

        private void Minimum_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MinimumFilter());
        }

        // Матричные фильтры

        private void Blur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new BlurFilter());
        }

        private void MotionBlur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MotionBlurFilter());
        }

        private void GaussianBlur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new GaussianFilter());
        }

        private void Sharpness1_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new Sharpness1Filter());
        }

        private void Sharpness2_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new Sharpness2Filter());
        }

        private void Embossing_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new EmbossingFilter());
        }

        // Фильтры выделения границы

        private void Prewitt_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new PrewittFilter());
        }

        private void Sobel_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new SobelFilter());
        }

        private void Scharr_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new ScharrFilter());
        }

        // Глобальные фильтры
        
        private void IncreaseContrast_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new ContrastFilter());
        }

        private void Autolevels_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new AutolevelsFilter());
        }

        // Шумы

        private void SaltAndPepper_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new SaltAndPepperFilter());
        }

        // Каналы

        // RGB

        private void rRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new rRGBFilter());
        }

        private void gRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new gRGBFilter());
        }

        private void bRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new bRGBFilter());
        }

        private void rgRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new rgRGBFilter());
        }

        private void rbRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new rbRGBFilter());
        }

        private void gbRGB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new gbRGBFilter());
        }

        // YIQ

        private void yYIQ_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new yYIQFilter());
        }

        private void iYIQ_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new iYIQFilter());
        }

        private void qYIQ_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new qYIQFilter());
        }

        // Квантование и дизеринг

        private void Quantization_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new QuantizationFilter());
        }

        private void FloydSteinbergDithering_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StartFilter(new FloydSteinbergDitheringFilter());
        }

        // Морфологические фильтры

        private void GrayWorld_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new GrayWorldFilter());
        }

        private void PerfectReflector_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new PerfectReflectorFilter());
        }

        private void Dilation_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MorphologicalDilationFilter());
        }

        private void Erosion_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MorphologicalErosionFilter());
        }

        private void OpeningFilter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MorphologicalOpeningFilter());
        }

        private void ClosingFilter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MorphologicalClosingFilter());
        }

        private void TopHat_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MorphologicalTopHatFilter());
        }

        private void BlackHat_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MorphologicalBlackHatFilter());
        }

        private void Grad_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new MorphologicalGradFilter());
        }

        private void ReferenceColorCorrection_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartFilter(new ReferenceColorFilter());
        }
    }
}
