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
    public class CoursesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Courses
        public async Task<ActionResult> Index()
        {
            return View(await db.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
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

        // POST: Courses/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Header,Company,Text,Tags")] Course course, string[] selectedTags)
        {
            if (selectedTags.Length != 0)
                if (ModelState.IsValid)
                {
                    for (int i = 0; i < selectedTags.Length; i++)
                    {
                        if (Int32.TryParse(selectedTags[i], out int res))
                            course.Tags.Add(db.Options.Find(res));
                    }
                    db.Courses.Add(course);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
           
                if (course == null)
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
                    Disabled = course.Tags.Contains(item)

                });
            }
            ViewBag.Tags = list;


            return View(course);
        }

        // POST: Courses/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Header,Company,Text,Tags")] Course course, string[] selectedTags)
        {
            if (selectedTags.Length != 0)
                if (ModelState.IsValid)
                {
                    Course c = course;
                    c.Tags.Clear();

                    for (int i = 0; i < selectedTags.Length; i++)
                    {
                        if (Int32.TryParse(selectedTags[i], out int res))
                            c.Tags.Add(db.Options.Find(res));
                    }

                    db.Entry(c).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Course course = await db.Courses.FindAsync(id);
            db.Courses.Remove(course);
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
