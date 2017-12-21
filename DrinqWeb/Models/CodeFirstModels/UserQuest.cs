using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class UserQuest
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Quest Quest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public UserQuestStatus Status { get; set; }
    }
}