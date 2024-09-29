using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.AppServices.DTOs
{
    /// <summary>
    /// DTO, описывающее повторение смены
    /// </summary>
    public class RepititionDto
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Смена
        /// </summary>
        public ShiftDto Shift { get; set; }

        /// <summary>
        /// Количество повторений смены, указанной в поле Shift
        /// </summary>
        public int RepetitionAmount { get; set; }

        /// <summary>
        /// Порядковый номер повторения в шаблоне
        /// </summary>
        public int Position { get; set; }
    }
}
