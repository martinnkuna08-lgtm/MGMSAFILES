using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class PatientMedication
    {
        [Key]
        public int PatientMedicationID { get; set; }

        public string FullName { get; set; }


        public string PatientID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Please Enter Ward Name")]
        public string ChronicMedicationID { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        [Display(Name = "Please Enter Medication Name")]
        public string MedicationName { get; set; }
    }
}
