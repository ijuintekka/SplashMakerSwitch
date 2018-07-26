using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace SplashMakerSwitch
{
    public partial class SplashMakerSwitch : Form
    {
        Bitmap image = new Bitmap(1280, 720, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

        public SplashMakerSwitch()
        {
            InitializeComponent();
            var graph = Graphics.FromImage(image);
            graph.Clear(Color.Black);
            PictureBox1.Image = image;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog svf = new SaveFileDialog();
            svf.Filter = "Saves As (*.bin)|*.bin";
            if (svf.ShowDialog() == DialogResult.OK)
            {
                image.RotateFlip(RotateFlipType.Rotate180FlipY);
                List<byte> pixels = new List<byte>();
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        Color pixel = image.GetPixel(i, j);
                        pixels.Add(pixel.R);
                        pixels.Add(pixel.G);
                        pixels.Add(pixel.B);
                        pixels.Add(0);
                    }
                    for (int j = 0; j < 192; j++)
                    {
                        pixels.Add(0);
                    }
                }
                File.WriteAllBytes(svf.FileName, pixels.ToArray());
                image.RotateFlip(RotateFlipType.Rotate180FlipY);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image (*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif;*.tiff)|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                Image import = new Bitmap(opf.FileName);
                var graph = Graphics.FromImage(image);
                graph.Clear(Color.Black);
                graph.DrawImage(import, (image.Width - import.Width) / 2, (image.Height - import.Height) / 2, import.Width, import.Height);
                PictureBox1.Image = image;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose BIN (*.bin)|*.bin";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                List<byte> pixels = new List<byte>(File.ReadAllBytes(opf.FileName));
                var graph = Graphics.FromImage(image);
                graph.Clear(Color.Black);
                int k = 0;
                for (int i = 0; (i < image.Width) && (k < (pixels.Count)); i++)
                {
                    for (int j = 0; (j < image.Height) && (k < (pixels.Count)); j++)
                    {
                        Color pixel = image.GetPixel(i, j);
                        image.SetPixel(i, j, Color.FromArgb(255, pixels[k], pixels[k + 1], pixels[k + 2]));
                        k = k + 4;
                    }
                    k = k + 192;
                }
                image.RotateFlip(RotateFlipType.Rotate180FlipY);
                PictureBox1.Image = image;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog svf = new SaveFileDialog();
            svf.Filter = "Saves As (*.png)|*.png";
            if (svf.ShowDialog() == DialogResult.OK)
            {
                image.Save(svf.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}