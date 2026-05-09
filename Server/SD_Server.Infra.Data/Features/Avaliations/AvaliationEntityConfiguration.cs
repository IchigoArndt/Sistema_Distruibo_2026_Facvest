using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD_Server.Domain.Features.Avaliations;

namespace SD_Server.Infra.Data.Features.Avaliations
{
    public class AvaliationEntityConfiguration : IEntityTypeConfiguration<Avaliation>
    {
        public void Configure(EntityTypeBuilder<Avaliation> builder)
        {
            builder.ToTable("tb_Avaliations");

            builder.HasKey(x => x.Id);

            #region Chaves Estrangeiras
            builder.Property(x => x.StudentId).IsRequired();
            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ProfessionalId).IsRequired();
            builder.HasOne(x => x.Professional)
                .WithMany()
                .HasForeignKey(x => x.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Tipos Simples

            builder.Property(x => x.Date).IsRequired().HasDefaultValueSql("GETDATE()"); //Estranho mais pega o valor default do SQL
            builder.Property(x => x.TechnicalOpinion).HasColumnType("VARCHAR(MAX)");
            builder.Property(x => x.IMC).HasColumnType("VARCHAR(50)");
            builder.Property(x => x.BodyFatPercentage).HasColumnType("VARCHAR(50)");

            #endregion

            #region Tipos Complexos
            builder.OwnsOne(x => x.Anamnesis, a =>
            {
                a.Property(p => p.HasInjury);
                a.Property(p => p.InjuryDescription).HasColumnType("VARCHAR(255)");
                a.Property(p => p.HasMedication);
                a.Property(p => p.MedicationDescription).HasColumnType("VARCHAR(255)");
                a.Property(p => p.ActivityLevel).HasConversion<int>();
            });

            builder.OwnsOne(b => b.BodyComposition, b =>
            {
                b.Property(p => p.Weight);
                b.Property(p => p.Height);
                b.Property(p => p.Waist);
                b.Property(p => p.Abdomen);
                b.Property(p => p.Hips);
                b.Property(p => p.Chest);
                b.Property(p => p.ArmRight);
                b.Property(p => p.ArmLeft);
                b.Property(p => p.RightThigh);
                b.Property(p => p.LeftThigh);
            });

            builder.OwnsOne(s => s.Skinfolds, s =>
            {
                s.Property(p => p.Pectoral);
                s.Property(p => p.MidAxillary);
                s.Property(p => p.Triceps);
                s.Property(p => p.Subscapular);
                s.Property(p => p.Suprailiac);
                s.Property(p => p.Abdominal);
                s.Property(p => p.Thigh);
            });
            #endregion

            #region Enums
            builder.Property(x => x.TypeAvaliation).HasConversion<int>();
            builder.Property(x => x.StudentObjective).HasConversion<int>();
            builder.Property(x => x.Status).HasConversion<int>();
            #endregion
        }
    }
}
