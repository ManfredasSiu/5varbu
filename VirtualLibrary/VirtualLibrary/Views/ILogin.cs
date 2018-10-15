using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary.Views
{
    public interface ILogin
    {
        void CloseForm();

        void ShowForm();

        Image<Bgr, byte> image { set; get; }

    }
}
