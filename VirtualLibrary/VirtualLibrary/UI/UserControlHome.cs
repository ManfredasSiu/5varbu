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
    public partial class UserControlHome : UserControl, IUControlH
    {
        
        public string BooksRead { set => label8.Text = value; }

        public string PagesRead { set => label9.Text = value; }

        public UserControlHome()
        {
            InitializeComponent();
            var UCHP = new UserControlHomePresenter(this); 
        }

        private void UserControlHome_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
