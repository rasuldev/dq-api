using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
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
                .Where(item => item.UserId == userId &&
                    item.Status == UserAssignmentStatus.InProgress)
                .FirstOrDefault();
        }


        public UserAssignment GetNextUserAssignment(UserAssignment currentUserAssignment, ApplicationDbContext db)
        {
            var nextAssignmentSortValue = currentUserAssignment.Assignment.Sort + 1;
            return db.UserAssignments.Include("Assignment")
                .Where(item =>
                    item.UserId == currentUserAssignment.UserId &&
                    item.Status == UserAssignmentStatus.NotAvailable &&
                    item.Assignment.Sort == nextAssignmentSortValue)
                .FirstOrDefault();
        }
    }
}