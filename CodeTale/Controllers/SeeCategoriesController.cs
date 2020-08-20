using CodeTale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeTale.Controllers
{
    public class SeeCategoriesController : Controller
    {
        // GET: SeeCategories
        public ActionResult Index()
        {
            using (CodetaleDb codetaleDb = new CodetaleDb())
            {
                List<Category> categories = codetaleDb.Categories.ToList();
                return View(categories);
            }
        }
    }
}