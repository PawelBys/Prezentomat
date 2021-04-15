using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prezentomat.Models;
using Prezentomat.DataContext;
using System.Data.SqlClient;

namespace Prezentomat.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _context;
        private ApplicationDbContext db = new ApplicationDbContext();


        public UserController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: User
        
        public ActionResult UserView()
        {
            return View(_context.UserDetails.ToList());
        }

       
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Regist()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regist([Bind(Include = "userID,email,password,firstname,lastname,birthdate")] UserClass userClass )
        {
            if (ModelState.IsValid)
            {
                //save date to db
                db.UserDetails.Add(userClass);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(userClass);
        }


    }
}