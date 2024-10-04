using Microsoft.EntityFrameworkCore;
using WebApiRPM.Model;

namespace WebApiRPM.DbContextApi
{
    public class LibraryApyDb : DbContext
    {
        public LibraryApyDb(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Books> Books { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Readers> Readers { get; set; }
        public DbSet<RentHistory> RentHistory { get; set; }
    }
}
