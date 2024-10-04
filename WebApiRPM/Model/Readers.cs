using System.ComponentModel.DataAnnotations;

namespace WebApiRPM.Model
{
    public class Readers
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Введите имя!")]
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public DateOnly Birthdate { get; set; }
        public string Phone {  get; set; }
    }
}
