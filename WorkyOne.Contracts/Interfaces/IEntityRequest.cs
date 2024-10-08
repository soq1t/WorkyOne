using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Contracts.Interfaces
{
    /// <summary>
    /// Интрерфейс запроса на получение сущности из базы данных
    /// </summary>
    public interface IEntityRequest
    {
        /// <summary>
        /// ID запрашиваемой сущности
        /// </summary>
        public string Id { get; set; }
    }
}
