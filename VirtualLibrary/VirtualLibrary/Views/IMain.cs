using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary.Views
{
    interface IMain
    {
        void StartTmer();
        
        void AddControl(Control control);
        
        string Name { set; get; }

        string Status { set; }

        int panelWdt { get; set; }
    }
}
