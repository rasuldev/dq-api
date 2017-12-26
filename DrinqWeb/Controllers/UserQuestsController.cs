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
using DrinqWeb.Models.Filter;

namespace DrinqWeb.Controllers
{
    [Authorize]
    public class UserQuestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserQuests
        public ActionResult Index(FilterUserQuestStatus? userQuestStatus, DateTime? StartDate, DateTime? EndDate)
        {
            IQueryable<UserQuest> userQuests = db.UserQuests.Include(item => item.Quest).Include(item => item.User);
            if (userQuestStatus != null && userQuestStatus != FilterUserQuestStatus.All)
            {
                userQuests = userQuests.Where(p => p.Status == (UserQuestStatus)userQuestStatus);
            }

            if (StartDate != null)
            {
                userQuests = userQuests.Where(p => p.StartDate > StartDate);
            }

            if (EndDate != null)
            {
                userQuests = userQuests.Where(p => p.EndDate < EndDate);
            }

            var statuses = Enum.GetValues(typeof(FilterUserQuestStatus)).Cast<FilterUserQuestStatus>().ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            UserQuestsListViewModel uqlvm = new UserQuestsListViewModel
            {
                UserQuests = userQuests.ToList(),
                Status = new SelectList(statuses, FilterUserQuestStatus.All)
            };

            return View(uqlvm);
        }

        // GET: UserQuests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuest userQuest = db.UserQuests.Include(item => item.Quest).Include(item => item.User).Where(item => item.Id == id).FirstOrDefault();
            if (userQuest == null)
            {
                return HttpNotFound();
            }
            UserQuestDetailViewModel model = new UserQuestDetailViewModel();
            model.UserQuest = userQuest;
            model.TotalAssignmentsCount = db.Assignments.Where(a => a.Quest.Id == userQuest.Quest.Id).Count();
            model.FinishedAssignmentCount = db.UserAssignments.Where(ua => ua.UserQuest.Id == userQuest.Id && ua.Status == UserAssignmentStatus.Completed).Count();

            var curAssignments = db.UserAssignments.Where(ua => ua.UserQuest.Id == userQuest.Id && ua.Status == UserAssignmentStatus.InProgress).Include(item => item.Assignment).ToList();
            var titles = "Нет активных";
            if (curAssignments.Count > 0)
            {
                titles = "";
                for (int i = 0; i < curAssignments.Count - 1; i++)
                {
                    titles += curAssignments[i].Assignment.Title + ", ";
                }
                titles += curAssignments.Last().Assignment.Title;
            }
            model.CurrentAssignmentsTitle = titles;
            return View(model);
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
