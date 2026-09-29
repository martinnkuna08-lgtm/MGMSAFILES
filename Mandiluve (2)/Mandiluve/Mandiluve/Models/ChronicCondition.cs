using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class ChronicCondition
    {
        [Key]
        public int ICDID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Please Enter Ward Name")]
        public string ICDCODEID { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        [Display(Name = "Please Enter Diagnosis")]
        public string Diagnosis { get; set; }
    }
}
