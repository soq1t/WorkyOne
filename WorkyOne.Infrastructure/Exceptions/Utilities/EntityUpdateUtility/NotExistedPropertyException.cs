using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Attributes.Updating;

namespace WorkyOne.Infrastructure.Exceptions.Utilities.EntityUpdateUtility
{
    public sealed class NotExistedPropertyException : Exception
    {
        public NotExistedPropertyException(string propName)
            : base(GetMessage(propName)) { }

        private static string GetMessage(string propName) =>
            $"{propName} not exists or has [NotUpdated] attribute.";
    }
}
