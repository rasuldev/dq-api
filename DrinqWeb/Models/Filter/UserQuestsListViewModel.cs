using DrinqWeb.Models.CodeFirstModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DrinqWeb.Models.Filter
{
    public class UserQuestsListViewModel
    {
        public IEnumerable<UserQuest> UserQuests { get; set; }
        public SelectList Status { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? EndDate { get; set; }
    }
}