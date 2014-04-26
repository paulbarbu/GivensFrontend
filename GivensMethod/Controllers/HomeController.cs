using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace GivensMethod.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(double[][] a, double[] b)
        {
            //TODO: solve & redirect to Load to show the result from the DB

            Debug.WriteLine("a:");
            for(int i=0; i<a.Length; i++)
            {
                for (int j = 0; j < a[i].Length; j++)
                {

                    Debug.Write(a[i][j] + " ");
                }
                Debug.WriteLine("");
			}

            Debug.WriteLine("b:");
            for (int j = 0; j < b.Length; j++)
            {

                Debug.WriteLine(b[j] + " ");
            }

            ViewBag.a = a;
            ViewBag.b = b;
            
            return View();
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
