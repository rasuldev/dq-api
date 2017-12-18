using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinqWeb.Models.CodeFirstModels
{
    // Mission
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool TextRequired { get; set; }
        public bool MediaRequired { get; set; }
        public int Sort { get; set; }
        public string TextCodes { get; set; }
    }
}