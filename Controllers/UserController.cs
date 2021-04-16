
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

        //niby dziala poprawnie ale nie otwiera sie user view bo tam jest blad 
        [HttpPost]
        public ActionResult Login([Bind(Include = "email,password")] UserClass userClass)
        {
            var email = "";
            var password = "";
            try
            {
                email = _context.UserDetails.Where(p => p.email == userClass.email).Single().email;
                password = _context.UserDetails.Where(p => p.password == userClass.password).Single().password;
            }catch(Exception e){;}
            if (!email.Equals("")&&!password.Equals(""))
            {
                //zalogowany
                return View("UserView");
            }
            else
            {
                //zle dane
                return RedirectToAction("Regist");
            }
        }

        public ActionResult Regist()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regist([Bind(Include = "userID,email,password,firstname,lastname,birthdate")] UserClass userClass)
        {
            if (ModelState.IsValid)
            {
                //save date to db
                _context.UserDetails.Add(userClass);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(userClass);
        }


    }
}