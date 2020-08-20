using CodeTale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeTale.Controllers
{
    public class SeeAlgorithmsController : Controller
    {
        public ActionResult Index()
        {
            using (CodetaleDb db = new CodetaleDb())
            {
                List<Algorithm> algorithms = db.Algorithms.ToList();
                return View(algorithms);
            }
        }

        public ActionResult SeeAlgorithm(int algorithmId)
        {
            using (CodetaleDb db = new CodetaleDb())
            {
                Algorithm algorithm = db.Algorithms.Find(algorithmId);
                return View(algorithm);
            }
        }
    }
}