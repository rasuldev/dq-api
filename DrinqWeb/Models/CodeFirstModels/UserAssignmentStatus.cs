using System.ComponentModel.DataAnnotations;

namespace DrinqWeb.Models.CodeFirstModels
{
    public enum UserAssignmentStatus
    {
        [Display(Name = "В процессе")]
        InProgress,
        [Display(Name = "Не доступно")]
        NotAvailable,
        [Display(Name = "Выполнено")]
        Completed,
        [Display(Name = "Провалено")]
        Failed
    }
}