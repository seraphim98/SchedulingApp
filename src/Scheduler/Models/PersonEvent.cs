using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Scheduler.Models
{
    public class PersonEvent
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public PersonEvent(Guid id, Guid personId, Guid eventId, string eventName) 
        {
            Id = id;
            PersonId = personId;
            EventId = eventId;
            EventName = eventName;
        }
    }
}