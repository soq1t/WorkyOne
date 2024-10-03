using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule
{
    /// <summary>
    /// Интерфейс репозитория по работе с последовательностями смен в шаблонах
    /// </summary>
    public interface IShiftSequencesRepository
    {
        public Task<List<ShiftSequenceEntity>>? GetByTemplateIdAsync(string templateId);
    }
}
