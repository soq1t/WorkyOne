using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkyOne.Domain.Entities.Configuration
{
    public class UserDataEntityConfiguration : IEntityTypeConfiguration<UserDataEntity>
    {
        public void Configure(EntityTypeBuilder<UserDataEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .HasMany(u => u.Templates)
                .WithOne(t => t.UserData)
                .HasForeignKey(t => t.UserDataId);
        }
    }
}
