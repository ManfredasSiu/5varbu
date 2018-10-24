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
    public class MainPresenter
    {
        IDataB ADB;
        public IMain main { get; set; }

        bool isCollapsed;
        int PanelWidth;

        public MainPresenter(IMain main)
        {
            try
            {
                this.main = main;
                ADB = RefClass.Instance.LogicC.DB;
                VoiceRecognition VR = RefClass.Instance.InitVoiceRecMain(this);
                VR.SetBlockFlagTrue();
                try
                {
                    LoadData(ADB);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Application.Exit();
                }
                try
                {
                    main.Tmr2.Start();
                }
                catch
                { throw; }
                PanelWidth = main.panelLft;
                isCollapsed = false;

                AddControlsToPanel((UserControl)RefClass.Instance.InitHomeControl()); //Inicijuojama Home user control

                //Atspausdinama reikiama info apie useri

                if (LoadUIPermission(StaticData.CurrentUser) == 0)
                {
                    MessageBox.Show("Nepavyko uzkrauti duomenu, paleiskite programa is naujo");
                    Application.Exit();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public int LoadUIPermission(User user)
        {
            if (user.getPermission() == "1")
            {
                main.Status = "ADMIN";
                return 2;
            }
            else if(user.getPermission() == "0")
            {
                main.Status = "Reader";
                return 1;
            }
            else { return 0; }
        }

        public int LoadData(IDataB DB)
        {
            try
            {
                //Uzkraunamos knygos is duombazes
                StaticData.Books = DB.GetAllBooks();
                //Uzkraunami userio duomenys
                StaticData.CurrentUser.setBooksRead(DB.GetAllBooksRead(StaticData.CurrentUser));
                StaticData.CurrentUser.setUserBooks(DB.GetAllUserBooks(StaticData.CurrentUser));
                main.UserName = StaticData.CurrentUser.getuserName();
                return 0;
            }
            catch
            {
                throw;
            }
        }

        //Data(?)

        public void timer2Ticks()
        {
            DateTime dateTime = DateTime.Now;
            main.Date = dateTime.ToString();
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

        public void RButtonBehaviour()
        {
            moveSidePanel(main.ButtonRecom);
            AddControlsToPanel((UserControl)RefClass.Instance.InitRecomControl());
        }
      
        public void LButtonBehaviour()
        {
            moveSidePanel(main.ButtonLibrary);
            AddControlsToPanel((UserControl)RefClass.Instance.InitLibControl());
        }

        public void HButtonBehaviour()
        {
            moveSidePanel(main.ButtonHome);
            AddControlsToPanel((UserControl)RefClass.Instance.InitHomeControl());
        }

        public void MBButtonBehaviour()
        {
            moveSidePanel(main.ButtonMyBooks);
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
