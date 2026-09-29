using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class Ward
    {

        [Key]
        public int WardID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Please Enter Ward Name")]
        public string WardName { get; set; }
        /// / <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>   
        /// 
        [Display(Name = "Active")]
        public string Active { get; set; } = "T";
    }
}
