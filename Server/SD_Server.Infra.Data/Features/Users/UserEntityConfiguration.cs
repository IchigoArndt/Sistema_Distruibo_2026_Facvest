using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD_Server.Domain.Features.Students;
using SD_Server.Domain.Features.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD_Server.Infra.Data.Features.Users
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tb_Users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).HasColumnType("VARCHAR(100)").IsRequired();
            builder.HasIndex(x => x.Email).IsUnique(); // email único no sistema
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.TypeAccess).HasConversion<int>().IsRequired();
        }
    }
}
