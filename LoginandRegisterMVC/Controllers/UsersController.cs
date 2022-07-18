using LoginandRegisterMVC.Models;
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
        private UserContext db = new UserContext();
        // GET: Users
        [Authorize]
        public ActionResult Index()
        {

            return View(db.Users.ToList());
        }
        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult NOTA()
        {
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
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

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

            return sOTP;

        }

       

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(User user)
        {
            using (UserContext db = new UserContext())
            {
                int e = Convert.ToInt32(user.EmployeeId);
                //FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password,FormsAuthPasswordFormat.SHA1);
                var obj = db.Users.Where(u => u.EmployeeId.Equals(e)).FirstOrDefault();
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
                            return RedirectToAction("VerifyOTP");
                        }
                        else
                        {
                            ViewBag.ErrMessage = "Error: Captcha is not valid.";
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User exists ,Please login with your password");
                }

                return View(user);
            }

        }


        public ActionResult ResetPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ResetPassword(User.PwdViewModel user)
        {
            System.Diagnostics.Debug.WriteLine("Entered reset password");
            int e = user.EmployeeId;
            System.Diagnostics.Debug.WriteLine("employee id is " + e);
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
                        System.Diagnostics.Debug.WriteLine("Saved");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
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
                ModelState.AddModelError("", "User doesn't exist ,Please try again");
            }
        
                    
      

            return RedirectToAction("Login");
    }

    public ActionResult VerifyOTP()
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sRandomOTP =  GenerateRandomOTP(8, saAllowedCharacters);

            User user = (User)TempData["user"];
            string to=user.UserEmail.ToString(); //To address    
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
            }

            catch (Exception ex)
            {
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



                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Index");             }
            else
            {
                ViewBag.Message= "Incorrect OTP, Register Again";
            }
            return View(user);
        }

        public ActionResult ForgotPassword()
        {


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
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }


        public ActionResult ViewElection()
        {
            int em = Convert.ToInt32(Session["EI"]);

            var obj = db.Candidates.Where(x=>x.EmployeeId.Equals(em)).FirstOrDefault();
            if (obj == null)
            {
                ViewBag.Message = "true";
            }
            else
            {
                ViewBag.Message = "false";
            }
            return View(db.Elections.ToList());

        }


        public ActionResult VoteElection(int id)
        {
            var obj = (from c in db.Candidates join u in db.Users on c.EmployeeId equals u.EmployeeId select new VoteModel
            { candidateM = c, userM = u }
            ).ToList();
            //var obj = db.Candidates.Where(u => u.ElectionId.Equals(id));
            return View(obj.ToList());
        }


        public ActionResult Vote(int id, int id2)
        {
            int empid = Convert.ToInt32(Session["EI"]);
            TempData["id"] = id;
            TempData.Keep(); 
            TempData["id2"] = id2;
            TempData.Keep();
            TempData["EmpId"] = empid;
            TempData.Keep();
            var VotedData = db.VotedUsers.Where(model => model.EmpId.Equals(empid) && model.ElectionId.Equals(id2)).FirstOrDefault();
            if(empid == VotedData.EmpId && id2 == VotedData.ElectionId)
            {
                ViewBag.valid = false;
            }
            else
            {
                ViewBag.valid = true;
            }

            return View();
        }


        [HttpPost]
        public ActionResult Vote()
        {
            int id = Convert.ToInt32(TempData["id"]);
            int id2 = Convert.ToInt32(TempData["id2"]);
            int EmpId = Convert.ToInt32(TempData["EmpId"]);
            var data = db.Candidates.Where(x => x.CandidateId.Equals(id) && x.ElectionId.Equals(id2)).FirstOrDefault();
            var VotedData = db.VotedUsers.Where(model => model.EmpId.Equals(EmpId) && model.ElectionId.Equals(id2)).FirstOrDefault();
            if (Convert.ToInt32(Session["EI"]) != VotedData.EmpId && id2 != VotedData.ElectionId)
            {
              
                VotedUser VS = new VotedUser(); 
                
                VS.EmpId = EmpId;
                VS.ElectionId = id2;
                data.Votes += 1;
                db.VotedUsers.Add(VS);
                db.SaveChanges();
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
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("ViewElection");
        }


        //logics  to be changed
        public ActionResult ApplyElection(int id)
        {
            TempData["id"] = id;
            TempData.Keep();
            return View();
        }
        

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
            db.Candidates.Add(c);
            db.SaveChanges();
            return RedirectToAction("ViewElection");
        }

        
        public ActionResult Login()
        {
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

                    return RedirectToAction("ViewElections", "Elections");

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
                        return RedirectToAction("ViewElection");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Email or password wrong");
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