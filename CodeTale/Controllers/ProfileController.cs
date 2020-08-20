using CodeTale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeTale.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            var v = System.Web.HttpContext.Current.User.Identity.Name;
            User u1;
            using (CodetaleDb codetaleDb = new CodetaleDb())
            {
                var user = codetaleDb.Users.Where(u => u.EmailID == v).FirstOrDefault();
                u1 = user;
            }
           

            return View(u1);

        }
    }
}