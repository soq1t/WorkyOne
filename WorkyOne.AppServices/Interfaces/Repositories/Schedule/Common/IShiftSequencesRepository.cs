using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="ShiftSequenceEntity"/>
    /// </summary>
    public interface IShiftSequencesRepository
        : IEntityRepository<ShiftSequenceEntity, ShiftSequenceRequest>
    {
        /// <summary>
        /// Возвращает список <see cref="ShiftSequenceEntity"/> для заданного <see cref="TemplateEntity"/>
        /// </summary>
        /// <param name="request">Запрос на получение <see cref="ShiftSequenceEntity"/></param>
        /// <returns></returns>
        public Task<ICollection<ShiftSequenceEntity>> GetByTemplateIdAsync(
            ShiftSequenceRequest request
        );
    }
}
