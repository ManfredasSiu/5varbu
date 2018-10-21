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
            ADB = RefClass.Instance.LogicC.DB;       //Uzkraunamos knygos is duombazes
            ADB.GetAllBooks();

            ADB.GetAllUserBooks();

            this.main = main;

            PanelWidth = main.panelLft;
            isCollapsed = false;

            AddControlsToPanel((UserControl)RefClass.Instance.InitHomeControl()); //Inicijuojama Home user control

            //Atspausdinama reikiama info apie useri
            main.UserName = StaticData.CurrentUser.getuserName();  
            if (StaticData.CurrentUser.getPermission() == "1")
                main.Status = "ADMIN";
            else
                main.Status = "Reader";
        }

        //Data(?)

        public void timer2Ticks()
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            main.Date = dateTime.ToString("yyyy-MM-dd");
        }

        //Soninio sliderio logika

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

        //Logika skirtingiems user controlams prideti i pagrindini langa ***

        public void RButtonBehaviour(Control btn)
        {
            moveSidePanel(btn);
            AddControlsToPanel((UserControl)RefClass.Instance.InitRecomControl());
        }

        public void LButtonBehaviour(Control btn)
        {
            moveSidePanel(btn);
            AddControlsToPanel((UserControl)RefClass.Instance.InitLibControl());
        }

        public void HButtonBehaviour(Control btn)
        {
            moveSidePanel(btn);
            AddControlsToPanel((UserControl)RefClass.Instance.InitHomeControl());
        }

        public void MBButtonBehaviour(Control btn)
        {
            moveSidePanel(btn);
            AddControlsToPanel((UserControl)RefClass.Instance.InitMBControl());
        }

        private void moveSidePanel(Control btn)// Perkeliama sonine panele prie reikiamo mygtuko
        {
            main.panelSideTop = btn.Top;
            main.panelSideHgh = btn.Height;
        }

        private void AddControlsToPanel(Control c) //UserControl pridedama i centrine panele
        {
            c.Dock = DockStyle.Fill;
            main.ClearControlsFromPanel();
            main.NewControl = c;
        }

        //***
    }
}
