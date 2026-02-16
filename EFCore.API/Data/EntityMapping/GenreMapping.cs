using EFCore.API.Data.ValueGenerators;
using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.API.EntityMapping;

public class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.Property<DateTime>("CreatedDate") // shadow property - not in genre model
            .HasColumnName("CreatedAt")
            // .HasDefaultValueSql("getdate()"); -generated on db side
            .HasValueGenerator<CreatedDateGenerator>(); // generated on app side ONLY when adding a new data
        
        builder.HasData(new Genre
        {
            Id = 1,
            Name = "Action"
        });
    }
}