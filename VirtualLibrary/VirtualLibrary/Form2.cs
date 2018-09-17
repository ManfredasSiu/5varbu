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
    public partial class Form2 : Form    //Register Langas, padarysiu irgi su face recognition
    {
        private LogicController LogicC;
        public Form2(LogicController LogicC)
        {
            InitializeComponent();
            this.LogicC = LogicC;
        }
    }
}
