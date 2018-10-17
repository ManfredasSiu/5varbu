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
            RefreshControl();
        }

        private void RefreshControl()
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
            UpdateTable();
        }

        private void UpdateTable()
        {
            IUCL.Table.Rows.Clear();
            foreach (Book item in StaticData.Books)
            {
                IUCL.Table.Rows.Add(item.getCode(), item.getName(), item.getAuthor(), item.getPressName(), item.getGenre(), item.getPages(), item.getQuantity());
            }
        }

        public void TakeBookInit()
        {
            RefClass.Instance.InitBorrowForm("Borrow");
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
