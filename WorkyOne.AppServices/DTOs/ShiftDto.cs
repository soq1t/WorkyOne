using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.AppServices.DTOs
{
    /// <summary>
    /// DTO рабочей смены
    /// </summary>
    public class ShiftDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? ColorCode { get; set; }
        public TimeOnly? Beginning { get; set; }
        public TimeOnly? Ending { get; set; }
    }
}
