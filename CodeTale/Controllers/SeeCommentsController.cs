using CodeTale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CodeTale.Controllers
{
    public class SeeCommentsController : Controller
    {
        // GET: SeeComments
        public ActionResult Index(int algorithmId)
        {
            using (CodetaleDb db = new CodetaleDb())
            {
                List<Comment> cmt = db.Comments.Where(x => x.AlgorithmId == algorithmId).ToList();
                ViewBag.algorithmId = algorithmId;
                return View(cmt);
            }
        }

        [Authorize]
        public ActionResult writecmt(int algorithmId)
        {
            var v = System.Web.HttpContext.Current.User.Identity.Name;
            User u1;
            using (CodetaleDb codetaleDb = new CodetaleDb())
            {
                var user = codetaleDb.Users.Where(u=>u.EmailID==v).FirstOrDefault();
                u1 = user;
            }
                ViewBag.algorithmId = algorithmId;
            
            ViewBag.Name = v;
            ViewBag.UserID = u1.UserID;
            return View();

        }
        [HttpPost]
        public ActionResult SaveRecord([Bind(Include = "Commentstmt, AlgorithmId, UserID")]Comment model)
        {
            int algorithmId = model.AlgorithmId;
            using (CodetaleDb db = new CodetaleDb())
            {

                Comment cmt = new Comment();
                cmt.Commentstmt = model.Commentstmt;
                cmt.AlgorithmId = model.AlgorithmId;
                cmt.UserID = model.UserID;
                db.Comments.Add(cmt);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { algorithmid=algorithmId});
        }

        public ActionResult UserComments(int UserID)
        {
            using (CodetaleDb db = new CodetaleDb())
            {
                List<Comment> cmt = db.Comments.Where(x => x.UserID == UserID).ToList();
             //   ViewBag.UserID = UserID;
                return View(cmt);
            }

        }
    }
}
