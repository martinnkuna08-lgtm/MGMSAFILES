using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class Treatment
    {
        [Key]
        public int CodeID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Please Enter Treatment Code Name")]
        public string CodeName { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        [Required]
        [Display(Name = "Please Enter Treatment Code Description")]
        public string CodeDescription { get; set; }

        [Display(Name = "Active")]
        public string Active { get; set; } = "T";
    }
}
