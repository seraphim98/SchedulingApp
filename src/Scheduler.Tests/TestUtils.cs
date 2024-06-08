using AutoMapper;
using Scheduler;
using Scheduler.Database;
using Scheduler.Controllers;
using Scheduler.Models;
namespace Scheduler.Tests
{
    public class TestUtils
    {
        static public IMapper GetMapper()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            return mapper;
        }
    }
}
