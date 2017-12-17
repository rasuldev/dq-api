using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class UserAssignment
    {
        public int Id { get; set; }
        public int UserQuestId { get; set; }
        public int AssignmentId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public UserAssignmentAcceptedStatus TextCodeAccepted { get; set; }
        public UserAssignmentAcceptedStatus MediaAccepted { get; set; }
        public int MediaId { get; set; }
        public UserAssignmentStatus Status { get; set; }
    }
}