
using System.Linq;
using System.Web.Mvc;
using Prezentomat.Models;
using Prezentomat.DataContext;
using System;
using System.Web.Routing;
using System.Web;

namespace Prezentomat.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _context;
        int id;
        string user_name;

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            //var Session = System.Web.HttpContext.Current.Session;
            if(Session!=null)
            {
                id = (int)Session["UserID"];
                user_name = _context.UserDetails.Where(p => p.user_id == id).Single().firstname;
                ViewBag.user_name = user_name;
            }


            return base.BeginExecute(requestContext, callback, state);
        }
        public UserController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: User
        
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                id = (int)Session["UserID"];
                user_name = _context.UserDetails.Where(p => p.user_id == id).Single().firstname;
                ViewBag.user_name = user_name;

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
        public ActionResult Login([Bind(Include = "email,password")] UserClass userClass)
        {
            var email = "";
            var password = "";
            var user_id =0;
            try
            {
                email = _context.UserDetails.Where(p => p.email == userClass.email).Single().email;
                password = _context.UserDetails.Where(p => p.email == userClass.email).Single().password;
                user_id = _context.UserDetails.Where(p => p.email == userClass.email).Single().user_id;
            }
            catch(Exception e){;}
            if (email.Equals(userClass.email)&&password.Equals(userClass.password))
            {
                //zalogowany
                Session["UserID"] = user_id;
                return RedirectToAction("Index"/*, new { id = user_id }*/);
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

        public ActionResult Logout()
        {
            Session["UserID"] = null;

            return RedirectToAction("Login");
        }


    }
}