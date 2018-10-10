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

namespace VirtualLibrary
{
    public partial class UserControlMyBooks : UserControl
    {

        private IDataB ADB;

        public UserControlMyBooks(IDataB ADB)
        {
            this.ADB = ADB;
            InitializeComponent();
            ADB.GetAllUserBooks();
            updateTable();
        }

        public void updateTable()
        {
            dataGridView1.Rows.Clear();
            foreach(Book item in StaticData.CurrentUser.getUserBooks())
            {
                dataGridView1.Rows.Add(item.getCode(), item.getName(), item.getAuthor(), item.getPressName(), null, null);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            returnBookInit();
        }

        public void returnBookInit()
        {
            using (FormBorrowBooks fuqr = new FormBorrowBooks(this, ADB))
            {
                fuqr.ShowDialog();
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
