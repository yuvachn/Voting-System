﻿using LoginandRegisterMVC.Models;
using log4net;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System;
using CaptchaMvc.HtmlHelpers;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web;

namespace LoginandRegisterMVC.Controllers
{
    public class UsersController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ElectionsController));

        private UserContext db = new UserContext();
        // GET: Users
        [Authorize]
        public ActionResult Index()
        {
            log.Info("Home Page");
            return View(db.Users.ToList());
        }

        [Authorize]
        public ActionResult Contact()
        {
            log.Info("Contact Page");
            return View();
        }

        [Authorize]
        public ActionResult NOTA()
        {
            log.Info("User voted for NOTA");
            ViewBag.Message = "You voted for None Of The Above(NOTA)";
            string to = Session["UserEmail"].ToString(); //To address    
            string from = "vaishali.anand.1276@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Hello User, You've successfully voted for NOTA";
            message.Subject = "Voting Successfull!!";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("vaishali.anand.1276@gmail.com", "ygnyygdjfkmpecnb");
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
                log.Info("User received confirmation mail");
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            return View();
        }

        [Authorize]
        public ActionResult Result(int id)
        {
            var obj = db.Candidates.Where(u => u.ElectionId.Equals(id));
            return View(obj.ToList());
        }

        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)

        {

            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }
            log.Info("OTP generated");
            return sOTP;

        }


        public ActionResult Register()
        {
            log.Info("Register Page.");
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            log.Info("Register HTTP");
            using (UserContext db = new UserContext())
            {
                int e = Convert.ToInt32(user.EmployeeId);
                //FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password,FormsAuthPasswordFormat.SHA1);
                var obj = db.Users.Where(u => u.EmployeeId.Equals(e)).FirstOrDefault();
                if (obj == null)
                {
                    obj = db.Users.Where(u => u.UserEmail.Equals(user.UserEmail)).FirstOrDefault();
                    if (obj == null)
                    {
                        if (ModelState.IsValid)
                        {
                            if (this.IsCaptchaValid("Captcha is not valid"))
                            {
                                user.Password = HashPassword(user.Password);
                                user.ConfirmPassword = HashPassword(user.ConfirmPassword);

                                TempData["user"] = user;
                                TempData.Keep();
                                log.Info("User details added.Verifying OTP");
                                return RedirectToAction("VerifyOTP");
                            }
                            else
                            {
                                ViewBag.ErrMessage = "Error: Captcha is not valid.";
                                log.Error("Captcha is not valid.");
                            }
                        }
                        else
                        {
                            log.Error("Details are not valid.");
                            ModelState.AddModelError("", "Details are not valid.");
                        }
                    }

                    else
                    {
                        ModelState.AddModelError("", "Email id exists, Please login with your password");
                        log.Error("Email exists already.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User exists, Please login with your password");
                    log.Error("User exists already.");
                }

            }
                return View(user);

            
        }


        public ActionResult ResetPassword()
        {
            log.Info("Reset Password Page.");
            return View();
        }


        [HttpPost]
        public ActionResult ResetPassword(User.PwdViewModel user)
        {
            System.Diagnostics.Debug.WriteLine("Entered reset password");
            int e = user.EmployeeId;
            System.Diagnostics.Debug.WriteLine("Employee id is " + e);
            //int e = Convert.ToInt32(user.EmployeeId);
            //FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password,FormsAuthPasswordFormat.SHA1);
            var obj = db.Users.Where(u => u.EmployeeId.Equals(e)).FirstOrDefault();
            System.Diagnostics.Debug.WriteLine("obj :" + obj);


            if (obj != null)
            {
                System.Diagnostics.Debug.WriteLine("obj :" + obj);

                if (ModelState.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("Model is valid");
                    try
                    {

                        user.PassWord = HashPassword(user.PassWord);
                        user.ConfirmPassword = HashPassword(user.ConfirmPassword);
                        System.Diagnostics.Debug.WriteLine("pwd" + user.PassWord);
                        //System.Diagnostics.Debug.WriteLine("con pwd :" + user.ConfirmPassword );

                        obj.Password = user.PassWord;
                        obj.ConfirmPassword = user.PassWord;
                        db.Entry(obj).State = EntityState.Modified;
                        System.Diagnostics.Debug.WriteLine("obj pwd" + obj.Password);
                        System.Diagnostics.Debug.WriteLine("entity modified");
                        db.SaveChanges();
                        log.Info("Reset Password successfull.");
                        System.Diagnostics.Debug.WriteLine("Saved");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        log.Error(ex.Message);
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }

                }

                else
                {
                    var message = string.Join(" | ", ModelState.Values
                                                         .SelectMany(v => v.Errors)
                                                         .Select(et => et.ErrorMessage));

                    //Log This exception to ELMAH:
                    //Exception exception = new Exception(message.ToString());
                    //Elmah.ErrorSignal.FromCurrentContext().Raise(exception);

                    //Return Status Code:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
                }

            }


            else
            {
                ModelState.AddModelError("", "User doesn't exist, Please try again.");
            }

            return RedirectToAction("Login");
        }

        public ActionResult VerifyOTP()
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sRandomOTP = GenerateRandomOTP(8, saAllowedCharacters);
            System.Diagnostics.Debug.WriteLine("otp" +sRandomOTP);
            User user = (User)TempData["user"];
            string to = user.UserEmail.ToString(); //To address    
            string from = "vaishali.anand.1276@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);
            TempData["otp"] = sRandomOTP;
            TempData.Keep();
            string mailbody = "Hello User, Your OTP is " + sRandomOTP;
            message.Subject = "Verify Account - Online Voting System";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("vaishali.anand.1276@gmail.com", "ygnyygdjfkmpecnb");
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
                log.Info("OTP sent");
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            return View();
        }

        [HttpPost]
        public ActionResult VerifyOTP(User user2)
        {
            ViewBag.Message = "";
            User user = (User)TempData["user"];
            if (user2.OTP == TempData["otp"].ToString())
            {
                try
                { 
                    db.Users.Add(user);
                    db.SaveChanges();
                    log.Info("OTP Verified. User Added to Database");

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    log.Error(ex.Message);
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Incorrect OTP, Register Again";
                log.Error("Incorrect OTP");

            }
            return View(user);
        }

        public ActionResult ForgotPassword()
        {
            log.Info("Forgot Password Page");
            return View();
        }


        [HttpPost]
        public ActionResult ForgotPassword(User user)
        {
            string to = user.UserEmail; //To address    
            string from = "vaishali.anand.1276@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Hello User, Here's the <a href='https://localhost:44316/users/resetpassword/'>link</a> to reset your password.";
            message.Subject = "Reset Password Request";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("vaishali.anand.1276@gmail.com", "ygnyygdjfkmpecnb");
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
                log.Info("Reset Password Mail Sent");
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            return View();
        }

        [Authorize]
        public ActionResult ViewElection()
        {
            log.Info("View Elections Page");
            int em = Convert.ToInt32(Session["EI"]);

            var obj = db.Candidates.Where(x => x.EmployeeId.Equals(em)).FirstOrDefault();
            List<Election> obj1 = db.Elections.ToList();
            //List<Election> obj1 = db.Elections.Where(model => model.ServiceLine.Equals("ADM")).ToList();
            ViewBag.Elections = obj1;
            System.Diagnostics.Debug.WriteLine("In get method");
            return View(db.Elections.ToList());

        }

        [Authorize]
        [HttpPost]
        public ActionResult ViewElection(string ServiceLine)
        {
            System.Diagnostics.Debug.WriteLine("Entered Post method " + ServiceLine);
            List<Election> obj;
            
            if(ServiceLine=="All")
            {
               
                obj = db.Elections.ToList();
                ViewBag.Elections = obj;
            }
            else
            {
                obj = db.Elections.Where(model => model.ServiceLine.Equals(ServiceLine)).ToList();
                System.Diagnostics.Debug.WriteLine("list " + obj);
            }
            ViewBag.Elections = obj;
            return View();

        }

        [Authorize]
        public ActionResult VoteElection(int id)
        {
            log.Info("Voting for Election");
            var obj = (from c in db.Candidates
                       join u in db.Users on c.EmployeeId equals u.EmployeeId
                       select new VoteModel
                       { candidateM = c, userM = u }
            ).ToList();
            //var obj = db.Candidates.Where(u => u.ElectionId.Equals(id));
            ViewBag.ElectionId = Convert.ToInt32(id);
            
           var vc = obj.Where(u => u.candidateM.ElectionId.Equals(id));
            return View(vc.ToList());
        }

        [Authorize]
        public ActionResult Vote(int id, int id2)
        {
            int empid = Convert.ToInt32(Session["EI"]);
            TempData["id"] = id;
            TempData.Keep();
            TempData["id2"] = id2;
            TempData.Keep();
            System.Diagnostics.Debug.WriteLine("get Vote method id2=" +id2);
            TempData["EmpId"] = empid;
            TempData.Keep();
            System.Diagnostics.Debug.WriteLine("get Vote method empid=" + empid);
            var VotedData = db.VotedUsers.Where(model => model.EmpId.Equals(empid) && model.ElecId.Equals(id2)).FirstOrDefault();
            if (VotedData!=null)
            {
                ViewBag.valid = false;
            }
            else
            {
                ViewBag.valid = true;
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Vote()
        {
            int id = Convert.ToInt32(TempData["id"]);
            int id2 = Convert.ToInt32(TempData["id2"]);
            int EmpId = Convert.ToInt32(TempData["EmpId"]);
            System.Diagnostics.Debug.WriteLine("Voted user data" + EmpId +id2);
            var data = db.Candidates.Where(x => x.CandidateId.Equals(id) && x.ElectionId.Equals(id2)).FirstOrDefault();
            var VotedData = db.VotedUsers.Where(model => model.EmpId.Equals(EmpId) && model.ElecId.Equals(id2)).FirstOrDefault();
      if(VotedData==null)
            {

                VotedUser VS = new VotedUser();

                VS.EmpId = EmpId;
                VS.ElecId = id2;
                data.Votes += 1;
                System.Diagnostics.Debug.WriteLine("Voted user data" + VS.EmpId + VS.ElecId);
                using (var transaction = db.Database.BeginTransaction())
                {
                    db.VotedUsers.Add(VS);
                    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.VotedUsers ON;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.VotedUsers OFF");
                    transaction.Commit();
                }
                   
                string to = Session["UserEmail"].ToString(); //To address    
                string from = "vaishali.anand.1276@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);

                string mailbody = "Hello User, You've successfully voted for Election " + id2;
                message.Subject = "Voting Successfull!!";
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("vaishali.anand.1276@gmail.com", "ygnyygdjfkmpecnb");
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(message);
                    log.Info("Voting Successfully and Confirmation Sent");

                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw ex;
                }
            }
            return RedirectToAction("ViewElection");
        }

        [Authorize]
        //logics  to be changed
        public ActionResult ApplyElection(int id)
        {
            log.Info("Applying for Election");
            TempData["id"] = id;
            TempData.Keep();
            return View();
        }

        [Authorize]
        //logics  to be changed [rsn: comparing db data to session data for accessing a object]
        [HttpPost]
        public ActionResult ApplyElection()
        {
            int em = Convert.ToInt32(Session["EI"]);
            var obj = db.Users.Where(u => u.EmployeeId.Equals(em)).FirstOrDefault();
            Candidate c = new Candidate();
            c.CandidateId = 1234;
            c.ElectionId = Convert.ToInt32(TempData["id"]);
            c.EmployeeId = Convert.ToInt32(obj.EmployeeId);
            var obj2= db.Candidates.Where(u => u.EmployeeId.Equals(em)).FirstOrDefault();
            if (obj2.EmployeeId!=c.EmployeeId){ 
                db.Candidates.Add(c);
                db.SaveChanges();
                log.Info("Applied Successfully");
                return RedirectToAction("ViewElection");
            }
            else
            {
                ModelState.AddModelError("", "Already Applied!");
                log.Warn("Already Applied!");
            }
            return View();
        }


        public ActionResult ViewElectionById(int id)
        {
            log.Info("View Election Details");
            var obj = db.Elections.Where(u => u.ElectionId.Equals(id)).FirstOrDefault();
            return View(obj);
        }

        
        public ActionResult Login()
        {
            log.Info("Login Page");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {

            using (UserContext db = new UserContext())
            {
                // if (db.Users.Where(u => u.UserEmail.Equals("admin@demo.com") && u.Password.Equals("admin")).FirstOrDefault() == null)
                if (user.UserEmail.Equals("admin@demo.com"))
                {
                    log.Info("Admin logged in");
                    return RedirectToAction("AdminHome", "Elections");

                }
                else
                {
                    user.Password = HashPassword(user.Password);

                    var obj = db.Users.Where(u => u.UserEmail.Equals(user.UserEmail) && u.Password.Equals(user.Password)).FirstOrDefault();
                    if (obj != null)
                    {

                        FormsAuthentication.SetAuthCookie(user.UserEmail, false);
                        Session["UserEmail"] = obj.UserEmail.ToString();
                        Session["Username"] = obj.Username.ToString();
                        Session["ServiceLine"] = obj.ServiceLine.ToString();
                        Session["LastName"] = obj.LastName.ToString();
                        Session["PhoneNo"] = obj.PhoneNo.ToString();
                        Session["EI"] = obj.EmployeeId.ToString();
                        Session["DOB"] = obj.PhoneNo.ToString();
                        log.Info("User logged in");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Email or Password is wrong");
                        log.Error("User Email or Password is wrong");
                    }
                }
            }
            return View(user);
        }


        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            log.Info("User Logget Out.");
            return RedirectToAction("Login");
        }


        public string HashPassword(string password)
        {
            var pwdarray = Encoding.ASCII.GetBytes(password);
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(pwdarray);
            var hashpwd = new StringBuilder(hash.Length);
            foreach (byte b in hash)
            {
                hashpwd.Append(b.ToString());
            }
            return hashpwd.ToString();
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