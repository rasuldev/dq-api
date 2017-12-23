using System.ComponentModel.DataAnnotations;

namespace DrinqWeb.Models.CodeFirstModels
{
    public enum UserQuestStatus
    {
        [Display(Name = "В процессе")]
        InProgress,
        [Display(Name = "Выполнено")]
        Completed,
        [Display(Name = "Провалено")]
        Failed
    }
}