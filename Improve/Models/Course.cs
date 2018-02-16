using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Improve.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string  Header { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Text { get; set; }
        public virtual ICollection<Option> Tags { get; set; }

        public Course()
        {
            Tags = new List<Option>();
        }
    }
}