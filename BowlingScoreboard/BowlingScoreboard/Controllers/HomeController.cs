using BowlingScoreboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BowlingScoreboard.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            HomeModel modelHome = new HomeModel();
            return View("~/Views/Home.cshtml", modelHome);
        }
	}
}