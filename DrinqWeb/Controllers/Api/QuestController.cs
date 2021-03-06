﻿using DrinqWeb.Models;
using DrinqWeb.Models.CodeFirstModels;
using DrinqWeb.Tools.QuestTools;
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
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            if (user == null)
                return Unauthorized();

            Quest quest = db.Quests.Find(questId);
            if (quest == null)
                return BadRequest("Указанный квест не существует.");
            if (!quest.IsPublished)
                return BadRequest("Квест не активен.");
            if (quest.IsDeleted)
                return BadRequest("Квест удален.");

            bool theOnlyQuestOfUser = db.UserQuests.Where(item => item.User.Id == user.Id && item.Status == UserQuestStatus.InProgress).ToList().Count == 0 ? true : false;
            if (!theOnlyQuestOfUser)
                return BadRequest("У Вас уже есть активный квест.");
            // TODO: Validation expressions


            List<Assignment> assignments = db.Assignments.Where(a => a.Quest.Id == quest.Id).OrderBy(item => item.Sort).ToList();
            UserQuest uq = new UserQuest();
            uq.Quest = quest;
            uq.StartDate = DateTime.Now;
            uq.EndDate = null;
            uq.Status = UserQuestStatus.InProgress;
            uq.User = db.Users.Find(user.Id);
            var uqAdded = db.UserQuests.Add(uq);
            db.SaveChanges();

            UserAssignment ua;
            var userAssignments = new List<UserAssignment>();
            foreach (var item in assignments)
            {
                ua = new UserAssignment();
                ua.Status = UserAssignmentStatus.NotAvailable;
                ua.User = db.Users.Find(user.Id);
                ua.UserQuest = uqAdded;
                ua.Assignment = item;
                ua.StartDate = null;
                ua.EndDate = null;
                // start date

                userAssignments.Add(ua);
            }
            // activate first assignment
            userAssignments[0].Status = UserAssignmentStatus.InProgress;
            userAssignments[0].StartDate = DateTime.Now;
            // -- activate first assignment

            db.UserAssignments.AddRange(userAssignments);
            db.SaveChanges();

            var firstUserAssignment = userAssignments[0];
            firstUserAssignment.Assignment.TextCodes = null;
            firstUserAssignment.Assignment.Quest = null;

            StartQuestResponseModel responseModel = new StartQuestResponseModel();
            responseModel.UserQuest = uq;
            responseModel.Assignment = firstUserAssignment.Assignment;
            responseModel.UserQuest.User = null;

            var outputJson = JsonConvert.SerializeObject(responseModel, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(outputJson);

        }

        [HttpPost]
        public IHttpActionResult stop()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = UserUtils.GetUserById(UserUtils.GetAuthorizationStringFromHeader(Request));
            if (user == null)
                return Unauthorized();

            UserQuest userQuest = db.UserQuests.Include("Quest").Where(item => item.User.Id == user.Id && item.Status == UserQuestStatus.InProgress).FirstOrDefault();
            if (userQuest != null)
            {
                QuestUtils.CancelQuest(db, userQuest);
                return Ok("Текущее задание отменено.");
            }
            else
                return BadRequest("У данного пользователя нет текущих заданий.");
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return Ok(JsonConvert.SerializeObject(db.Quests.ToList()));
        }
    }
}