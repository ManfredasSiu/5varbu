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
using VirtualLibrary.Views;

namespace VirtualLibrary
{
    public partial class MainTry : Form, IMain
    {
        private LogicController logicC;
        private IDataB ADB;

        int PanelWidth;
        bool isCollapsed;

        public string Status { set => label6.Text = value; }

        public int panelWdt { get => panelLeft.Width; set => panelLeft.Width = value; }

        public MainTry()
        {
            InitializeComponent();
            this.FormClosing += OnCloseReq;
        }



        private void OnCloseReq(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void moveSidePanel(Control btn)
        {
            panelSide.Top = btn.Top;
            panelSide.Height = btn.Height;
        }

        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panelControls.Controls.Clear();
            panelControls.Controls.Add(c);
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            moveSidePanel(buttonHome);
            UserControlHome uch = new UserControlHome(ADB);
            AddControlsToPanel(uch);
        }

        private void buttonMyBooks_Click(object sender, EventArgs e)
        {
            moveSidePanel(buttonMyBooks);
            UserControlMyBooks ucmb = new UserControlMyBooks(ADB);
            AddControlsToPanel(ucmb);
        }

        private void buttonLibrary_Click(object sender, EventArgs e)
        {
            moveSidePanel(buttonLibrary);
            UserControlLibrary ucl = new UserControlLibrary(ADB);
            AddControlsToPanel(ucl);
        }


        private void buttonRecom_Click(object sender, EventArgs e)
        {
            moveSidePanel(buttonRecom);
            UserControlRecom ucr = new UserControlRecom();
            AddControlsToPanel(ucr);
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
            if (isCollapsed)
            {
                panelLeft.Width = panelLeft.Width + 10;
                if (panelLeft.Width >= PanelWidth)
                {
                    timer1.Stop();
                    isCollapsed = false;
                    this.Refresh();
                }
            }
            else
            {
                panelLeft.Width = panelLeft.Width - 10;
                if (panelLeft.Width <= 85)
                {
                    timer1.Stop();
                    isCollapsed = true;
                    this.Refresh();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            labelDate.Text = dateTime.ToString("yyyy-MM-dd");
        }

        public void StartTmer()
        {
            throw new NotImplementedException();
        }

        public void AddControl(Control control)
        {
            throw new NotImplementedException();
        }
    }
}
