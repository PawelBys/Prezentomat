
using System.Linq;
using System.Web.Mvc;
using Prezentomat.Models;
using Prezentomat.DataContext;
using System;
using System.Web.Routing;
using System.Web;
using Prezentomat.Classes;
using System.Windows;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.Net;
using System.Data.Entity;

namespace Prezentomat.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _context;
        int id;
        string user_name;
        int wallet;


        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            //var Session = System.Web.HttpContext.Current.Session;
            if(Session!=null)
            {
                id = (int)Session["UserID"];
                user_name = _context.UserDetails.Where(p => p.user_id == id).Single().firstname;
                wallet = _context.UserDetails.Where(p => p.user_id == id).Single().wallet;
                ViewBag.user_name = user_name;
                ViewBag.user_id = id;
                ViewBag.wallet = wallet;
            }


            return base.BeginExecute(requestContext, callback, state);
        }

        public UserController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: User
        

        //chuj kurwa 
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                id = (int)Session["UserID"];
                user_name = _context.UserDetails.Where(p => p.user_id == id).Single().firstname;
                ViewBag.user_name = user_name;
                wallet = _context.UserDetails.Where(p => p.user_id == id).Single().wallet;
                ViewBag.wallet = wallet;

                var userOfGatherings = _context.UserOfGatheringDetails.Where(b => b.user_id == id).ToList();
                List<GatheringClass> gatherings = new List<GatheringClass>();
                for (int i = 0; i < userOfGatherings.Count(); i++)
                {
                    int gatheringId = userOfGatherings[i].gathering_id;
                    gatherings.Add(_context.GatheringDetails.Where(b => b.gathering_id == gatheringId).Single());
                }

                ViewBag.gatherings = gatherings;
                ViewBag.size = gatherings.Count();

                return View(_context.UserDetails.Where(p => p.user_id == id).Single());
            }
            else
            {
                return View("Login");
            }
            
        }

       
        public ActionResult Login()
        {
            return View();
        }

       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "email, password")] UserClass userClass)
        {
            if (ModelState.IsValidField("email") && ModelState.IsValidField("password")) 
            {
                var email = "";
                var password = "";
                var user_id = 0;
                var userEmail = userClass.email;

                try
                {
                    email = _context.UserDetails.Where(p => p.email == userEmail).Single().email;
                    password = _context.UserDetails.Where(p => p.email == userEmail).Single().password;
                    user_id = _context.UserDetails.Where(p => p.email == userEmail).Single().user_id;

                }
                catch(Exception e){;}
                if (email.Equals(userClass.email)&&password.Equals(Hash.ComputeSha512Hash(userClass.password)))
                {

                    //zalogowany
                    Session["UserID"] = user_id;
                    return RedirectToAction("Index"/*, new { id = user_id }*/);
                }
                else
                {
                    //zle dane
                    ModelState.AddModelError("email", "Email i hasło nie zgadzają się");
                    return View(userClass);
                }
            }
            else
            {
                ModelState.AddModelError("email", "Niepoprawne dane");
                return View(userClass);
            }

            return View();
        }
        
        public ActionResult Regist()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regist([Bind(Include = "userID,email,password, repeat_password, firstname,lastname,birthdate")] UserClass userClass)
        {
            if (ModelState.IsValid)
            {
                if( !ifUserExist(userClass.email)  ) //jesli taki użytkownik nie istnieje
                {

                
                    if (userClass.repeat_password.Equals(userClass.password)){
                                        //save date to db
                            _context.UserDetails.Add(new UserClass()
                            {
                                user_id = userClass.user_id,
                                email = userClass.email,
                                password = Hash.ComputeSha512Hash(userClass.password),
                                repeat_password = Hash.ComputeSha512Hash(userClass.repeat_password),
                                firstname = userClass.firstname,
                                lastname = userClass.lastname,
                                birthdate = userClass.birthdate.ToString()
                            });
                            _context.SaveChanges();
                            return RedirectToAction("Login");
                    }
                    else
                    {
                        //hasła się nie zgadzają
                        ModelState.AddModelError("repeat_password", "Hasła się nie zgadzają.");

                    }
                }
                else
                {
                    ModelState.AddModelError("email", "Taki użytkownik już istnieje.");
                }


            }

            return View(userClass);
        }

        private bool ifUserExist (string userEmail)
        {
            if (_context.UserDetails.Any(p => p.email == userEmail))
            {
                return true;
            }

            return false;
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;

            return RedirectToAction("Login");
        }

        public ActionResult AddCash()
        {

            id = (int)Session["UserID"];
            user_name = _context.UserDetails.Where(p => p.user_id == id).Single().firstname;
            ViewBag.user_name = user_name;
            wallet = _context.UserDetails.Where(p => p.user_id == id).Single().wallet;
            ViewBag.wallet = wallet;
            return View(_context.UserDetails.Where(p => p.user_id == id).Single());

            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCash([Bind(Include = "wallet")] UserClass userClass)
        {
            if (ModelState.IsValidField("wallet"))
            {

                id = (int)Session["UserID"];
                user_name = _context.UserDetails.Where(p => p.user_id == id).Single().firstname;
                ViewBag.user_name = user_name;
                wallet = _context.UserDetails.Where(p => p.user_id == id).Single().wallet;
                ViewBag.wallet = wallet;

                //return View();

                //wyszukanie po id
                var temp_user = _context.UserDetails.Find(id);
                //MessageBox.Show(id.ToString());

                if (temp_user != null)
                {
                    temp_user.wallet = temp_user.wallet + userClass.wallet;
                    _context.SaveChanges();
                };
                //return View(_context.UserDetails.Where(p => p.user_id == id).Single());
            }

            //return View(_context.UserDetails.Where(p => p.user_id == id).Single());
            return RedirectToAction("AddCash");
        }


        // GET: UserClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            id = (int)Session["UserID"];
            user_name = _context.UserDetails.Where(p => p.user_id == id).Single().firstname;
            ViewBag.user_name = user_name;
            wallet = _context.UserDetails.Where(p => p.user_id == id).Single().wallet;
            ViewBag.wallet = wallet;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserClass userClass = _context.UserDetails.Find(id);
            if (userClass == null)
            {
                return HttpNotFound();
            }
            return View(userClass);
        }

        // POST: UserClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "email,firstname,lastname,birthdate")] UserClass userClass)
        {
            id = (int)Session["UserID"];
            user_name = _context.UserDetails.Where(p => p.user_id == id).Single().firstname;
            ViewBag.user_name = user_name;
            wallet = _context.UserDetails.Where(p => p.user_id == id).Single().wallet;
            ViewBag.wallet = wallet;

            if (ModelState.IsValidField("email") && ModelState.IsValidField("firstname") && ModelState.IsValidField("lastname") && ModelState.IsValidField("birthdate")) 
            {
                _context.Entry(userClass).State = EntityState.Modified;
               // _context.SaveChanges();

                // tu sypie błędem

                return RedirectToAction("Index");
            }
            return View(userClass);
        }



    }
}