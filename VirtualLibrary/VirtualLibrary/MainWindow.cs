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
    public partial class MainWindow : Form
    {
    

        private void ShowMyBooks(object sender, EventArgs e)
        {
            MessageBox.Show("pavyko");
        }

        private LogicController logicC;

        public MainWindow(LogicController logicC)
        {
            InitializeComponent();
            label2.Text = ("Hello, " + StaticData.CurrentUser.getuserName() + "!");


            this.logicC = logicC;
            FormClosed += OnCloseRequest;

        }

        private void OnCloseRequest(Object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Knygos autorius ");
            dt.Columns.Add("Pavadinimas ");
            dt.Columns.Add("Kodas ");
            dt.Columns.Add("0/1 ");
            for (int x = 0; x < StaticData.Books.Count; x=+4)
            {
                dt.Rows.Add(StaticData.Books[x], StaticData.Books[x+1], StaticData.Books[x+2], StaticData.Books[x+3]);
            }
            dataGridView1.DataSource = dt;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
