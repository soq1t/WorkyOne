using System.Drawing;
using System.Globalization;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Infrastructure.Exceptions.Utilities.ColorUtility;

namespace WorkyOne.Infrastructure.Utilities
{
    /// <summary>
    /// Инструмент по работе с цветом
    /// </summary>
    public class ColorUtility : IColorUtility
    {
        public string GetRgbaFromHex(string hex, double alpha)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return "rgba(0, 0, 0, 0.0)";
            }

            if (alpha < 0 || alpha > 1)
            {
                throw new WrongAlphaValueException();
            }

            hex = hex.Replace("#", "");

            if (hex.Length != 6)
            {
                throw new WrongColorFormatException();
            }

            var color = ColorTranslator.FromHtml("#" + hex);

            return $"rgba({color.R}, {color.G}, {color.B}, {alpha.ToString("F1", CultureInfo.InvariantCulture)})";
        }
    }
}
