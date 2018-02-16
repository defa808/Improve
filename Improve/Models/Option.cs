using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Improve.Models
{
    public class Option
    {
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Value { get; set; }
        public int IdNextQuestion { get; set; }

        public Question Question { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }

        public Option()
        {
            Courses = new List<Course>();
            Vacancies = new List<Vacancy>();
        }
    }
}