using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class VerificationItem
    {
        public int Id { get; set; }
        public UserAssignment UserAssignment { get; set; }
        public DateTime? IncomingDate { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public Media Media { get; set; }
        public ApplicationUser VerifiedById { get; set; }
        public VerificationItemStatus Status { get; set; }
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}