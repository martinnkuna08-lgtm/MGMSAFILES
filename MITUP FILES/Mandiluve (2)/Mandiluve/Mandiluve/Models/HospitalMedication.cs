using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
	public class HospitalMedication
	{
		[Key]
		public int HospitalMedicationID { get; set; }
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
		[Display(Name = "Enter Re-Order Level")]
		public string ReOrderLevel { get; set; }
		[Required]
		[Display(Name = "Enter Stock On Hand")]
		public string StockOnHand { get; set; }
		[Required]
		[Display(Name = "Enter Medication Status")]
		public string Status { get; set; } = "Available";
	}
}
