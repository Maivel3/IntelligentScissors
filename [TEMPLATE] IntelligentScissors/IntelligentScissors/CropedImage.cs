using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;


using System.Windows.Forms;
using System.Drawing.Imaging;
namespace IntelligentScissors
{
    public partial class CropedImage : Form
    {
        
        public CropedImage()
        {
            InitializeComponent();
        }

        public CropedImage(RGBPixel[,] cropedImage)
        {
            InitializeComponent();
            //display imageoperation
            ImageOperations.DisplayImage(cropedImage, PB1);
        }
        private void CropedImage_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // 1-Displays a SaveFil to save the Image
            // 2-assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|.jpg|Bitmap Image|.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            //check if the file name is empty or not(string open it for saving.)
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream file =
                (System.IO.FileStream)saveFileDialog1.OpenFile();
                //cases
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        PB1.Image.Save(file,
                            System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        PB1.Image.Save(file,
                            System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        PB1.Image.Save(file,
                            System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
                file.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}