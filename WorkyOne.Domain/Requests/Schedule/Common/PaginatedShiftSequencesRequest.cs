using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Schedule.Common
{
    /// <inheritdoc/>
    public sealed class PaginatedShiftSequencesRequest : PaginatedRequest<ShiftSequenceEntity>
    {
        public PaginatedShiftSequencesRequest(string? templateId = null)
        {
            TemplateId = templateId;
        }

        private string? _templateId;

        /// <summary>
        /// Идентификатор <see cref="TemplateEntity"/>, для которого запрашиваются <see cref="ShiftSequenceEntity"/>
        /// </summary>
        public string? TemplateId
        {
            get => _templateId;
            set
            {
                _templateId = value;
                Predicate = (x) => x.TemplateId == value;
            }
        }
    }
}
