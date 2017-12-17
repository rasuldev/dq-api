using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxDuration { get; set; }

    }
}