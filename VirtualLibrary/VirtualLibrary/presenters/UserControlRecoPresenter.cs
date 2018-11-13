using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.API_s;
using VirtualLibrary.Views;

namespace VirtualLibrary.presenters
{
    public class UserControlRecoPresenter
    {
        IDataB ADB;
        IUControlR UCR;
        private List<Book> allBooks = new List<Book>();
        private List<Book> recoBooks = new List<Book>();
        private List<Book> randomOnes = new List<Book>();
        private String genre;
        CurrentUserStatistics Stats;

        public UserControlRecoPresenter(IUControlR UCR)
        {
            this.UCR = UCR;
            this.ADB = RefClass.Instance.LogicC.DB;
            if (RefClass.Instance.VR != null)
                RefClass.Instance.VR.SetBlockFlagTrue();
            getRecommendedBooks();
            getRandom();
            UpdateTable(randomOnes);
        }

        public void getRecommendedBooks()
        {
            Stats = RefClass.Instance.InitStatistics();
            genre = Stats.PreferedGenre;
            //reik patikrint ar neskaite ir ar neskaito
            foreach (Book book in ADB.GetAllBooks())
                if (book.getGenre() == genre)
                    recoBooks.Add(new Book(book.getName(), book.getAuthor(), book.getCode(), book.getGenre(), book.getQuantity(), book.getPages(), book.getPressName(), book.ID)); 
            foreach (Book book in recoBooks)
            {
                 var temp1 = StaticData.CurrentUser.getBooksRead().Find(x => x.ID == book.ID);
                 var temp2 = StaticData.CurrentUser.getUserBooks().Find(x => x.ID == book.ID);
                 if (temp1 == null && temp2 == null)
                 {
                     allBooks.Add(book);
                     temp1 = null;
                     temp2 = null;
                 }
            }
        }

        public void getRandom()
        {
            int index;
            Random r = new Random();
            for (int y = 0; y < 3; y++)
            {
                index = r.Next(allBooks.Count);

                if (randomOnes.Find(x => x.ID == allBooks[index].ID) == null)
                    randomOnes.Add(allBooks[index]);
                else
                    y--;
            }
        }

        public void UpdateTable(List<Book> list)
        {
            UCR.DGW2.Rows.Clear();
            if (list.ToArray().Length > 0)
            {
                foreach (Book item in list)
                    UCR.DGW2.Rows.Add(item.getName(), item.getAuthor(), item.getPages(), item.getGenre());
            }
        }
         
    }
}