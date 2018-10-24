using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualLibrary;
using VirtualLibrary.presenters;

namespace UnitTestProject2
{
    [TestClass]
    public class UserControlLibraryPresenterTests
    {
        [TestMethod]
        public void UpdateButtons_IfAdmin_ReturnsTrue()
        {
            UserControlLibraryPresenter UCLP = new UserControlLibraryPresenter(new UserControlLibrary());

            var result = UCLP.UpdateButtons(status: new User("", "") { permission = "1"});

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdateButtons_IfUser_ReturnsTrue()
        {
            UserControlLibraryPresenter UCLP = new UserControlLibraryPresenter(new UserControlLibrary());

            var result = UCLP.UpdateButtons(status: new User("", "") { permission = "0" });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdateButtons_IfnullOrBadInput_ReturnsTrue()
        {
            UserControlLibraryPresenter UCLP = new UserControlLibraryPresenter(new UserControlLibrary());

            var result = UCLP.UpdateButtons(status: new User("", "") { permission = null });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateTable_NoBooks_ReturnsFalse()
        {
            UserControlLibraryPresenter UCLP = new UserControlLibraryPresenter(new UserControlLibrary());

            List<Book> books = new List<Book>();
            var result = UCLP.UpdateTable(items: books);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateTable_SomeBooks_ReturnsTrue()
        {
            UserControlLibraryPresenter UCLP = new UserControlLibraryPresenter(new UserControlLibrary());

            List<Book> books = new List<Book>() { new Book() };
            var result = UCLP.UpdateTable(items: books);

            Assert.IsTrue(result);
        }

    }
}
