using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace VirtualLibrary
{
    public partial class LoginWindow : Form
    {
        private LogicController logicC;
        private Form1 main;
        Image<Gray, byte> GrayFace = null;
        Image<Bgr, Byte> frame= null;
        string name = "";
        Capture cam;
        HaarCascade faceDetect;
        Image<Gray, byte> result;
        int StartTime, EndTime;
        MainWindow mainW;
        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);

        public LoginWindow(LogicController logicC, Form1 main)
        {
            InitializeComponent();
            this.main = main;
            try
            {
                cam = new Capture();
            }
            catch(Exception e)
            {
                cam = null;
            }
            this.logicC = logicC;
            faceDetect = new HaarCascade("haarcascade_frontalface_default.xml");
            StartTime = DateTime.Now.TimeOfDay.Seconds;
            this.FormClosing += OnCloseRequest;
            Application.Idle += new EventHandler(FaceRecognition);
        }

        private void OnCloseRequest(object sender, EventArgs e)
        {
            if (StaticData.CurrentUser == null)
            {
                main.Show();
                MessageBox.Show("Didn't find your face :( \n Try again or Register");
            }
            cam.Dispose();
            Application.Idle -= FaceRecognition;
        }

        private void TransitionToMainW()
        {
            main.OpenMainWindow();
            this.Close();
        }

        public void FaceRecognition(object sender, EventArgs e)
        {                      
             if(cam == null)
             {
                StaticData.CurrentUser = new User("Debug", "Debug");
                TransitionToMainW();
                return;
             }
            frame = cam.QueryFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            Camera.Image = frame;
            GrayFace = frame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetectedNow = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(640 / 4, 480 / 4));
            if (facesDetectedNow[0].Length > 1)
            {
                MessageBox.Show("Too many faces");
            }
            foreach (MCvAvgComp f in facesDetectedNow[0])
            {
                result = frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                frame.Draw(f.rect, new Bgr(Color.Green), 3);
                if (StaticData.training.ToArray().Length != 0)
                {
                    MCvTermCriteria termCriteria = new MCvTermCriteria(StaticData.numLablels, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(StaticData.training.ToArray(), StaticData.labels.ToArray(), 1500, ref termCriteria);
                    name = recognizer.Recognize(result);
                    if (!name.Equals(""))
                    {
                        StaticData.CurrentUser = new User(name, "fsdfsdgsd");
                        TransitionToMainW();
                    }
                }
                break;
            }
            Camera.Image = frame;
            EndTime = DateTime.Now.TimeOfDay.Seconds;
            if (EndTime - StartTime >= 20 || (EndTime - StartTime >= -40 && EndTime - StartTime < 0))
            {
                this.Hide();
                Close();
            }
        }
    }
}
