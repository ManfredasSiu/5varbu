using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary.Views
{
    public interface IUControlL
    {
        bool AddBookVisible { set; }

        bool RemoveBookVisible { set; }

        DataGridView Table { get; }
    }
}
