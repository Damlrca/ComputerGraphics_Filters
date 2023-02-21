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
        Bitmap image;

        public Form1()
        {
            InitializeComponent();
        }

        private void Open_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(openFileDialog1.FileName);
            }

            pictureBox1.Image = image;
            pictureBox1.Refresh();
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

        private void Inverse_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new InvertFilter());
        }

        private void Blur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new BlurFilter());
        }

        private void MotionBlur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MotionBlurFilter());
        }

        private void Sharpness1_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new Sharpness1Filter());
        }

        private void Sharpness2_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new Sharpness2Filter());
        }

        private void GrayScale_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new GrayScaleFilter());
        }

        private void Sepia_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new SepiaFilter());
        }

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

        private void Moving_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MovingFilter());
        }

        private void Turn_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new TurnFilter());
        }

        private void Wave1_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new Wave1Filter());
        }

        private void Wave2_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new Wave2Filter());
        }

        private void Glass_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new GlassFilter());
        }

        private void IncreaseBrightness_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new IncreaseBrightnessFilter());
        }

        private void GaussianBlur_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new GaussianFilter());
        }

        private void Embossing_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new EmbossingFilter());
        }

        private void Median_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(new MedianFilter());
        }
    }
}
