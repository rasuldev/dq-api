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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using DrinqWeb.Tools.AssignmentTools;

namespace DrinqWeb.Controllers
{
    [Authorize]
    public class VerificationItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public VerificationItemsController()
        {
        }

        public VerificationItemsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: VerificationItems
        public ActionResult Index()
        {
            return View(db.VerificationItems.Include("Media").ToList());
        }

        // GET: VerificationItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VerificationItem verificationItem = db.VerificationItems.Find(id);
            if (verificationItem == null)
            {
                return HttpNotFound();
            }
            return View(verificationItem);
        }

        // GET: VerificationItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VerificationItems/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IncomingDate,VerifiedDate,VerifiedById,Status,Message")] VerificationItem verificationItem)
        {
            if (ModelState.IsValid)
            {
                db.VerificationItems.Add(verificationItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(verificationItem);
        }

        // GET: VerificationItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VerificationItem verificationItem = db.VerificationItems.Include("Media").Where(item => item.Id == id).FirstOrDefault();
            if (verificationItem == null)
            {
                return HttpNotFound();
            }
            return View(verificationItem);
        }

        // POST: VerificationItems/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IncomingDate,VerifiedDate,VerifiedById,Status,Message")] VerificationItem verificatedItem)
        {
            if (ModelState.IsValid)
            {
                VerificationItem verificationItem = db.VerificationItems.Include("Media").Include("UserAssignment").Where(item => item.Id == verificatedItem.Id).FirstOrDefault();
                verificationItem.Status = verificatedItem.Status;
                verificationItem.Message = verificatedItem.Message;
                verificationItem.VerifiedById = User.Identity.GetUserId();
                verificationItem.VerifiedDate = DateTime.Now;
                switch (verificatedItem.Status)
                {
                    case VerificationItemStatus.Accepted:
                        verificationItem.UserAssignment.MediaAccepted = UserAssignmentAcceptedStatus.Accepted;
                        break;
                    case VerificationItemStatus.Declined:
                        verificationItem.UserAssignment.MediaAccepted = UserAssignmentAcceptedStatus.Declined;
                        break;
                    case VerificationItemStatus.NotVerified:
                        verificationItem.UserAssignment.MediaAccepted = UserAssignmentAcceptedStatus.Verifying;
                        break;
                }
                db.Entry(verificationItem).State = EntityState.Modified;
                db.SaveChanges();
                AssignmentFactory aFactory = new AssignmentFactory();
                aFactory.SetNextUserAssignmentIfFinished(db, aFactory.GetCurrentUserAssignment(db, verificationItem.UserAssignment.User.Id), verificationItem.UserAssignment.User.Id);
                return RedirectToAction("Index");
            }
            return View(verificatedItem);
        }

        // GET: VerificationItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VerificationItem verificationItem = db.VerificationItems.Find(id);
            if (verificationItem == null)
            {
                return HttpNotFound();
            }
            return View(verificationItem);
        }

        // POST: VerificationItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VerificationItem verificationItem = db.VerificationItems.Find(id);
            db.VerificationItems.Remove(verificationItem);
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
