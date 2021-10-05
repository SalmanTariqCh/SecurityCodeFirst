using SecurityDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SecurityDB.Controllers
{
    public class UserDetailsController : Controller
    {
        SecurityContext _context;
        public UserDetailsController()
        {
            _context = new SecurityContext();
        }
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                string name = Session["UserName"].ToString();
                var _user = _context.Users.Where(u => u.UserName == name).ToList();
                return View(_user);
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int? Id)
        {
            try
            {
                if (Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User _userdetails = _context.Users.Where(u => u.Id == Id).SingleOrDefault();

                if (_userdetails == null)
                {
                    return HttpNotFound();
                }

                return View("Create", _userdetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(User _user)
        {
            try
            {
                ModelState.Remove("Code");
                if (ModelState.IsValid)
                {
                        string name = Session["UserName"].ToString();
                        int Code = (int)Session["Code"];
                        _user.Code = Code;

                        _context.Entry(_user).State = System.Data.Entity.EntityState.Modified;

                    //User _useredit = _context.Users.Find(_user.Id);

                    //    _useredit.Id = _user.Id;
                    //    _useredit.FirstName = _user.FirstName;
                    //    _useredit.LastName = _user.LastName;
                    //    _useredit.UserName = _user.UserName;
                    //    _useredit.Password = _user.Password;
                    //    _useredit.Address = _user.Address;
                    //    _useredit.Mobile = _user.Mobile;
                    //    _useredit.Email = _user.Email;
                    //    _useredit.IsActive = _user.IsActive;
                    //    _useredit.JoiningDate = _user.JoiningDate;
                    //    _useredit.SIAno = _user.SIAno;
                    //    _useredit.SIAType = _user.SIAType;
                    //    _useredit.SIAExpiryDate = _user.SIAExpiryDate;
                    //    _useredit.Code = Code;

                        _context.SaveChanges();
                        return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return View("Index");
        }
        public ActionResult Delete()
        {
            int Id;

            if (Session["UserName"] != null)
            {
                string name = Session["UserName"].ToString();
                Id = _context.Users.Where(u => u.UserName == name).Select(u => u.Id).FirstOrDefault();
            }
            else
            {
                Id = 0;
            }

            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User _userdetails = _context.Users.Where(u => u.Id == Id).FirstOrDefault();

            if (_userdetails == null)
            {
                return HttpNotFound();
            }

            _context.Users.Remove(_userdetails);
            _context.SaveChanges();

            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult Refresh()
        {
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