using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;
using VirtualLibrary.presenters;

namespace VirtualLibrary
{
    public partial class UserControlMyBooks : UserControl, IUControlMB
    {
        public DataGridView DGW { get => dataGridView1; }

        public UserControlMyBooks()
        {
            InitializeComponent();
            UpdateTable();
        }

        public void UpdateTable()
        {
            new UserControlMBPresenter(this);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            new UserControlMBPresenter(this).returnBookInit();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
