using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary.Views
{
    public interface IMain
    {
        Control NewControl { set; }

        Control ButtonHome { get; }

        Control ButtonMyBooks { get; }

        Control ButtonLibrary { get; }

        Control ButtonRecom { get; }

        string Date { set; }

        void ClearControlsFromPanel();

        string UserName { set; get; }

        string Status { set; }

        int panelLft { get; set; }

        int panelSideTop { set; }

        int panelSideHgh { set; }

        Timer Tmr1 { get; set; }

        Timer Tmr2 { get; }

        void refresh();
    }
}
