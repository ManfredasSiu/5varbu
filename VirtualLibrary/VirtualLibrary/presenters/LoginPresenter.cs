using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    class LoginPresenter
    {
        Image<Gray, byte> GrayFace = null;
        Image<Bgr, Byte> frame = null;
        Capture cam;
        HaarCascade faceDetect;
        int StartTime, EndTime;

        private bool block = false;

        ILogin loginview;

        public LoginPresenter(ILogin loginview)
        {
            this.loginview = loginview;
            try
            {
                cam = new Capture();
            }
            catch (Exception e)
            {
                cam = null;
            }
            faceDetect = new HaarCascade("haarcascade_frontalface_default.xml");
            StartTime = DateTime.Now.TimeOfDay.Seconds;
            Application.Idle += new EventHandler(FaceRecognitionAsync);
        }

        public async void FaceRecognitionAsync(object sender, EventArgs e)
        {
            if (cam == null)
            {
                StaticData.CurrentUser = new User(999, "Debug", "Debug", null, "1");
                loginview.CloseForm();
                RefClass.Instance.InitMainForm();
                return;
            }
            frame = cam.QueryFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            loginview.image = frame;
            GrayFace = frame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetectedNow = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(640 / 4, 480 / 4));
            if (facesDetectedNow[0].Length > 1)
            {
                MessageBox.Show("Too many faces");
            }
            else if (facesDetectedNow[0].Length != 0)
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
                            loginview.CloseForm();
                            RefClass.Instance.InitMainForm();
                        }
                    }
                }
            }
            loginview.image = frame;
            EndTime = DateTime.Now.TimeOfDay.Seconds;
            if (EndTime - StartTime >= 20 || (EndTime - StartTime >= -40 && EndTime - StartTime < 0))
            {
                loginview.CloseForm();
            }
        }

        public void OnCloseForm(object sender, EventArgs e)
        {
            if (StaticData.CurrentUser == null)
            {
                RefClass.Instance.menuForm.ShowForm();
                MessageBox.Show("Didn't find your face :( \n Try again or Register");
            }
            if (cam != null)
                cam.Dispose();
            Application.Idle -= FaceRecognitionAsync;
        }

        public async Task<string> startRecAsync()
        {
            block = true;
            FaceApiCalls FAC = new FaceApiCalls();
            try
            {
                var name = await FAC.RecognitionAsync(Application.StartupPath + "TempImg.jpg");
                if (name != null)
                {
                    String[] data = RefClass.Instance.LogicC.DB.GetUser(name);
                }
                else loginview.CloseForm();
                return name;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                StaticData.CurrentUser = null;
                loginview.CloseForm();
                return null;
            }
        }

    }
}
