using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class UserQuest
    {
        public int Id { get; set; }
        [DisplayName("Пользователь")]
        public virtual ApplicationUser User { get; set; }
        [DisplayName("Квест")]
        public Quest Quest { get; set; }
        [DisplayName("Дата начала")]
        public DateTime StartDate { get; set; }
        [DisplayName("Дата окончания")]
        public DateTime? EndDate { get; set; }
        [DisplayName("Статус")]
        public UserQuestStatus Status { get; set; }
    }
}