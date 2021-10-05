using SecurityDB.Models;
using SecurityDB.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecurityDB.Controllers
{
    public class HomeController : Controller
    {
        SecurityContext _context;

        public HomeController()
        {
            _context = new SecurityContext();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", user);
            }

            if (_context.Users.Where(u => u.Email == user.Email).Any())
            {
                ModelState.AddModelError("Email", "This Email already exist");
                return View("Register", user);
            }

            if (_context.Users.Where(u => u.UserName == user.UserName).Any())
            {
                ModelState.AddModelError("UserName", "This user name already exist");
                return View("Register", user);
            }

            //3344 is for employee and 7788 is for administrator
            if(user.Code != 3344 && user.Code != 7788) 
            {
                ModelState.AddModelError("Code", "Please enter the correct code for authentication");
                return View("Register", user);
            }

            user.IsActive = true;
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index");

            //return Content("Your registration has performed successfully, Please log in");
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            ModelState.Remove("Code");
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Email");

            if (!ModelState.IsValid)
            {
                return View("Register", user);
            }

            var loginuser = _context.Users.Where(u => u.UserName == user.UserName && u.Password == user.Password && u.IsActive == true).FirstOrDefault();

            if (loginuser == null)
            {
                //ModelState.AddModelError("UserName", "User Name or password is not correct, Please check it");
                ViewBag.LoginMessage = "Please enter the correct User Name or Password. Alternatively please contact the Administrator";
                return View("Index", user);
            }
            else
            {
                Session["UserName"] = loginuser.UserName;
                Session["Code"] = loginuser.Code;
                ViewBag.Code = loginuser.Code;
                ViewBag.LoginMessage = "";
                return RedirectToAction("Index", "UserDetails");
            }

            //return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}