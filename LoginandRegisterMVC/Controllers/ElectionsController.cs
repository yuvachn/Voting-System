using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoginandRegisterMVC.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;


namespace LoginandRegisterMVC.Controllers
{
    public class ElectionsController : Controller
    {
        private UserContext db = new UserContext();
        // GET: Elections
      
        public ActionResult ViewElections()
        {
            return View(db.Elections.ToList());
        }

        public ActionResult AddElections()
        { 
            return View(); 
        }

        [HttpPost]
        public ActionResult AddElections(Election election)
        {
            using (UserContext db = new UserContext())
            {
                if (ModelState.IsValid)
               
                    {
                        db.Elections.Add(election);
                        db.SaveChanges();
                        return RedirectToAction("ViewElections");
                    }
                else
                {
                    ModelState.AddModelError("", "Error");
                }
            }
                
                return View(election);
            }

        
        public ActionResult EditElections(int id)
        {
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
            }
           
            return RedirectToAction("ViewElections");
        }


        public ActionResult RemoveElections(int id)
        {
            var obj = db.Elections.Where(x => x.ElectionId == id).FirstOrDefault();
            db.Elections.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("ViewElections");
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