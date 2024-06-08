using AutoMapper;
using Scheduler.Models;
using Scheduler.Convertors;

namespace Scheduler
{
    public class MappingProfile : Profile 
    {   
        public MappingProfile() 
        {
            CreateMap<Event, EventModel>();
            CreateMap<EventCreateModel, Event>().ForCtorParam(nameof(Event.Id), x => x.MapFrom(y => Guid.NewGuid()));
            CreateMap<EventUpdateModel, Event>();
            CreateMap<Holiday, HolidayModel>();
            CreateMap<HolidayCreateModel, Holiday>().ForCtorParam(nameof(Event.Id), x => x.MapFrom(y => Guid.NewGuid()));
            CreateMap<HolidayUpdateModel, Holiday>();
            CreateMap<Person, PersonModel>().ForCtorParam(nameof(PersonModel.EventNames), x => x.MapFrom(y => y.PersonEvents.Select(z => z.EventName)));
            CreateMap<PersonCreateModel, Person>().ForCtorParam(nameof(Person.Id), x => x.MapFrom(y => Guid.NewGuid()));
            CreateMap<PersonUpdateModel, Person>();
            CreateMap<Guid, Person>().ConvertUsing(new GuidToPersonCovertor());
        }
    }
}
