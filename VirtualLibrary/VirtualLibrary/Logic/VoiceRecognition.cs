using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Windows.Forms;
using VirtualLibrary.presenters;

namespace VirtualLibrary
{
    class VoiceRecognition // neveikia kolkas
    {

        //enum flagai
        [Flags]
        public enum SecureBlocks
        {
            none = 0,
            BLOCK = 2,
            ALLOW = 4
        }

        public SecureBlocks SB;

        
        SpeechRecognitionEngine SRecEng = new SpeechRecognitionEngine();
        MenuPresenter MP;

        // Flagu nustatymas is isoriniu klasiu

        public void SetBlockFlagTrue()
        {
            SB |= SecureBlocks.BLOCK;
        }

        public void SetBlockFlagFalse()
        {
            SB &= ~SecureBlocks.BLOCK;
        }

        public void SetALLOWFlagTrue()
        {
            SB |= SecureBlocks.ALLOW;
        }

        public void SetALLOWFlagFalse()
        {
            SB &= ~SecureBlocks.ALLOW;
        }

        public void SetALLOWFlagSwitch()
        {
            SB ^= SecureBlocks.ALLOW;
        }
        //--------

        //-----For Menu Presenter
        public VoiceRecognition(MenuPresenter MP) //Konstruktorius skirtas isskirtinai menu langui
        {
            this.MP = MP;
            Choices commands = new Choices();
            commands.Add(new string[] { "Register", "Connect", "Exit" }); // Comandos register, Connect, Exit
            UniversalInit(commands);
            SRecEng.SpeechRecognized += MenuVoiceRec; //Iskvieciam metoda MenuVoiceRec() kai balsas atpazistamas

        }

        private void MenuVoiceRec(Object sender, SpeechRecognizedEventArgs e)
        {
            if ((SB & SecureBlocks.BLOCK) != 0)
            {
                //Patikrinamos kokios komandos ir iskvieciami reikiami metodai is presenteriu

                if (e.Result.Text.Equals("Register")) 
                {
                    MP.RegisterButtonPressed();
                }
                if (e.Result.Text.Equals("Connect"))
                {
                    MP.LoginButtonPressed();
                }
                if (e.Result.Text.Equals("Exit"))
                {
                    Application.Exit();
                }
            }
        }
        //-----

        //-----For Main
        MainPresenter Main;
        public VoiceRecognition(MainPresenter Main)  // Konstruktorius skirtas main langui
        {
            this.Main = Main;
            Choices commands = new Choices();
            // komandos Home, My Books, Library, Recommended (,Exit<Deprecated>)
            commands.Add(new string[] { "Home", "My Books", "Library", "Recommended" , "Exit" });
            UniversalInit(commands);
            SRecEng.SpeechRecognized += MainVoiceRec; //Atpazinus balsa iskvieciama MainVoiceRec()
        }

        private void MainVoiceRec(Object sender, SpeechRecognizedEventArgs e) //Security blokai ir logika skirta atpazintom komandom
        {
            if ((SB & SecureBlocks.BLOCK) != 0 && (SB & SecureBlocks.ALLOW) != 0)
            {
                if (e.Result.Text.Equals("Home"))
                {
                    Main.HButtonBehaviour();
                }
                else if (e.Result.Text.Equals("My Books"))
                {
                    Main.MBButtonBehaviour();
                }
                else if (e.Result.Text.Equals("Library"))
                {
                    Main.LButtonBehaviour();
                }
                else if (e.Result.Text.Equals("Recommended"))
                {
                    Main.RButtonBehaviour();
                }
               /* else if (e.Result.Text.Equals("Exit"))
                {
                    Application.Exit();
                }*/
            }
        }

        //-----

        //-----Universal
        private void UniversalInit(Choices commands) //Universalus "konstruktorius" iskvieciamas is visu public konstruktoriu,
            //kadangi si informacija kartojas
        {
            SRecEng = new SpeechRecognitionEngine();                 //Sukuriamas engine

            GrammarBuilder gBuild = new GrammarBuilder();            //Gramatikos builderis, panasu i string builder

            SB = SecureBlocks.BLOCK;                                 //Inicijuojamas pirmasis flagas 

            gBuild.Culture = new System.Globalization.CultureInfo("en-GB"); //Pridedama kultura pagal kuria atpazinsime zodzius
            gBuild.Append(commands);                                 //Pridedame komandas

            Console.WriteLine(SpeechRecognitionEngine.InstalledRecognizers()); //Msg developeriui patikrinti ar yra recognizeriu

            Grammar gram = new Grammar(gBuild);                      //Sukuriama "gramatika"

            SRecEng.LoadGrammarAsync(gram);                          //Gramatika uskraunama i engine
            try
            {
                SRecEng.SetInputToDefaultAudioDevice();                  //Paimama numatytoji vaizdo irasymo priemone rasta kompiuteryje
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            SRecEng.RecognizeAsync(RecognizeMode.Multiple);             //Atpazinimo tipas (multiple commands arba single commands)

            Console.WriteLine("Info::::" + SRecEng.RecognizerInfo);
        }
        //-----
    }
}
