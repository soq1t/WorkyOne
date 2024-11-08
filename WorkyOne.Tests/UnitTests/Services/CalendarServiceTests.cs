using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using Xunit;

namespace WorkyOne.Tests.UnitTests.Services
{
    public class CalendarServiceTests
    {
        [Theory]
        [InlineData(2024, 11, 10, 28, 12, 1)]
        [InlineData(2024, 12, 11, 25, 1, 5)]
        [InlineData(2024, 8, 7, 29, 9, 1)]
        [InlineData(2024, 7, 7, 1, 8, 4)]
        public void GetCalendarInfo_ShouldReturnCorrectResult(
            int year,
            int month,
            int expectedStartMonth,
            int expectedStartDay,
            int expectedEndMonth,
            int expectedEndDay
        )
        {
            // Arrange
            var calendarService = new CalendarService();

            // Act
            var calendarInfo = calendarService.GetCalendarInfo(year, month);

            // Assert

            Assert.NotNull(calendarInfo);
            Assert.Equal(calendarInfo.Start.Month, expectedStartMonth);
            Assert.Equal(calendarInfo.Start.Day, expectedStartDay);
            Assert.Equal(calendarInfo.End.Month, expectedEndMonth);
            Assert.Equal(calendarInfo.End.Day, expectedEndDay);
        }
    }
}
