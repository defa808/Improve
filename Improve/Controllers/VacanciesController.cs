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
    public class VacanciesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Vacancies
        public async Task<ActionResult> Index()
        {
            return View(await db.Vacancies.ToListAsync());
        }

        // GET: Vacancies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = await db.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }
            return View(vacancy);
        }

        // GET: Vacancies/Create
        public ActionResult Create()
        {
            var list = new List<SelectListItem>();
            foreach (var item in db.Options)
            {
                list.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Value,
                    Disabled = true

                });
            }
            ViewBag.Tags = list;

            return View();
        }

        // POST: Vacancies/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Company,Text,Salary")] Vacancy vacancy, string[] selectedTags)
        {
            if (selectedTags.Length != 0)
                if (ModelState.IsValid)
                {
                    for (int i = 0; i < selectedTags.Length; i++)
                    {
                        if (Int32.TryParse(selectedTags[i], out int res))
                            vacancy.Tags.Add(db.Options.Find(res));
                    }
                    db.Vacancies.Add(vacancy);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            return View(vacancy);

        }

        // GET: Vacancies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = await db.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }

            List<Option> ops = db.Options.ToList();

            //Added list AllOptions
            var list = new List<SelectListItem>();
            foreach (var item in ops)
            {
                list.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Value,
                    Disabled = vacancy.Tags.Contains(item)

                });
            }
            ViewBag.Tags = list;




            return View(vacancy);
        }

        // POST: Vacancies/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Company,Text,Salary")] Vacancy vacancy, string[] selectedTags)
        {
            if (selectedTags.Length != 0)
                if (ModelState.IsValid)
                {
                    Vacancy v = vacancy;
                    v.Tags.Clear();

                    for (int i = 0; i < selectedTags.Length; i++)
                    {
                        if (Int32.TryParse(selectedTags[i], out int res))
                            v.Tags.Add(db.Options.Find(res));
                    }

                    db.Entry(v).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            return View(vacancy);
        }

        // GET: Vacancies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = await db.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }
            return View(vacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Vacancy vacancy = await db.Vacancies.FindAsync(id);
            db.Vacancies.Remove(vacancy);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
