using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
using DrinqWeb.Tools.QuestTools;
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
                return BadRequest("У Вас нет активного задания.");

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
            QuestFactory questFactory = new QuestFactory();
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            var currentUserAssignment = assignmentFactory.GetCurrentUserAssignment(db, user.Id);

            // Verification
            var VerificationResult = Verification(db, VerificationFor.CheckText);
            if (VerificationResult != null)
                return VerificationResult;
            // -- Verification

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

            Assignment responseNextAssignment = null;

            switch (status)
            {
                case Status.Accepted:
                    currentUserAssignment.TextCodeAccepted = UserAssignmentAcceptedStatus.Accepted;

                    var result = assignmentFactory.SetNextUserAssignmentIfFinished(db, currentUserAssignment, user.Id);
                    if (result == null) // квест закончился (нет следующего задания)
                        status = Status.Completed;
                    else
                    {
                        if (result.Id != currentUserAssignment.Id) // следующее задание
                        {
                            status = Status.Completed;
                            responseNextAssignment = result.Assignment;
                        }
                    }
                    break;
                case Status.Wrong:
                    currentUserAssignment.TextCodeAccepted = UserAssignmentAcceptedStatus.Declined;
                    break;
            }

            db.SaveChanges();

            // RESPONSE
            if (responseNextAssignment != null)
                responseNextAssignment.TextCodes = null;
            jsonResponse = JsonConvert.SerializeObject(new AssignmentResponseModel(status, responseNextAssignment), Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(jsonResponse);

        }

        [HttpPost]
        public IHttpActionResult Upload()
        {
            AssignmentFactory assignmentFactory = new AssignmentFactory();
            VerificationItemFactory itemFactory = new VerificationItemFactory();
            ApplicationDbContext db = new ApplicationDbContext();
            MediaFactory mediaFactory = new MediaFactory();
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            // Verification
            var VerificationResult = Verification(db, VerificationFor.Upload);
            if (VerificationResult != null)
                return VerificationResult;
            // -- Verification

            string[] param = mediaFactory.SaveFile(file, FileKind.VerificationItem);
            string newFileName = param[0];
            string dirPath = param[1];

            Media media = mediaFactory.CreateMedia(newFileName, dirPath);

            VerificationItem item = itemFactory.CreateVerificationItem(db, media, user.Id);
            itemFactory.AddVerificationItemToDb(db, item);

            return Ok();
        }
        private enum VerificationFor
        {
            Upload,
            CheckText
        }
        private IHttpActionResult Verification(ApplicationDbContext db, VerificationFor verificationFor)
        {
            // init
            AssignmentFactory assignmentFactory = new AssignmentFactory();
            VerificationItemFactory itemFactory = new VerificationItemFactory();
            MediaFactory mediaFactory = new MediaFactory();
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            var currentUserAssignment = assignmentFactory.GetCurrentUserAssignment(db, user.Id);

            //both verification
            if (user == null)
                return Unauthorized();

            if (currentUserAssignment == null)
                return BadRequest("У Вас нет активного задания.");

            double currentUserAssignmentDuration = (DateTime.Now - currentUserAssignment.UserQuest.StartDate).TotalMinutes;
            if (currentUserAssignmentDuration > currentUserAssignment.UserQuest.Quest.MaxTime)
            {
                QuestTools.CancelQuest(db, currentUserAssignment.UserQuest);
                return BadRequest("Время, отведенное на выполнение квеста, закончилось.");
            }

            // checktext verification
            if (verificationFor == VerificationFor.CheckText)
            {
                if (!currentUserAssignment.Assignment.TextRequired)
                    return BadRequest("Для текущего задания не требуются ответы.");

                if (currentUserAssignment.TextCodeAccepted == UserAssignmentAcceptedStatus.Accepted)
                    return BadRequest("Ваш ответ уже прошел проверку.");
            }

            // checkmedia verification
            if (verificationFor == VerificationFor.Upload)
            {
                var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
                if (file == null)
                    return BadRequest("Отсутствует файл.");

                if (!currentUserAssignment.Assignment.MediaRequired)
                    return BadRequest("Для текущего задания не требуются изображение/видео.");

                // check for exist
                var currentUserAssignmentId = assignmentFactory.GetCurrentUserAssignment(db, user.Id).Id;
                var currentUserAssignmentVerificationItem = db.VerificationItems.Where(vi => vi.UserAssignment.Id == currentUserAssignmentId).OrderByDescending(key => key.IncomingDate).FirstOrDefault();
                if (currentUserAssignmentVerificationItem != null)
                {
                    if (currentUserAssignmentVerificationItem.Status == VerificationItemStatus.Accepted)
                        return BadRequest("Ваш ответ уже был принят администратором.");
                    if (currentUserAssignmentVerificationItem.Status == VerificationItemStatus.NotVerified)
                        return BadRequest("Ваш предыдущий ответ еще не был оценен.");
                }
            }
            return null;
        }
    }
}
