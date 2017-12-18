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
            ApplicationUser user = Utils.GetUserById(Utils.GetAuthorizationStringFromHeader(Request));
            if (user == null)
                return Unauthorized();

            UserAssignment userAssignment = db.UserAssignments.Include("Assignment")
                .Where(item => item.UserId == user.Id &&
                    item.Status == UserAssignmentStatus.InProgress)
                .FirstOrDefault();

            if (userAssignment == null)
                return BadRequest("У Вас нет активных заданий.");

            UserAssignment userAssignmentViewModel = new UserAssignment();
            userAssignmentViewModel.Assignment = new Assignment();
            userAssignmentViewModel.Assignment.Description = userAssignment.Assignment.Description;
            userAssignmentViewModel.Status = userAssignment.Status;

            var json = JsonConvert.SerializeObject(userAssignmentViewModel);
            return Ok(json);
        }
    }
}
