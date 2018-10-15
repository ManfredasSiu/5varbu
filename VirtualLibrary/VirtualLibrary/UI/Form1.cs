using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using VirtualLibrary.presenters;
using VirtualLibrary.Views;

namespace VirtualLibrary
{
    public partial class Form1 : Form, IMenu     //Pagrindinis Langas, reikes veliau gal padaryt forgot password arba login with username UI
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            MenuPresenter MP = new MenuPresenter(this);
            MP.RegisterButtonPressed();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            MenuPresenter MP = new MenuPresenter(this);
            MP.LoginButtonPressed();
        }
        
        public void HideForm()
        {
            this.Hide();
        }

        private void buttonShutDown_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void ShowForm()
        {
            this.Show();
        }
    }
}
