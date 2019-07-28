using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieRank.Models;
using System.Data;
using System.Data.Entity;

namespace MovieRank.Controllers
{
    public class HomeController : Controller
    {
        private MovieRankEntities db = new MovieRankEntities();
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
                return View();
        }
    }
}