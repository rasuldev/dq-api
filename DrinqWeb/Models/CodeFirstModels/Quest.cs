using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class Quest
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Длительность")]
        public int? MaxTime { get; set; }
        [DisplayName("Опубликован")]
        public bool IsPublished { get; set; }
        [DisplayName("Порядок")]
        public int Sort { get; set; }
        [DisplayName("Удален")]
        public bool IsDeleted { get; set; }

    }
}