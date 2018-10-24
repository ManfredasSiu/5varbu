using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualLibrary.presenters;
using VirtualLibrary;

namespace UnitTestProject2
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UserControlMBPresenterTests
    {

        [TestMethod]
        public void updateTable_SomeBooksGiven_ReturnsTrue()
        {
            var UCMBP = new UserControlMBPresenter(new UserControlMyBooks());

            List<Book> UB = new List<Book>() { new Book() };
            var result = UCMBP.updateTable(items: UB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void updateTable_NoBooksGiven_ReturnsTrue()
        {
            var UCMBP = new UserControlMBPresenter(new UserControlMyBooks());

            List<Book> UB = new List<Book>();
            var result = UCMBP.updateTable(items: UB);

            Assert.IsFalse(result);
        }
    }
}
