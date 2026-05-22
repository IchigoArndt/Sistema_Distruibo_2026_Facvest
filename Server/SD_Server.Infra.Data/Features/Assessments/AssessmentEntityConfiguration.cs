using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD_Server.Domain.Features.Assessments;

namespace SD_Server.Infra.Data.Features.Assessments
{
    public class AssessmentEntityConfiguration : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder.ToTable("tb_Assessments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.ProfessionalId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnType("DATETIME2");

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Methodology)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasColumnType("TEXT");

            builder.Property(x => x.Results)
                .HasColumnType("TEXT");

            builder.HasIndex(x => x.StudentId);
            builder.HasIndex(x => x.ProfessionalId);
        }
    }
}
