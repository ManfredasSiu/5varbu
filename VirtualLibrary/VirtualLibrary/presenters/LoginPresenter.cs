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
            catch 
            {
                cam = null;
            }
            faceDetect = new HaarCascade("haarcascade_frontalface_default.xml");  //HaarCascade skirta veido bruozam nustatyti         
            StartTime = DateTime.Now.TimeOfDay.Seconds;                           //Laikmatis
            Application.Idle += new EventHandler(FaceRecognitionAsync);           //Main threadas dirba su veido detectinimu
        }

        //Veido atradimo funkcija***
        public async void FaceRecognitionAsync(object sender, EventArgs e) 
        {
            if (cam == null) //Tikrinama ar pavyko sukurti kamera
            {
                //StaticData.CurrentUser = new User(999, "Debug", "Debug", null, "1");

                loginview.CloseForm();
                //RefClass.Instance.InitMainForm();  //Vladislav atsikometuok abudu
                return;
            }
            frame = cam.QueryFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); //Nuotrauka paruosiama atpazinimui
            loginview.image = frame;                                                        //Keiciami freimai
            GrayFace = frame.Convert<Gray, Byte>();                                         //Nuotrauka nudazoma pilkai
            MCvAvgComp[][] facesDetectedNow = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(640 / 4, 480 / 4)); //Visu veidu atradimas
            if (facesDetectedNow[0].Length > 1)                                             //Ar veidu nera perdaug?
            {
                MessageBox.Show("Too many faces");
            }
            else if (facesDetectedNow[0].Length != 0)                                       //Ar veidu apskritai yra?
            {
                cam.QueryFrame().Save(Application.StartupPath + "TempImg.jpg");             //Veidas issaugomas laikinai
                if (StaticData.CurrentUser == null)
                {
                    if (block == false)
                    {
                        string name = null; 
                        name = await startRecAsync();                                       //Gaunamas vartotojo vardas arba null jei nera      
                        if (StaticData.CurrentUser != null)
                        {
                            loginview.CloseForm();                                          //Jei vartotojas rastas atidaroma pagrindime programos forma
                            RefClass.Instance.InitMainForm();
                        }
                    }
                }
            }
            loginview.image = frame;
            EndTime = DateTime.Now.TimeOfDay.Seconds;
            if (EndTime - StartTime >= 20 || (EndTime - StartTime >= -40 && EndTime - StartTime < 0))
            {
                loginview.CloseForm();                                                      //Jei baigesi laikas griztame
            }
        }
        //***

        public void OnCloseForm(object sender, EventArgs e)
        {
            if (cam == null && StaticData.CurrentUser == null)                       //Jei nera kameros
            {
                MessageBox.Show("Neturite Kameros\nprijunkite kamera ir bandykite dar syki");
                RefClass.Instance.menuForm.ShowForm();
            }
            else if (StaticData.CurrentUser == null)                                 //Jei kamera yra bet vartotojas nerastas
            {
                MessageBox.Show("Didn't find your face :( \n Try again or Register");
                RefClass.Instance.menuForm.ShowForm();
            }
            if (cam != null)
                cam.Dispose();
            Application.Idle -= FaceRecognitionAsync;
        }

        public async Task<string> startRecAsync()   //Veido atpazinimo metodas
        {
            block = true;
            var FAC = RefClass.Instance.InitAzureFaceApi();
            try
            {
                var name = await FAC.RecognitionAsync(Application.StartupPath + "TempImg.jpg");  //Siunciama uzklausa i API
                if (name != null)
                {
                    User data = RefClass.Instance.LogicC.DB.GetUser(name);
                    StaticData.CurrentUser = data;      //Uzkraunamas vartotojas
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
