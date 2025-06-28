using Microsoft.EntityFrameworkCore;
using ContactWithWhatsApp.Models;

namespace ContactWithWhatsApp.Data
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options) { }

        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}