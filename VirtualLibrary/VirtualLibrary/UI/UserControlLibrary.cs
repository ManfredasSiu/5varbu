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
    public partial class UserControlLibrary : UserControl
    {
        private IDataB ADB; 

        public UserControlLibrary( IDataB ADB)
        {
            InitializeComponent();
            this.ADB = ADB;

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
            UpdateTable();
        }

        public void UpdateTable()
        {
            dataGridView2.Rows.Clear();
            foreach (Book item in StaticData.Books)
            {
                dataGridView2.Rows.Add(item.getCode(), item.getName(), item.getAuthor(), item.getPressName(), item.getGenre(), item.getPages(), item.getQuantity());
            }
        }

        private void buttonTake_Click(object sender, EventArgs e)
        {
            TakeBookInit();
        }

        private void TakeBookInit()
        {
            using (FormBorrowBooks fbb = new FormBorrowBooks(this, ADB))
            {
                fbb.ShowDialog();
            }
        }
        

        private void buttonAddBook_Click(object sender, EventArgs e)
        {
            AddBookInit();
        }

        private void AddBookInit()
        {
            using (FormAdminAddBook faab = new FormAdminAddBook(this, ADB))
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