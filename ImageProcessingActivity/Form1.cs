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
            CenterToScreen();

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
                    processed = new Bitmap(loaded.Width, loaded.Height);
                    Color pixel;
                    int gray;

                    for (int x = 0; x < loaded.Width; x++)
                    {
                        for (int y = 0; y < loaded.Height; y++)
                        {
                            pixel = loaded.GetPixel(x, y);
                            gray = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                            processed.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                        }
                    }
                    int[] histdata = new int[processed.Width];
                    for (int x = 0; x < processed.Width; x++)
                    {
                        for (int y = 0; y < processed.Height; y++)
                        {
                            pixel = processed.GetPixel(x, y);
                            histdata[pixel.R]++;
                        }
                    }

                    Bitmap histogramBitmap = new Bitmap(processed.Width, processed.Height);
                    for (int x = 0; x < histogramBitmap.Width; x++)
                    {
                        for (int y = 0; y < histogramBitmap.Height; y++)
                        {
                            histogramBitmap.SetPixel(x, y, Color.White);
                        }
                    }

                    for (int x = 0; x < histogramBitmap.Width; x++)
                    {
                        for (int y = 0; y < Math.Min(histdata[x] / 5, histogramBitmap.Height); y++)
                        {
                            histogramBitmap.SetPixel(x, (histogramBitmap.Height - 1) - y, Color.Black);
                        }
                    }
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsImageLoaded())
            {
                try
                {
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Bitmap Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png";
                        saveFileDialog.Title = "Save Processed Image";
                        saveFileDialog.ShowDialog();
                        if(saveFileDialog.FileName != "")
                        {
                            processed.Save(saveFileDialog.FileName);
                            MessageBox.Show("Image saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("Error exporting image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show("Please process an image first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsImageLoaded())
            {
                loaded = null;
                processed = null;

                pictureBox1.Image = Properties.Resources.no_image;
                pictureBox2.Image = Properties.Resources.no_image;
                pictureBox1.Image.Tag = "default";
                MessageBox.Show("Image closed successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No image to close.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
