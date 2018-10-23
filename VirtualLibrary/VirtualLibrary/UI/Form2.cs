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
        PictureBox redDot = new PictureBox();

        public Image<Bgr, byte> Frame { get => throw new NotImplementedException(); set => imageBox1.Image = value; }

        public string NameText => textBox1.Text;

        public string password => textBox2.Text;

        public Color BGColor { set => this.BackColor = value; }

        public String InformationText { set => this.Information.Text = value; }

        public Point ImgBoxLoc { get => imageBox1.Location; set => imageBox1.Location = value; }

        public Size ImgBoxSize { set => imageBox1.Size = value; }

        public int Hgt { get => this.Height; }

        public int Wdt => this.Width;

        private RegisterPresenter registerPresenter;

        public Form2()
        {
            InitializeComponent();
            registerPresenter = new RegisterPresenter(this);
            this.FormClosing += OnCloseRequest;
        }

        public void ShowForm()
        {
            this.Show();
        }

        private void OnCloseRequest(object sender, EventArgs e) //Metodas iskvieciamas pries uzdarant langa.
        {
            registerPresenter.WinClose();
        }
        
        private void RegisterButton_CLicked(object sender, EventArgs e)
        {
            registerPresenter.RegisterButtonPressed();
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
