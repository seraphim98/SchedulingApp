using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Person> People { get; set;} = new();
        public Event (Guid id, string name, DateTime startTime, DateTime endTime)
        {
            Id = id;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }
    }

    
}
