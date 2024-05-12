using System;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Scheduler;
using Scheduler.Models;

namespace Scheduler.Database
{
    public interface IDatabaseContext : IDisposable
    {

        DbSet<Person> People { get; }
        public DbSet<Event> Events { get; set; }
        public DbSet<PersonEvent> PersonEvents { get; set;}
        public DbSet<Holiday> Holidays { get; set; }
        Task<int> SaveChangesAsynchronous();
        public void MarkAsModified(Person person);
        public void MarkAsModified(Event @event);
        public void MarkAsModified(Holiday holiday);
        
    }
}