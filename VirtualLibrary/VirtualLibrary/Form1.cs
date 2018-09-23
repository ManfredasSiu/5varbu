using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary
{
    public partial class Form1 : Form     //Pagrindinis Langas, reikes veliau gal padaryt forgot password arba login with username UI
    {
        private LogicController LogicC = new LogicController();
        public Form1()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 registerFaceWindow = new Form2(LogicC, this);
            registerFaceWindow.Show();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            var LoginW = new LoginWindow(LogicC, this);
            LoginW.Show();
        }
    }
}
