using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Improve.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public virtual ICollection<Option> Options { get; set; }

        public Question()
        {
            Options = new List<Option>();
        }
    }
}