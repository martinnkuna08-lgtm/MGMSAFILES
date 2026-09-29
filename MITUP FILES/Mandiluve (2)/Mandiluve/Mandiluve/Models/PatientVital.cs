using System.ComponentModel.DataAnnotations;
namespace Mandiluve.Models
{
    public class PatientVital
    {
        [Key]
        public int PatientVitalID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        /// 
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        [Required]
        [Display(Name = "Enter Body Temperature")]
        public string BodyTemperature { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        [Required]
        [Display(Name = "Enter Heart Rate")]
        public string HeartRate { get; set; }
        [Required]
        [Display(Name = "Enter Blood Pressure")]
        public string BloodPressure { get; set; }
        [Required]
        [Display(Name = "Enter Oxygen Saturation")]
        public string OxygenSaturation { get; set; }
        [Required]
        [Display(Name = "Select Patient Vital Time")]
        public string VitalTime { get; set; }
    }
}
