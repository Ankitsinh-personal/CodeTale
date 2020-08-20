using CodeTale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeTale.Controllers
{
    public class EditQuestionController : Controller
    {
        // GET: EditQuestion
        [Authorize]
        public ActionResult Index(int questionId)
        {
            Question que;
            var v = System.Web.HttpContext.Current.User.Identity.Name;
            User u1;
            using (CodetaleDb codetaleDb = new CodetaleDb())
            {
                Question que1 = codetaleDb.Questions.Find(questionId);

                var user = codetaleDb.Users.Where(u => u.EmailID == v).FirstOrDefault();
                u1 = user;
                que = que1;
            }
            ViewBag.UserID = u1.UserID;
            return View(que);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "QuestionStatement, QuestionId,CategoryId,UserID")] Question question)
        {
            int categoryId = question.CategoryId;
            int questionId = question.QuestionId;
            if (ModelState.IsValid) {
                using (CodetaleDb codetaleDb = new CodetaleDb())
                {
                    Question que = codetaleDb.Questions.Find(questionId);
                    que.QuestionStatement = question.QuestionStatement;
                    codetaleDb.SaveChanges();
                }
                return RedirectToAction("Index", "SeeQuestions", new { categoryId });
            }
            return RedirectToAction("Index", new { questionId });
            
        }
        [HttpDelete]
        public ActionResult Index()
        {
            return View();
        }
    }
}