using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class PatientCondition
    {

        [Key]
        public int ConditionID { get; set; }

        public string FullName { get; set; }

        public int ICDID { get; set; }
        public string PatientID { get; set; }
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
