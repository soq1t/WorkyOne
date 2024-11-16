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
        public string GetForegroundColor(string? hex)
        {
            var color = HexToColor(hex);

            double brightness = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;

            return brightness < 128 ? "#FFFFFF" : "#000000";
        }

        public string GetRgbaFromHex(string? hex, double alpha)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return "rgba(255, 255, 255, 0.0)";
            }

            if (alpha < 0 || alpha > 1)
            {
                throw new WrongAlphaValueException();
            }

            var color = HexToColor(hex);

            return $"rgba({color.R}, {color.G}, {color.B}, {alpha.ToString("F1", CultureInfo.InvariantCulture)})";
        }

        private Color HexToColor(string? hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return Color.White;
            }

            hex = hex.Replace("#", "");

            if (hex.Length != 6)
            {
                throw new WrongColorFormatException();
            }

            return ColorTranslator.FromHtml("#" + hex);
        }
    }
}
