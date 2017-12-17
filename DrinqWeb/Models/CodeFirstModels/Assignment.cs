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
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAnswerText { get; set; }
        public bool isAnswerPicture { get; set; }
        public List<string> ValidTextAnswers { get; set; }
    }
}