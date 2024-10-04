using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRPM.DbContextApi;
using WebApiRPM.Model;

namespace WebApiRPM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        readonly LibraryApyDb _context;


        public BooksController(LibraryApyDb context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("getAllBooks")] 
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            return new OkObjectResult(new
            {
                books = books,
                status = true
            });
        }

        [HttpDelete]
        [Route("DeleteBook/{Id}")]
        public async Task<IActionResult> DeleteBook(int Id)
        {
            var books = await _context.Books.FindAsync(Id);
            if (books == null)//404
            {
                return NotFound(new { status = false, MessageContent = "Книга не найдена" });
            }

            _context.Books.Remove(books);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true,
                MessageContent = "Книга удалена"
            });
        }

        [HttpPut]
        [Route("UpdateBook")]
        public async Task<IActionResult> PutBook(int ID, string Author, string Description, DateOnly date, int GenreID)
        {
            var books = await _context.Books.FindAsync(ID);
            if (books == null) //404
            {
                return NotFound(new { status = false, MessageContent = "Книга не найдена" });
            }

            books.Author = Author;
            books.Description = Description;
            books.Publishing = date;
            books.GenreID = GenreID;

            _context.Books.Update(books);
            await _context.SaveChangesAsync();


            return Ok(new { status = true, MessageContent = "Книга обновлена", books = books });
        }

        [HttpPost]
        [Route("ADDBook")] 
        public async Task<IActionResult> PostBook(string Name, string Author, string Description, DateOnly date, int GenreID)
        {
            var existingGenre = await _context.Genres.FindAsync(GenreID);
            if (existingGenre == null)
            {
                return BadRequest("Жанр с указанным Id не найден.");
            }

            var book = new Books()
            {
                Name = Name,
                Author = Author,
                Description = Description,
                GenreID = GenreID,
                Publishing = date
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("getBook/{Id}")] 
        public async Task<IActionResult> GetBookId(int Id)
        {
            var books = await _context.Books.FirstOrDefaultAsync(z => z.ID == Id);
            if (books == null)//404
            {
                return NotFound(new { status = false, MessageContent = "Книга не найдена" });
            }

            return Ok(new { status = true, books });
        }

        [HttpGet]
        [Route("getBookZhanr/{GenreID}")]
        public async Task<IActionResult> GetBookGenreID(int GenreID)
        {
            var books = await _context.Books.FirstOrDefaultAsync(z => z.GenreID == GenreID);
            if (books == null)//404
            {
                return NotFound(new { status = false, MessageContent = "Книга не найдена" });
            }

            return Ok(new { status = true, books });
        }

        [HttpGet]
        [Route("getBookAuthorName/{Author}, {Name}")]
        public async Task<IActionResult> GetBookAuhtor_Name(string Author, string Name)
        {
            var books = await _context.Books
                   .Where(b => b.Author == Author && b.Name == Name)
                   .FirstOrDefaultAsync();
            if (books == null)//404
            {
                return NotFound(new { status = false, MessageContent = "Книга не найдена" });
            }

            return Ok(new { status = true, books });
        }

    }
}
