using AsqMe.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsqMe.Data.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<QuestionTag> QuestionTags { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Earning> Earnings { get; set; }
    }
}
