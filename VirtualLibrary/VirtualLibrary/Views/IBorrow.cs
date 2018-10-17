using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary.Views
{
    public interface IBorrow
    {
        void CloseForm();

        Image NewFrame { set; get; }

        string barcodeText { set; get; }
    }
}
