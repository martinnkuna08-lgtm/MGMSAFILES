using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
	public class Surgery
	{
		[Key]
		public int SurgeryID { get; set; }
		/// <summary>
		/// ///////////////////////////////////////////
		/// </summary>
		[Required]
		[Display(Name = "Please Enter Date")]
		public DateTime Date { get; set; }
		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public string SurgeonName { get; set; }
		public string AnaesthesiologistName { get; set; }
		public string TheatreName { get; set; }
		public string CodeName { get; set; }
		public string CodeDescription { get; set; }
		[Required]
		[Display(Name = "Please Select Time Slot")]
		public DateTime TimeSlot { get; set; }
		[Required]
		[Display(Name = "Please Enter User ID")]
		public int UserID { get; set; }
		[Required]
		[Display(Name = "Please Enter Anaestesiologist ID")]
		public int AnaesthesiologistID { get; set; }
		[Required]
		[Display(Name = "Please Enter  Theatre")]
		public int TheatreID { get; set; }
		[Required]
		[Display(Name = "Please Enter  Theatre")]
		public int CodeID { get; set; }
		[Required]
		[Display(Name = "Please Enter  Status")]
		public string Status { get; set; } = "Scheduled";
	}
}
