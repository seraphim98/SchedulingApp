using Scheduler.Models;
using AutoMapper;
namespace Scheduler.Convertors
{
    public class GuidToPersonCovertor: ITypeConverter<Guid, Person> 
    {
        public Person Convert(Guid source, Person person, ResolutionContext context)
        {
            return new Person(source, string.Empty, string.Empty);
        }
    }
}
