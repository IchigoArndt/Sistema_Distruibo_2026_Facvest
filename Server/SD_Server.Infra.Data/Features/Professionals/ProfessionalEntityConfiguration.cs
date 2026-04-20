using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD_Server.Domain.Features.Professionals;

namespace SD_Server.Infra.Data.Features.Professionals
{
    public class ProfessionalEntityConfiguration : IEntityTypeConfiguration<Professional>
    {
        public void Configure(EntityTypeBuilder<Professional> builder)
        {
            builder.ToTable("tb_Professionals");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(50)")
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.Cref)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();
            builder.HasIndex(x => x.Cref).IsUnique();

            builder.Property(x => x.Phone)
                .HasColumnType("VARCHAR(15)");

            builder.Property(x => x.Bio)
                .HasColumnType("TEXT");

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}
