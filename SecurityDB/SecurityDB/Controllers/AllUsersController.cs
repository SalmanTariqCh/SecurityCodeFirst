using SecurityDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SecurityDB.Controllers
{
    public class AllUsersController : Controller
    {
        SecurityContext _context;
        public AllUsersController()
        {
            _context = new SecurityContext();
        }
        public ActionResult Index()
        {
            var _users = _context.Users.ToList();
            return View(_users);
        }

        public ActionResult Create()
        {
            return View(new Site { Id = 0 });
        }

        [HttpPost]
        public ActionResult Create(User _user)
        {
            try
            {
                ModelState.Remove("Code");
                ModelState.Remove("UserName");
                ModelState.Remove("Password");

                if (ModelState.IsValid)
                {
                    int Id;

                    if (Session["UserName"] != null)
                    {
                        var _usersave = _context.Users.Where(u => u.Id == _user.Id).FirstOrDefault(); 

                        User _useredit = _context.Users.Find(_user.Id);
                        _useredit.Id = _user.Id;
                        _useredit.FirstName = _user.FirstName;
                        _useredit.LastName = _user.LastName;
                        _useredit.UserName = _usersave.UserName;
                        _useredit.Password = _usersave.Password;
                        _useredit.Address = _user.Address;
                        _useredit.Mobile = _user.Mobile;
                        _useredit.Email = _user.Email;
                        _useredit.IsActive = _user.IsActive;
                        _useredit.JoiningDate = _user.JoiningDate;
                        _useredit.SIAno = _user.SIAno;
                        _useredit.SIAType = _user.SIAType;
                        _useredit.SIAExpiryDate = _user.SIAExpiryDate;
                        _user.Code = _usersave.Code;

                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return View("Index");

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _useredit = _context.Users.Where(s => s.Id == id).SingleOrDefault();

            if (_useredit == null)
            {
                return HttpNotFound();
            }

            return View("Create", _useredit);
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