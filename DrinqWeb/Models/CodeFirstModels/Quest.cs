using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? MaxTime { get; set; }
        public string Name { get; set; }
        public bool IsPublished { get; set; }
        public int Sort { get; set; }
        public bool IsDeleted { get; set; }

    }
}