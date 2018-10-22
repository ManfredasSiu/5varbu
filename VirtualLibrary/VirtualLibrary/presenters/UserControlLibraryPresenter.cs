using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    class UserControlLibraryPresenter
    {
        IUControlL IUCL;
        IDataB ADB;

        public UserControlLibraryPresenter(IUControlL IUCL)
        {
            this.IUCL = IUCL;
            this.ADB = RefClass.Instance.LogicC.DB;
            StaticData.Books = ADB.GetAllBooks();
            RefreshControl();
        }

        private void RefreshControl() //Lenteliu ir mygtuku atnaujinimas
        {
            UpdateButtons();
            UpdateTable();
        }

        private void UpdateButtons() //Mygtuku "perpiesimas"
        {
            if (StaticData.CurrentUser.getPermission() == "1")
            {
                IUCL.AddBookVisible = true;
                IUCL.RemoveBookVisible = true;
            }
            else
            {
                IUCL.AddBookVisible = false;
                IUCL.RemoveBookVisible = false;
            }
        }

        private void UpdateTable()  //Lenteles "perpiesimas"
        {
            IUCL.Table.Rows.Clear();
            foreach (Book item in StaticData.Books)
            {
                IUCL.Table.Rows.Add(item.getCode(), item.getName(), item.getAuthor(), 
                    item.getPressName(), item.getGenre(), item.getPages(), item.getQuantity());
            }
        }


        //Paemimo, pridejimo (ir sunaikinimo) langu inicijavimas
        public void TakeBookInit()
        {
            RefClass.Instance.InitScannerForm("Borrow");
        }

        public void AddBookInit()
        {
            RefClass.Instance.InitAddBookForm();
        }

        public void RemoveBookInit()
        {
            using (FormAdminRemoveBook farb = new FormAdminRemoveBook())
            {
                farb.ShowDialog();
                StaticData.CurrentUser.getPermission();
            }
        }
    }
}
