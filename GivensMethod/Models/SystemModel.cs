using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GivensMethod.Models.ModelBinders;

namespace GivensMethod.Models
{
    [ModelBinder(typeof(SystemModelBinder))]
    public class SystemModel
    {
        public string id { get; set; }
        public double[,] a { get; set; }
        public double[] b { get; set; }
    }
}