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


        public ActionResult Result(int id)
        {
            var obj = db.Candidates.Where(u => u.ElectionId.Equals(id));
            return View(obj.ToList());
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

                            db.Users.Add(user);
                            try
                            {
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
                            return RedirectToAction("Index");

                        }
                        else
                        {
                            ModelState.AddModelError("", "Error Occured! Try again!!");
                        }
                    }

                    ViewBag.ErrMessage = "Error: Captcha is not valid.";
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
        public ActionResult ResetPassword(User user) 
        {

            int e = Convert.ToInt32(user.EmployeeId);
            //FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password,FormsAuthPasswordFormat.SHA1);
            var obj = db.Users.Where(u => u.EmployeeId.Equals(e)).FirstOrDefault();
            if (obj != null)
            {
                if (ModelState.IsValid)
                {
                    if (user.Password == user.ConfirmPassword)
                    {
                        var Password = HashPassword(user.Password);
                        obj.Password = Password;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Passwords don't match ,Please try again");

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Model not valid ,Please try again");
                }
            } 
            else
            {
                ModelState.AddModelError("", "User doesn't exist ,Please try again");
            }

            return RedirectToAction("Login");
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
            return View(db.Elections.ToList());
        }


        public ActionResult VoteElection(int id)
        {
            var obj = db.Candidates.Where(u => u.ElectionId.Equals(id));
            return View(obj.ToList());
        }


        public ActionResult Vote(int id, int id2)
        {
            TempData["id"] = id;
            TempData.Keep(); 
            TempData["id2"] = id2;
            TempData.Keep();
            return View();
        }


        [HttpPost]
        public ActionResult Vote()
        {
            int id = Convert.ToInt32(TempData["id"]);
            int id2 = Convert.ToInt32(TempData["id2"]);
            var data = db.Candidates.Where(x => x.CandidateId.Equals(id) && x.ElectionId.Equals(id2)).FirstOrDefault();
            data.Votes+= 1;
            db.SaveChanges();
            string to = Session["UserEmail"].ToString(); //To address    
            string from = "vaishali.anand.1276@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Hello User, You've successfully voted for Election "+id2;
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
            return View();
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