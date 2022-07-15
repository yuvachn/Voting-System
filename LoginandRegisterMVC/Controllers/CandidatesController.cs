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
    public class CandidatesController : Controller
    {
        private UserContext db = new UserContext();

        // GET: Candidates
        public ActionResult AddCandidates()
        {
            return View(db.Candidates.ToList());
        }

        public ActionResult AddCandidatesById(int id)
        {
            var obj = db.Candidates.Where(x => x.ElectionId == id);
            return View(obj.ToList());
        }

        public ActionResult ViewCandidates(int id)
        {
            var obj = db.Users.Where(u => u.EmployeeId.Equals(id)).FirstOrDefault();
            return View(obj);
        }

        public ActionResult RemoveCandidates(int id)
        {
            var obj = db.Candidates.Where(x => x.CandidateId == id).FirstOrDefault();
            db.Candidates.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("AddCandidates");
        }
    }
}