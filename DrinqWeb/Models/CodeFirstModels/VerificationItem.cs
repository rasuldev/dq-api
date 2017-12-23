using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class VerificationItem
    {
        public int Id { get; set; }
        [DisplayName("Задание")]
        public UserAssignment UserAssignment { get; set; }
        [DisplayName("Дата создания")]
        public DateTime? IncomingDate { get; set; }
        [DisplayName("Дата оценки")]
        public DateTime? VerifiedDate { get; set; }
        [DisplayName("Медиа")]
        public Media Media { get; set; }
        [DisplayName("Оценщик")]
        public ApplicationUser VerifiedById { get; set; }
        [DisplayName("Статус")]
        public VerificationItemStatus Status { get; set; }
        [DisplayName("Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}