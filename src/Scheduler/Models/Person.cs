using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Event> Events { get; set;} = new();
        public List<PersonEvent> PersonEvents { get; set;} = new();
        public Person(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }
    }

 
}