using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prezentomat.Models;
using Prezentomat.DataContext;

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
    }
}