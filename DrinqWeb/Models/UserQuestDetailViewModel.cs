using DrinqWeb.Models.CodeFirstModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DrinqWeb.Models
{
    public class UserQuestDetailViewModel
    {
        public UserQuest UserQuest;
        [DisplayName("Всего заданий")]
        public int TotalAssignmentsCount { get; set; }
        [DisplayName("Выполнено")]
        public int FinishedAssignmentCount { get; set; }
        [DisplayName("Выполняются")]
        public string CurrentAssignmentsTitle { get; set; }
    }
}