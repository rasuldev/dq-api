using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DrinqWeb.Controllers.Api
{
    public class QuestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult start(int questId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Quest quest = db.Quests.Find(questId);

            // TODO: Validation expressions

            if (quest.IsPublished)
            {
                ApplicationUser user = db.Users.Where(u => u.Id == "95ea29d9-b3d6-4651-a56d-02fc23e0923b").SingleOrDefault();
                List<Assignment> assignments = db.Assignments.Where(a => a.Quest.Id == quest.Id).OrderBy(item => item.Sort).ToList();
                UserQuest uq = new UserQuest();
                uq.Quest = quest;
                uq.StartDate = DateTime.Now;
                uq.EndDate = null;
                uq.Status = UserQuestStatus.InProgress;
                uq.UserId = user.Id;
                var uqAdded = db.UserQuests.Add(uq);
                db.SaveChanges();

                UserAssignment ua;
                var userAssignments = new List<UserAssignment>();
                foreach (var item in assignments)
                {
                    ua = new UserAssignment();
                    ua.Status = UserAssignmentStatus.NotAvailable;
                    ua.UserId = user.Id;
                    ua.UserQuest = uqAdded;
                    ua.Assignment = item;
                    ua.StartDate = null;
                    ua.EndDate = null;
                    // start date

                    userAssignments.Add(ua);
                }
                // first assignment
                userAssignments[0].Status = UserAssignmentStatus.InProgress;
                userAssignments[0].StartDate = DateTime.Now;
                // -- first assignment

                db.UserAssignments.AddRange(userAssignments);
                db.SaveChanges();

                var firstUserAssignment = userAssignments[0];

                StartQuestResponseModel responseModel = new StartQuestResponseModel();
                responseModel.UserQuest.Quest.Title = quest.Title;
                responseModel.UserQuest.StartDate = uqAdded.StartDate;
                responseModel.UserQuest.Quest.Description = quest.Description;
                responseModel.Assignment.Title = firstUserAssignment.Assignment.Title;
                responseModel.Assignment.Description = firstUserAssignment.Assignment.Description;

                var outputJson = JsonConvert.SerializeObject(responseModel, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Ok(outputJson);
            }
            return BadRequest();
        }
    }
}