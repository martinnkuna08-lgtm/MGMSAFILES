using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class Bed
    {
        [Key]
        public int BedID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Please Enter Bed Name")]
        public string BedName { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        [Display(Name = "Active")]
        public string Active { get; set; } = "T";
    }
}
