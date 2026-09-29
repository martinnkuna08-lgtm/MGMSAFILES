using System.ComponentModel.DataAnnotations;
namespace Mandiluve.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionID { get; set; }

        public int UserID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string SurgeonName { get; set; }
        public string MedicationName { get; set; }

        [Required]
        [Display(Name = "Please Select Prescription Date")]
        public DateTime PrescriptionDate { get; set; }

        [Required]
        [Display(Name = "Please Enter Prescription Time")]
        public DateTime PrescriptionTime { get; set; }
        [Required]
        [Display(Name = "Please Select Prescription Date")]
        public DateTime ReceivedDate { get; set; }


        public DateTime ReceivedTime { get; set; }
        public int HospitalMedicationID { get; set; }
        [Required]
        [Display(Name = "Please Enter Medication Quantity")]
        public string Qty { get; set; }

    
        [Display(Name = "Please Enter Medication Rejection Reason")]
        public string RejectionReason { get; set; }
  
        [Display(Name = "Please Enter Nurse Instructions")]
        public string Instructions { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; } = "Urgent";
    }
}
