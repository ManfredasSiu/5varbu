using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using VirtualLibrary;
using VirtualLibrary.API_s;

namespace UnitTestProject2
{
    /// <summary>
    /// Summary description for AzureDatabaseTests
    /// </summary>
    [TestClass]
    public class AzureDatabaseTests
    {
        private TransactionScope scope;

        [TestInitialize]
        public void Initialize()
        {
            this.scope = new TransactionScope();
        }

        [TestMethod]
        public void AddBook_ValidBookAdded_Return0()
        {
            IDataB ADB = new AzureDatabase();

            var result = ADB.AddBook(new Book("1", "2", "3","7", 4, 5, "6", 0));

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void AddBook_ValidBookAdded_Return1()
        {
            IDataB ADB = new AzureDatabase();

            var result = ADB.AddBook(new Book(null, null, null, null, 4, 5, null, 0));

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void AddUser_ValidUserAdded_Return0()
        {
            IDataB ADB = new AzureDatabase();
            var result = ADB.AddUser("1", "2", "3", 0);
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void AddUser_InvalidUserAdded_Return1()
        {
            IDataB ADB = new AzureDatabase();
            var result = ADB.AddUser(null, null, null, 0);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void BorrowBook_ValidBook_Return0()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
            ADB.AddUser("1", "2", "3", 0);
            var Fakeuser = ADB.GetUser("1");
            var Fakebook = new Book("1", "2", "3", "7", 4, 5, "6", 0) { code = "80085" };
            ADB.AddBook(Fakebook);
            //---

            var result = ADB.BorrowBook(Fakebook, Fakeuser);

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void BorrowBook_BookDoesntExist_Return1()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
            ADB.AddUser("1", "2", "3", 0);
            var Fakeuser = ADB.GetUser("1");
            //---

            var result = ADB.BorrowBook(new Book { ID = -1, code = null }, Fakeuser);

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void ReturnBook_ValidBook_Return0()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
            ADB.AddUser("1", "2", "3", 0);
            var Fakeuser = ADB.GetUser("1");
            var Fakebook = new Book("1", "2", "3", "7", 4, 5, "6", 0) { code = "80085" };
            ADB.AddBook(Fakebook);
            ADB.BorrowBook(Fakebook, Fakeuser);
            var result = ADB.ReturnBook(Fakebook, Fakeuser);
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void ReturnBook_CanNotReturnBook_Return1()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
            ADB.AddUser("1", "2", "3", 0);
            var Fakeuser = ADB.GetUser("1");
            var Fakeuser1 = ADB.GetUser("2");
            var Fakebook = new Book("1", "2", "3", "7", 4, 5, "6", 0) { code = "80085" };
            ADB.AddBook(Fakebook);
            ADB.BorrowBook(Fakebook, Fakeuser);
            var result = ADB.ReturnBook(Fakebook, Fakeuser1);
            Assert.AreEqual(result, 1);
         }

        [TestMethod]
        public void GetAllUserBooks_Books_ReturnList()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
            ADB.AddUser("1", "2", "3", 0);
            var Fakeuser = ADB.GetUser("1"); 
            var Fakebook = new Book("1", "2", "3", "7", 4, 5, "6", 0) { code = "80085" };
            ADB.AddBook(Fakebook);
            ADB.BorrowBook(Fakebook, Fakeuser);
            var result = ADB.GetAllUserBooks(Fakeuser);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllUserBooks_Books_DoNotReturnList()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
          
            var result = ADB.GetAllUserBooks(new User {ID =-1});
            Assert.IsNull(result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.scope.Dispose();
        }
    }
}
