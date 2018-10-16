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
    public partial class UserControlLibrary : UserControl, IUControlL
    {
        public bool AddBookVisible { set=> buttonAddBook.Visible = value; }

        public bool RemoveBookVisible { set=> buttonRemoveBook.Visible = value; }

        public DataGridView Table => dataGridView2;

        public UserControlLibrary()
        {
            InitializeComponent();
            new UserControlLibraryPresenter(this);
        }

        private void buttonTake_Click(object sender, EventArgs e)
        {
            new UserControlLibraryPresenter(this).TakeBookInit();
        }

        private void buttonAddBook_Click(object sender, EventArgs e)
        {
            new UserControlLibraryPresenter(this).AddBookInit();
        }
        
        private void buttonRemoveBook_Click(object sender, EventArgs e)
        {
            new UserControlLibraryPresenter(this).RemoveBookInit();
        }
    }
}