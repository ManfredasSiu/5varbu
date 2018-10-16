using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary.Views
{
    public interface IAddBook
    {
        void ShowAsDialog();

        String BarcodeField { set; }
    }
}
