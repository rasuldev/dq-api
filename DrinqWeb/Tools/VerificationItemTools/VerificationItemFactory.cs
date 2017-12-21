using System;
using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
using DrinqWeb.Tools.AssignmentTools;
using System.Linq;

namespace DrinqWeb.Tools.VerificationItemTools
{
    public class VerificationItemFactory
    {
        public VerificationItemFactory()
        {

        }

        public VerificationItem CreateVerificationItem(ApplicationDbContext db, Media media, string userId)
        {
            AssignmentFactory assignmentFactory = new AssignmentFactory();
            VerificationItem item = new VerificationItem();
            item.IncomingDate = DateTime.Now;
            item.Media = media;
            item.Status = VerificationItemStatus.NotVerified;
            item.UserAssignment = assignmentFactory.GetCurrentUserAssignment(db, userId);
            return item;
        }

        public void AddVerificationItemToDb(ApplicationDbContext db, VerificationItem item)
        {
            db.VerificationItems.Add(item);
            db.SaveChanges();
        }
    }
}