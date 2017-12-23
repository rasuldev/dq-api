using System.ComponentModel.DataAnnotations;

namespace DrinqWeb.Models.CodeFirstModels
{
    public enum VerificationItemStatus
    {
        [Display(Name = "Не подтверждено")]
        NotVerified,
        [Display(Name = "Принято")]
        Accepted,
        [Display(Name = "Отклонено")]
        Declined
    }
}