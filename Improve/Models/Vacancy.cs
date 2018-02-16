using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Improve.Models
{
    public class Vacancy
    {
        public int Id { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public int Salary { get; set; }
        public virtual ICollection<Option> Tags { get; set; }

        public Vacancy()
        {
            Tags = new List<Option>();
        }

    }
}