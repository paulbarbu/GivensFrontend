using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GivensAlgorithms;

namespace GivensMethod.Models
{
    public class LoadViewModel
    {
        public Matrix X { get; set; }
        public Matrix A { get; set; }
        public Matrix b { get; set; }
        public IEnumerable<string> ids { get; set; }
        public string id { get; set; }
    }
}