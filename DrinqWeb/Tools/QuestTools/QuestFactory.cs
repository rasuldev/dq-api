using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Tools.QuestTools
{
    public class QuestFactory
    {
        public QuestFactory()
        {

        }

        public void CompleteUserQuest(ApplicationDbContext db, UserQuest userQuest, string userId)
        {
            userQuest.Status = UserQuestStatus.Completed;
            userQuest.EndDate = DateTime.Now;
            db.Entry(userQuest).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}