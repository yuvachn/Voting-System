using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;

namespace LoginandRegisterMVC.Models
{
    public class User
    {
        [Key]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must provide a phone number,Phone Number at least 10 digit")]
        [Display(Name = "ContactNo")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]

        public string PhoneNo { get; set; }

        [Required]
        [Display(Name = "EmployeeID")]
        public int EmployeeId { get; set; }


        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        //[StringLength(100, ErrorMessage = "The {0} must be atleast {2} characters long.", MinimumLength = 8)]
        //[MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your Password needs to contain atleast !,@,#,$ etc.", ErrorMessage = "Your Password must be 8 characters long and contain atleast one symbol(!,@,#,etc).")]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password required")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "ServiceLine")]
        public string ServiceLine { get; set; }
        [Required]
        //[DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "DOB")]
        [DataType(DataType.Date)]

        public DateTime DOB { get; set; }
    }
}