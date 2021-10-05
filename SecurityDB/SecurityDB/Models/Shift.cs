using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecurityDB.Models
{
    public class Shift
    {
        public int Id { get; set; }

        [Display(Name ="Date From")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? FromDate { get; set; }

        [Display(Name ="Date To")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage ="No of hours required")]
        [Display(Name ="No of Hours")]
        [Range(0, 24, ErrorMessage = "Value for Hours must be between {1} and {2}.")]
        public int Hoursno { get; set; }

        [Display(Name ="No of Minutes")]
        [Range(0, 60, ErrorMessage = "Value for Minutes must be between {1} and {2}.")]
        public int? MinutesNo { get; set; }

        //Foregin Key
        [Required(ErrorMessage = "Site Name is required")]
        public int SiteId { get; set; }

        //Navigation
        public Site Site { get; set; }

        //Foregin Key
        public int UserId { get; set; }

        //Navigation
        public User User { get; set; }

        [StringLength(400, ErrorMessage ="Description cannot be greater tha 400 characters")]
        [Display(Name ="Note")]
        [DataType(DataType.MultilineText)]
        public string Desc { get; set; }

        [Display(Name ="Approved?")]
        public bool? IsApproved { get; set; }

    }
}