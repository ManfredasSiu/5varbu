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
    public partial class MainTry : Form, IMain
    {

        public string Status { set => label6.Text = value; }

        public int panelLft { get => panelLeft.Width; set => panelLeft.Width = value; }

        public int panelSideTop { set => panelSide.Top = value; }

        public int panelSideHgh { set => panelSide.Height = value; }

        public Control NewControl { set => panelControls.Controls.Add(value); }

        public string Date { set => labelDate.Text = value; }

        public Timer Tmr1 { get => timer1; set => throw new NotImplementedException(); }

        string IMain.UserName { get => UserName.Text; set => UserName.Text = value; }

        MainPresenter MP;

        public void refresh()
        {
            this.Refresh();
        }

        public MainTry()
        {
            InitializeComponent();
            MP = new MainPresenter(this);
            this.FormClosing += OnCloseReq;
        }

        public void ClearControlsFromPanel()
        {
            panelControls.Controls.Clear();
        }

        private void OnCloseReq(object sender, EventArgs e)
        {
            Application.Exit();
        }
        

        private void buttonHome_Click(object sender, EventArgs e)
        {
            MP.HButtonBehaviour(buttonHome);
        }

        private void buttonMyBooks_Click(object sender, EventArgs e)
        {
            MP.MBButtonBehaviour(buttonMyBooks);
        }

        private void buttonLibrary_Click(object sender, EventArgs e)
        {
            MP.LButtonBehaviour(buttonLibrary);
        }


        private void buttonRecom_Click(object sender, EventArgs e)
        {
            MP.RButtonBehaviour(buttonRecom);
        }
        
        private void buttonShutDown_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            MP.timer1Ticks();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            MP.timer2Ticks();
        }
    }
}
