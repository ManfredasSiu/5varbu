using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    public class UserControlLibraryPresenter
    {
        IUControlL IUCL;
        IDataB ADB;

        public UserControlLibraryPresenter(IUControlL IUCL)
        {
            try
            {
                this.IUCL = IUCL;
                if (RefClass.Instance.VR != null)
                    RefClass.Instance.VR.SetBlockFlagTrue();
                this.ADB = RefClass.Instance.LogicC.DB;
                StaticData.Books = ADB.GetAllBooks();
                RefreshControl();
            }
            catch
            {
                return;
            }
            StaticData.Books = ADB.GetAllBooks();
        }

        private void RefreshControl() //Lenteliu ir mygtuku atnaujinimas
        {
            if(!UpdateButtons(StaticData.CurrentUser))
                Application.Exit();
            UpdateTable(StaticData.Books);
        }

        public bool UpdateButtons(User status) //Mygtuku "perpiesimas"
        {
            if (status.getPermission() == "1")
            {
                IUCL.AddBookVisible = true;
                IUCL.RemoveBookVisible = true;
                return true;
            }
            else if (status.getPermission() == "0")
            {

                IUCL.removeBtn.Dispose();
                IUCL.addBtn.Dispose();
                return true;
            }
            else
                return false;
        }

        public bool UpdateTable(List<Book> items)  //Lenteles "perpiesimas"
        {
            IUCL.Table.Rows.Clear();
            if (items.ToArray().Length > 0)
            {
                foreach (Book item in items)
                {
                    IUCL.Table.Rows.Add(item.getCode(), item.getName(), item.getAuthor(),
                        item.getPressName(), item.getGenre(), item.getPages(), item.getQuantity());
                }
                return true;
            }
            else
                return false;
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
