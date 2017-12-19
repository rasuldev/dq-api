using DrinqWeb.Models.CodeFirstModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models
{
    public class AssignmentResponseModel
    {
        public Controllers.Api.AssignmentController.Status Status { get; set; }
        public Assignment Assignment { get; set; }

        public AssignmentResponseModel(Controllers.Api.AssignmentController.Status status, Assignment assignment)
        {
            Status = status;
            Assignment = assignment;
        }
    }
}