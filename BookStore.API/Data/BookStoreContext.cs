using Microsoft.EntityFrameworkCore;
using BookStore.API.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookStore.API.Data
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser>
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {

        }
        public DbSet<Books> Books { get; set; }




        // As we declared connection string in Program.cs class

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=Database=BookStoreAPI;Integrated Security=True");
        //        base.OnConfiguring(optionsBuilder);
        //}
    }
}
