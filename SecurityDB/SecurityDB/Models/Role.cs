using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecurityDB.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [StringLength(100, ErrorMessage ="Title cannot be bigger than 100 characters")]
        public string Title { get; set; }

        [StringLength(100)]
        public string WebForms { get; set; }
    }
}