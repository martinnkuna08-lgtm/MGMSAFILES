using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class Suburb
    {
        [Key]
        public int SuburbID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Select Title")]
        public string SuburbName { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        public int PostalCode { get; set; }
        public int CityID { get; set; }
    }
}
