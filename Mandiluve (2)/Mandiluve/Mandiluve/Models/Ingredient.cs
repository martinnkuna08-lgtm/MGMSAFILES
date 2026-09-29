using System.ComponentModel.DataAnnotations;

namespace Mandiluve.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientID { get; set; }
        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        [Required]
        [Display(Name = "Select Ingredient Name")]
        public string ActiveIngredient { get; set; }
    }
}
