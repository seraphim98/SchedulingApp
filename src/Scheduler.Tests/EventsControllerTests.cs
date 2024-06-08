using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Moq;
using AutoMapper;
using Scheduler;
using Scheduler.Database;
using Scheduler.Controllers;
using Scheduler.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace Scheduler.Tests;

[TestClass]
public class EventsControllerTests
{

    private readonly Mock<IRepository<Event>> mockEventRepository = new();
    private readonly Mock<IRepository<PersonEvent>> mockPersonEventRepository = new();
    private readonly List<Event> eventData = new List<Event>
        {
            new(Guid.NewGuid(), "Event1", new DateTime(), new DateTime()),
            new(Guid.NewGuid(), "Event2", new DateTime(), new DateTime()),
            new(Guid.NewGuid(), "Event3", new DateTime(), new DateTime())
        };
    private readonly List<Guid> peopleIds = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
    [TestMethod]
    async public Task EventsReturnedOnGet()
    {
        mockEventRepository.Setup(x => x.Query()).ReturnsAsync(eventData);

        var controller = GetController();

        var Events = await controller.GetEvents();

        mockEventRepository.Verify(x => x.Query(), Times.Once());
    }
    [TestMethod]
    async public Task EventCreatedOnPost()
    {
        var controller = GetController();

        await controller.PostEvent(new EventCreateModel(eventData[0].Name, eventData[0].StartTime, eventData[0].EndTime));

        mockEventRepository.Verify(m => m.CreateAsync(It.IsAny<Event>()), Times.Once());
    }
    [TestMethod]
    async public Task EventDeletedOnDelete_NoPeople()
    {
        var personEventData = new List<PersonEvent>
        {
            new(Guid.NewGuid(), peopleIds[1], eventData[1].Id, "Event1"),
            new(Guid.NewGuid(), peopleIds[0], eventData[1].Id, "Event2"),
            new(Guid.NewGuid(), peopleIds[0], eventData[2].Id, "Event3")
        };
        mockPersonEventRepository.Setup(x => x.Query()).ReturnsAsync(personEventData);
        mockEventRepository.Setup(x => x.GetByIdAsync(eventData[0].Id)).ReturnsAsync(eventData[0]);

        var controller = GetController();
        await controller.DeleteEvent(eventData[0].Id);

        mockEventRepository.Verify(m => m.DeleteAsync(eventData[0]), Times.Once());
    }
    [TestMethod]
    async public Task EventDeletedOnDeleteAndDeletesPersonEvents()
    {
        var personEventData = new List<PersonEvent>
        {
            new(Guid.NewGuid(), peopleIds[0], eventData[0].Id, "Event1"),
            new(Guid.NewGuid(), peopleIds[0], eventData[1].Id, "Event2"),
            new(Guid.NewGuid(), peopleIds[0], eventData[2].Id, "Event3")
        };
        mockPersonEventRepository.Setup(x => x.Query()).ReturnsAsync(personEventData);
        mockEventRepository.Setup(x => x.GetByIdAsync(eventData[0].Id)).ReturnsAsync(eventData[0]);

        var controller = GetController();
        await controller.DeleteEvent(eventData[0].Id);

        mockPersonEventRepository.Verify(m => m.Delete(It.IsAny<PersonEvent>()), Times.Once());
        mockPersonEventRepository.Verify(m => m.SaveAsync(), Times.Once());
        mockEventRepository.Verify(m => m.DeleteAsync(eventData[0]), Times.Once());
    }
    [TestMethod]
    async public Task EventUpdatedOnPut_NoPeople()
    {
        var controller = GetController();
        var updateModel = new EventUpdateModel(eventData[0].Id, eventData[0].Name, eventData[0].StartTime, eventData[0].EndTime, new List<Guid>());

        await controller.PutEvent(eventData[0].Id, updateModel);

        mockEventRepository.Verify(m => m.Update(It.IsAny<Event>()), Times.Once());
        mockEventRepository.Verify(m => m.SaveAsync(), Times.Once());
    }
    [TestMethod]
    async public Task EventUpdatedOnPut_AddsPeople()
    {
        var personEventData = new List<PersonEvent>
        {
            new(Guid.NewGuid(), peopleIds[0], eventData[0].Id, "Event1"),
            new(Guid.NewGuid(), peopleIds[0], eventData[1].Id, "Event2"),
            new(Guid.NewGuid(), peopleIds[0], eventData[2].Id, "Event3")
        };
        mockPersonEventRepository.Setup(x => x.Query()).ReturnsAsync(personEventData);
        var controller = GetController();
        var updateModel = new EventUpdateModel(eventData[0].Id, eventData[0].Name, eventData[0].StartTime, eventData[0].EndTime, peopleIds);

        await controller.PutEvent(eventData[0].Id, updateModel);

        
        mockPersonEventRepository.Verify(m => m.Create(It.IsAny<PersonEvent>()), Times.Exactly(1));
        mockPersonEventRepository.Verify(m => m.SaveAsync(), Times.Exactly(1));
        mockEventRepository.Verify(m => m.Update(It.IsAny<Event>()), Times.Once());
        mockEventRepository.Verify(m => m.SaveAsync(), Times.Once());
    }
    [TestMethod]
    async public Task EventUpdatedOnPut_RemovesPeople()
    {
        var personEventData = new List<PersonEvent>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), eventData[0].Id, "Event1"),
            new(Guid.NewGuid(), Guid.NewGuid(), eventData[0].Id, "Event2"),
            new(Guid.NewGuid(), Guid.NewGuid(), eventData[0].Id, "Event3")
        };
        mockPersonEventRepository.Setup(x => x.Query()).ReturnsAsync(personEventData);
        var controller = GetController();
        var updateModel = new EventUpdateModel(eventData[0].Id, eventData[0].Name, eventData[0].StartTime, eventData[0].EndTime, peopleIds);

        await controller.PutEvent(eventData[0].Id, updateModel);


        mockPersonEventRepository.Verify(m => m.Delete(It.IsAny<PersonEvent>()), Times.Exactly(3));
        mockPersonEventRepository.Verify(m => m.Create(It.IsAny<PersonEvent>()), Times.Exactly(2));
        mockPersonEventRepository.Verify(m => m.SaveAsync(), Times.Exactly(1));
        mockEventRepository.Verify(m => m.Update(It.IsAny<Event>()), Times.Once());
        mockEventRepository.Verify(m => m.SaveAsync(), Times.Once());
    }

    private EventsController GetController()
    {
        var mapper = TestUtils.GetMapper();
        return new EventsController(mockEventRepository.Object, mockPersonEventRepository.Object, mapper);
    }
}