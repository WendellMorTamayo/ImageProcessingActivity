using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WebCamLib;

namespace ImageProcessingActivity
{
    public partial class Form1 : Form
    {
        Bitmap loaded;
        Bitmap processed;
        Bitmap imageB, imageA, colorGreen;
        private Device selectedDevice;
        private Device[] devices;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.no_image;
            pictureBox2.Image = Properties.Resources.no_image;
            pictureBox3.Image = Properties.Resources.no_image;
            pictureBox1.Image.Tag = "default";
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            CenterToScreen();
            LoadWebcamDevices();
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

        private void LoadWebcamDevices()
        {
            cbDevices.Items.Clear();
            devices = DeviceManager.GetAllDevices();
            if (devices.Length > 0)
            {
                cbDevices.Items.AddRange(devices);
                cbDevices.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No webcam device found.");
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                imageB = new Bitmap(openFileDialog2.FileName);
                pictureBox1.Image = imageB;
                pictureBox1.Tag = "not_default";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog3.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                imageA = new Bitmap(openFileDialog3.FileName);
                pictureBox2.Image = imageA;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (cbDevices.SelectedIndex >= 0 && cbDevices.SelectedIndex < devices.Length)
            {
                Device d = DeviceManager.GetDevice(cbDevices.SelectedIndex);

                d.Sendmessage();

                IDataObject data = Clipboard.GetDataObject();
                if (data != null && data.GetDataPresent(DataFormats.Bitmap))
                {
                    Image clipboardImage = (Image)(data.GetData("System.Drawing.Bitmap", true));

                    pictureBox1.Image = new Bitmap(clipboardImage);
                    pictureBox1.Tag = "not_default";

                    ProcessImages();
                }
                else
                {
                    MessageBox.Show("Failed to retrieve image from the clipboard.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a webcam device from the list.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ProcessImages()
        {
            if (IsImageLoaded())
            {
                try
                {
                    Bitmap resultImage = new Bitmap(imageB.Width, imageB.Height);
                    Color mygreen = Color.FromArgb(0, 255, 0);
                    int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
                    int threshold = 5;

                    for (int x = 0; x < imageB.Width; x++)
                    {
                        for (int y = 0; y < imageB.Height; y++)
                        {
                            Color pixel = imageB.GetPixel(x, y);
                            Color backpixel = imageA.GetPixel(x, y);
                            int grey = (pixel.R + pixel.G + pixel.B) / 3;
                            int subtractValue = Math.Abs(grey - greygreen);

                            if (subtractValue < threshold)
                            {
                                resultImage.SetPixel(x, y, backpixel);
                            }
                            else
                            {
                                resultImage.SetPixel(x, y, pixel);
                            }
                        }
                    }
                    pictureBox3.Image = resultImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing images: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please import images into pictureBox1 and pictureBox2 first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cbDevices.SelectedIndex >= 0 && cbDevices.SelectedIndex < devices.Length)
            {
                selectedDevice = devices[cbDevices.SelectedIndex];
                selectedDevice.ShowWindow(pictureBox1);
            }
            else
            {
                MessageBox.Show("Please select a webcam device from the list.");
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
