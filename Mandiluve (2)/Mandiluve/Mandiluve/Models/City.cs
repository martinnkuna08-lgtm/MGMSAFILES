using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Select Title")]
        public string CityName { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 

        public int ProvinceID { get; set; }
    }
}
