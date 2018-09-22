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


        // List<string> users = new List<string>();
        //  int count = 0, numLablels, t;
        string name, names = null;

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
            imageBox1.Image = frame;
        }

        private void RegisterButton_CLicked(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Username or password is left empty");
                return;
            }
            else if(textBox1.Text[0] == ' ' || textBox2.Text[0] == ' ')
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
                return;
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
            LogicC.SaveFaceData();
            
            //this.Close();
            //cam.Dispose();
            //Application.Idle -= FrameProcedure;
                 //Sitie reikalingi
            
            /*File.WriteAllText(Application.StartupPath + "/faces/faces.txt", StaticData.training.ToArray().Length + ",");
            for (int i = 1; i <= StaticData.numLablels; i++)
            {
                StaticData.training.ToArray()[i - 1].Save(Application.StartupPath + "/faces/face" + i + ".bmp");
                File.AppendAllText(Application.StartupPath + "/faces/faces.txt", StaticData.labels.ToArray()[i - 1] + ",");
            }*/

            //MessageBox.Show(textBox1.Text + ", Welcome.");
        }
    }
}
