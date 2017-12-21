using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
using DrinqWeb.Tools.QuestTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Tools.AssignmentTools
{
    public class AssignmentFactory
    {

        public AssignmentFactory()
        {

        }

        public UserAssignment GetCurrentUserAssignment(ApplicationDbContext db, string userId)
        {
            return db.UserAssignments.Include("Assignment.Quest").Include("UserQuest")
                .Where(item => item.UserId == userId && item.Status == UserAssignmentStatus.InProgress)
                .FirstOrDefault();
        }

        public UserAssignment GetNextUserAssignment(ApplicationDbContext db, UserAssignment currentUserAssignment)
        {
            var nextAssignmentSortValue = currentUserAssignment.Assignment.Sort + 1;
            return db.UserAssignments.Include("Assignment")
                .Where(item =>
                    item.UserId == currentUserAssignment.UserId &&
                    item.Status == UserAssignmentStatus.NotAvailable &&
                    item.Assignment.Sort == nextAssignmentSortValue)
                .FirstOrDefault();
        }

        public void SetAssignmentForUser(ApplicationDbContext db, UserAssignment userAssignment, string userId)
        {
            userAssignment.Status = UserAssignmentStatus.InProgress;
            userAssignment.StartDate = DateTime.Now;
            db.Entry(userAssignment).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void CompleteAssigmentForUser(ApplicationDbContext db, UserAssignment UA, string userId)
        {
            UA.Status = UserAssignmentStatus.Completed;
            UA.EndDate = DateTime.Now;
            db.Entry(UA).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public bool IsAssignmentFinished(UserAssignment currentUA)
        {
            bool mediaFininished = true;
            bool textCodeFinished = true;

            if (currentUA.Assignment.MediaRequired == true)
                if (currentUA.MediaAccepted != UserAssignmentAcceptedStatus.Accepted)
                    mediaFininished = false;
            if (currentUA.Assignment.TextRequired == true)
                if (currentUA.TextCodeAccepted != UserAssignmentAcceptedStatus.Accepted)
                    textCodeFinished = false;

            if (mediaFininished == true && textCodeFinished == true)
                return true;
            return false;

        }

        public UserAssignment SetNextUserAssignmentIfFinished(ApplicationDbContext db, UserAssignment currentUA, string userId)
        {
            QuestFactory questFactory = new QuestFactory();
            if (IsAssignmentFinished(currentUA))
            {
                // finish current assignment
                CompleteAssigmentForUser(db, currentUA, userId);

                // set next assignment
                var nextUA = GetNextUserAssignment(db, currentUA);
                if (nextUA == null)
                {
                    // complete quest
                    questFactory.CompleteUserQuest(db, currentUA.UserQuest, userId);
                    return null;
                }
                else
                {
                    SetAssignmentForUser(db, nextUA, userId);
                    return nextUA;
                }
            }
            return currentUA;
        }
    }
}