using System;
using System.Data.Entity;
using Writers.Models;


namespace Writers.DAL
{
    public class WritersContext : DbContext
    {
        public WritersContext() : base("WritersContext")
        {

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonsImages> PersonsImages { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Genre> Genres { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
}