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
        public LoginWindow(LogicController logicC)
        {
            InitializeComponent();
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
