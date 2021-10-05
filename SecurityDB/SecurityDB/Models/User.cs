using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecurityDB.Models
{
    public class User
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name ="Joining Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? JoiningDate { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot be greater than 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name cannot be greater than 50 characters")]
        public string LastName { get; set; }

        [Display(Name ="SIA License No")]
        public string SIAno { get; set; }

        [Display(Name ="License Type")]
        public string SIAType { get; set; }

        [Display(Name ="SIA Expiry Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? SIAExpiryDate { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(150, ErrorMessage ="Email Address cannot be bigger than 150 characters")]
        public string Email { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage ="User Name cannot be less than 8 characters or greater than 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength =10, ErrorMessage ="Password cannot be less than 10 or greater than 100 chacaters")]
        public string Password { get; set; }

        [StringLength(350, ErrorMessage = "Address cannot be greater than 350 characters")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(100)]
        public string Mobile { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Code is Required for registration")]
        [Range(1000, 9999, ErrorMessage ="Code must be between 1000 and 9999")]
        public int Code { get; set; }
    }
}