using SecurityDB.Models;
using SecurityDB.ModelView;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SecurityDB.Controllers
{
    public class ShiftController : Controller
    {
        SecurityContext _context;
        public ShiftController()
        {
            _context = new SecurityContext();
        }
        public ActionResult Index()
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    int Id;
                
                    string name = Session["UserName"].ToString();
                    Id = _context.Users.Where(u => u.UserName == name).Select(u => u.Id).FirstOrDefault();

                    var _shiftsite = _context.Shifts.Where(u => u.UserId == Id).Include(s => s.Site).ToList();

               /*     var _shiftsite = from s in _context.Sites
                                     join sh in _context.Shifts
                                     on s.Id equals sh.SiteId
                                     join u in _context.Users
                                     on sh.UserId equals Id
                                     select new ShiftModelView()
                                     {
                                         Shifts = new Shift()
                                         {
                                             FromDate = sh.FromDate,
                                             EndDate = sh.EndDate,
                                             Hoursno = sh.Hoursno,
                                             MinutesNo = sh.MinutesNo,
                                             SiteId = sh.SiteId,
                                             Site = new Site()
                                             {
                                                 SiteName = s.SiteName,
                                                 SiteAddress = s.SiteAddress,
                                                 ContactPerson = s.ContactPerson,
                                                 Telephone = s.Telephone,
                                                 Mobile = s.Mobile,
                                                 Email = s.Email
                                             },
                                             UserId = sh.UserId,
                                             Desc = sh.Desc
                                         }
                                     };  */

                    //var _site = from s in _context.Sites
                    //             join sh in _context.Shifts
                    //             on s.Id equals sh.SiteId
                    //             join u in _context.Users
                    //             on sh.UserId equals Id
                    //             select new Site()
                    //             {
                    //                 SiteName = s.SiteName,
                    //                 SiteAddress = s.SiteAddress,
                    //                 ContactPerson = s.ContactPerson,
                    //                 Telephone = s.Telephone,
                    //                 Mobile = s.Mobile,
                    //                 Email = s.Email
                    //             };

                    //ViewBag.ShiftSite = _shiftsite;
                    return View(_shiftsite);
                }
                else
                {
                    return View();
                }
            }
            catch(Exception Ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                throw;
            }   
        }

        public ActionResult Create()
        {
            try
            {
                var _site = _context.Sites.ToList();
                string name;

                if (Session["UserName"] != null)
                {
                    name = Session["UserName"].ToString();
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ViewBag.Name = name;

                var _siteviewmodel = new ShiftModelView
                {
                    Shifts = new Shift(),
                    Sites = _site
                };

                return View(_siteviewmodel);
            }
            catch (Exception)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(ShiftModelView _shiftmdview)
        {
            try
            {
                int UserId;

                if (Session["UserName"] != null)
                {

                    string name = Session["UserName"].ToString();
                    UserId = _context.Users.Where(u => u.UserName == name).Select(u => u.Id).FirstOrDefault();


                    Shift _shift = new Shift();
                    if (_shiftmdview.Shifts.Id > 0)
                    {
                        _shift.Id = _shiftmdview.Shifts.Id;
                    }
                    _shift.FromDate = _shiftmdview.Shifts.FromDate;
                    _shift.EndDate = _shiftmdview.Shifts.EndDate;
                    _shift.Hoursno = _shiftmdview.Shifts.Hoursno;
                    _shift.MinutesNo = _shiftmdview.Shifts.MinutesNo;
                    _shift.SiteId = _shiftmdview.Shifts.SiteId;
                    _shift.UserId = UserId;
                    _shift.Desc = _shiftmdview.Shifts.Desc;

                    
                    if (_shiftmdview.Shifts.Id > 0)
                    {
                        _context.Entry(_shift).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        _context.Shifts.Add(_shift);
                    }

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
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
        }

        public ActionResult Edit(int? id)
        {
            try
            {
                int UserId;
                if (Session["UserName"] != null)
                {
                    string name = Session["UserName"].ToString();
                    UserId = _context.Users.Where(u => u.UserName == name).Select(u => u.Id).FirstOrDefault();

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    var _shiftedit = _context.Shifts.Where(sh => sh.Id == id).Include(s => s.Site).SingleOrDefault();

                    if (_shiftedit == null)
                    {
                        return HttpNotFound();
                    }

                    var _shifteditmodel = new ShiftModelView
                    {
                        Shifts = _shiftedit,
                        Sites = _context.Sites.ToList()
                    };

                    return View("Create", _shifteditmodel);
                }
                return View();
            }
            catch(Exception e)
            {

                throw;
            }
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var _shiftdelete = _context.Shifts.SingleOrDefault(sh => sh.Id == id);

                if (_shiftdelete == null)
                {
                    return HttpNotFound();
                }

                _context.Shifts.Remove(_shiftdelete);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
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