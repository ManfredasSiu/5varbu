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
    public partial class UserControlHome : UserControl
    {
        private IDataB ADB;
        CurrentUserStatistics CUS; 


        public UserControlHome(IDataB ADB)
        {
            this.ADB = ADB;
            InitializeComponent();
            ADB.GetAllBooksRead();
            CUS = new CurrentUserStatistics();
            label8.Text = CUS.BooksRead.ToString();
            label9.Text = CUS.PagesRead.ToString();
        }

        private void UserControlHome_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
