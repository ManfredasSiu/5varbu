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
    class MainPresenter
    {

        IDataB ADB;
        IMain main;

        bool isCollapsed;
        int PanelWidth;

        public MainPresenter(IMain main)
        {
            ADB = RefClass.Instance.LogicC.DB;
            ADB.GetAllBooks();
            main.StartTmer();
            PanelWidth = main.panelWdt;
            isCollapsed = false;
            UserControlHome uch = new UserControlHome(ADB);
            main.AddControl(uch);
            main.Name = StaticData.CurrentUser.getuserName();
            if (StaticData.CurrentUser.getPermission() == "1")
                main.Status = "ADMIN";
            else
                main.Status = "Reader";
        }
    }
}
