using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Domain.Exceptions.Scedule
{
    public class WrongShiftTimeException : Exception
    {
        public WrongShiftTimeException(bool isBeginningDeclared)
            : base(GetMessage(isBeginningDeclared)) { }

        private static string GetMessage(bool isBeginningDeclared)
        {
            return isBeginningDeclared
                ? "Не указано время окончания смены"
                : "Не указано время начала смены";
        }
    }
}
