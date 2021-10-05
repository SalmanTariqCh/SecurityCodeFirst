using SecurityDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using SecurityDB.ModelView;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace SecurityDB.Controllers
{
    public class ShiftManagementController : Controller
    {
        SecurityContext _context;
        public ShiftManagementController()
        {
            _context = new SecurityContext();
        }

        public ActionResult Index()
        {
            try
            {
                //var _shiftmangement = from sh in _context.Shifts
                //                      join s in _context.Sites
                //                      on sh.SiteId equals s.Id
                //                      join u in _context.Users
                //                      on sh.UserId equals u.Id
                //                      orderby sh.FromDate descending
                //                      select new Shift()
                //                      {
                //                          ShiftId = sh.Id,
                //                          SiteId = s.Id,
                //                          SiteName = s.SiteName,
                //                          UserId = u.Id,
                //                          FirstName = u.FirstName,
                //                          LastName = u.LastName,
                //                          FromDate = sh.FromDate,
                //                          EndDate = sh.EndDate,
                //                          Desc = sh.Desc,
                //                          IsConfirmed = sh.IsConfirmed
                //                      };

                var _shiftsite = _context.Shifts.Include(s => s.Site).Include(u => u.User).OrderByDescending(sh => sh.FromDate).ToList();
                
                return View(_shiftsite);
            }
            catch (Exception)
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
                //int UserId;
                //UserId = _context.Users.Where(u => u.FirstName == _shiftmdview.Users.FirstName && u.LastName == _shiftmdview.Users.LastName).Select(u => u.Id).FirstOrDefault();

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
                _shift.UserId = _shiftmdview.Shifts.UserId;
                _shift.Desc = _shiftmdview.Shifts.Desc;
                _shift.IsApproved = _shiftmdview.Shifts.IsApproved;

                 if (_shiftmdview.Shifts.Id > 0)
                 {
                    _context.Entry(_shift).State = System.Data.Entity.EntityState.Modified;
                 }
                 else
                 {
                    _context.Shifts.Add(_shift);
                 }

                ViewBag.HoursMinutes = "";

                 _context.SaveChanges();
                 return RedirectToAction("Index");
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

                ViewBag.HoursMinutes = "";
                return View("Create", _shifteditmodel);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                throw;
            }
        }

        public ActionResult Search(string search, DateTime? datefrom, DateTime? dateend)
        {

            try
            {
                decimal _shifttotalminutes, hours, minutes;
                int _shiftsumhours, _shiftminutes;

                if (search == "" && datefrom == null && dateend == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else if (search != "" && datefrom == null && dateend == null)
                {
                    var _shiftsearch = (from s in _context.Shifts
                                        where s.User.FirstName.Contains(search) || s.User.LastName.Contains(search)
                                        select s).Include(s => s.Site).Include(u => u.User).OrderByDescending(s => s.FromDate).ToList();

                    if (_shiftsearch == null)
                    {
                        return HttpNotFound();
                    }

                     _shiftsumhours = (int)_shiftsearch.Sum(h => h.Hoursno);
                     _shiftminutes = (int)_shiftsearch.Sum(m => m.MinutesNo);

                    _shifttotalminutes = (_shiftsumhours * 60) + _shiftminutes;
                    hours = Math.Truncate(_shifttotalminutes / 60);
                    minutes = _shifttotalminutes % 60;
                    ViewBag.HoursMinutes = "Total number of hour(s) are " + hours + " and minute(s) are " + minutes;

                    return View("Index", _shiftsearch);
                }
                else if (search != "" && datefrom != null && dateend == null)
                {
                    var _shiftsearch = (from s in _context.Shifts
                                        where (s.User.FirstName.Contains(search) || s.User.LastName.Contains(search))
                                        && s.FromDate >= datefrom
                                        select s).Include(s => s.Site).Include(u => u.User).OrderByDescending(s => s.FromDate).ToList();

                    if (_shiftsearch == null)
                    {
                        return HttpNotFound();
                    }

                    _shiftsumhours = (int)_shiftsearch.Sum(h => h.Hoursno);
                    _shiftminutes = (int)_shiftsearch.Sum(m => m.MinutesNo);

                    _shifttotalminutes = (_shiftsumhours * 60) + _shiftminutes;
                    hours = Math.Truncate(_shifttotalminutes / 60);
                    minutes = _shifttotalminutes % 60;
                    ViewBag.HoursMinutes = "Total number of hour(s) are " + hours + " and minute(s) are " + minutes;

                    return View("Index", _shiftsearch);
                }
                else if (search != "" && datefrom != null && dateend != null)
                {
                    var _shiftsearch = (from s in _context.Shifts
                                        where (s.User.FirstName.Contains(search) || s.User.LastName.Contains(search))
                                        && s.FromDate >= datefrom && s.EndDate <= dateend
                                        select s).Include(s => s.Site).Include(u => u.User).OrderByDescending(s => s.FromDate).ToList();

                    if (_shiftsearch == null)
                    {
                        return HttpNotFound();
                    }

                    _shiftsumhours = (int)_shiftsearch.Sum(h => h.Hoursno);
                    _shiftminutes = (int)_shiftsearch.Sum(m => m.MinutesNo);

                    _shifttotalminutes = (_shiftsumhours * 60) + _shiftminutes;
                    hours = Math.Truncate(_shifttotalminutes / 60);
                    minutes = _shifttotalminutes % 60;
                    ViewBag.HoursMinutes = "Total number of hour(s) are " + hours + " and minute(s) are " + minutes;

                    return View("Index", _shiftsearch);
                }
                else if (search == "" && datefrom != null && dateend != null)
                {
                    var _shiftsearch = (from s in _context.Shifts
                                        where s.FromDate >= datefrom && s.EndDate <= dateend
                                        select s).Include(s => s.Site).Include(u => u.User).OrderByDescending(s => s.FromDate).ToList();

                    if (_shiftsearch == null)
                    {
                        return HttpNotFound();
                    }

                    _shiftsumhours = (int)_shiftsearch.Sum(h => h.Hoursno);
                    _shiftminutes = (int)_shiftsearch.Sum(m => m.MinutesNo);

                    _shifttotalminutes = (_shiftsumhours * 60) + _shiftminutes;
                    hours = Math.Truncate(_shifttotalminutes / 60);
                    minutes = _shifttotalminutes % 60;
                    ViewBag.HoursMinutes = "Total number of hour(s) are " + hours + " and minute(s) are " + minutes;

                    return View("Index", _shiftsearch);
                }
                else if (search == "" && datefrom != null && dateend == null)
                {
                    var _shiftsearch = (from s in _context.Shifts
                                        where s.FromDate >= datefrom
                                        select s).Include(s => s.Site).Include(u => u.User).OrderByDescending(s => s.FromDate).ToList();

                    if (_shiftsearch == null)
                    {
                        return HttpNotFound();
                    }

                    _shiftsumhours = (int)_shiftsearch.Sum(h => h.Hoursno);
                    _shiftminutes = (int)_shiftsearch.Sum(m => m.MinutesNo);

                    _shifttotalminutes = (_shiftsumhours * 60) + _shiftminutes;
                    hours = Math.Truncate(_shifttotalminutes / 60);
                    minutes = _shifttotalminutes % 60;
                    ViewBag.HoursMinutes = "Total number of hour(s) are " + hours + " and minute(s) are " + minutes;

                    return View("Index", _shiftsearch);
                }
                else if (search == "" && datefrom == null && dateend != null)
                {
                    var _shiftsearch = (from s in _context.Shifts
                                        where s.EndDate <= dateend
                                        select s).Include(s => s.Site).Include(u => u.User).OrderByDescending(s => s.FromDate).ToList();

                    if (_shiftsearch == null)
                    {
                        return HttpNotFound();
                    }

                    _shiftsumhours = (int)_shiftsearch.Sum(h => h.Hoursno);
                    _shiftminutes = (int)_shiftsearch.Sum(m => m.MinutesNo);

                    _shifttotalminutes = (_shiftsumhours * 60) + _shiftminutes;
                    hours = Math.Truncate(_shifttotalminutes / 60);
                    minutes = _shifttotalminutes % 60;
                    ViewBag.HoursMinutes = "Total number of hour(s) are " + hours + " and minute(s) are " + minutes;

                    return View("Index", _shiftsearch);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception)
            {
                throw;
            }
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