using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Web;
using LoginandRegisterMVC.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using static LoginandRegisterMVC.Models.Election;

namespace LoginandRegisterMVC.Controllers
{
    public class ElectionsController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ElectionsController));

        private UserContext db = new UserContext();
        // GET: Elections

        public ActionResult AdminHome()
        {
            log.Info("In admin home");
            return View();
        }
        public ActionResult ViewElections()
        {
            log.Info("View Elections");
            return View(db.Elections.ToList());
        }

        public ActionResult AddElections()
        {
            
            
            List<CheckBox> obj = new List<CheckBox>() {
                 new CheckBox { Text = "ADM", Value = 1, IsChecked = false },
               new CheckBox { Text = "MDU", Value = 2, IsChecked = false },
               new CheckBox { Text = "QEA", Value = 2, IsChecked = false },
               new CheckBox { Text = "CSD", Value = 2, IsChecked = false },};
            
            Election objbind = new Election();
            objbind.ServiceLines = obj;
            ViewBag.SL = obj;
            log.Info("Add New Elections");
            return View(objbind);
            
            
        }

        [HttpPost]
        public ActionResult AddElections(Election election)
        {
            using (UserContext db = new UserContext())
            {
                if (ModelState.IsValid)

                {
                    System.Diagnostics.Debug.WriteLine("Entered for each");
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in election.ServiceLines.ToList())
                    {
                        System.Diagnostics.Debug.WriteLine("Entered for each");
                        System.Diagnostics.Debug.WriteLine(election.ServiceLines.ToString());

                        if (item.IsChecked)
                        {
                            System.Diagnostics.Debug.WriteLine("Entered IF");
                            sb.Append(item.Text + ",");
                           System.Diagnostics.Debug.WriteLine(sb.ToString());
                            if(item.ToString() == "ADM")
                            {
                                election.ADM = true;
                            }

                            if (item.ToString() == "CSD")
                            {
                                election.CSD = true;
                            }
                            if (item.ToString() == "QEA")
                            {
                                election.QEA = true;
                            }
                            if (item.ToString() == "MDU")
                            {
                                election.MDU = true;
                            }

                        }
                    }
                   
                     db.Elections.Add(election);
                        db.SaveChanges();
                    log.Info("Election Added");
                    return RedirectToAction("ViewElections");
                    }
                else
                {
                    ModelState.AddModelError("", "Error while adding Election");
                    log.Error("Error while adding Election");
                }
            }
            return View(election);
            }

        
        public ActionResult EditElections(int id)
        {
            log.Info("Edit Election");
            var obj = db.Elections.Where(x => x.ElectionId == id).FirstOrDefault();
            if (obj != null)
            {
                TempData["ElectionId"] = id;
                TempData.Keep();
            }
            return View(obj);

        }

        [HttpPost]
         public ActionResult EditElections(Election election)
         {

            int id = (int)TempData["ElectionId"];
            var obj = db.Elections.Where(x => x.ElectionId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.ElectionTitle = election.ElectionTitle;
                obj.StartTime = election.StartTime;
                obj.EndTime = election.EndTime;
                obj.Description = election.Description;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                log.Warn("Election Edited");
            }

            return RedirectToAction("ViewElections");
        }


        public ActionResult RemoveElections(int id)
        {
            var obj = db.Elections.Where(x => x.ElectionId == id).FirstOrDefault();
            db.Elections.Remove(obj);
            try { 
            db.SaveChanges();
            log.Warn("Election Removed");
            return RedirectToAction("ViewElections");
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                throw (e);
            }
        }
        public ActionResult ViewElectionById(int id)
        {
            log.Info("View Election By Id");
            var obj = db.Elections.Where(u => u.ElectionId.Equals(id)).FirstOrDefault();
            return View(obj);
        }

        //Dispose the database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}