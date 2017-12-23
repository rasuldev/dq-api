using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class Media
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Файл")]
        public string MediaFile { get; set; }
        [DisplayName("Расширение")]
        public string MediaExt { get; set; }
        [DisplayName("Тип")]
        public MediaType MediaType { get; set; }
        [DisplayName("Задание")]
        public Assignment Assignment { get; set; }
    }
}