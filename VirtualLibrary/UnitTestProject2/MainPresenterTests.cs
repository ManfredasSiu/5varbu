using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualLibrary;
using VirtualLibrary.presenters;

namespace UnitTestProject2
{
    [TestClass]
    public class MainPresenterTests
    {
        [TestMethod]
        public void CheckTBs_UserIsAdmin_Return2()
        {
            var MP = new MainPresenter(new MainTry());

            var result = MP.LoadUIPermission(user: new User("", "") { permission = "1" });
            Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void CheckTBs_UserIsReader_Return1()
        {
            var MP = new MainPresenter(new MainTry());

            var result = MP.LoadUIPermission(user: new User("", "") { permission = "0" });

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void CheckTBs_UserNoPermission_Return0()
        {
            var MP = new MainPresenter(new MainTry());

            var result = MP.LoadUIPermission(user: new User("", ""));

            Assert.AreEqual(result, 0);
        }
    }
}
