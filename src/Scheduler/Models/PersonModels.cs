using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models;

public record class PersonModel(Guid Id, string FirstName, string LastName, List<string> EventNames);

public class PersonCreateModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public PersonCreateModel(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public class PersonUpdateModel
{
    public Guid Id {get; set;}
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public PersonUpdateModel(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
}