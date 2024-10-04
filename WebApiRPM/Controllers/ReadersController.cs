using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WebApiRPM.DbContextApi;
using WebApiRPM.Model;

namespace WebApiRPM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadersController : Controller
    {
        readonly LibraryApyDb _context;

        public ReadersController(LibraryApyDb context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getAllReaders")]
        public async Task<IActionResult> GetAllBooks()
        {
            var readers = await _context.Readers.ToListAsync();
            return new OkObjectResult(new
            {
                readers = readers,
                status = true
            });
        }

        [HttpGet]
        [Route("GetReader/{Id}")]
        public async Task<IActionResult> GetReaderId(int Id)
        {
            var reader = await _context.Readers.FirstOrDefaultAsync(b => b.ID == Id);
            if (reader == null)//404
            {
                return NotFound(new { status = false, MessageContent = "Читатель не найден" });
            }

            return Ok(new { status = true, reader });
        }

        [HttpPost]
        [Route("AddReader")]
        public async Task<IActionResult> PostReader(string name, string surname, DateOnly date, string phone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = false, MessageContent = "Данные неверные" });
            }
            Readers Reader = new Readers()
            {
                Name = name,
                Surname = surname,
                Phone = phone,
                Birthdate = date
            };
            _context.Readers.Add(Reader);
            await _context.SaveChangesAsync();

            return Ok(new { status = true, MessageContent = "Читатель добавлен", reader = Reader });

        }

        [HttpPut]
        [Route("UpdateReader")]
        public async Task<IActionResult> PutReader(int Id, string name, string surname, DateOnly date, string phone)
        {
            var readers = await _context.Readers.FindAsync(Id);
            if (readers == null) //404
            {
                return NotFound(new { status = false, MessageContent = "Читатель не найден" });
            }

            readers.Name = name;
            readers.Surname = surname;
            readers.Birthdate = date;
            readers.Phone = phone;

            _context.Readers.Update(readers);
            await _context.SaveChangesAsync();


            return Ok(new { status = true, MessageContent = "Читатель обновлен", readers = readers });
        }

        [HttpDelete]
        [Route("DeleteReader/{Id}")]
        public async Task<IActionResult> DeleteReader(int Id)
        {
            var readers = await _context.Readers.FindAsync(Id);
            if (readers == null)//404
            {
                return NotFound(new { status = false, MessageContent = "Читатель не найден" });
            }

            _context.Readers.Remove(readers);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true,
                MessageContent = "Читатель удален"
            });
        }
    }
}
