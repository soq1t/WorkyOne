﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Repositories.Configurations.Common
{
    public class UserDataEntityConfiguration : IEntityTypeConfiguration<UserDataEntity>
    {
        public void Configure(EntityTypeBuilder<UserDataEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .HasMany(u => u.Schedules)
                .WithOne(t => t.UserData)
                .HasForeignKey(t => t.UserDataId);
        }
    }
}
