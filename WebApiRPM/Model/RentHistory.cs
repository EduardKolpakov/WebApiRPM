using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApiRPM.Model
{
    public class RentHistory
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Укажите дату начала аренды")]
        public required DateOnly RentStart { get; set; }
        [Required(ErrorMessage = "Укажите срок аренды (в днях)")]
        public required int RentTime {  get; set; }
        public string Status { get; set; }

        [ForeignKey("Books")]
        public required int BookID { get; set; }
        public Books Books { get; set; }
        [ForeignKey("Readers")]
        public required int ReaderID { get; set; }
        public Readers Readers { get; set; }
    }
}
