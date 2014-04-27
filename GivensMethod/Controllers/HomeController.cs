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
using System.Text;

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
                    model.matrix = "";

                    StreamReader reader;
                    Stream ms = new MemoryStream();
                    IFormatter f = new BinaryFormatter();

                    f.Serialize(ms, system.a);

                    using (reader = new StreamReader(ms, Encoding.ASCII))
                    {
                        ms.Position = 0;
                        model.matrix += reader.ReadToEnd();
                    }

                    ms = new MemoryStream();
                    f.Serialize(ms, system.b);

                    using (reader = new StreamReader(ms, Encoding.ASCII))
                    {
                        ms.Position = 0;
                        model.matrix += MARKER + reader.ReadToEnd();
                    }

                    db.MatrixModels.AddObject(model);
                    db.SaveChanges();

                    return RedirectToAction("Load", new { id = system.id });
                }
            }

            return View();
        }

        public ActionResult Load(string id)
        {
            //id should be non empty, load from db if id exists, solve, show            

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
            
            MemoryStream ms = new MemoryStream();
            StreamWriter sw;
            IFormatter f = new BinaryFormatter();

            using (sw = new StreamWriter(ms))
            {
                sw.Write(aMatrix);
                sw.Flush();

                ms.Position = 0;
                a = (double[,])f.Deserialize(ms);
            }

            ms = new MemoryStream();
            using (sw = new StreamWriter(ms))
            {
                sw.Write(bMatrix);
                sw.Flush();

                ms.Position = 0;
                b = (double[])f.Deserialize(ms);
            }
            
            return Tuple.Create<double[,], double[]>(a, b);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
