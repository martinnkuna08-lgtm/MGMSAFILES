using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class PatientAllergies
    {
        [Key]
        public int AllergiesID { get; set; }

        public string FullName { get; set; }
        public string ActiveIngredient { get; set; }
        public int IngredientID { get; set; }
        public string PatientID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
    }
}
