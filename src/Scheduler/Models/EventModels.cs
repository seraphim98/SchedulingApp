using Scheduler.Models;

public record class EventModel(Guid Id, string Name, DateTime StartTime, DateTime EndTime, List<PersonModel> People);

public class EventCreateModel
{
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public EventCreateModel(string name, DateTime startTime, DateTime endTime)
    {
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
    }
}

public class EventUpdateModel
{
    public Guid Id {get; set;}
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<Guid> People { get; set;}
    public EventUpdateModel(Guid id, string name, DateTime startTime, DateTime endTime, List<Guid> people)
    {
        Id = id;
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
        People = people;
    }
}