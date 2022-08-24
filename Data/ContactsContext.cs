using ContactsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Data
{
    public class ContactsContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<YourNumber> YourNumber { get; set; }

        public ContactsContext(DbContextOptions options) : base(options)
        {
        }
    }
}
