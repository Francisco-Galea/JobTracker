using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Persistence.Configurations
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            builder.ToTable("job_applications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Company)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Position)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.JobUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>() // Guarda el enum como texto en la DB
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>() // Guarda el enum como texto en la DB
                .HasMaxLength(50);

            builder.Property(x => x.AppliedAt)
                .IsRequired();

            builder.Property(x => x.LastUpdatedAt);
        }
    }
}
