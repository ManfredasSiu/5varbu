using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.ProjectOxford.Face.Contract;
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
using VirtualLibrary.API_s;
using VirtualLibrary.presenters;
using VirtualLibrary.Views;

namespace VirtualLibrary
{
    public partial class Form2 : Form, IRegister    //Register Langas, padarysiu irgi su face recognition
    {
        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        
        PictureBox redDot = new PictureBox();

        private Thread RegProcess;
        private int Hei = 640, Len = 480;
        private bool InProgress = false;

        private IDataB ADB;
        private LogicController LogicC;

        public Image<Bgr, byte> Frame { get => throw new NotImplementedException(); set => imageBox1.Image = value; }

        public string NameText => textBox1.Text;

        public string password => textBox2.Text;

        public Color BGColor { set => this.BackColor = value; }

        public String InformationText { set => this.Information.Text = value; }

        public Point ImgBoxLoc { get => imageBox1.Location; set => imageBox1.Location = value; }

        public Size ImgBoxSize { set => imageBox1.Size = value; }

        public int Wgh { get => this.Width; }

        public int Hgt { get => this.Height; }

        private RegisterPresenter registerPresenter;

        public Form2(LogicController LogicC, IDataB ADB)
        {
            InitializeComponent();
            registerPresenter = new RegisterPresenter(this);
            this.FormClosing += OnCloseRequest;
        }


        private void OnCloseRequest(object sender, EventArgs e) //Metodas iskvieciamas pries uzdarant langa.
        {
            registerPresenter.WinClose();
            LogicC.TempDirectoryController("Delete", textBox1.Text, null, 0);
            if (InProgress == true)              //Jei registracija vyksta ir isjungiamas langas pvz alt+f4
                RegProcess.Abort();
        }
        
        private void RegisterButton_CLicked(object sender, EventArgs e)
        {
            registerPresenter.RegisterButtonPressed();
        }

        private void RegistrationBehaviour()
        {
            RegProcess = new Thread(new ThreadStart(RegisterProcessAsync));
            RegProcess.Start();
        }

        public void MaximizeForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        public void HideInputPanel()
        {
            panel1.Hide();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    if (LogicC.TempDirectoryController("Create", textBox1.Text, cam.QueryFrame().ToBitmap(), iterator) == 1) //Sukuriu direktorija arba ne
                    {
                        MessageBox.Show("Neuztenka vietos diske");
                        this.Close();
                    }
                    iterator++;
                    if (iterator % 2 == 1)
                       Thread.Sleep(700);
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
                        Thread.Sleep(200);
                    }       
                }
            }
            if (!await FAC.FaceSave(textBox1.Text))
            {
                StaticData.CurrentUser = null;
                MessageBox.Show("Registracija nepavyko\nPerdaug veidu kadre\nArba serveris uzimtas");
                this.Close();
                return;
            }
            ADB.AddUser(textBox1.Text, textBox2.Text, null, 0);
            ADB.GetUser(textBox1.Text);
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
            //main.OpenMainWindow();
            this.Close();
        }

        public delegate void MoveDot(Point newLoc);

        private void MoveRedDot(Point newLoc)
        {
            redDot.Location = newLoc;
        }

        public delegate void ChangeText(String text);

        private void buttonShutDown_Click(object sender, EventArgs e)
        {

        }

        private void ChText(String text)
        {
            Information.Text = text;
        }

        public void CloseForm()
        {
            this.Close();
        }

        public void InitMessageBox(string Message)
        {
            MessageBox.Show(Message);
        }

        public void AddControll(Control control)
        {
            this.Controls.Add(control);
        }
    }

}
