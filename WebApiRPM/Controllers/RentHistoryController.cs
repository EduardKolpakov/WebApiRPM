using Microsoft.AspNetCore.Mvc;
using WebApiRPM.DbContextApi;
using Microsoft.EntityFrameworkCore;
using WebApiRPM.Model;

namespace WebApiRPM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentHistoryController : Controller
    {
        private readonly LibraryApyDb _context;
        public RentHistoryController(LibraryApyDb context)
        {
            _context = context;
        }

        [HttpPost]

        public async Task<IActionResult> RentBook(int BookID, int ReaderID, int RentTime)
        {
            var book = await _context.Books.FindAsync(BookID);
            if (book == null)
            {
                return NotFound(new { status = false, message = "Книга не найдена" });
            }
            var reader = await _context.Readers.FindAsync(ReaderID);
            if (reader == null)
            {
                return NotFound(new { status = false, message = "Читатель не найден" });
            }


            var rental = new RentHistory()
            {
                BookID = BookID,
                ReaderID = ReaderID,
                RentStart = DateOnly.FromDateTime(DateTime.Now),
                RentTime = RentTime

            };


            _context.RentHistory.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(new { status = true, message = "Книга успешно арендована", rental });
        }

        [HttpPut]
        [Route("return")]
        public async Task<IActionResult> ReturnBook(int BookID, int ReaderID)
        {
            var rental = await _context.RentHistory.FirstOrDefaultAsync(z => z.BookID == BookID && z.ReaderID == ReaderID);

            if (rental == null)
            {
                return NotFound(new { status = false, message = "Аренда не найдена для данной книги и читателя." });
            }

            // Проверяем, была ли книга уже возвращена
            if (rental.Status == "true")
            {
                return BadRequest(new { status = false, message = "Книга уже была возвращена." });
            }

            // Помечаем книгу как возвращенную (обновляем аренду)
            rental.Status = "false";
            await _context.SaveChangesAsync();

            return Ok(new { status = true, message = "Книга успешно возвращена" });
        }

        [HttpGet]
        [Route("history/reader/{ReaderID}")]
        public async Task<IActionResult> GetRentalHistoryForReader(int ReaderID)
        {
            var rentals = await _context.RentHistory
                .Where(r => r.ReaderID == ReaderID)
                .Include(r => r.Books)
                .ToListAsync();

            if (!rentals.Any())
            {
                return NotFound(new { status = false, message = "История аренды для читателя не найдена." });
            }

            return Ok(new { status = true, rentals });
        }

        [HttpGet]
        [Route("history/book/{BookID}")]
        public async Task<IActionResult> GetRentalHistoryForBook(int BookID)
        {
            var rentals = await _context.RentHistory
                .Where(r => r.BookID == BookID)
                .Include(r => r.Readers)
                .ToListAsync();

            if (!rentals.Any())
            {
                return NotFound(new { status = false, message = "История аренды для книги не найдена." });
            }

            return Ok(new { status = true, rentals });
        }

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrentRentals()
        {
            var currentRentals = await _context.RentHistory.Where(r => r.Status == "true")
                .Include(r => r.Books)
                .Include(r => r.Readers)
                .ToListAsync();

            if (!currentRentals.Any())
            {
                return NotFound(new { status = false, message = "Текущих аренд не найдено." });
            }

            return Ok(new { status = true, rentals = currentRentals });
        }
    }
}
