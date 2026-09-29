using System.ComponentModel.DataAnnotations;
namespace Mandiluve.Models
{
	public class Theatre
	{
		[Key]
		public int TheatreID { get; set; }
		/// <summary>
		/// ///////////////////////////////////////////
		/// </summary>
		[Required]
		[Display(Name = "Please Enter Theatre Name")]
		public string TheatreName { get; set; }
		[Required]
		[Display(Name = "Please Enter  Status")]
		public string Status { get; set; } = "Available";
	}
}
