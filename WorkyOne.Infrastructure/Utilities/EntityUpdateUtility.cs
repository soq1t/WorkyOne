﻿using System.Reflection;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Infrastructure.Exceptions.Utilities.EntityUpdateUtility;

namespace WorkyOne.Infrastructure.Utilities
{
    /// <summary>
    /// Сервис по обновлению сущностей
    /// </summary>
    public class EntityUpdateUtility : IEntityUpdateUtility
    {
        public void Update<TEntity>(
            TEntity target,
            TEntity source,
            IEnumerable<string>? propNames = null
        )
        {
            List<PropertyInfo> props = GetUpdatableProperties(typeof(TEntity));

            if (propNames == null)
                propNames = props.Select(p => p.Name).ToList();

            foreach (var item in propNames)
            {
                var prop = props.FirstOrDefault(p => p.Name == item);

                if (prop == null)
                    throw new NotExistedPropertyException(item);

                prop.SetValue(target, prop.GetValue(source));
            }
        }

        private List<PropertyInfo> GetUpdatableProperties(Type type)
        {
            return type.GetProperties()
                .Where(p => p.GetCustomAttribute<AutoUpdatedAttribute>() != null)
                .ToList();
        }
    }
}