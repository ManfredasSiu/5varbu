using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    class UserControlMBPresenter
    {
        IDataB ADB;
        IUControlMB IUCMB;

        public UserControlMBPresenter(IUControlMB IUCMB)
        {
            this.IUCMB = IUCMB;
            this.ADB = RefClass.Instance.LogicC.DB;
            ADB.GetAllUserBooks();
            updateTable();
        }

        public void updateTable()
        {
            IUCMB.DGW.Rows.Clear();
            foreach (Book item in StaticData.CurrentUser.getUserBooks())
                IUCMB.DGW.Rows.Add(item.getCode(), item.getName(), item.getAuthor(), item.getPressName(), null, null);
        }

        public void returnBookInit()
        {
            RefClass.Instance.InitBorrowForm("Return");
        }
    }
}
