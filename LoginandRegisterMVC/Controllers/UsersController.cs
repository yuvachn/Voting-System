using LoginandRegisterMVC.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;


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
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            using (UserContext db = new UserContext())
            {
                //FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password,FormsAuthPasswordFormat.SHA1);
                var obj = db.Users.Where(u => u.UserEmail.Equals(user.UserEmail)).FirstOrDefault();
                if (obj == null)
                {
                    if (ModelState.IsValid)
                    {   //comparison to be done
                        user.Password = HashPassword(user.Password); ;
                        user.ConfirmPassword = HashPassword(user.ConfirmPassword);
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error Occured! Try again!!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User exists ,Please login with your password");
                }
                return View(user);
            }

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
                    Session["DOB"] = obj.PhoneNo.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "User Email or password wrong");
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


