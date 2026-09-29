using System.ComponentModel.DataAnnotations;
namespace Mandiluve.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
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
        public string Name { get; set; }

        [Required]
        [Display(Name = "Enter Last Name")]
        public string Surname { get; set; }




        [Display(Name = "Enter Mobile number")]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "Please Enter Email Address")]

        public string Email { get; set; }


        [Display(Name = "Please Enter Date Of Birth")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Enter Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Please Enter Address Line1")]

        public string AddressLine1 { get; set; }


        [Display(Name = "Please Select Suburb")]

        public int SuburbID { get; set; }

        public int CityID { get; set; }

        public int ProvinceID { get; set; }

        public string SuburbName { get; set; }

        public string CityName { get; set; }
        public string FullName { get; set; }
        public string ProvinceName { get; set; }
        public string PostalCode { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; } = "T";
    }
}
