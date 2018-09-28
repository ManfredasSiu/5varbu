using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary
{
    public partial class Form2 : Form    //Register Langas, padarysiu irgi su face recognition
    {
        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        HaarCascade faceDetect;
        Image<Bgr, Byte> frame;
        Capture cam;
        Image<Gray, byte> result;
        Image<Gray, byte> TrainImg = null;
        Image<Gray, byte> GrayFace = null;
        PictureBox redDot = new PictureBox();

        string name;

        private int Hei = 640, Len = 480;
        private LogicController LogicC;
        private Form1 main;

        public Form2(LogicController LogicC, Form1 main)
        {
            InitializeComponent();
            this.main = main;
            this.LogicC = LogicC;
            faceDetect = new HaarCascade("haarcascade_frontalface_default.xml");
            this.FormClosing += OnCloseRequest;
            cam = new Capture();
            cam.QueryFrame();
            Application.Idle += new EventHandler(FrameProcedure);
        }


        private void OnCloseRequest(object sender, EventArgs e)
        {
            main.Show();
            cam.Dispose();
            Application.Idle -= FrameProcedure;
        }

        private void FrameProcedure(object sender, EventArgs e)
        {
            if (cam.Equals(null))
                return;
            frame = cam.QueryFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            imageBox1.Image = frame;
            GrayFace = frame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetectedNow = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(640/4, 480/4));
            foreach (MCvAvgComp f in facesDetectedNow[0])
            {
                result = frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                frame.Draw(f.rect, new Bgr(Color.Green), 3);
                if (StaticData.training.ToArray().Length != 0)
                {
                    MCvTermCriteria termCriteria = new MCvTermCriteria(StaticData.numLablels, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(StaticData.training.ToArray(), StaticData.labels.ToArray(), 2000, ref termCriteria);
                    name = recognizer.Recognize(result);
                    frame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                }
            }
            imageBox1.Image = frame.Resize(Hei, Len, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
        }

        private void RegisterButton_CLicked(object sender, EventArgs e)
        {
            if(CheckTheTB() == 1) return;

            GrayFace = cam.QueryGrayFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            MCvAvgComp[][] DetectedFaces = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            if (DetectedFaces[0].Length == 0)
            {
                MessageBox.Show("Veidas nerastas, bandykite dar karta");
                return;
            }
            else if (DetectedFaces[0].Length > 1)
            {
                MessageBox.Show("Kadre perdaug veidu");
                return;
            }

            PrepareForRegister();

            InstantiateRedDot();

            Thread RegProcess = new Thread(new ThreadStart(RegisterProcess));
            RegProcess.Start();

            User thisUser = new User(textBox1.Text, textBox2.Text);
            StaticData.CurrentUser = thisUser;
            /*
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Username or password is left empty");
                return;
            }
            else if (textBox1.Text[0] == ' ' || textBox2.Text[0] == ' ')
            {
                MessageBox.Show("Username or pasword can't start with a space");
                return;
            }
            else
            {
                if (StaticData.labels.Contains(textBox1.Text))
                {
                    MessageBox.Show("Username is already taken");
                    // return;
                }
            }
            GrayFace = cam.QueryGrayFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            MCvAvgComp[][] DetectedFaces = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            if (DetectedFaces[0].Length == 0)
            { 
                MessageBox.Show("Veidas nerastas, bandykite dar karta");
                return;
            }
            else if(DetectedFaces[0].Length > 1)
            {
                MessageBox.Show("Kadre perdaug veidu");
                return;
            }

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            panel1.Hide();
            imageBox1.Location = new Point(imageBox1.Location.X, imageBox1.Location.Y - 50);
            Hei = 320; Len = 240;

            //Raudonas taskas
            Bitmap bim = new Bitmap(Image.FromFile(Application.StartupPath + "/Images/RedPoint.png"), 64, 64);
            var redDot = new PictureBox
            {
                Name = "RedDot",
                Size = new Size(64, 64),
                Image = bim,
            };
            this.Controls.Add(redDot);
            Console.WriteLine(this.Height + " " + this.Width);
            redDot.Location = new Point(this.Width / 2, this.Height / 2);
            redDot.BringToFront();

            foreach (MCvAvgComp f in DetectedFaces[0])
            {
                TrainImg = frame.Copy(f.rect).Convert<Gray, byte>();
                break;
            }
            StaticData.numLablels++;
            TrainImg = TrainImg.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            StaticData.training.Add(TrainImg);
            StaticData.labels.Add(textBox1.Text);
            User thisUser = new User(textBox1.Text, textBox2.Text);
            StaticData.CurrentUser = thisUser;
            LogicC.SaveFaceData();*/
        }

        private int CheckTheTB()
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Username or password is left empty");
                return 1;
            }
            else if (textBox1.Text[0] == ' ' || textBox2.Text[0] == ' ')
            {
                MessageBox.Show("Username or pasword can't start with a space");
                return 1;
            }
            else
            {
                if (StaticData.labels.Contains(textBox1.Text))
                {
                    MessageBox.Show("Username is already taken");
                    // return 1;
                }
            }
            return 0;
        }

        private void InstantiateRedDot()
        {
            //Raudonas taskas
            Bitmap bim = new Bitmap(Image.FromFile(Application.StartupPath + "/Images/RedPoint.png"), 64, 64);
            redDot.Name = "RedDot";
            redDot.Size = new Size(64, 64);
            redDot.Image = bim;
            this.Controls.Add(redDot);
            Console.WriteLine(this.Height + " " + this.Width);
            redDot.Location = new Point(this.Width / 2, this.Height / 2);
            redDot.BringToFront();
        }

        private void PrepareForRegister()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            panel1.Hide();
            imageBox1.Location = new Point(imageBox1.Location.X, imageBox1.Location.Y - 50);
            Hei = 320; Len = 240;
            imageBox1.Size = new Size(320, 240);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void RegisterProcess()
        {
            int iterator = 0;
            while (iterator < 10)
            {
                GrayFace = cam.QueryGrayFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                MCvAvgComp[][] DetectedFaces = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
                if (DetectedFaces[0].Length == 1)
                {
                    Information.Invoke(new ChangeText(ChText), "Sekite Taska");
                    foreach (MCvAvgComp f in DetectedFaces[0])
                    {
                        StaticData.training.Add(frame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC));
                        StaticData.numLablels++;
                        iterator++;
                        break;
                    }
                    if (iterator % 2 == 1)
                        Thread.Sleep(100);
                    else
                    {
                        if (iterator == 2)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(64, redDot.Location.Y));
                        else if (iterator == 4)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(this.Width - 64, redDot.Location.Y));
                        else if (iterator == 6)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(this.Width / 2, this.Height - 64));
                        else if (iterator == 8)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(redDot.Location.X, 64));
                        Thread.Sleep(3000);
                    }
                }
                else { Information.Invoke(new ChangeText(ChText), "Kadras netinkamas registracijai"); }
            }
            LogicC.SaveFaceData();
            this.Close();
        }

        public delegate void MoveDot(Point newLoc);

        private void MoveRedDot(Point newLoc)
        {
            redDot.Location = newLoc;
        }

        public delegate void ChangeText(String text);

        private void ChText(String text)
        {
            Information.Text = text;
        }
    }

}
