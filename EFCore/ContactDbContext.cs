using Contact_Manager.Model;
using Contact_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.EfCore
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options)
            : base(options) { }
        //public override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.
        //}

        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>();
        }

    }
    

}
