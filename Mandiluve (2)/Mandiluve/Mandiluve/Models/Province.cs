using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class Province
    {
        [Key]
        public int ProvinceID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Select Province Nme")]
        public string ProvinceName { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
    }
}
