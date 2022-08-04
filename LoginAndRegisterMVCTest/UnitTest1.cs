using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LoginandRegisterMVC.Controllers;
using System.Web;
using LoginandRegisterMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;

using System.Web.Mvc;
//using System.Data.Entity;
namespace LoginAndRegisterMVCTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    public class Tests
    {
        [TestMethod]
        [DeploymentItem("app.config")]
        public void TestUserLogin()
        {
            var obj = new UsersController();

            var actResult = obj.Login() as ViewResult;

            //Assert.AreEqual<>(actResult.ViewName,"Index");
            Assert.IsNotNull(actResult);
        }

        [TestMethod]
        [DeploymentItem("app.config")]
        public void TestUserSignup()
        {
            var obj = new UsersController();

            var actResult = obj.Register() as ViewResult;

            //Assert.AreEqual<>(actResult.ViewName,"Index");
            Assert.IsNotNull(actResult);
        }
        [TestMethod]
        [DeploymentItem("app.config")]
        public void TestAdminIndex()
        {
            var obj = new ElectionsController();

            var actResult = obj.AdminHome() as ViewResult;

            //Assert.AreEqual<>(actResult.ViewName,"Index");
            Assert.IsNotNull(actResult);
        }
        [TestMethod]
        [DeploymentItem("app.config")]
        public void TestContact()
        {
            var obj = new UsersController();

            var actResult = obj.Contact() as ViewResult;

            //Assert.AreEqual<>(actResult.ViewName,"Index");
            Assert.IsNotNull(actResult);
        }

        [TestMethod]
        [DeploymentItem("Web.config")]
        public void GetConnStringFromAppConfig()
        {
            //Dbconfig da = new Dbconfig();
            string actualString = "data source=.;initial catalog=testuserdbfk;integrated security=true;";
            string expectedString = System.Configuration.ConfigurationManager.ConnectionStrings["Dbconfig"].ConnectionString;
            Assert.AreEqual(expectedString, actualString);
        }
       

    }
}