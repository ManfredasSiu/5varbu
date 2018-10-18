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
using VirtualLibrary.API_s;
using VirtualLibrary.presenters;
using VirtualLibrary.Views;

namespace VirtualLibrary
{
    public partial class LoginWindow : Form, ILogin
    {
        private LoginPresenter LP;

        public LoginWindow()
        {
            InitializeComponent();
            LP = new LoginPresenter(this);
            this.FormClosing += LP.OnCloseForm;
        }

        private bool block = false;

        public Image<Bgr, byte> image { get => (Image<Bgr, byte>)Camera.Image; set => Camera.Image = value; }


        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.Show();
        }

        private void buttonShutDown_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
