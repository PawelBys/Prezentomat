using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Prezentomat.DataContext;
using Prezentomat.Models;

namespace Prezentomat.Controllers
{
    //[CustomAuthorize]
    public class GatheringController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        int id;
        string user_name;

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var Session = System.Web.HttpContext.Current.Session;
            if (Session != null)
            {
                id = (int)Session["UserID"];
                user_name = db.UserDetails.Where(p => p.user_id == id).Single().firstname;
                ViewBag.user_name = user_name;
            }


            return base.BeginExecute(requestContext, callback, state);
        }

        // GET: Gathering
        public ActionResult Index()
        {
            //int userId = Convert.ToInt32(Session["id"]);
            //var bookings = db.GatheringDetails.Where(b => b.creator_id == 1).ToList();


            //wyswietlenie zbiurek tylko zalogowanego uzytkownika
            var userOfGatherings = db.UserOfGatheringDetails.Where(b=>b.user_id==id).ToList();
            List<GatheringClass> gatherings=new List<GatheringClass>();
            for(int i = 0; i < userOfGatherings.Count(); i++)
            {
                int gatheringId = userOfGatherings[i].gathering_id;
                gatherings.Add(db.GatheringDetails.Where(b => b.gathering_id == gatheringId).Single());
            }

            return View(gatherings);
        }

        // GET: Gathering/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GatheringClass gatheringClass = db.GatheringDetails.Find(id);
            var userOfGatheringClass = db.UserOfGatheringDetails.Where(b => b.gathering_id == gatheringClass.gathering_id).ToList();
            int size = userOfGatheringClass.Count();
            string[] wplaty=new string[size];
            for(int i=0; i < size; i++)
            {
                int user_id = userOfGatheringClass[i].user_id;
                int user_of_gathering_id = userOfGatheringClass[i].user_of_gathering_id;
                int ile = 0;
                List<PaymentHistoryClass> payments=new List<PaymentHistoryClass>();
                try
                {
                    payments = db.PaymentHistoryDetails.Where(b => b.user_of_gathering_id == user_of_gathering_id).ToList();
                    for(int j = 0; j < payments.Count(); j++)
                    {
                        ile = ile + payments[j].amount_of_payment;
                    }

                }catch(Exception e){; }
                var imie = db.UserDetails.Where(b => b.user_id == user_id).Single().firstname;
                var nazwisko = db.UserDetails.Where(b => b.user_id == user_id).Single().lastname;
                wplaty[i]=imie+" "+nazwisko+" - wplata: "+ile+" zł";
            }
            ViewBag.wplaty = wplaty;
            ViewBag.size = size;
           

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
        public ActionResult Create([Bind(Include = "gathering_id,current_amount,target_amount,finish_date,gathering_name,creator_id,user_of_gathering_id")] GatheringClass gatheringClass, UserOfGatheringClass userOfGatheringClass)
        {
            if (ModelState.IsValid)
            {
                db.GatheringDetails.Add(gatheringClass);
                db.UserOfGatheringDetails.Add(userOfGatheringClass);
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
        public ActionResult Edit([Bind(Include = "gathering_id,current_amount,target_amount,finish_date,gathering_name,creator_id")] GatheringClass gatheringClass)
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
