using Scheduler.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Scheduler.Database {
    public class DatabaseContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Person> People { get; set;}
        public DbSet<PersonEvent> PersonEvents { get; set;}
        public DbSet<Holiday> Holidays { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        { 
        }

        public Task<int> SaveChangesAsynchronous()
        {
            return SaveChangesAsync();
        }

        public void MarkAsModified(Person person) 
        {
            Entry(person).State = EntityState.Modified;
        }
        public void MarkAsModified(Event @event)
        {
            Entry(@event).State = EntityState.Modified;
        }
        public void MarkAsModified(Holiday holiday) {
            Entry(@holiday).State = EntityState.Modified;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasMany(e => e.People).WithMany(p => p.Events).UsingEntity<PersonEvent>();
            modelBuilder.Entity<Event>().Navigation(e => e.People).AutoInclude();
            modelBuilder.Entity<Person>().Navigation(p => p.PersonEvents).AutoInclude();
        }
    }
}
