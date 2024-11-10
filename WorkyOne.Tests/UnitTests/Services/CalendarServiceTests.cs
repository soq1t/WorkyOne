using Moq;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using Xunit;

namespace WorkyOne.Tests.UnitTests.Services
{
    public class CalendarServiceTests
    {
        private readonly Mock<IDateTimeService> _dateTimeServiceMock = new Mock<IDateTimeService>();

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
            var calendarService = new CalendarService(_dateTimeServiceMock.Object);

            // Act
            var calendarInfo = calendarService.GetCalendarInfo(year, month);

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
