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
    public partial class MainTry : Form
    {
        private LogicController logicC;

        public MainTry(LogicController logicC)
        {
            InitializeComponent();
            this.logicC = logicC;
            DataTable dt = new DataTable();
            dt.Columns.Add("Knygos autorius ");
            dt.Columns.Add("Pavadinimas ");
            dt.Columns.Add("Kodas ");
            dt.Columns.Add("0/1 ");
            for (int x = 0; x < StaticData.Books.Count(); x =+4)
            {
                dt.Rows.Add(StaticData.Books[x], StaticData.Books[x + 1], StaticData.Books[x + 2], StaticData.Books[x + 3]);
            }
            dataGridView1.DataSource = dt;
            this.FormClosing += OnCloseReq;
            UserName.Text = StaticData.CurrentUser.getuserName();
        }

        private void OnCloseReq(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
