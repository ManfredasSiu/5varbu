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
    public partial class MainWindow : Form
    {
        private LogicController logicC;
        public MainWindow(LogicController logicC)
        {
            InitializeComponent();
            this.logicC = logicC;
        }
    }
}
