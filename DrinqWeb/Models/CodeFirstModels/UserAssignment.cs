using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class UserAssignment
    {
        public int Id { get; set; }
        public UserQuest UserQuest { get; set; }
        public Assignment Assignment { get; set; }
        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public UserAssignmentAcceptedStatus TextCodeAccepted { get; set; }
        public UserAssignmentAcceptedStatus MediaAccepted { get; set; }
        public int? MediaId { get; set; }
        public UserAssignmentStatus Status { get; set; }
    }
}