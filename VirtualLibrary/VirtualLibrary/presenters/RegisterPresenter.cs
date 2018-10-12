using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    class RegisterPresenter
    {
        IRegister RegView;
        Image<Bgr, Byte> frame;
        Capture cam;

        PictureBox redDot = new PictureBox();
        Image<Gray, byte> result;
        Image<Gray, byte> GrayFace = null;
        MCvAvgComp[][] facesDetectedNow;
        HaarCascade faceDetect;

        bool InProgress = false;

        private int Hei = 640, Len = 480;

        public RegisterPresenter(IRegister RegView)
        {
            this.RegView = RegView;
            faceDetect = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                cam = new Capture();
            }
            catch (Exception e)
            {
                cam = null;
                RegView.InitMessageBox("Neturi kameros, arba ji blogai prijungta, registracija negalima");
                RegView.CloseForm(); //Uzdaromas langas, kamera nerasta
            }
            Application.Idle += FrameProcedure;
        }

        private void FrameProcedure(Object sender, EventArgs e)
        {
            if (cam.Equals(null))
                return;
            frame = cam.QueryFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            GrayFace = frame.Convert<Gray, Byte>();
            facesDetectedNow = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(640 / 4, 480 / 4));
            BackGroundColorChange();
            foreach (MCvAvgComp f in facesDetectedNow[0])
            {
                result = frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                frame.Draw(f.rect, new Bgr(Color.Green), 3);
            }
            RegView.Frame = frame.Resize(Hei, Len, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); //Freimai perdaromi ir atsiranda imageboxe
        }

        private void InstantiateRedDot()
        {
            //Raudonas taskas
            Bitmap bim = new Bitmap(Image.FromFile(Application.StartupPath + "/Images/RedPoint.png"), 64, 64);
            redDot.Name = "RedDot";
            redDot.Size = new Size(64, 64);
            redDot.Image = bim;
            RegView.AddControll(redDot);
            redDot.Location = new Point(RegView.Wdt / 2, RegView.Hgt / 2);
            redDot.BringToFront();
        }

        private void PrepareForRegister()
        {
            RegView.MaximizeForm();
            RegView.HideInputPanel();
            RegView.ImgBoxLoc = new Point(RegView.ImgBoxLoc.X, RegView.ImgBoxLoc.Y - 50); //Imagebox vieta pastumiama i virsu
            Hei = 320; Len = 240;
            RegView.ImgBoxSize = new Size(321, 241);
        }

        private int CheckHowManyFaces()
        {
            if (facesDetectedNow[0].Length == 0)
            {
                RegView.InitMessageBox("Veidas nerastas, bandykite dar karta");
                return 1;
            }
            else if (facesDetectedNow[0].Length > 1)
            {
                RegView.InitMessageBox("Kadre perdaug veidu");
                return 1;
            }
            return 0;
        }

        private void BackGroundColorChange()
        {
            if (InProgress == true && facesDetectedNow[0].Length != 1)
            {
                RegView.BGColor = Color.Red;
                RegView.InformationText = "Kadras netinkamas registracijai";
            }
            else if (InProgress == true)
            {
                RegView.InformationText = "Sekite Taska";
                RegView.BGColor = Color.Green;
            }
        }

        public void RegisterButtonPressed()
        {
            if (CheckTheTB() == 1) return;

            if (CheckHowManyFaces() == 1) return;
        }

        private int CheckTheTB()
        {
            if (RegView.NameText.Replace(" ", "") == "")
            {
                RegView.InitMessageBox("Username Field is empty");
                return 1;
            }
            else if (RegView.password.Replace(" ", "") == "")
            {
                RegView.InitMessageBox("Password Field is empty");
                return 1;
            }
            return 0;
        }
    }
}
