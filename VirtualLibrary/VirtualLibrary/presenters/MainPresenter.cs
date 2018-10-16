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
            this.main = main;
            PanelWidth = main.panelLft;
            isCollapsed = false;
            UserControlHome uch = new UserControlHome();
            main.NewControl = uch;
            main.UserName = StaticData.CurrentUser.getuserName();
            if (StaticData.CurrentUser.getPermission() == "1")
                main.Status = "ADMIN";
            else
                main.Status = "Reader";
        }

        public void timer2Ticks()
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            main.Date = dateTime.ToString("yyyy-MM-dd");
        }

        public void timer1Ticks()
        {
            if (isCollapsed)
            {
                main.panelLft = main.panelLft + 10;
                if (main.panelLft >= PanelWidth)
                {
                    main.Tmr1.Stop();
                    isCollapsed = false;
                    main.refresh();
                }
            }
            else
            {
                main.panelLft = main.panelLft - 10;
                if (main.panelLft <= 85)
                {
                    main.Tmr1.Stop();
                    isCollapsed = true;
                    main.refresh();
                }
            }
        }

        public void RButtonBehaviour(Control btn)
        {
            moveSidePanel(btn);
            UserControlRecom ucr = new UserControlRecom();
            AddControlsToPanel(ucr);
        }

        public void LButtonBehaviour(Control btn)
        {
            moveSidePanel(btn);
            UserControlLibrary ucl = new UserControlLibrary();
            AddControlsToPanel(ucl);
        }

        public void HButtonBehaviour(Control btn)
        {
            moveSidePanel(btn);
            AddControlsToPanel((UserControl)RefClass.Instance.InitHomeControl());
        }

        public void MBButtonBehaviour(Control btn)
        {
            moveSidePanel(btn);
            UserControlMyBooks ucmb = new UserControlMyBooks();
            AddControlsToPanel(ucmb);
        }

        private void moveSidePanel(Control btn)
        {
            main.panelSideTop = btn.Top;
            main.panelSideHgh = btn.Height;
        }

        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            main.ClearControlsFromPanel();
            main.NewControl = c;
        }
    }
}
