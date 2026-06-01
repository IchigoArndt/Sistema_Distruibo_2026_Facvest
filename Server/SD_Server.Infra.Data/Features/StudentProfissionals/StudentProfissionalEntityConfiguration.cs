using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD_Server.Domain.Features.StudentProfessionals;

namespace SD_Server.Infra.Data.Features.StudentProfissionals;

public class StudentProfissionalEntityConfiguration : IEntityTypeConfiguration<StudentProfessional>
{
    public void Configure(EntityTypeBuilder<StudentProfessional> builder)
    {
        builder.ToTable("tb_StudentProfessionals"); //Cria a tabela
        
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
        
        builder.Property(x => x.LinkedAt).IsRequired().HasDefaultValueSql("GETDATE()"); //Estranho mais pega o valor default do SQL
        builder.Property(x => x.Status).HasConversion<int>();
    }
}