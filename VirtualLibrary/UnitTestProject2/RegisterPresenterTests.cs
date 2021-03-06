﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualLibrary;
using VirtualLibrary.presenters;


namespace UnitTestProject2
{
    [TestClass]
    public class RegisterPresenterTests
    {
        [TestMethod]
        public void CheckHowManyFaces_FacesNotFound_Return1()
        {
            RegisterPresenter RP = new RegisterPresenter(new Form2());
            int result = RP.CheckHowManyFaces(FaceArrayLength: 0);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void CheckHowManyFaces_OneFace_Return0()
        {
            RegisterPresenter RP = new RegisterPresenter(new Form2());
            int result = RP.CheckHowManyFaces(FaceArrayLength: 1);
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void CheckHowManyFaces_MoreThanOneFace_Return2()
        {
            RegisterPresenter RP = new RegisterPresenter(new Form2());
            int result = RP.CheckHowManyFaces(FaceArrayLength: 2);
            Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void CheckTheTB_NoWarnings_Return0()
        {
            RegisterPresenter RP = new RegisterPresenter(new Form2());
            int result = RP.CheckTheTB(Nam: "name", pass: "Pass", DB: new AzureDatabase());
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void CheckTheTB_NameFieldIsEmpty_Return1()
        {
            RegisterPresenter RP = new RegisterPresenter(new Form2());
            int result = RP.CheckTheTB(Nam: "", pass: "password", DB: new AzureDatabase());
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void CheckTheTB_PasswordFieldIsEmpty_Return3()
        {
            RegisterPresenter RP = new RegisterPresenter(new Form2());
            int result = RP.CheckTheTB(Nam: "name", pass: "", DB: new AzureDatabase());
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void CheckTheTB_NameHasIlegalChar_Return4()
        {
            RegisterPresenter RP = new RegisterPresenter(new Form2());
            int result = RP.CheckTheTB(Nam: "%%%Name#@$", pass: "notEmp", DB: new AzureDatabase());
            Assert.AreEqual(result, 4);
        }
    }
}
