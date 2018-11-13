using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary.Views
{
    public interface IUControlR
    {
        DataGridView DGW2 { get; }

        void UpdateTable();
    }
}
