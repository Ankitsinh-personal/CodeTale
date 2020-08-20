using CodeTale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeTale.Controllers
{
    public class SeeQuestionsController : Controller
    {
        // GET: SeeQuestions
        public ActionResult Index(int categoryId)
        {
            using (CodetaleDb codetaleDb = new CodetaleDb())
            {
                List<Question> questions = codetaleDb.Questions.Where(que => que.CategoryId == categoryId).ToList();
                ViewData["categoryId"] = categoryId;
                return View(questions);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult AskQuestion(int categoryId)
        {
            ViewBag.categoryId = categoryId;
            var v = System.Web.HttpContext.Current.User.Identity.Name;
            User u1;
            using (CodetaleDb codetaleDb = new CodetaleDb())
            {
                var user = codetaleDb.Users.Where(u => u.EmailID == v).FirstOrDefault();
                u1 = user;
            }
            ViewBag.UserID = u1.UserID;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AskQuestion([Bind(Include = "QuestionStatement, CategoryId, UserID")]Question question)
        {
            int categoryId = question.CategoryId;
            if (ModelState.IsValid)
            {
                using (CodetaleDb codetaleDb = new CodetaleDb())
                {
                    Question que = new Question
                    {

                        QuestionStatement = question.QuestionStatement,
                        CategoryId = question.CategoryId,
                        UserID=question.UserID

                    };
                    codetaleDb.Questions.Add(que);
                    codetaleDb.SaveChanges();
                    return RedirectToAction("Index", new { categoryId });
                }
            }
            return RedirectToAction("AskQuestion", new { categoryId });
        }

        [HttpGet]
        public ActionResult UserQuestion(int UserID)
        {
            using (CodetaleDb codetaleDb = new CodetaleDb())
            {
                List<Question> questions=codetaleDb.Questions.Where(que=>que.UserID==UserID).ToList();
                ViewBag.UserID = UserID;
                return View(questions);
            }
            
        }

    }
}