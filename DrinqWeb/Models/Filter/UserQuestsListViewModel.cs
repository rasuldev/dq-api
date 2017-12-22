using DrinqWeb.Models.CodeFirstModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DrinqWeb.Models.Filter
{
    public class UserQuestsListViewModel
    {
        public IEnumerable<UserQuest> UserQuests { get; set; }
        public SelectList Status { get; set; }
    }
}