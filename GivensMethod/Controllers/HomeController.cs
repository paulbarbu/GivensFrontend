using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using GivensMethod.Models;
using GivensAlgorithms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;

namespace GivensMethod.Controllers
{
    public class HomeController : Controller
    {
        private const string MARKER = "$paul$";
        private MatrixDb db = new MatrixDb();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SystemModel system)
        {            
            if (ModelState.IsValid)
            {
                var ids = from s in db.MatrixModels
                          orderby s.id
                          select s.id;

                if (ids.Contains(system.id))
                {
                    ModelState.AddModelError("Empty", "Numele sistemului trebuie să fie unic!");
                }
                else
                {
                    MatrixModel model = new MatrixModel();
                    model.id = system.id;

                    model.matrix = JsonConvert.SerializeObject(system.a);
                    model.matrix += MARKER + JsonConvert.SerializeObject(system.b);

                    db.MatrixModels.AddObject(model);
                    db.SaveChanges();

                    return RedirectToAction("Load", new { id = system.id });
                }
            }

            return View();
        }

        public ActionResult Load(string id)
        {
            var ids = from s in db.MatrixModels
                      orderby s.id
                      select s.id;

            LoadViewModel model = new LoadViewModel();
            model.X = null;
            model.ids = ids;

            //this prevents showing the error message if I only access /load or if there is nothing in the DB
            if ("" != id && ids.Count() > 0)
            {
                if (!ids.Contains(id))
                {
                    ModelState.AddModelError("Empty", "Numele sistemului trebuie să existe!");
                }
                else
                {
                    var system = from s in db.MatrixModels
                                 where s.id == id
                                 select s.matrix;

                    Tuple<double[,], double[]> t = systemToMatrices(system.First().ToString());
                    Matrix A = new Matrix(t.Item1);
                    Matrix b = new Matrix(t.Item2);

                    GivensAlgorithm gm = new GivensAlgorithm(A, b);
                    Matrix X = gm.solve();

                    model.X = X;
                    model.A = A;
                    model.b = b;
                    model.id = id;
                }
            }

            return View(model);
        }

        private Tuple<double[,], double[]> systemToMatrices(string system)
        {
            double[,] a;
            double[] b;

            int pos = system.IndexOf(MARKER);
            string aMatrix = system.Substring(0, pos);
            string bMatrix = system.Substring(pos + MARKER.Length);

            a = JsonConvert.DeserializeObject<double[,]>(aMatrix);
            b = JsonConvert.DeserializeObject<double[]>(bMatrix);
            
            return Tuple.Create<double[,], double[]>(a, b);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
