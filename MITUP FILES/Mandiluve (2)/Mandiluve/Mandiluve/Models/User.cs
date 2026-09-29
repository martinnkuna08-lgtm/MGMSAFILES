using System.ComponentModel.DataAnnotations;
namespace Mandiluve.Models
{
    public class User
    {

        [Key]
        public int UserID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Select Title")]
        public string Title { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        [Required]
        [Display(Name = "Enter First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Enter Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Please Enter Email Address")]

        public string Email { get; set; }


        [Display(Name = "Enter Mobile number")]
        public string MobileNumber { get; set; }



        [Display(Name = "Please Enter Password")]
        public string Password { get; set; }


        [Display(Name = "Active")]
        public string Active { get; set; } = "T";

        [Required]
        [Display(Name = "Please Select User Role")]
        public string UserType { get; set; }
    }
}
