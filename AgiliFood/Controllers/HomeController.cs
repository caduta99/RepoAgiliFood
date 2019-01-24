using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgiliFood.Controllers
{
    public class HomeController : Controller
    {
        //Retorna a View "Index"
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //Retorna a View "Menu"
        [HttpGet]
        public ActionResult Menu()
        {
            return View();
        }
    }
}