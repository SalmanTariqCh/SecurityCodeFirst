using SecurityDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace SecurityDB.Controllers
{
    public class SiteController : Controller
    {
        SecurityContext _context;
        public SiteController()
        {
            _context = new SecurityContext();
        }
        public ActionResult Index()
        {
            var _site = _context.Sites.ToList();
            return View(_site);
        }

        public ActionResult Create()
        {
            return View(new Site { Id = 0 });
        }

        [HttpPost]
        public ActionResult Create(Site _site)
        {
            if (ModelState.IsValid)
            {
                if (_site.Id > 0)
                {
                    _context.Entry(_site).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    _context.Sites.Add(_site);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Create", _site);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _siteedit = _context.Sites.Where(s => s.Id == id).SingleOrDefault();

            if (_siteedit == null)
            {
                return HttpNotFound();
            }

            return View("Create", _siteedit);
        }
        public ActionResult Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var _sitedelte = _context.Sites.Find(id);

                if (_sitedelte == null)
                {
                    return HttpNotFound();
                }

                _context.Sites.Remove(_sitedelte);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
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