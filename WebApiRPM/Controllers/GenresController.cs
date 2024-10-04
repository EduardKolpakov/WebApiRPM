using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApiRPM.DbContextApi;
using WebApiRPM.Model;

namespace WebApiRPM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        readonly LibraryApyDb _context;

        public GenresController(LibraryApyDb context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("getAllGenres")]
        public async Task<IActionResult> GetAllGenres()
        {
            var genre = await _context.Genres.ToListAsync();
            return new OkObjectResult(new
            {
                genre = genre,
                status = true
            });
        }

        [HttpPost]
        [Route("AddGenre")] 
        public async Task<IActionResult> PostGenre([FromBody] Genres newGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = false, MessageContent = "Данные неверные" });
            }

            _context.Genres.Add(newGenre);
            await _context.SaveChangesAsync();

            return Ok(new { status = true, MessageContent = "Жанр добавлен", genre = newGenre });

        }

        [HttpPut]
        [Route("UpdateGenre")]
        public async Task<IActionResult> PutZhanr(int Id, [FromBody] Genres UpdateGenre)
        {
            var Genre = await _context.Genres.FindAsync(Id);
            if (Genre == null) //404
            {
                return NotFound(new { status = false, MessageContent = "Жанр не найден" });
            }

            Genre.genre = UpdateGenre.genre;


            _context.Genres.Update(Genre);
            await _context.SaveChangesAsync();


            return Ok(new { status = true, MessageContent = "Жанр обновлен", Genre = Genre });
        }

        [HttpDelete]
        [Route("DeleteGenre/{Id}")]
        public async Task<IActionResult> DeleteGenre(int Id)
        {
            var Genre = await _context.Genres.FindAsync(Id);
            if (Genre == null)//404
            {
                return NotFound(new { status = false, MessageContent = "Жанр не найден" });
            }

            _context.Genres.Remove(Genre);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true,
                MessageContent = "Жанр удален"
            });
        }

    }
}
