using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GivensMethod.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            //return View();
            return Content("/");
        }

        [HttpPost]
        public ActionResult Solve()
        {
            //TODO: get the linear system via POST & solve & redirect to Load to show the result from the DB
            return Content("Solving");
        }

        [HttpGet]
        public ActionResult Load(string id)
        {
            if ("" == id)
            {
                return Content("id must be non empty");
            }

            return Content("/load, id: " + id);
        }
    }
}
