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
            capture.Open(0);
            Application.Idle += FrameProcedure;
        }

        BarcodeDecoder Scanner;

        private void FrameProcedure(Object sender, EventArgs e)
        {
            frame = new Mat();
            if (capture.IsOpened())
            {
                capture.Read(frame);
                img = BitmapConverter.ToBitmap(frame);
                if (borrowView.NewFrame != null)
                {
                    borrowView.NewFrame.Dispose();
                }
                borrowView.NewFrame = img;
            }
        }

        private void ReturnLogic(Book book)
        {
            if (book != null && StaticData.CurrentUser.getUserBooks().Contains(book) && procedure == "Return")
            {
                book.setQuantityPlius();
                StaticData.CurrentUser.AddReadBook(book);
                StaticData.CurrentUser.removeUserBook(book);
                ADB.ReturnBook(book);
                borrowView.CloseForm();
                RefClass.Instance.MBControl.UpdateTable();
                Application.Idle -= FrameProcedure;
                return;
            }
            else
            {
                MessageBox.Show("Neturite tokios knygos\nArba knyga neegzistuoja bibliotekoje");
            }
        }

        private void AddBookLogic(string code)
        {
            RefClass.Instance.IABook.BarcodeField = code;
        }

        private void BorrowLogic(Book book)
        {
            if (book.getQuantity() > 0 && StaticData.CurrentUser.getUserBooks().Contains(book) == false)
            {
                book.setQuantityMinus();
                StaticData.CurrentUser.AddTakenBook(book);
                ADB.BorrowBook(book);
                RefClass.Instance.LControl.UpdateTable();
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

        public void ScanBarcode()
        {
            //Skenavimas
            Scanner = new BarcodeDecoder();
            try
            {
                Result result = Scanner.Decode(new Bitmap(img));
                var book = StaticData.Books.Find(x => x.getCode() == result.Text);
                if (procedure == "Return")
                    ReturnLogic(book);
                else if (procedure == "Borrow")
                    BorrowLogic(book);
                else if (procedure == "Add")
                    AddBookLogic(borrowView.barcodeText);
            }
            catch (Exception e)
            {
                if (borrowView.barcodeText == "")
                    MessageBox.Show("Nuskanuoti nepavyko, iveskite barkoda ranka\nArba bandykite dar karta");
                else
                {
                    var book = StaticData.Books.Find(x => x.getCode() == borrowView.barcodeText);
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
