using SecurityDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecurityDB.ModelView
{
    public class LoginFormModelView
    {
        [Required(ErrorMessage ="User Name is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "User Name cannot be greater than 100 characters")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 12, ErrorMessage = "Password cannot be less than 12 or greater than 100 chacaters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}