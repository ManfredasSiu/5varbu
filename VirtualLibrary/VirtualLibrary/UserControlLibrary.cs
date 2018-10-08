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
            if (StaticData.CurrentUser.getPermission()=="1")
            {
                buttonAddBook.Visible = true;
                buttonRemoveBook.Visible = true;
            }
            else
            {
                buttonAddBook.Visible = false;
                buttonRemoveBook.Visible = false;
            }
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
                StaticData.CurrentUser.getPermission();
            }
        }
    }
}