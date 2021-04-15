using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Prezentomat.DataContext;
using Prezentomat.Models;

namespace Prezentomat.Controllers
{
    public class GatheringController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Gathering
        public ActionResult Index()
        {

            var maxZbiorka = db.GatheringDetails.Where(p => p.target_amount == 50).Single();

            ViewBag.Zbiorka = maxZbiorka; /// jak to wyswietlic we view?

            return View(db.GatheringDetails.ToList());
        }

        // GET: Gathering/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GatheringClass gatheringClass = db.GatheringDetails.Find(id);
            if (gatheringClass == null)
            {
                return HttpNotFound();
            }
            return View(gatheringClass);
        }

        // GET: Gathering/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gathering/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "current_amount,target_amount,finish_date")] GatheringClass gatheringClass)
        {
            if (ModelState.IsValid)
            {
                db.GatheringDetails.Add(gatheringClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gatheringClass);
        }

        // GET: Gathering/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GatheringClass gatheringClass = db.GatheringDetails.Find(id);
            if (gatheringClass == null)
            {
                return HttpNotFound();
            }
            return View(gatheringClass);
        }

        // POST: Gathering/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "current_amount,target_amount,finish_date")] GatheringClass gatheringClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gatheringClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gatheringClass);
        }

        // GET: Gathering/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GatheringClass gatheringClass = db.GatheringDetails.Find(id);
            if (gatheringClass == null)
            {
                return HttpNotFound();
            }
            return View(gatheringClass);
        }

        // POST: Gathering/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GatheringClass gatheringClass = db.GatheringDetails.Find(id);
            db.GatheringDetails.Remove(gatheringClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
