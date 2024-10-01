using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.AppServices.DTOs;

namespace WorkyOne.AppServices.Interfaces.Services
{
    /// <summary>
    /// Интерфейс сервиса по работе с расписаниями
    /// </summary>
    public interface IScheduleService
    {
        #region Смены
        /// <summary>
        /// Удаляет смену из базы данны
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        public Task DeleteShiftAsync(string shiftId);
        public Task AddShiftAsync(ShiftDto shiftDto, string templateId);
        public Task UpdateShiftAsync(ShiftDto shiftDto);
        #endregion
    }
}
