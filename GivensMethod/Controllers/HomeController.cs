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
        MatrixDb db = new MatrixDb();

        [HttpGet]
        public ActionResult Index()
        {
            var matrices = db.MatrixModels.ToList();

            return View(matrices);
        }

        [HttpPost]
        public ActionResult Index(string id, double[][] a, double[] b)
        {
            //TODO: id should be nonempty & unique
            //TODO: add to db
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
            ViewBag.id = id;

            //TODO: redirect to "Load" to solve the system from the DB            
            return View();
        }

        [HttpGet]
        public ActionResult Load(string id)
        {

            //if should be non empty, load from db, solve, show
            if ("" == id)
            {
                return Content("id must be non empty");
            }

            return Content("/load, id: " + id);
        }
    }
}
