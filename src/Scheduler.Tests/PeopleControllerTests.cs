using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Moq;
using AutoMapper;
using Scheduler;
using Scheduler.Database;
using Scheduler.Controllers;
using Scheduler.Models;


namespace Scheduler.Tests;

[TestClass]
public class PersonControllerTests
{
    [TestMethod]
    public void PersonCreatedOnPost()
    {
        var mockSet = new Mock<DbSet<Person>>();

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(m => m.People).Returns(mockSet.Object);
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            var service = new PeopleController(mockContext.Object, mapper);

            service.PostPerson(new PersonCreateModel("Test", "Dummy"));

            mockSet.Verify(m => m.Add(It.IsAny<Person>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsynchronous(), Times.Once());
    }
}