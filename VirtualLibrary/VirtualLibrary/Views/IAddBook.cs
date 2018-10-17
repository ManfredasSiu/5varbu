using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary.Views
{
    public interface IAddBook
    {

        void CloseForm();

        string BarcodeField { set; get; }

        string NameField { get; }

        string PressField { get; }

        string Pagesfield { get; }

        string QuantityField { get; }

        string GenreField { get; }

        string AuthorField { get; }
    }
}
