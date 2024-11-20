using AutoMapper;
using Moq;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.AppServices.Services.Schedule.Common;
using WorkyOne.Contracts.Services.Requests;
using Xunit;

namespace WorkyOne.Tests.UnitTests.Services
{
    public class CalendarServiceTests
    {
        private readonly Mock<IDateTimeService> _dateTimeServiceMock = new Mock<IDateTimeService>();
        private readonly Mock<ISchedulesRepository> _schedulesRepository =
            new Mock<ISchedulesRepository>();
        private readonly Mock<IAccessFiltersStore> _accessFiltersStore =
            new Mock<IAccessFiltersStore>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IWorkGraphicService> _workGraphicService =
            new Mock<IWorkGraphicService>();

        public CalendarServiceTests()
        {
            _dateTimeServiceMock.Setup(x => x.GetNow()).Returns(DateTime.Now);
        }

        [Theory]
        [InlineData(2024, 11, 10, 28, 12, 1, 35)]
        [InlineData(2024, 12, 11, 25, 1, 5, 42)]
        [InlineData(2024, 8, 7, 29, 9, 1, 35)]
        [InlineData(2024, 7, 7, 1, 8, 4, 35)]
        public void GetCalendarInfo_ShouldReturnCorrectResult(
            int year,
            int month,
            int expectedStartMonth,
            int expectedStartDay,
            int expectedEndMonth,
            int expectedEndDay,
            int expectedDaysAmount
        )
        {
            // Arrange
            var calendarService = new CalendarService(
                _dateTimeServiceMock.Object,
                _schedulesRepository.Object,
                _accessFiltersStore.Object,
                _mapper.Object,
                _workGraphicService.Object
            );

            // Act
            var calendarInfo = calendarService.GetCalendarInfo(
                new CalendarInfoRequest { Year = year, Month = month }
            );

            // Assert

            Assert.NotNull(calendarInfo);
            Assert.Equal(calendarInfo.Start.Month, expectedStartMonth);
            Assert.Equal(calendarInfo.Start.Day, expectedStartDay);
            Assert.Equal(calendarInfo.End.Month, expectedEndMonth);
            Assert.Equal(calendarInfo.End.Day, expectedEndDay);

            Assert.Equal(calendarInfo.DaysAmount, expectedDaysAmount);
        }
    }
}
