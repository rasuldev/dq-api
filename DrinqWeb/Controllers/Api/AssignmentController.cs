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
    public class AssignmentController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            if (user == null)
                return Unauthorized();

            var curUserAssignment = GetCurrentUserAssignment(db, user.Id);
            Assignment currentAssignment = curUserAssignment == null ? null : curUserAssignment.Assignment;

            if (currentAssignment == null)
                return BadRequest("У Вас нет активных заданий.");

            currentAssignment.TextCodes = null;
            var json = JsonConvert.SerializeObject(currentAssignment);
            return Ok(json);
        }

        private UserAssignment GetCurrentUserAssignment(ApplicationDbContext db, string userId)
        {
            return db.UserAssignments.Include("Assignment.Quest").Include("UserQuest")
                .Where(item => item.UserId == userId &&
                    item.Status == UserAssignmentStatus.InProgress)
                .FirstOrDefault();
        }


        private UserAssignment GetNextUserAssignment(UserAssignment currentUserAssignment, ApplicationDbContext db)
        {
            var nextAssignmentSortValue = currentUserAssignment.Assignment.Sort + 1;
            return db.UserAssignments.Include("Assignment")
                .Where(item =>
                    item.UserId == currentUserAssignment.UserId &&
                    item.Status == UserAssignmentStatus.NotAvailable &&
                    item.Assignment.Sort == nextAssignmentSortValue)
                .FirstOrDefault();
        }

        public enum Status
        {
            Wrong, Accepted, Completed
        }

        [HttpPost]
        public IHttpActionResult CheckText([FromBody] string codes)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            if (user == null)
                return Unauthorized();

            var currentUserAssignment = GetCurrentUserAssignment(db, user.Id);
            if (currentUserAssignment == null)
                return BadRequest("У Вас нет активного задания.");

            // Check user quest duration
            int currentUserAssignmentDuration = (DateTime.Now - currentUserAssignment.UserQuest.StartDate).Minutes;
            if (currentUserAssignmentDuration > currentUserAssignment.UserQuest.Quest.MaxTime)
            {
                QuestTools.CancelQuest(currentUserAssignment.UserQuest);
                return Ok("Время, отведенное на выполнение квеста, закончилось.");
            }
            // -- Check user quest duration

            var jsonResponse = "";
            var status = Status.Accepted;

            // Check answers
            string[] userCodes = codes.Split(new string[] { "|$|" }, StringSplitOptions.RemoveEmptyEntries);
            string[] correctCodes = currentUserAssignment.Assignment.TextCodes.Split(new string[] { "|$|" }, StringSplitOptions.RemoveEmptyEntries);
            if (userCodes.Length == correctCodes.Length)
                for (int i = 0; i < userCodes.Length; i++)
                {
                    if (userCodes[i] != correctCodes[i])
                    {
                        status = Status.Wrong;
                        break;
                    }
                }
            else
                status = Status.Wrong;
            // -- Check answers

            // Get next assignment
            UserAssignment nextUserAssignment = null;
            Assignment responseNextAssignment = null;
            switch (status)
            {
                case Status.Accepted:
                    currentUserAssignment.TextCodeAccepted = UserAssignmentAcceptedStatus.Accepted;
                    currentUserAssignment.Status = UserAssignmentStatus.Completed;
                    currentUserAssignment.EndDate = DateTime.Now;
                    // todo: check media (not alpha)
                    status = Status.Completed;
                    nextUserAssignment = GetNextUserAssignment(currentUserAssignment, db);
                    if (nextUserAssignment == null)
                    {
                        currentUserAssignment.UserQuest.Status = UserQuestStatus.Completed;
                        currentUserAssignment.UserQuest.EndDate = DateTime.Now;
                        responseNextAssignment = null;
                    }
                    else
                    {
                        nextUserAssignment.Status = UserAssignmentStatus.InProgress;
                        nextUserAssignment.StartDate = DateTime.Now;
                        responseNextAssignment = nextUserAssignment.Assignment;
                    }
                    break;
                case Status.Wrong:
                    currentUserAssignment.TextCodeAccepted = UserAssignmentAcceptedStatus.Declined;
                    break;
            }
            // -- Get next assignment
            db.SaveChanges();

            // RESPONSE
            if (responseNextAssignment != null)
                responseNextAssignment.TextCodes = null;
            jsonResponse = JsonConvert.SerializeObject(new AssignmentResponseModel(status, responseNextAssignment), Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(jsonResponse);

            //  Check user
            //  Check currentAssignment
            //  CUQD
            //  var jsonStr
            //  Compare code and correct text code:
            //      OK->    currentAssignment.TextAccepted = Accepted;
            //              currentAssignment.Status = Completed;
            //              status = Completed;
            //              Get next assignment:
            //                  OK ->   assignment.Status = InProgress;
            //                  NULL -> currentAssignment.Quest = Completed;
            //              jsonStr = status + assignment(may be null);
            //      WRONG-> currentAssignment.TextAccepted = Declined;
            //              status = Declined;
            //  return jsonStr;
        }
    }
}
