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
        {//Transaction scope, kad nesugadinti duombazes
            this.scope = new TransactionScope();
        }

        [TestMethod]
        public void AddBook_ValidBookAdded_Return0()
        {
            IDataB ADB = new AzureDatabase();

            var result = ADB.AddBook(AddThis: new Book("1", "2", "3","7", 4, 5, "6", 0));

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void AddBook_InvalidBookAdded_Return1()
        {
            IDataB ADB = new AzureDatabase();

            var result = ADB.AddBook(AddThis: new Book(null, null, null, null, 4, 5, null, 0));

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void AddUser_ValidUserAdded_Return0()
        {
            IDataB ADB = new AzureDatabase();
            var result = ADB.AddUser(name: "1", Password: "2", email: "3", Permission: 0);
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

            var result = ADB.BorrowBook(addThis: Fakebook, user: Fakeuser);

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

            var result = ADB.BorrowBook(addThis: new Book { ID = -1, code = null }, user: Fakeuser);

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
            //---

            var result = ADB.ReturnBook(delThis: Fakebook, user: Fakeuser);

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void GetAllUserBooks_ValidInput_ReturnList()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
            ADB.AddUser("1", "2", "3", 0);
            var Fakeuser = ADB.GetUser("1"); 
            var Fakebook = new Book("1", "2", "3", "7", 4, 5, "6", 0) { code = "80085" };
            ADB.AddBook(Fakebook);
            ADB.BorrowBook(Fakebook, Fakeuser);
            //---

            var result = ADB.GetAllUserBooks(user: Fakeuser);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SearchUser_UserFound_Return2()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
            ADB.AddUser("1", "2", "3", 0);

            var result = ADB.SearchUser(name: "1");

            Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void SearchUser_UserNotFound_Return0()
        {
            IDataB ADB = new AzureDatabase();

            var result = ADB.SearchUser(name: "%%%");

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void GetUser_UserFound_ReturnUser()
        {
            IDataB ADB = new AzureDatabase();
            //Fake data
            ADB.AddUser("1", "2", "3", 0);
            //---

            var result = ADB.GetUser(name: "1");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUser_UserNotFound_ReturnNull()
        {
            IDataB ADB = new AzureDatabase();

            var result = ADB.GetUser(name: "%%%");

            Assert.IsNull(result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.scope.Dispose();
        }
    }
}
