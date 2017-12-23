using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    // Mission
    public class Assignment
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Ответы")]
        public string TextCodes { get; set; }
        [DisplayName("Требуется ответ")]
        public bool TextRequired { get; set; }
        [DisplayName("Требуется медиа")]
        public bool MediaRequired { get; set; }
        [DisplayName("Порядок")]
        public int Sort { get; set; }
        [DisplayName("Квест")]
        public Quest Quest { get; set; }

    }
}