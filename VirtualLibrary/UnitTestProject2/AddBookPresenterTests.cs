using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualLibrary;
using VirtualLibrary.presenters;

namespace UnitTestProject2
{
    [TestClass]
    public class AddBookPresenterTests
    {
        [TestMethod]
        public void CheckTBs_OneOrMoreFieldAreEmpty_Return1()
        {
            var ABP = new AddBookPresenter(new FormAdminAddBook());

            var result = ABP.CheckTBs(NameField: "1", AuthorField: "2", PressField: "3", Pagesfield: "4", GenreField: "5", QuantityField: "6", BarcodeField: "");

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void CheckTBs_EveryFieldAreValid_Return0()
        {
            var ABP = new AddBookPresenter(new FormAdminAddBook());

            var result = ABP.CheckTBs(NameField: "1", AuthorField: "2", PressField: "3", Pagesfield: "4", GenreField: "5", QuantityField: "6", BarcodeField: "7");

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void CheckTBs_PagesOrQuantityFieldsNotParsable_Return2()
        {
            var ABP = new AddBookPresenter(new FormAdminAddBook());

            var result = ABP.CheckTBs(NameField: "1", AuthorField: "2", PressField: "3", Pagesfield: "NoParse", GenreField: "5", QuantityField: "NoParse", BarcodeField: "6");

            Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void CheckTBs_OneOrMoreFieldsHasSpecChars_Return3()
        {
            var ABP = new AddBookPresenter(new FormAdminAddBook());

            var result = ABP.CheckTBs(NameField: "1", AuthorField: "2", PressField: "%%%3", Pagesfield: "4", GenreField: "5", QuantityField: "6", BarcodeField: "5");

            Assert.AreEqual(result, 3);
        }
    }
}
