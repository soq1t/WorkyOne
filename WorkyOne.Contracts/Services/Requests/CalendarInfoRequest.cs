using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Contracts.Services.Requests
{
    public class CalendarInfoRequest
    {
        [Required]
        [Range(1900, 3000)]
        public int Year { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }
    }
}
