using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class ChronicMedication
    {
        [Key]
        public int ChronicMedicationID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Enter Medication Name")]
        public string MedicationName { get; set; }

        [Required]
        [Display(Name = "Enter Medication Schedule")]
        public string Schedule { get; set; }
        public int DosageID { get; set; }
        public string DosageName { get; set; }
        public int IngredientID { get; set; }
        public string ActiveIngredient { get; set; }
        [Required]
        [Display(Name = "Enter Medication Strength")]
        public string Strength { get; set; }

        [Required]
        [Display(Name = "Enter Medication Status")]
        public string Status { get; set; } = "Available";
    }
}
