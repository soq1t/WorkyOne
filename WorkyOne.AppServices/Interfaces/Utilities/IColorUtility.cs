﻿namespace WorkyOne.AppServices.Interfaces.Utilities
{
    /// <summary>
    /// Интерфейс инструмента по работе с цветом
    /// </summary>
    public interface IColorUtility
    {
        /// <summary>
        /// Преобразует HEX код цвета в RGBA
        /// </summary>
        /// <param name="hex">Значение цвета в формате HEX(##RRGGBB)</param>
        /// <param name="alpha">Значение прозрачности (0.0 - 1.0)</param>
        public string GetRgbaFromHex(string? hex, double alpha);

        /// <summary>
        /// Возвращает значение цвета для текста, который будет лучше всего виден на указанном цвете фона
        /// </summary>
        /// <param name="hex">Цвет фона</param>
        public string GetForegroundColor(string? hex);
    }
}