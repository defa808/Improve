using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Improve.Models;

namespace Improve.Controllers
{
    [Authorize]
    public class OptionsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Options
        public async Task<ActionResult> Index()
        {
            var options = db.Options.Include(o => o.Question);
            return View(await options.ToListAsync());
        }

        // GET: Options/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await db.Options.FindAsync(id);
            if (option == null)
            {
                return HttpNotFound();
            }
            return View(option);
        }


        // GET: Options/Create
        public ActionResult Create(int? id)
        {
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", id);
            ViewBag.IdNextQuestion = new SelectList(db.Questions, "Id", "Text");
            return View();
        }

        // POST: Options/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,QuestionId,Value,IdNextQuestion")] Option option)
        {
            if (ModelState.IsValid)
            {
                db.Options.Add(option);
                await db.SaveChangesAsync();
                return RedirectToAction("../Questions/Index");
            }

            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", option.QuestionId);
            ViewBag.IdNextQuestion = new SelectList(db.Questions, "Id", "Text");

            return View(option);
        }

        // GET: Options/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await db.Options.FindAsync(id);
            if (option == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", option.QuestionId);
            ViewBag.IdNextQuestion = new SelectList(db.Questions, "Id", "Text", option.IdNextQuestion);
            return View(option);
        }

        // POST: Options/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,QuestionId,Value,IdNextQuestion")] Option option)
        {
            if (ModelState.IsValid)
            {
                db.Entry(option).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("../Questions/Index");
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", option.QuestionId);
            ViewBag.IdNextQuestion = new SelectList(db.Questions, "Id", "Text");
            return View(option);
        }

        // GET: Options/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await db.Options.FindAsync(id);
            if (option == null)
            {
                return HttpNotFound();
            }
            return View(option);
        }

        // POST: Options/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Option option = await db.Options.FindAsync(id);
            db.Options.Remove(option);
            await db.SaveChangesAsync();
            return RedirectToAction("../Questions/Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
