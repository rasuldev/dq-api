using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
using DrinqWeb.Tools.AssignmentTools;
using DrinqWeb.Tools.MediaTools;
using DrinqWeb.Tools.VerificationItemTools;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DrinqWeb.Controllers.Api
{
    public class AssignmentController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            AssignmentFactory assignmentFactory = new AssignmentFactory();
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            if (user == null)
                return Unauthorized();

            var curUserAssignment = assignmentFactory.GetCurrentUserAssignment(db, user.Id);
            Assignment currentAssignment = curUserAssignment == null ? null : curUserAssignment.Assignment;

            if (currentAssignment == null)
                return BadRequest("У Вас нет активных заданий.");

            currentAssignment.TextCodes = null;
            var json = JsonConvert.SerializeObject(currentAssignment);
            return Ok(json);
        }



        public enum Status
        {
            Wrong, Accepted, Completed
        }

        [HttpPost]
        public IHttpActionResult CheckText([FromBody] string codes)
        {
            AssignmentFactory assignmentFactory = new AssignmentFactory();
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            if (user == null)
                return Unauthorized();

            var currentUserAssignment = assignmentFactory.GetCurrentUserAssignment(db, user.Id);
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
                    nextUserAssignment = assignmentFactory.GetNextUserAssignment(db, currentUserAssignment);
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

        [HttpPost]
        public IHttpActionResult Upload()
        {
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            if (user == null)
                return Unauthorized();

            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            if (file == null)
                return BadRequest("Отсутствует файл.");

            MediaFactory mediaFactory = new MediaFactory();
            VerificationItemFactory itemFactory = new VerificationItemFactory();
            ApplicationDbContext db = new ApplicationDbContext();
            AssignmentFactory assignmentFactory = new AssignmentFactory();


            // check for exist
            var currentUserAssignmentId = assignmentFactory.GetCurrentUserAssignment(db, user.Id).Id;
            var currentUserAssignmentVerificationItem = db.VerificationItems.Where(vi => vi.UserAssignment.Id == currentUserAssignmentId).FirstOrDefault();
            if (currentUserAssignmentVerificationItem.Status == VerificationItemStatus.Accepted)
                return BadRequest("Ваш ответ уже был принят администратором.");
            if (currentUserAssignmentVerificationItem.Status == VerificationItemStatus.NotVerified)
                return BadRequest("Ваш предыдущий ответ еще не был оценен.");
            // --


            string[] param = mediaFactory.SaveFile(file, FileKind.VerificationItem);
            string newFileName = param[0];
            string dirPath = param[1];

            Media media = mediaFactory.CreateMedia(newFileName, dirPath);

            VerificationItem item = itemFactory.CreateVerificationItem(db, media, user.Id);
            itemFactory.AddVerificationItemToDb(db, item);

            return Ok();
        }
    }
}
