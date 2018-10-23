﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void AddUser_ValidUserAdded_Return1()
        {
            IDataB ADB = new AzureDatabase();
            var result = ADB.AddUser(null, null, null, 0);
            Assert.AreEqual(result, 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.scope.Dispose();
        }
    }
}