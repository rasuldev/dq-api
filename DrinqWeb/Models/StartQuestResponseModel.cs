using DrinqWeb.Models.CodeFirstModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models
{
    public class StartQuestResponseModel
    {
        public UserQuest UserQuest { get; set; }
        public Assignment Assignment { get; set; }

        public StartQuestResponseModel()
        {
            UserQuest = new UserQuest();
            Assignment = new Assignment();
            UserQuest.Quest = new Quest();
        }
    }
}