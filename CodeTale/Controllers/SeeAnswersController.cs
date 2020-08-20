using CodeTale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeTale.Controllers
{
    public class SeeAnswersController : Controller
    {
        // GET: SeeAnswers
        public ActionResult Index(int questionId)
        {
            using (CodetaleDb codetaleDb = new CodetaleDb())
            {
                List<Answer> answers = codetaleDb.Answers.Where(ans => ans.QuestionId == questionId).ToList();
                Question question = codetaleDb.Questions.Find(questionId);
                ViewData["categoryId"] = question.CategoryId;
                ViewData["questionId"] = questionId;
                return View(answers);
            }
        }
        [HttpGet]
        [Authorize]
        public ActionResult WriteAnswer(int questionId)
        {
            ViewBag.questionId = questionId;

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
        public ActionResult WriteAnswer([Bind(Include = "AnswerStatement, QuestionId, UserID")] Answer answer)
        {
            int questionId = answer.QuestionId;

            if (ModelState.IsValid)
            {
                using (CodetaleDb codetaleDb = new CodetaleDb())
                {
                    Answer ans = new Answer
                    {
                        AnswerStatement = answer.AnswerStatement,
                        QuestionId = answer.QuestionId,
                        UserID=answer.UserID
                    };
                    codetaleDb.Answers.Add(ans);
                    codetaleDb.SaveChanges();

                    return RedirectToAction("Index", "SeeAnswers", new { questionId });
                }
            }
            return RedirectToAction("WriteAnswer", "SeeAnswers", new { questionId });

        }
    }
}