using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;

namespace DrinqWeb.Controllers
{
    public class UserQuestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserQuests
        public ActionResult Index()
        {
            return View(db.UserQuests.Include(item => item.Quest).Include(item => item.User).ToList());
        }

        // GET: UserQuests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuest userQuest = db.UserQuests.Find(id);
            if (userQuest == null)
            {
                return HttpNotFound();
            }
            return View(userQuest);
        }

        // GET: UserQuests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserQuests/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartDate,EndDate,Status")] UserQuest userQuest)
        {
            if (ModelState.IsValid)
            {
                db.UserQuests.Add(userQuest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userQuest);
        }

        // GET: UserQuests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuest userQuest = db.UserQuests.Find(id);
            if (userQuest == null)
            {
                return HttpNotFound();
            }
            return View(userQuest);
        }

        // POST: UserQuests/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartDate,EndDate,Status")] UserQuest userQuest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userQuest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userQuest);
        }

        // GET: UserQuests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuest userQuest = db.UserQuests.Find(id);
            if (userQuest == null)
            {
                return HttpNotFound();
            }
            return View(userQuest);
        }

        // POST: UserQuests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserQuest userQuest = db.UserQuests.Find(id);
            db.UserQuests.Remove(userQuest);
            db.SaveChanges();
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
