using DrinqWeb.Models.CodeFirstModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models
{
    public class QuestTools
    {
        public static void CancelQuest(UserQuest userQuest)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            CancelAllAssignments(userQuest);
            userQuest.Status = UserQuestStatus.Failed;
            userQuest.EndDate = DateTime.Now;
            context.Entry(userQuest).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public static void CancelAllAssignments(UserQuest userQuest)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userAssignments = context.UserAssignments.Where(item =>
                item.UserQuest.Id == userQuest.Id &&
                item.UserId == userQuest.UserId &&
                item.Status != UserAssignmentStatus.Completed &&
                item.Status != UserAssignmentStatus.Failed).ToList();
            foreach (var assignment in userAssignments)
            {
                assignment.Status = UserAssignmentStatus.Failed;
                context.Entry(assignment).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}