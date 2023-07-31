using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IntelligentScissors
{
    public partial class MainForm : Form
    {
        RGBPixel[,] ImageMatrix;
        RGBPixel[,] PartOfImg;
        int CS = -1, MS = -1;
        List<Point> AutoAnchorPath;
        List<Point> MyImgSelected;
        float fpoint = 0.0f;
        Boundary boundry;
        bool ActiveAnchor = false;
        int Frequancy = 57;
        List<int> Globel;
        int OldPos;
        float[] DashPattern = { (float)1, (float)0.000000000001 }; 
        float wf = .02f;
        Point size = new Point(5, 5);  
        Point[] ActualPath;
        bool leavemouse = false;
        Pen m_pen = new Pen(Brushes.Blue, 1); 
        Pen c_pen = new Pen(Brushes.Aqua, 1);

        void init()
        {
            AutoAnchorPath = new List<Point>();
            MyImgSelected = new List<Point>();
        }
        public MainForm()
        {
            InitializeComponent();
            init();
        }

        void reset()
        {

            ActualPath = null;
            AutoAnchorPath.Clear();
            MyImgSelected.Clear();
            CS = -1;
            OldPos = -1;
            MS = -1;
            leavemouse = false;
            ActiveAnchor = false;
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                reset();
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }


        public void update(MouseEventArgs e)
        {
            var g = pictureBox1.CreateGraphics();
            if (fpoint > wf * 2)
            {
                if (ImageMatrix != null)
                {
                    var mouseNode = e.X + (e.Y * ImageOperations.GetWidth(ImageMatrix));

                    if (CS != -1 && OldPos != mouseNode)
                    {
                        OldPos = mouseNode;
                        if (Functions.checkBoundary(mouseNode, boundry, ImageOperations.GetWidth(ImageMatrix)))
                        {
                            int Segment_mouse = Functions.Cr_P(mouseNode, boundry,
                                ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(PartOfImg));
                            List<Point> segmentpath = new List<Point>();
                            segmentpath = ShortestPathalgorithm.Backtracking(Globel, Segment_mouse, ImageOperations.GetWidth(PartOfImg));
                            List<Point> Curpath = new List<Point>();
                            //Curpath = Functions.Cr_P(segmentpath, boundry);
                            for (int i = 0; i < segmentpath.Count; i++)
                            {
                                Point x = new Point();
                                x.X = (int)segmentpath[i].X + (int)boundry.Xminimum;
                                x.Y = (int)segmentpath[i].Y + (int)boundry.Yminimum;
                                Curpath.Add(x);

                            }
                            ActualPath = Curpath.ToArray();
                            if (ActiveAnchor)
                            {
                                double freq = (double)Frequancy / 1000;
                                AnchorPoint.PathUpdate(Curpath, freq);
                                List<Point> cooledpath = AnchorPoint.PathOfAnchor();
                                if (cooledpath.Count > 0)
                                {
                                    Point anchor = cooledpath[cooledpath.Count - 1];
                                    AutoAnchorPath.Add(anchor);
                                    CS = anchor.X + (anchor.Y * ImageOperations.GetWidth(ImageMatrix));


                                    if (MyImgSelected == null || cooledpath == null)
                                    {
                                        throw new ArgumentNullException();
                                    }
                                    List<Point> tmp = MyImgSelected;
                                    for (int i = 0; i < cooledpath.Count; i++)
                                    {
                                        tmp.Add(cooledpath[i]);
                                    }

                                    Functions.Insert_Des_To_Source<Point>(MyImgSelected, cooledpath);
                                    boundry = new Boundary();
                                    boundry = ShortestPathalgorithm.Square_Boundary(CS,
                                        ImageOperations.GetWidth(ImageMatrix) - 1, ImageOperations.GetHeight(ImageMatrix) - 1);
                                    PartOfImg = Functions.CropedImg(ImageMatrix, boundry);
                                    int newsrc = Functions.Cr_P(CS, boundry, ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(PartOfImg));
                                    Globel = ShortestPathalgorithm.Dijkstra(newsrc, PartOfImg);
                                    AnchorPoint.ClearPath();
                                }
                            }
                        }
                        else
                        {
                            ActualPath = ShortestPathalgorithm.GenerateShortestPath(CS, mouseNode, ImageMatrix).ToArray();
                        }
                    }
                }
                fpoint = 0.0f;
            }
            if (fpoint > wf)
            {
                pictureBox1.Refresh();
                g.Dispose();
            }
            fpoint += .019f;
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            update(e);
            X_TXTBOX.Text = e.X.ToString(); //mouse text box 
            Y_TXTBOX.Text = e.Y.ToString();
            if (pictureBox1.Image != null)
                NODETXTBOX.Text = (e.X + (e.Y * ImageOperations.GetWidth(ImageMatrix))).ToString();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (ImageMatrix != null)
            {
                var g = e.Graphics;
                for (int i = 0; i < AutoAnchorPath.Count; i++)
                {
                    g.FillEllipse(Brushes.Red, new Rectangle( new Point(AutoAnchorPath[i].X - size.X / 2, AutoAnchorPath[i].Y - size.Y / 2),
                        new Size(size)));
                }
                if (ActualPath != null)
                    if (ActualPath.Length > 10)
                        Drawing.DrawingPath(g, c_pen, ActualPath, DashPattern);
                if (MyImgSelected != null && MyImgSelected.Count > 5)
                    Drawing.DrawingPath(e.Graphics, m_pen, MyImgSelected.ToArray(), DashPattern);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                double w = ImageOperations.GetWidth(ImageMatrix);

                var node = (int)e.X + (int)(e.Y * w);
                if (CS != node)
                {
                    if (CS == -1) // in the first click save frist clicked anchor
                        MS = node;
                    else
                        Functions.Insert_Des_To_Source<Point>(MyImgSelected, ActualPath);
                    CS = node;
                    AutoAnchorPath.Add(e.Location);
                    boundry = new Boundary();
                    boundry = ShortestPathalgorithm.Square_Boundary(CS,
                        ImageOperations.GetWidth(ImageMatrix) - 1, ImageOperations.GetHeight(ImageMatrix) - 1);
                    //make a square segment
                    PartOfImg = Functions.CropedImg(ImageMatrix, boundry);
                    // currsrc in segment
                    int newsrc = Functions.Cr_P(CS, boundry, ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(PartOfImg));
                    Globel = ShortestPathalgorithm.Dijkstra(newsrc, PartOfImg);
                    AnchorPoint.ClearPath();
                }
            }
        }
        #region MOUSE CLICKED
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            leavemouse = true;
        }
        #endregion
        #region MOUSE RELEASE
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            leavemouse = false;
        }
        #endregion
        #region AUTO ANCHOR ACTIVATION
        private void AutoAnchor_Click(object sender, EventArgs e)
        {
            if (!ActiveAnchor)
            {
                ActiveAnchor = true;
            }
        }
        #endregion
        #region CHANGE FREQUENCY
        private void frequancyNUD_ValueChanged(object sender, EventArgs e)
        {
            Frequancy = (int)frequancyNUD.Value;
        }
        #endregion

        private void crop_Click(object sender, EventArgs e)
        {
            if (CS != MS)
            {
                // check if first node in shortest path range of last node
                //if yes get it fast else try to get it by dikstra
                if (Functions.checkBoundary(MS, boundry, ImageOperations.GetWidth(ImageMatrix)))
                {
                    int Segment_mouse = Functions.Cr_P(MS, boundry, ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(PartOfImg));
                    List<Point> segmentpath = new List<Point>();
                    segmentpath = ShortestPathalgorithm.Backtracking(Globel, Segment_mouse, ImageOperations.GetWidth(PartOfImg));
                    ActualPath = Functions.Cr_P(segmentpath, boundry).ToArray();
                }
                else
                    ActualPath = ShortestPathalgorithm.
                        GenerateShortestPath(CS, MS, ImageMatrix).ToArray();
                Functions.Insert_Des_To_Source<Point>(MyImgSelected, ActualPath);
                
                CropedImage CI = new CropedImage(Flood.fill(MyImgSelected, ImageMatrix));
                CI.Show();
                reset();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }



       
        private void clear_Click(object sender, EventArgs e)
        {
            reset();
        }
       

    }
}




