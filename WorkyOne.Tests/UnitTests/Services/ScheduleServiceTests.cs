using AutoMapper;
using Moq;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Services;
using WorkyOne.AppServices.Services.Schedule.Common;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Schedule.Common;
using WorkyOne.Repositories.Repositories.Schedule.Common;
using Xunit;

namespace WorkyOne.Tests.UnitTests.Services
{
    public class ScheduleServiceTests
    {
        private readonly Mock<ISchedulesRepository> _schedulesRepoMock =
            new Mock<ISchedulesRepository>();
        private readonly Mock<IDailyInfosRepository> _dailyInfosRepoMock =
            new Mock<IDailyInfosRepository>();
        private readonly Mock<IUserDatasRepository> _userDatasRepoMock =
            new Mock<IUserDatasRepository>();
        private readonly Mock<IDateTimeService> _dateTimeServiceMock = new Mock<IDateTimeService>();

        private readonly IMapper _mapper;

        public ScheduleServiceTests()
        {
            var config = new MapperConfiguration(mc => mc.AddProfile(new DailyInfoProfile()));

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GenerateDailyAsync_ShouldGenerateCorrectResultFromFiveTwo()
        {
            // Arrange
            var template = new TemplateEntity()
            {
                StartDate = new DateOnly(2024, 10, 14),
                Shifts = new List<TemplatedShiftEntity>
                {
                    new TemplatedShiftEntity
                    {
                        ColorCode = "#111111",
                        Name = "День",
                        Beginning = new TimeOnly(9, 0),
                        Ending = new TimeOnly(17, 0)
                    },
                    new TemplatedShiftEntity { ColorCode = "#333333", Name = "Выходной" }
                }
            };

            var sequences = new List<ShiftSequenceEntity>
            {
                new ShiftSequenceEntity { Position = 1, Shift = template.Shifts[0] },
                new ShiftSequenceEntity { Position = 2, Shift = template.Shifts[0] },
                new ShiftSequenceEntity { Position = 3, Shift = template.Shifts[0] },
                new ShiftSequenceEntity { Position = 4, Shift = template.Shifts[0] },
                new ShiftSequenceEntity { Position = 5, Shift = template.Shifts[0] },
                new ShiftSequenceEntity { Position = 6, Shift = template.Shifts[1] },
                new ShiftSequenceEntity { Position = 7, Shift = template.Shifts[1] },
            };

            template.Sequences = sequences;
            var schedule = new ScheduleEntity { Name = "Расписание", Template = template };

            _schedulesRepoMock
                .Setup(r => r.GetAsync(It.IsAny<ScheduleRequest>(), default))
                .Returns(Task.FromResult(schedule));
            _dailyInfosRepoMock
                .Setup(r => r.CreateManyAsync(It.IsAny<List<DailyInfoEntity>>(), default))
                .Returns(Task.FromResult(new RepositoryResult("1")));

            var service = new ScheduleService(
                _schedulesRepoMock.Object,
                _dailyInfosRepoMock.Object,
                _userDatasRepoMock.Object,
                _mapper,
                _dateTimeServiceMock.Object
            );

            // Act

            var graphic = await service.GenerateDailyAsync(
                "1",
                new DateOnly(2024, 10, 1),
                new DateOnly(2024, 10, 31)
            );

            // Assert

            Assert.NotNull(graphic);
            Assert.Equal(31, graphic.Count);
            Assert.False(graphic.First(i => i.Date == new DateOnly(2024, 10, 12)).IsBusyDay);
            Assert.False(graphic.First(i => i.Date == new DateOnly(2024, 10, 13)).IsBusyDay);
            Assert.False(graphic.First(i => i.Date == new DateOnly(2024, 10, 19)).IsBusyDay);
            Assert.False(graphic.First(i => i.Date == new DateOnly(2024, 10, 20)).IsBusyDay);
        }

        [Fact]
        public async Task GenerateDailyAsync_ShouldGenerateCorrectResultFromTwoTwo()
        {
            // Arrange
            var template = new TemplateEntity()
            {
                StartDate = new DateOnly(2024, 10, 14),
                Shifts = new List<TemplatedShiftEntity>
                {
                    new TemplatedShiftEntity
                    {
                        ColorCode = "#111111",
                        Name = "День",
                        Beginning = new TimeOnly(8, 0),
                        Ending = new TimeOnly(20, 0)
                    },
                    new TemplatedShiftEntity
                    {
                        ColorCode = "#111111",
                        Name = "Ночь",
                        Beginning = new TimeOnly(20, 0),
                        Ending = new TimeOnly(8, 0)
                    },
                    new TemplatedShiftEntity { ColorCode = "#333333", Name = "Выходной" }
                }
            };

            var sequences = new List<ShiftSequenceEntity>
            {
                new ShiftSequenceEntity { Position = 1, Shift = template.Shifts[0] },
                new ShiftSequenceEntity { Position = 2, Shift = template.Shifts[1] },
                new ShiftSequenceEntity { Position = 3, Shift = template.Shifts[2] },
                new ShiftSequenceEntity { Position = 4, Shift = template.Shifts[2] }
            };

            template.Sequences = sequences;
            var schedule = new ScheduleEntity { Name = "Расписание", Template = template };

            _schedulesRepoMock
                .Setup(r => r.GetAsync(It.IsAny<ScheduleRequest>(), default))
                .Returns(Task.FromResult(schedule));
            _dailyInfosRepoMock
                .Setup(r => r.CreateManyAsync(It.IsAny<List<DailyInfoEntity>>(), default))
                .Returns(Task.FromResult(new RepositoryResult("1")));

            var service = new ScheduleService(
                _schedulesRepoMock.Object,
                _dailyInfosRepoMock.Object,
                _userDatasRepoMock.Object,
                _mapper,
                _dateTimeServiceMock.Object
            );

            // Act

            List<DailyInfoDto> graphic = await service.GenerateDailyAsync(
                "1",
                new DateOnly(2024, 10, 1),
                new DateOnly(2024, 10, 31)
            );

            // Assert

            Assert.NotNull(graphic);
            Assert.Equal(31, graphic.Count);
            Assert.False(graphic.First(i => i.Date == new DateOnly(2024, 10, 4)).IsBusyDay);
            Assert.False(graphic.First(i => i.Date == new DateOnly(2024, 10, 5)).IsBusyDay);

            Assert.False(graphic.First(i => i.Date == new DateOnly(2024, 10, 24)).IsBusyDay);
            Assert.False(graphic.First(i => i.Date == new DateOnly(2024, 10, 25)).IsBusyDay);

            Assert.True(graphic.First(i => i.Date == new DateOnly(2024, 10, 10)).IsBusyDay);
            Assert.Equal(
                graphic.First(i => i.Date == new DateOnly(2024, 10, 10)).ShiftProlongation,
                new TimeSpan(12, 0, 0)
            );
            Assert.True(graphic.First(i => i.Date == new DateOnly(2024, 10, 11)).IsBusyDay);
            Assert.Equal(
                graphic.First(i => i.Date == new DateOnly(2024, 10, 11)).ShiftProlongation,
                new TimeSpan(12, 0, 0)
            );
        }
    }
}
