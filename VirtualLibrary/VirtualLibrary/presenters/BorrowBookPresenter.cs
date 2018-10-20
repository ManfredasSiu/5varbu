using MessagingToolkit.Barcode;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    class BorrowBookPresenter
    {
        String procedure;

        IDataB ADB;
        IBorrow borrowView;

        VideoCapture capture;
        Mat frame;
        Bitmap img;

        public BorrowBookPresenter(String procedure, IBorrow borrowView)
        {
            this.procedure = procedure;
            this.borrowView = borrowView;
            this.ADB = RefClass.Instance.LogicC.DB;
            capture = new VideoCapture(0);
            capture.Open(0);                              //Kameros inicijavimas
            Application.Idle += FrameProcedure;
        }

        BarcodeDecoder Scanner;

        private void FrameProcedure(Object sender, EventArgs e)  //Live freimai
        {
            frame = new Mat();
            if (capture.IsOpened())
            {
                capture.Read(frame);
                img = BitmapConverter.ToBitmap(frame);
                if (borrowView.NewFrame != null)
                {
                    borrowView.NewFrame.Dispose();              //Ismetamas senas
                }
                borrowView.NewFrame = img;                      //Idedamas naujas
            }
        }

        private void ReturnLogic(Book book)                //Knygos atidavimo su barkodu logika
        {
            if (book != null && StaticData.CurrentUser.getUserBooks().Contains(book) && procedure == "Return")
            {
                book.setQuantityPlius();                   //Pridedamas knygos kiekis
                StaticData.CurrentUser.AddReadBook(book);  //Knyga pridedama prie perskaitytu knygu
                StaticData.CurrentUser.removeUserBook(book); //Knyga isimama is vartotojo skaitomu knygu
                ADB.ReturnBook(book);                        //Visa tai padaroma duomenu bazeje
                borrowView.CloseForm();
                RefClass.Instance.MBControl.UpdateTable();  //Atnaujinama vartotojo lentele
                Application.Idle -= FrameProcedure;
                return;
            }
            else
            {
                MessageBox.Show("Neturite tokios knygos\nArba knyga neegzistuoja bibliotekoje");
            }
        }

        private void AddBookLogic(string code)   //Knygos pridejimo logika
        {
            RefClass.Instance.IABook.BarcodeField = code; //Barkodas perkeliamas i reikiama textfielda
        }

        private void BorrowLogic(Book book)    //Knygos pasiskolinimo logika
        {
            if (book.getQuantity() > 0 && StaticData.CurrentUser.getUserBooks().Contains(book) == false)
            {
                book.setQuantityMinus();                   //Sumazinama knygos quantity
                StaticData.CurrentUser.AddTakenBook(book); //Pridedama paimta knyga
                ADB.BorrowBook(book);                      //Visa tia padaroma duombazeje
                RefClass.Instance.LControl.UpdateTable();  //atnaujinama lentele
                Application.Idle -= FrameProcedure;
                borrowView.CloseForm();
            }
            else
            {
                MessageBox.Show("Sios knygos egzemplioriu nebera\nArba jau tokia knyga turite");
            }
        }

        public void ExitScanner()
        {
            Application.Idle -= FrameProcedure;
            capture.Dispose();
            borrowView.CloseForm();
        }

        public void ScanBarcode()     //Pati barkodo skanavimo logika
        { 
            //Skenavimas
            Scanner = new BarcodeDecoder();
            try              //Tikrinama ar duotas freimas turi barkoda
            {
                Result result = Scanner.Decode(new Bitmap(img));
                var book = StaticData.Books.Find(x => x.getCode() == result.Text);
                //Atsizvelgiant i proceduros tipa iskvieciama reikiama skanavimo logika
                if (procedure == "Return")
                    ReturnLogic(book);
                else if (procedure == "Borrow")
                    BorrowLogic(book);
                else if (procedure == "Add")
                    AddBookLogic(borrowView.barcodeText);
            }
            catch       //Jei ne bandoma ieskoti barkodo textfielde
            {
                if (borrowView.barcodeText == "")
                    MessageBox.Show("Nuskanuoti nepavyko, iveskite barkoda ranka\nArba bandykite dar karta");
                else
                {
                    var book = StaticData.Books.Find(x => x.getCode() == borrowView.barcodeText);
                    //Atsizvelgiant i proceduros tipa iskvieciama reikiama skanavimo logika
                    if (procedure == "Return")
                        ReturnLogic(book);
                    else if (procedure == "Borrow")
                        BorrowLogic(book);
                    else if (procedure == "Add")
                        AddBookLogic(borrowView.barcodeText);
                }
            }
        }
    }
}
