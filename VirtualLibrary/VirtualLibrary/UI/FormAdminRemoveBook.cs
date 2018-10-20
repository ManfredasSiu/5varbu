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
    public partial class FormAdminRemoveBook : Form
    {
        public FormAdminRemoveBook()
        {
            InitializeComponent();
        }

        //Mygtuko paspaudimas turetu isimti knyga is MyBooks saraso ir Grazinti i library sarasa
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void FormReturnBook_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void buttonScanner_Click(object sender, EventArgs e)
        {
            //using (FormBorrowBooks fbb = new FormBorrowBooks())
            //{
            //    fbb.ShowDialog();
            //}
        }
    }
}
