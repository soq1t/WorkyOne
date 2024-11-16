using WorkyOne.Infrastructure.Utilities;
using Xunit;

namespace WorkyOne.Tests.UnitTests.Utilities
{
    public class ColorUtilityTests
    {
        [Theory]
        [InlineData("#B3D89C", 0.5, "rgba(179, 216, 156, 0.5)")]
        [InlineData("#4D7298", 1, "rgba(77, 114, 152, 1.0)")]
        [InlineData("#840032", 0.1, "rgba(132, 0, 50, 0.1)")]
        [InlineData("", 0.6, "rgba(0, 0, 0, 0.0)")]
        public void Hex_MustConvertCorrectly(string hex, double alpha, string expected)
        {
            // Arrange

            var colorUtility = new ColorUtility();

            // Act

            var result = colorUtility.GetRgbaFromHex(hex, alpha);

            // Assert

            Assert.NotEmpty(result);
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }
    }
}
