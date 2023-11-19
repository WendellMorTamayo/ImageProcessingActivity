using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingActivity
{
    public partial class Form1 : Form
    {
        Bitmap loaded;
        Bitmap processed;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.no_image;
            pictureBox2.Image = Properties.Resources.no_image;
            pictureBox1.Image.Tag = "default";
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void importImageToolStripMenuItem_Click(object sender, EventArgs e)

        {
            openFileDialog1.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                loaded = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = loaded;
                pictureBox1.Tag = "not_default";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void basicCopyStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsImageLoaded())
            {
                try
                {
                    loaded = new Bitmap(pictureBox1.Image);
                    processed = new Bitmap(loaded.Width, loaded.Height);

                    for (int x = 0; x < loaded.Width; x++)
                    {
                        for (int y = 0; y < loaded.Height; y++)
                        {
                            Color pixel = loaded.GetPixel(x, y);
                            processed.SetPixel(x, y, pixel);
                        }
                    }

                    pictureBox2.Image = processed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please import an image into pictureBox1 first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(IsImageLoaded())
            {
                try
                {
                    loaded = new Bitmap(pictureBox1.Image);
                    processed = new Bitmap(loaded.Width, loaded.Height);
                    Color pixel;
                    byte gray;
                    for(int x = 0; x < loaded.Width; x++)
                    {
                        for(int y = 0; y < loaded.Height; y++)
                        {
                            pixel = loaded.GetPixel(x, y);
                            gray = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                            processed.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                        }
                    }
                    pictureBox2.Image = processed;
                } catch (Exception ex)
                {
                    MessageBox.Show("Error processing image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show("Please import an image into pictureBox1 first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool IsImageLoaded()
        {
            return pictureBox1.Image.Tag?.ToString() != "default";
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(IsImageLoaded())
            {
                try
                {
                    loaded = new Bitmap(pictureBox1.Image);
                    processed = new Bitmap(loaded.Width, loaded.Height);
                    Color pixel;
                    for(int x = 0; x < loaded.Width; x++)
                    {
                        for(int y = 0; y < loaded.Height; y++)
                        {
                            pixel = loaded.GetPixel(x, y);
                            processed.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                        }
                    }
                    pictureBox2.Image = processed;
                } catch (Exception ex)
                {
                    MessageBox.Show("Error processing image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show("Please import an image into pictureBox1 first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsImageLoaded())
            {
                try
                {
                    loaded = new Bitmap(pictureBox1.Image);
                    processed = new Bitmap(loaded.Width, loaded.Height);
                    Color pixel;
                    Byte grayData;

                    // Convert the image to grayscale
                    for (int x = 0; x < loaded.Width; x++)
                    {
                        for (int y = 0; y < loaded.Height; y++)
                        {
                            pixel = loaded.GetPixel(x, y);
                            grayData = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                            processed.SetPixel(x, y, Color.FromArgb(grayData, grayData, grayData));
                        }
                    }

                    // Calculate the histogram data
                    int[] histdata = new int[256];
                    for (int x = 0; x < processed.Width; x++)
                    {
                        for (int y = 0; y < processed.Height; y++)
                        {
                            pixel = processed.GetPixel(x, y);
                            histdata[pixel.R]++;
                        }
                    }

                    // Create a new bitmap for displaying the histogram
                    Bitmap histogramBitmap = new Bitmap(256, processed.Height);

                    // Clear the bitmap
                    for (int x = 0; x < histogramBitmap.Width; x++)
                    {
                        for (int y = 0; y < histogramBitmap.Height; y++)
                        {
                            histogramBitmap.SetPixel(x, y, Color.White);
                        }
                    }

                    // Draw the histogram on the bitmap
                    for (int x = 0; x < 256; x++)
                    {
                        for (int y = 0; y < Math.Min(histdata[x] / 5, histogramBitmap.Height - 1); y++)
                        {
                            histogramBitmap.SetPixel(x, (histogramBitmap.Height - 1) - y, Color.Black);
                        }
                    }

                    // Display the histogram bitmap in pictureBox2
                    pictureBox2.Image = histogramBitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please import an image into pictureBox1 first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(IsImageLoaded())
            {
                try
                {
                    loaded = new Bitmap(pictureBox1.Image);
                    processed = new Bitmap(loaded.Width, loaded.Height);
                    Color pixel;
                    for (int x = 0; x < loaded.Width; x++)
                    {
                        for (int y = 0; y < loaded.Height; y++)
                        {
                            pixel = loaded.GetPixel(x, y);
                            int sepiaR = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                            int sepiaG = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                            int sepiaB = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                            sepiaR = Math.Min(255, Math.Max(0, sepiaR));
                            sepiaG = Math.Min(255, Math.Max(0, sepiaG));
                            sepiaB = Math.Min(255, Math.Max(0, sepiaB));
                            
                            processed.SetPixel(x, y, Color.FromArgb(sepiaR, sepiaG, sepiaB));
                        }
                    }
                    pictureBox2.Image = processed;
                } catch(Exception ex)
                {
                    MessageBox.Show("Error processing image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show("Please import an image into pictureBox1 first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
