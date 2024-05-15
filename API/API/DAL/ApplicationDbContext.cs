using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.DAL
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Camera> Cameras { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
