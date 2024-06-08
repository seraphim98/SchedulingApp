using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Moq;
using AutoMapper;
using Scheduler;
using Scheduler.Database;
using Scheduler.Controllers;
using Scheduler.Models;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;
using System.Data.Entity.Infrastructure;

//Source https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
namespace Scheduler.Tests;

[TestClass]
public class HolidaysControllerTests
{
    Mock<IRepository<Holiday>> mockRepository = new();
    private readonly List<Holiday> data = new List<Holiday>
        {
            new(Guid.NewGuid(), "1", new DateTime()),
            new(Guid.NewGuid(), "2", new DateTime()),
            new(Guid.NewGuid(), "3", new DateTime())
        };
    [TestMethod]
    async public Task HolidaysReturnedOnGet()
    {
        mockRepository.Setup(x => x.Query()).ReturnsAsync(data);
        var mapper = TestUtils.GetMapper();
        var service = new HolidaysController(mockRepository.Object, mapper);

        var holidays = await service.GetHolidays();

        mockRepository.Verify(x => x.Query(), Times.Once());

    }
    [TestMethod]
    async public Task HolidayCreatedOnPost()
    {
        var mapper = TestUtils.GetMapper();
        var service = new HolidaysController(mockRepository.Object, mapper);

        await service.PostHoliday(new HolidayCreateModel("Test", new DateTime()));

        mockRepository.Verify(m => m.CreateAsync(It.IsAny<Holiday>()), Times.Once());
    }
    [TestMethod]
    async public Task HolidayDeletedOnDelete()
    {
        var mapper = TestUtils.GetMapper();
        mockRepository.Setup(x => x.GetByIdAsync(data[0].Id)).ReturnsAsync(data[0]);

        var service = new HolidaysController(mockRepository.Object, mapper);
        await service.DeleteHoliday(data[0].Id);

        mockRepository.Verify(m => m.DeleteAsync(data[0]), Times.Once());
    }
    [TestMethod]
    async public Task HolidayUpdatedOnPut()
    {
        var mapper = TestUtils.GetMapper();
        var service = new HolidaysController(mockRepository.Object, mapper);

        var updateModel = new HolidayUpdateModel(data[0].Id, data[0].Name, data[0].Date);

        await service.PutHoliday(data[0].Id, updateModel);

        mockRepository.Verify(m => m.UpdateAsync(It.IsAny<Holiday>()), Times.Once());
    }
}