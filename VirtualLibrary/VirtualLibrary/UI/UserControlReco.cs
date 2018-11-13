using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.presenters;
using VirtualLibrary.Views;

namespace VirtualLibrary.UI
{
    public partial class UserControlReco : UserControl, IUControlR
    {
        public DataGridView DGW2 { get => dataGridView2; }
        public UserControlReco()
        {
            InitializeComponent();
            UpdateTable();
        }
        public void UpdateTable()
        {
            new UserControlRecoPresenter(this); // reiks pridet this, kai parasysiu presenteri
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //kiti metodai, jei reikia
    }
}
