using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoginandRegisterMVC.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using log4net;
using System.Data.Entity;


namespace LoginandRegisterMVC.Controllers
{
    public class CandidatesController : Controller

    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(CandidatesController));
       
      
        private UserContext db = new UserContext();

        // GET: Candidates
        public ActionResult AddCandidates()
        {
            log.Info("View All Candidates");
            return View(db.Candidates.ToList());
        }

        public ActionResult AddCandidatesById(int id)
        {
            log.Info("View Candidates By Id");
            var obj = db.Candidates.Where(x => x.ElectionId == id);
            return View(obj.ToList());
        }

        public ActionResult ViewCandidates(int id)
        {
            log.Info("View Candidates Details");
            var obj = db.Users.Where(u => u.EmployeeId.Equals(id)).FirstOrDefault();
            return View(obj);
        }

        public ActionResult RemoveCandidates(int id)
        {
            var obj = db.Candidates.Where(x => x.CandidateId == id).FirstOrDefault();
            db.Candidates.Remove(obj);
            try
            {
                db.SaveChanges();
                log.Warn("Candidate Removed");
                return RedirectToAction("AddCandidates");
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                throw (e);
            }
     
        }
    }
}