using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD_Server.Domain.Features.Students;

namespace SD_Server.Infra.Data.Features.Students
{
    public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("tb_Student");

            //Chave primaria
            builder.HasKey(x => x.Id); //Define a chave primaria da tabela;

            builder.Property(x => x.Name)
                .IsRequired() //Define que o campo é obrigatório
                .HasColumnType("VARCHAR(50)")
                .HasMaxLength(50); //Define o tamanho máximo do campo

            builder.Property(x => x.Email).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(x => x.Age).IsRequired();
            builder.Property(x => x.CellPhone).HasColumnType("VARCHAR(15)").IsRequired();
            builder.Property(x => x.LastReview);
        }
    }
}
