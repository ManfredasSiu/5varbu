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
        private AzureDatabase ADB;

        Image<Gray, byte> GrayFace = null;
        Image<Bgr, Byte> frame= null;
        Capture cam;
        HaarCascade faceDetect;
        int StartTime, EndTime;
        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        public Object locket = null;

        public LoginWindow(LogicController logicC, Form1 main)
        {
            ADB = new AzureDatabase();
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
            Application.Idle += new EventHandler(FaceRecognitionAsync);
        }

        private void OnCloseRequest(object sender, EventArgs e)
        {
            if (StaticData.CurrentUser == null)
            {
                main.Show();
                MessageBox.Show("Didn't find your face :( \n Try again or Register");
            }
            if (cam != null)
                cam.Dispose();
            Application.Idle -= FaceRecognitionAsync;
        }

        private void TransitionToMainW()
        {
            main.OpenMainWindow();
            this.Close();
        }

        private bool block = false;

        public async Task<string> startRecAsync()
        {
            block = true;
            FaceApiCalls FAC = new FaceApiCalls();
            try
            {
                var name = await FAC.RecognitionAsync(Application.StartupPath + "TempImg.jpg");
                if (name != null)
                {
                    String[] data = ADB.GetUser(name);
                }
                else this.Close();
                return name;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                StaticData.CurrentUser = null;
                this.Close();
                return null;
            }
            
        }

        public async void FaceRecognitionAsync(object sender, EventArgs e)
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
            else if(facesDetectedNow[0].Length != 0)
            {
                cam.QueryFrame().Save(Application.StartupPath + "TempImg.jpg");
                
                if (StaticData.CurrentUser == null)
                {
                    if (block == false)
                    {
                        string name = null;
                        name = await startRecAsync();
                        if (StaticData.CurrentUser != null)
                        {
                            TransitionToMainW();
                        }
                    }
                }
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
