using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    public class UserControlMBPresenter
    {
        IDataB ADB;
        IUControlMB IUCMB;

        public UserControlMBPresenter(IUControlMB IUCMB)
        {
            try
            {
                this.IUCMB = IUCMB;
                this.ADB = RefClass.Instance.LogicC.DB;
                if (RefClass.Instance.VR != null)
                    RefClass.Instance.VR.SetBlockFlagTrue();
                StaticData.CurrentUser.setUserBooks(ADB.GetAllUserBooks(StaticData.CurrentUser));             //Gaunavos visos user knygos
                updateTable(StaticData.CurrentUser.getUserBooks());                     //Perpiesiama lentele
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public bool updateTable(List<Book> items)//Perpiesiama lentele
        {
            IUCMB.DGW.Rows.Clear();
            if (items.ToArray().Length > 0)
            {
                foreach (Book item in items)
                    IUCMB.DGW.Rows.Add(item.getCode(), item.getName(), item.getAuthor(), item.getPressName(), null, null);
                return true;
            }
            else return false;
        }

        //Knygos atidavimo logika
        public void returnBookInit()
        {
            RefClass.Instance.InitScannerForm("Return");
        }
    }
}
