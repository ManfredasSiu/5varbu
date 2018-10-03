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
        MCvAvgComp[][] facesDetectedNow;

        string name;

        private Thread RegProcess;
        private int Hei = 640, Len = 480;
        private bool InProgress = false;

        private LogicController LogicC;
        private Form1 main;

        public Form2(LogicController LogicC, Form1 main)
        {
            InitializeComponent();
            this.main = main;
            this.LogicC = LogicC;
            faceDetect = new HaarCascade("haarcascade_frontalface_default.xml");
            this.FormClosing += OnCloseRequest;
            //Tikrinama ar turima kamera
            try
            {
                cam = new Capture();
                Application.Idle += new EventHandler(FrameProcedure);
            }
            catch(Exception e)
            {
                cam = null;
                MessageBox.Show("Neturi kameros, arba ji blogai prijungta, registracija negalima");
                this.Close(); //Uzdaromas langas, kamera nerasta
            }
        }


        private void OnCloseRequest(object sender, EventArgs e) //Metodas iskvieciamas pries uzdarant langa.
        {
            if(StaticData.CurrentUser == null)  //Jei registracija nepavyko gryztam
                main.Show();
            cam.Dispose();
            if(InProgress == true)              //Jei registracija vyksta ir isjungiamas langas pvz alt+f4
                RegProcess.Abort();
            Application.Idle -= FrameProcedure;
        }

        private void BackGroundColorChange()
        {
            if (InProgress == true && facesDetectedNow[0].Length != 1)
            {
                this.BackColor = Color.Red;
                Information.Text = "Kadras netinkamas registracijai";
            }
            else if (InProgress == true)
            {
                Information.Text = "Sekite Taska";
                this.BackColor = Color.Green;
            }
        }

        private void FrameProcedure(object sender, EventArgs e)  //Kameros freimai atsiranda imageboxe
        {
            if (cam.Equals(null))
                return;
            frame = cam.QueryFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            GrayFace = frame.Convert<Gray, Byte>();
            facesDetectedNow = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(640/4, 480/4));
            BackGroundColorChange();
            foreach (MCvAvgComp f in facesDetectedNow[0])
            {
                result = frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                frame.Draw(f.rect, new Bgr(Color.Green), 3);
            }
            imageBox1.Image = frame.Resize(Hei, Len, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); //Freimai perdaromi ir atsiranda imageboxe
        }

        private int CheckHowManyFaces()
        {
            if (facesDetectedNow[0].Length == 0)
            {
                MessageBox.Show("Veidas nerastas, bandykite dar karta");
                return 1;
            }
            else if (facesDetectedNow[0].Length > 1)
            {
                MessageBox.Show("Kadre perdaug veidu");
                return 1;
            }
            return 0;
        }

        private void RegisterButton_CLicked(object sender, EventArgs e)
        {
            if(CheckTheTB() == 1) return;

            if(CheckHowManyFaces() == 1) return;
            
            PrepareForRegister();

            InstantiateRedDot();

            RegProcess = new Thread(new ThreadStart(RegisterProcessAsync));
            RegProcess.Start();
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
                    return 1;
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

            redDot.Location = new Point(this.Width / 2, this.Height / 2);
            redDot.BringToFront();
        }

        private void PrepareForRegister()
        {
            //Langas maximizuojamas
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            panel1.Hide(); //Paslepiame register is Login laukus

            imageBox1.Location = new Point(imageBox1.Location.X, imageBox1.Location.Y - 50); //Imagebox vieta pastumiama i virsu

            //Pakeiciamas imagebox dydis
            Hei = 320; Len = 240;
            imageBox1.Size = new Size(321, 241);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public async void RegisterProcessAsync()
        {
            InProgress = true;
            int iterator = 0;
            while (iterator < 10)
            {
                if (facesDetectedNow[0].Length == 1)
                {
                    if (iterator == 0)
                        Thread.Sleep(1000); //Laikas klientui susiprasti, kad reikia sekti taska.
                    if (LogicC.TempDirectoryController("Create", textBox1.Text, cam.QueryFrame().ToBitmap(), iterator) == 1) //Sukuriu direktorija arba ne
                    {
                        MessageBox.Show("Neuztenka vietos diske");
                        this.Close();
                    }
                    iterator++;
                    if (iterator % 2 == 1)
                        Thread.Sleep(1000);
                    else
                    {   //Judinamas taskas, pagal nuotrauku skaiciu
                        if (iterator == 2)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(64, redDot.Location.Y));
                        else if (iterator == 4)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(this.Width - 64, redDot.Location.Y));
                        else if (iterator == 6)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(this.Width / 2, this.Height - 64));
                        else if (iterator == 8)
                            redDot.Invoke(new MoveDot(MoveRedDot), new Point(redDot.Location.X, 64));
                        Thread.Sleep(500);
                    }
                }
            }
            FaceApiCalls FAC = new FaceApiCalls();
            if (!await FAC.FaceSave(textBox1.Text))
            {
                StaticData.CurrentUser = null;
                MessageBox.Show("Registracija nepavyko\nPerdaug veidu kadre\nArba serveris uzimtas");
                this.Close();
                return;
            }
            User thisUser = new User(textBox1.Text, textBox2.Text);
            StaticData.CurrentUser = thisUser;
            if (LogicC.TempDirectoryController("Delete", textBox1.Text, null, 0) == 1)
            {
                this.Close();
            }
            File.AppendAllText(Application.StartupPath + "/names.txt","" + textBox1.Text + ",");
            InProgress = false;
            this.Invoke(new closeForm(closeThisFormFromAnotherThread));
        }

        public delegate void ChangeBackColor(Color color);
            
        private void ChangeColor(Color color)
        {
            this.BackColor = color;
        }

        public delegate void closeForm();

        private void closeThisFormFromAnotherThread()
        {
            main.OpenMainWindow();
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
