using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary.Views
{
    public interface IRegister
    {
        void CloseForm();
        void ShowForm();

        void InitMessageBox(String Message);
        Image<Bgr, Byte> Frame { get; set; }

        String NameText { get; }
        String password { get; }

        Color BGColor { set; }

        String InformationText { set; }

        void MaximizeForm();
        void HideInputPanel();

        Point ImgBoxLoc { set; get; }
        Size ImgBoxSize { set; }

        void AddControll(Control control);

        int Wdt { get; }
        int Hgt { get; }
    }
}
