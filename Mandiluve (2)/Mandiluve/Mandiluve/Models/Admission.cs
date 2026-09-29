using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class Admission
    {
        [Key]
        public int AdmissionID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>

        public int PatientID { get; set; }

        public int UserID { get; set; }
        public int WardID { get; set; }
        public int BedID { get; set; }
        public int CodeID { get; set; }
        public string PatientName { get; set; }

        public string NurseName { get; set; }
        public string WardName { get; set; }
        public string BedName { get; set; }
        public string CodeName { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        [Required]
        [Display(Name = "Please Enter Admission Date")]
        public DateTime AdmissionDate { get; set; }

        [Required]
        [Display(Name = "Please Enter Admission Time")]
        public DateTime AdmissionTime { get; set; }
        [Display(Name = "Active")]
        public string Status { get; set; } = "Admitted";
    }
}
