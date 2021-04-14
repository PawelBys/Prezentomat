using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prezentomat.Models;
using Prezentomat.DataContext;


namespace Prezentomat.Controllers
{
    public class GatheringController : Controller
    {
        ApplicationDbContext _context;

        public GatheringController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Gathering
        public ActionResult GatheringView()
        {
            return View(_context.GatheringDetails.ToList());
        }
    }
}