using System.ComponentModel.DataAnnotations;

namespace WebApiRPM.Model
{
    public class Genres
    {
        [Key]
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Название жанра обязательно")]
        public required string genre { get; set; }
    }
}
