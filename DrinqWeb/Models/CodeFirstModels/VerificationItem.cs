using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class VerificationItem
    {
        public int Id { get; set; }
        public UserAssignment UserAssignment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MediaId { get; set; }
        public string VerifiedById { get; set; }
        public VerificationItemStatus Status { get; set; }
        public string Message { get; set; }
    }
}