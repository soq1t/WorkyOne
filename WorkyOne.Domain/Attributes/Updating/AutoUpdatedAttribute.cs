using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Domain.Attributes.Updating
{
    /// <summary>
    /// Аттрибут указывающий, что свойство может быть обновлено сервисом по обновлению сущностей
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AutoUpdatedAttribute : Attribute { }
}
