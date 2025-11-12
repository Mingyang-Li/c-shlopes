using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkiFieldTracker.Domain.Entities;

namespace SkiFieldTracker.Infrastructure.Persistence.Configurations;

public class SkiFieldConfiguration : IEntityTypeConfiguration<SkiField>
{
    public void Configure(EntityTypeBuilder<SkiField> builder)
    {
        builder.ToTable("ski_fields");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasMaxLength(120)
            .IsRequired()
            .HasColumnName("name");

        builder.Property(x => x.Country)
            .HasMaxLength(80)
            .IsRequired()
            .HasColumnName("country");

        builder.Property(x => x.Region)
            .HasMaxLength(120)
            .IsRequired()
            .HasColumnName("region");

        builder.Property(x => x.AdultFullDayPassUsd)
            .HasColumnType("numeric(10,2)")
            .IsRequired()
            .HasColumnName("adult_full_day_pass_usd");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("timezone('utc', now())")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("timezone('utc', now())");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("ux_ski_fields_name");
    }
}

