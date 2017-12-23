using System.ComponentModel.DataAnnotations;

namespace DrinqWeb.Models.CodeFirstModels
{
    public enum UserAssignmentAcceptedStatus
    {
        [Display(Name ="Начальное")]
        Initial,
        [Display(Name = "Отклонен")]
        Declined,
        [Display(Name = "Принят")]
        Accepted,
        [Display(Name = "Обрабатывается")]
        Verifying,
        [Display(Name = "Не требуется")]
        NotApplicable
    }
}