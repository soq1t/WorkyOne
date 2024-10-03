using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Domain.Entities.Schedule.Shifts
{
    /// <summary>
    /// Сущность, описывающая смену, используемую в качестве примера при создании пользовательских смен
    /// </summary>
    public sealed class ExampleShiftEntity : ShiftEntity
    {
        public static ExampleShiftEntity BuildExampleFromShift(ShiftEntity shift)
        {
            return new ExampleShiftEntity
            {
                Name = shift.Name,
                Beginning = shift.Beginning,
                Ending = shift.Ending,
                ColorCode = shift.ColorCode,
            };
        }
    }
}
