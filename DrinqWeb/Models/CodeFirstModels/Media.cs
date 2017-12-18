using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    public class Media
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageFile { get; set; }
        public string ImageExt { get; set; }
        public string MediaType { get; set; }
        public Assignment Assignment { get; set; }
    }
}