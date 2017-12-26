using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
using System;
using System.Linq;

namespace DrinqWeb.Tools.QuestTools
{
    public class QuestUtils
    {
        public static void CancelQuest(ApplicationDbContext context, UserQuest userQuest)
        {
            CancelAllAssignments(context, userQuest);
            userQuest.Status = UserQuestStatus.Failed;
            userQuest.EndDate = DateTime.Now;
            context.Entry(userQuest).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public static void CancelAllAssignments(ApplicationDbContext context, UserQuest userQuest)
        {
            var userAssignments = context.UserAssignments.Where(item =>
                item.UserQuest.Id == userQuest.Id &&
                item.User.Id == userQuest.User.Id &&
                item.Status != UserAssignmentStatus.Completed &&
                item.Status != UserAssignmentStatus.Failed).ToList();
            foreach (var assignment in userAssignments)
            {
                assignment.Status = UserAssignmentStatus.Failed;
                context.Entry(assignment).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
        }

        public static bool HasCurrentQuestTimeExpired(ApplicationDbContext db, UserAssignment currentUserAssignment)
        {
            double currentUserAssignmentDuration = (DateTime.Now - currentUserAssignment.UserQuest.StartDate).TotalMinutes;
            if (currentUserAssignmentDuration > currentUserAssignment.UserQuest.Quest.MaxTime)
            {
                CancelQuest(db, currentUserAssignment.UserQuest);
                return true;
            }
            return false;
        }
    }
}