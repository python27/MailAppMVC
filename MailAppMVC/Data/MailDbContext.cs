using MailAppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace MailAppMVC.Data
{
    public class MailDbContext : DbContext
    {
        public MailDbContext(DbContextOptions<MailDbContext> options) : base(options)
        {
        }

        public DbSet<Mail> Mail { get; set; }
    }
}
