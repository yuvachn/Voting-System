using LoginandRegisterMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Web;
using System.Web.Mvc;

namespace LoginandRegisterMVC.Controllers
{
    public class ResultController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));


        private UserContext db = new UserContext();
        // GET: Result
        public ActionResult Graph(int id)
        {
            log.Info("View Result");
            var obj = db.Candidates.Where(u => u.ElectionId.Equals(id));

            int totalusers = db.Users.Count();

            int votedUsers = db.VotedUsers.Where(model => model.ElecId.Equals(id)).Count();

            //column chart
            List<Result> PieDatapoints = new List<Result> { };
            List<Result> dataPoints = new List<Result>{
                
                //new Result(20, 36),
                
            };

            foreach (int item in obj.Select(c => c.CandidateId).ToList())
            {
                ////dataPoints.Append((Convert.ToInt32(obj.Where(c => c.CandidateId == item).Select(s => s.EmployeeId)),Convert.ToInt32(obj.Where(c => c.CandidateId == item).Select(s => s.Votes))));
                //foreach (int item1 in obj.Where(c => c.CandidateId == item).Select(s => s.EmployeeId).ToList())
               // {
                    foreach (int item2 in obj.Where(c => c.CandidateId == item).Select(s => s.Votes).ToList())
                    {
                        dataPoints.Add(new Result(item.ToString(), item2));
                        
                    ViewBag.Range = item;
                    }
                    ////dataPoints.Add(new Result(obj.Where(c => c.CandidateId == item).Select(s => s.EmployeeId),
                    //obj.Where(c => c.CandidateId == item).Select(s => s.Votes)));
               // }
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            //pieChart
           
            PieDatapoints.Add(new Result("Total Users", totalusers));
            PieDatapoints.Add(new Result("Voted Users", votedUsers));

            ViewBag.PieDataPoints = JsonConvert.SerializeObject(PieDatapoints);
            ViewBag.ElectionID = id;

            return View(obj.ToList());

            
        }
    }
}