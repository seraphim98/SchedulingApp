using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Holiday
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public Holiday(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }
    }
    
}