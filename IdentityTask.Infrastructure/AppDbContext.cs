using IdentityTask.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace IdentityTask.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
    {
        public virtual DbSet<User> Users { get; set; }
    }
}
