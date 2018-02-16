using Improve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using System.Threading.Tasks;
using System.Net;
using System.Security.Claims;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;

namespace Improve.Controllers
{
    public class HomeController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var courses = db.Courses.ToList();
            //Logoff();

            ViewBag.Vacancies = db.Vacancies.ToList();
            return View(courses);

        }

        [HttpPost]
        public void CancelAnswer()
        {
            List<Option> answers = (List<Option>)Session["Answers"];
            if (answers.Count == 0)
            {
                return;
            }
            answers.Remove(answers.Last());

            Session["Answers"] = answers;
        }

        [HttpPost]
        public void ResetAnswers()
        {
            Session["Answers"] = null;
            Session["Questions"] = null;
        }

        public ActionResult LoadCourse()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("ShowCourses", GetCourses());
            }
            return PartialView("ShowCourses", db.Courses);
        }

        public ActionResult LoadVacancies()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("ShowVacancies", GetVacancies());
            }
            return PartialView("ShowVacancies", db.Vacancies);
        }

        private List<Vacancy> GetVacancies()
        {
            List<Option> ans = (List<Option>)Session["Answers"];
            List<Vacancy> vacancies = new List<Vacancy>();

            if (ans == null)
            {
                ans = db.Options.ToList();
                vacancies = db.Vacancies.AsNoTracking().OrderBy(t => t.Id).ToList();
            }
            else
            {
                var allcourses = db.Vacancies.ToList();
                foreach (Vacancy item in allcourses)
                {
                    foreach (Option op in ans)
                    {
                        if (item.Tags.FirstOrDefault(i => i.Id == op.Id && !vacancies.Contains(item)) != null)
                        {
                            vacancies.Add(item);
                        }
                    }
                }
            }
            return vacancies;
        }

        private List<Course> GetCourses()
        {
            List<Option> ans = (List<Option>)Session["Answers"];
            List<Course> courses = new List<Course>();

            if (ans == null)
            {
                ans = db.Options.ToList();
                courses = db.Courses.AsNoTracking().OrderBy(t => t.Id).ToList();
            }
            else
            {
                var allcourses = db.Courses.ToList();
                foreach (Course item in allcourses)
                {
                    foreach (Option op in ans)
                    {
                        if (item.Tags.FirstOrDefault(i => i.Id == op.Id && !courses.Contains(item)) != null)
                        {
                            courses.Add(item);
                        }
                    }
                }
            }

            return courses;
        }

        [HttpPost]
        public ActionResult ShowQuestion(int? optionId)
        {
            List<Question> questions = (List<Question>)Session["Questions"] ?? new List<Question>();
            List<Option> answers = (List<Option>)Session["Answers"] ?? new List<Option>();

            Option op;
            //Not exist in session
            if (optionId == null)
            {
                op = new Option { Id = 0, IdNextQuestion = db.Questions.First().Id };
            }
            else
            {
                op = db.Options.FirstOrDefault(c => c.Id == optionId);
            }

            if (db.Questions.Find(op.IdNextQuestion) == null)
            {
                return HttpNotFound();
            }

            Question question = db.Questions.FirstOrDefault(a => a.Id == op.IdNextQuestion);

            if (question == null)
                return HttpNotFound();





            if (questions.Count == 0)
                questions.Add(question);
            else
            {
                if(op.Id != 0)
                {
                    if (questions.Count == 1 && questions.Find(b => b.Id == question.Id) == null)
                    {
                        questions.Add(question);
                        answers.Add(op);
                    }
                    else
                    {
                        if (questions.Last().Id == question.Id)
                        {
                            questions.Remove(questions.Last());
                            answers.Remove(answers.Last());
                        }
                        else
                        {
                            questions.Add(question);
                            answers.Add(op);
                        }
                    }
                }
                
            }




            //Save all settings in session
            Session["Answers"] = answers;
            Session["Questions"] = questions;

            return PartialView(questions);
        }

        public ActionResult Admin(string returnUrl = "/Questions")
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Admin(LoginViewModel details, string returnUrl)
        {

            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(details.Name + " " + details.Password);
            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            //content heyteplofak
            if (sb.ToString().Equals("9E9502BCB9F2116C000D74047196D8A624276554A9682D67F5735AF44B647BCC"))
            {

                FormsAuthentication.SetAuthCookie(details.Name, true);
                return Redirect(returnUrl);

            }
            else
            {
                ModelState.AddModelError("AdminModel", "Некорректное имя или пароль.");
                ViewBag.returnUrl = returnUrl;
                return View();
            }


        }

        public ActionResult Logoff()
        {
            Session["Answers"] = null;
            Session["Questions"] = null;

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}