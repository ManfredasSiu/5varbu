using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary
{
    public partial class UserControlLibrary : UserControl
    {
        public UserControlLibrary()
        {
            InitializeComponent();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonTake_Click(object sender, EventArgs e)
        {
            using (FormUserQR fbb = new FormUserQR())
            {
                fbb.ShowDialog();
            }
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonAddBook_Click(object sender, EventArgs e)
        {
            using (FormAdminAddBook faab = new FormAdminAddBook())
            {
                faab.ShowDialog();
            }
        }

        private void buttonRemoveBook_Click(object sender, EventArgs e)
        {
            using (FormAdminRemoveBook farb = new FormAdminRemoveBook())
            {
                farb.ShowDialog();
            }
        }
    }
}
/*if(StaticData.CurrentUser.Permission == '1')
        {
            buttonAddBook.Visible;
            buttonRemoveBook.Visible;
        }
        else
        {
            !buttonAddBook.Visible;
            !buttonRemoveBook.Visible;
        }*/