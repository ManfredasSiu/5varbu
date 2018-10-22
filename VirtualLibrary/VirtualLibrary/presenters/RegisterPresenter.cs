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
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    public class RegisterPresenter
    {
        IRegister RegView;
        Image<Bgr, Byte> frame;
        Capture cam;

        PictureBox redDot = new PictureBox();
        Image<Gray, byte> result;
        Image<Gray, byte> GrayFace = null;
        MCvAvgComp[][] facesDetectedNow;
        HaarCascade faceDetect;
        DataController LogicC;
        IDataB ADB;
        Thread RegProcess;

        bool InProgress = false;

        private int Hei = 640, Len = 480;

        public RegisterPresenter(IRegister RegView)
        {
            try
            {
                this.RegView = RegView;
                faceDetect = new HaarCascade("haarcascade_frontalface_default.xml"); //Skirta veidu atpazinimui
                LogicC = RefClass.Instance.LogicC;
                this.ADB = LogicC.DB;
                try
                {
                    cam = new Capture();
                }
                catch
                {
                    cam = null;
                    RegView.InitMessageBox("Neturi kameros, arba ji blogai prijungta, registracija negalima");
                    RegView.CloseForm();
                }
                Application.Idle += FrameProcedure;
            }
            catch
            {
                return;
            }
        }

        private void FrameProcedure(Object sender, EventArgs e) //Analogiskaip kaip ir login formoj
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
            //Raudonas taskas, jo sukurimas
            Bitmap bim = new Bitmap(Image.FromFile(Application.StartupPath + "/Images/RedPoint.png"), 64, 64);
            redDot.Name = "RedDot";
            redDot.Size = new Size(64, 64);
            redDot.Image = bim;
            RegView.AddControll(redDot);
            redDot.Location = new Point(RegView.Wdt / 2, RegView.Hgt / 2);
            redDot.BringToFront();
        }

        private void PrepareForRegister()  //Registracijos proceso paruosimas
        {
            RegView.MaximizeForm();
            RegView.HideInputPanel();
            RegView.ImgBoxLoc = new Point(RegView.ImgBoxLoc.X, RegView.ImgBoxLoc.Y - 50); //Imagebox vieta pastumiama i virsu
            Hei = 320; Len = 240;
            RegView.ImgBoxSize = new Size(321, 241);
        }

        public int CheckHowManyFaces(int FaceArrayLength)  //Security blokai veidu atzvilgiu
        {
            if (FaceArrayLength == 0)
            {
                return 1;
            }
            else if (FaceArrayLength > 1)
            {
                return 2;
            }
            return 0;
        }

        private void BackGroundColorChange()  //backgroundo spalvos keitimas
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

        public void RegisterButtonPressed()   //Register mygtuko logika
        {
            int check = CheckTheTB(RegView.password, RegView.NameText, ADB);
            switch(check)
            {
                case 1:
                    RegView.InitMessageBox("Username Field is empty");
                    return;
                case 2:
                    RegView.InitMessageBox("This username already exists");
                    return;
                case 3:
                    RegView.InitMessageBox("Password Field is empty");
                    return;
            }
        
            check = CheckHowManyFaces(facesDetectedNow[0].Length);

            switch (check)
            {
                case 1:
                    RegView.InitMessageBox("Face not found. Try again");
                    return;
                case 2:
                    RegView.InitMessageBox("Too many faces");
                    return;
            }


            PrepareForRegister();

            InstantiateRedDot();

            RegProcess = new Thread(new ThreadStart(RegisterProcessAsync));
            RegProcess.Start();
        }

        public void WinClose()  //Formos uzdarymo metodas
        {
            if (StaticData.CurrentUser == null)
                RefClass.Instance.menuForm.ShowForm();
            else
                RefClass.Instance.InitMainForm();
            LogicC.TempDirectoryController("Delete", RegView.NameText, null, 0);
            if (InProgress == true)              //Jei registracija vyksta ir isjungiamas langas pvz alt+f4
                RegProcess.Abort();
        }

        public int CheckTheTB(String pass, String Nam, IDataB DB) //Security blokai textbox atzvilgiu
        {
            if (Nam.Replace(" ", "") == "")
            {
                return 1;
            }
            else if (DB.SearchUser(Nam) == 2)
            {
                return 2;
            }
            else if (pass.Replace(" ", "") == "")
            {
                return 3;
            }
            return 0;
        }

        public async void RegisterProcessAsync()
        {
            FaceApiCalls FAC = new FaceApiCalls();
            InProgress = true;
            int iterator = 0;
            while (iterator < 10)
            {
                if (facesDetectedNow[0].Length == 1)
                {
                    if (iterator == 0)
                        Thread.Sleep(1000); //Laikas klientui susiprasti, kad reikia sekti taska.
                    if (LogicC.TempDirectoryController("Create", RegView.NameText, cam.QueryFrame().ToBitmap(), iterator) == 1) //Sukuriu direktorija arba ne
                    {
                        MessageBox.Show("Neuztenka vietos diske");
                        RegView.CloseForm();
                    }
                    iterator++;
                    if (iterator % 2 == 1)
                        Thread.Sleep(700);
                    else
                    {   //Judinamas taskas, pagal nuotrauku skaiciu
                        if (iterator == 2)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(64, redDot.Location.Y));
                        else if (iterator == 4)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(RegView.Wdt - 64, redDot.Location.Y));
                        else if (iterator == 6)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(RegView.Wdt / 2, RegView.Hgt - 64));
                        else if (iterator == 8)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(redDot.Location.X, 64));
                        Thread.Sleep(200);
                    }
                }
            }
            if (!await FAC.FaceSave(RegView.NameText))  //Veidas issaugomas
            {
                StaticData.CurrentUser = null;
                MessageBox.Show("Registracija nepavyko\nPerdaug veidu kadre\nArba serveris uzimtas"); //Jei issaugoti nepavyko gryztame i menu
                ((Form)RegView).Invoke(new closeForm(closeThisFormFromAnotherThread));
                return;
            }
            //Informacija padedama i duomenu baze
            ADB.AddUser(RegView.NameText, RegView.password, null, 0);
            ADB.GetUser(RegView.NameText);

            InProgress = false;
            ((Form)RegView).Invoke(new closeForm(closeThisFormFromAnotherThread));
        }

        //Delegate funkcijos skirtos atlitki tam tikrus veiksmus is ktio thredo
        public delegate void ChangeBackColor(Color color);

        private void ChangeColor(Color color)
        {
            RegView.BGColor = color;
        }

        public delegate void closeForm();

        private void closeThisFormFromAnotherThread()
        {
            RegView.CloseForm();
        }

        public delegate void MoveDot(Point newLoc);

        private void MoveRedDot(Point newLoc)
        {
            redDot.Location = newLoc;
        }
    }
}
