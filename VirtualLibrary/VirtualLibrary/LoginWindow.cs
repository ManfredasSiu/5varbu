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
    public partial class LoginWindow : Form
    {
        private LogicController logicC;
        private Form1 main;

        public LoginWindow(LogicController logicC, Form1 main)
        {
            InitializeComponent();
            this.main = main;
            this.logicC = logicC;
        }

        public void FaceRecognition()
        {


            if (true)//Kolkas, Kol neidejau face Recognitiono
            {
                this.Hide();
                MainWindow registerFaceWindow = new MainWindow(logicC);
                registerFaceWindow.Show();
            }
        }
    }
}
