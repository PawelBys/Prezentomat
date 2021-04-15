
using System.Linq;
using System.Web.Mvc;
using Prezentomat.Models;
using Prezentomat.DataContext;
using System;

namespace Prezentomat.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _context;

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

        [HttpPost]
        public ActionResult Login(UserClass uss)
        {
            var email = "";
            var password = "";
            try
            {
                email = _context.UserDetails.Where(p => p.email == uss.email).Single().email;
                password = _context.UserDetails.Where(p => p.password == uss.password).Single().password;
            }catch(Exception e){;}
            if (!email.Equals("")&&!password.Equals(""))
            {
                return View("UserView");
            }
            else
            {
                return View("Regist");
            }
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