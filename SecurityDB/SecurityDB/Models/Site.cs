using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecurityDB.Models
{
    public class Site
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Site Name is required")]
        [Display(Name ="Site Name")]
        [StringLength(150, ErrorMessage ="Site Name cannot be greater than 300 characters")]
        public string SiteName { get; set; }

        [Required(ErrorMessage = "Site Address is required")]
        [Display(Name = "Site Address")]
        [StringLength(350, ErrorMessage = "Site Address cannot be greater than 350 characters")]
        [DataType(DataType.MultilineText)]
        public string SiteAddress { get; set; }

        [Display(Name = "Contact Name")]
        [StringLength(150, ErrorMessage = "Contact Name cannot be greater than 150 characters")]
        public string ContactPerson { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(100)]
        public string Telephone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(100)]
        public string Mobile { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(250)]
        public string Email { get; set; }
    }
}