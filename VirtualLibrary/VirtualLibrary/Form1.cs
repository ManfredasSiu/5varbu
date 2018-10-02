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

namespace VirtualLibrary
{
    public partial class Form1 : Form     //Pagrindinis Langas, reikes veliau gal padaryt forgot password arba login with username UI
    {
        private LogicController LogicC = new LogicController();
        private MainTry mainW;

        public Form1()
        {
            InitializeComponent();
            //VoiceRecognition VRec = new VoiceRecognition(this);
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            Register();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Login();
        }

        public void OpenMainWindow()
        {
            this.Hide();
            mainW = new MainTry(LogicC);
            mainW.Show();
        }

        public void Login()
        {
            //OpenMainWindow();
            this.Hide();
            var LoginW = new LoginWindow(LogicC, this);
            LoginW.Show();
        }

        public void Register()
        {
            this.Hide();
            Form2 registerFaceWindow = new Form2(LogicC, this);
            registerFaceWindow.Show();
        }
    }
}
