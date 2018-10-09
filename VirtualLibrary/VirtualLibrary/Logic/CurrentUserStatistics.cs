using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary
{
    class CurrentUserStatistics
    {
        //--------------------
        //Overall Statistics
        public int[] GenreBooksCount;
        public int PagesRead;
        public int BooksRead;
        public double PagesPerBook;
        public Book BiggestBook;
        //Horror Statistics
        public int PagesReadH;
        public int BooksReadH;
        //ComedyStatistics
        public int PagesReadCo;
        public int BooksReadCo;
        //CrimeStatistics
        public int PagesReadCr;
        public int BooksReadCr;
        //DocumentaryStatistics
        public int PagesReadD;
        public int BooksReadD;
        //ScienceStatistics
        public int PagesReadS;
        public int BooksReadS;
        //--------------------

        //Preferences
        public String PreferedGenre;
        public String PreferedPageCount;


        public CurrentUserStatistics()
        {
            SortBooks();
            SetThePreferences();
        }

        public void ReCalculate()
        {
            SortBooks();
            SetThePreferences();
        }

        private void SetThePreferences()
        {
            if (PagesPerBook > 500)
                PreferedPageCount = "Storos 500+";
            else if (PagesPerBook > 200)
                PreferedPageCount = "Vidutines 200+";
            else if (PagesPerBook > 0)
                PreferedPageCount = "Plonos tarp 0 - 200 psl";
            else
            {
                PreferedPageCount = "Unknown";
                PreferedGenre = "Unknown";
                return;
            }
            GenreBooksCount = new int[5] { BooksReadH, BooksReadCo, BooksReadCr, BooksReadD, BooksReadS };
            int tempStorage = 0;
            for (int x = 0;x < 5;x++)
            {
                if(GenreBooksCount[x] > tempStorage)
                {
                    if (x == 0)
                        PreferedGenre = "Horror";
                    else if (x == 1)
                        PreferedGenre = "Comedy";
                    else if (x == 2)
                        PreferedGenre = "Crime";
                    else if (x == 3)
                        PreferedGenre = "Documentary";
                    else if (x == 4)
                        PreferedGenre = "Science";
                }
            }
        }

        private void SortBooks()
        {
            foreach (Book bk in StaticData.CurrentUser.getBooksRead())
            {
                BooksRead++;
                PagesRead += bk.getPages();
                if (bk.getGenre() == "Horror")
                {
                    BooksReadH++;
                    PagesReadH += bk.getPages();
                }
                if (bk.getGenre() == "Comedy")
                {
                    BooksReadCr++;
                    PagesReadCr += bk.getPages();
                }
                if (bk.getGenre() == "Documentary")
                {
                    BooksReadD++;
                    PagesReadD += bk.getPages();
                }
                if (bk.getGenre() == "Science")
                {
                    BooksReadS++;
                    PagesReadS += bk.getPages();
                }
                if (bk.getGenre() == "Crime")
                {
                    BooksReadCr++;
                    PagesReadCr += bk.getPages();
                }
            }
            StaticData.CurrentUser.getBooksRead().Sort((x, y) => x.getPages().CompareTo(y.getPages()));
            if (StaticData.CurrentUser.getBooksRead().ToArray().Length > 0)
                BiggestBook = StaticData.CurrentUser.getBooksRead().ToArray()[0];
        }
    }
}
