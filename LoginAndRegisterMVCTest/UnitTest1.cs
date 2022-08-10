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
using System.Data.Entity;

namespace LoginAndRegisterMVCTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    public class Tests
    {
        public void FixEfProviderServicesProblem()
        {
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
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

        [TestMethod]
        [DeploymentItem("Web.config")]
        public void TestElections()
        {
            int n = -1000;
            UserContext db = new UserContext();
            var obj = db.Elections.Where(x => x.ElectionId == n).FirstOrDefault();
            Assert.AreEqual(obj, null);

        }

        [TestMethod]
        [DeploymentItem("Web.config")]
        public void TestCandidates()
        {
            int n = -1000;
            UserContext db = new UserContext();
            var obj = db.Candidates.Where(x => x.CandidateId == n).FirstOrDefault();
            Assert.AreEqual(obj, null);

        }


        [TestMethod]
        [DeploymentItem("Web.config")]
        public void TestUsers()
        {
            int n = -1000;
            UserContext db = new UserContext();
            var obj = db.Users.Where(x => x.EmployeeId == n).FirstOrDefault();
            Assert.AreEqual(obj, null);

        }

        [TestMethod]
        [DeploymentItem("Web.config")]
        public void TestMethod2()
        {
            UsersController home = new UsersController();
            string result = home.GetNameById(0);
            Assert.AreEqual("Admin", result); 
        }
    }
}