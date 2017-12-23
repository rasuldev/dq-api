using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class UserAssignment
    {
        public int Id { get; set; }
        [DisplayName("Квест")]
        public UserQuest UserQuest { get; set; }
        [DisplayName("Задание")]
        public Assignment Assignment { get; set; }
        [DisplayName("Пользователь")]
        public virtual ApplicationUser User { get; set; }
        [DisplayName("Дата начала")]
        public DateTime? StartDate { get; set; }
        [DisplayName("Дата окончания")]
        public DateTime? EndDate { get; set; }
        [DisplayName("Статус ответа")]
        public UserAssignmentAcceptedStatus TextCodeAccepted { get; set; }
        [DisplayName("Статус медии")]
        public UserAssignmentAcceptedStatus MediaAccepted { get; set; }
        [DisplayName("Медиа")]
        public int? MediaId { get; set; }
        [DisplayName("Статус")]
        public UserAssignmentStatus Status { get; set; }
    }
}