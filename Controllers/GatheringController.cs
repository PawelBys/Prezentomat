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
        private ApplicationDbContext _context = new ApplicationDbContext();
        int uid;
        string user_name;
        int wallet;

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var Session = System.Web.HttpContext.Current.Session;
            if (Session != null)
            {
                uid = (int)Session["UserID"];
                user_name = _context.UserDetails.Where(p => p.user_id == uid).Single().firstname;
                ViewBag.user_name = user_name;
                wallet = _context.UserDetails.Where(p => p.user_id == uid).Single().wallet;
                ViewBag.wallet = wallet;
            }


            return base.BeginExecute(requestContext, callback, state);
        }

        // GET: Gathering
        public ActionResult Index()
        {
            //int userId = Convert.ToInt32(Session["id"]);
            //var bookings = db.GatheringDetails.Where(b => b.creator_id == 1).ToList();


            //wyswietlenie zbiurek tylko zalogowanego uzytkownika
            var userOfGatherings = _context.UserOfGatheringDetails.Where(b=>b.user_id== uid).ToList();
            List<GatheringClass> gatherings=new List<GatheringClass>();
            for(int i = 0; i < userOfGatherings.Count(); i++)
            {
                int gatheringId = userOfGatherings[i].gathering_id;
                gatherings.Add(_context.GatheringDetails.Where(b => b.gathering_id == gatheringId).Single());
            }

            return View(gatherings);
        }

        // GET: Gathering/Details/5
        public ActionResult Details(int? id)
        {
         // stad wziac #######################
            var userGatherings = _context.UserOfGatheringDetails.Where(p => p.user_id == uid).Select(a => a.gathering_id).ToList();
            int notNullableGatheringId = id.GetValueOrDefault();
            var ifUserHasThisGathering = _context.UserOfGatheringDetails.Any(x => userGatherings.Contains(notNullableGatheringId)); // musi być tak, bo id może być nullem
            ViewBag.gathering_id = notNullableGatheringId;
            if (id == null || !ifUserHasThisGathering) // jesli user moze oglada zbiorke (jesli jest w tabeli user of gathering)
            {
                // tutaj ewentualnie coś w stylu " nie masz uprawnień do oglądania tej witryny"
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
           

            GatheringClass gatheringClass = _context.GatheringDetails.Find(id);
            var userOfGatheringClass = _context.UserOfGatheringDetails.Where(b => b.gathering_id == gatheringClass.gathering_id).ToList();
            //  zrobic tutaj zliczanie nie uzytkownikow zbiorki, bo wyswietlaja sie tez uzytkownicy ktorzy nic nie wplacili, ale liczyc w tabeli payment_history
            int size = userOfGatheringClass.Count();
            int s = 0;
            List<string> wplaty = new List<string>();
            for(int i=0; i < size; i++)
            {
                int user_id = userOfGatheringClass[i].user_id;
                int user_of_gathering_id = userOfGatheringClass[i].user_of_gathering_id;
                int ile = 0;
                List<PaymentHistoryClass> payments=new List<PaymentHistoryClass>();
                try
                {
                    payments = _context.PaymentHistoryDetails.Where(b => b.user_of_gathering_id == user_of_gathering_id).ToList();
                    for(int j = 0; j < payments.Count(); j++)
                    {
                        ile = ile + payments[j].amount_of_payment;
                    }

                }catch(Exception e){; }
                var imie = _context.UserDetails.Where(b => b.user_id == user_id).Single().firstname;
                var nazwisko = _context.UserDetails.Where(b => b.user_id == user_id).Single().lastname;
                if( !payments.Equals(""))
                {
                    if (ile > 0)
                    {
                        wplaty.Add(imie + " " + nazwisko + " - wplata: " + ile + " zł");
                        s++;
                    }
                }
                
            }
            if (uid == gatheringClass.creator_id)
            {
                ViewBag.creator = true;
            }
            else
            {
                ViewBag.creator = false;
            }
            ViewBag.wplaty = wplaty;
            ViewBag.size = s;
            

            if (gatheringClass == null)
            {
                return HttpNotFound();
            }
            return View(gatheringClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int wallet, int? id)
        {
            var userofgathering = _context.UserOfGatheringDetails.Where(p => p.user_id == uid).Where(a =>a.gathering_id == id).Single();
             _context.PaymentHistoryDetails.Add(new PaymentHistoryClass()
             {
                 user_of_gathering_id = userofgathering.user_of_gathering_id,
                 payment_date = DateTime.Now,
                 amount_of_payment = wallet

             });

             var temp_user = _context.UserDetails.Find(uid);

             if (temp_user != null)
             {
                 temp_user.wallet = temp_user.wallet - wallet;
                 _context.SaveChanges();
             };
            
            _context.SaveChanges();


            return RedirectToAction("Details", id);
        }

        // GET: Gathering/AddUser
        public ActionResult AddUser(int? id)
        {

            var users = _context.UserDetails.ToList();

            var userOfGatherings = _context.UserOfGatheringDetails.Where(b => b.gathering_id == id).ToList();
            for (int i = 0; i < userOfGatherings.Count(); i++)
            {
                int userId = userOfGatherings[i].user_id;
                users.Remove(czyUser(users,userId));
            }

            return View(users);
        }

        private UserClass czyUser(List<UserClass> users, int userId)
        {
            for(int i = 0; i < users.Count(); i++)
            {
                if (users[i].user_id == userId)
                {
                    return users[i];
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult AddUser(UserClass userClass,int? id)
        {
            
            _context.UserOfGatheringDetails.Add(new UserOfGatheringClass()
            {
                user_id=userClass.user_id,
                gathering_id= (int)id,
                joining_date= DateTime.Now

            });
            _context.SaveChanges();


            return RedirectToAction("AddUser", id);
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
        public ActionResult Create(AddGatheringModel addGatheringModel)
        {
            if (ModelState.IsValid)
            {
                GatheringClass gathering = new GatheringClass();
                gathering.current_amount = 0;
                gathering.target_amount = addGatheringModel.target_amount;
                gathering.finish_date = addGatheringModel.finish_date;
                gathering.gathering_name = addGatheringModel.gathering_name;
                gathering.creator_id = uid;
                _context.GatheringDetails.Add(gathering);
                _context.SaveChanges();

                UserOfGatheringClass userOfGathering = new UserOfGatheringClass();

                userOfGathering.user_id = uid;
                userOfGathering.gathering_id = gathering.gathering_id;
                userOfGathering.joining_date = DateTime.Now;
                _context.UserOfGatheringDetails.Add(userOfGathering);   
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(addGatheringModel);
        }

        // GET: Gathering/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GatheringClass gatheringClass = _context.GatheringDetails.Find(id);
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
                _context.Entry(gatheringClass).State = EntityState.Modified;
                _context.SaveChanges();
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
            GatheringClass gatheringClass = _context.GatheringDetails.Find(id);
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
            GatheringClass gatheringClass = _context.GatheringDetails.Find(id);
            _context.GatheringDetails.Remove(gatheringClass);
            _context.SaveChanges();
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
