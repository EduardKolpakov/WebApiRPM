using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApiRPM.Model
{
    public class Books
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Введите название книги")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Укажите автора книги")]
        public required string Author { get; set; }
        [Required(ErrorMessage = "Укажите дату издания книги")]
        public required DateOnly Publishing {  get; set; }
        [Required(ErrorMessage = "Укажите описание книги")]
        public required string Description { get; set; }


        [ForeignKey("Genres")]
        public required int GenreID { get; set; }
        public Genres Genres { get; set; }
    }
}
