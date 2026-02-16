using System.Globalization;
using EFCore.API.Data.ValueConverters;
using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.API.EntityMapping;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        //Global filter queries only data from 1999
        // builder.HasQueryFilter(m => m.ReleaseDate >= new DateTime(1999, 3, 10));
        
        builder.Property(m => m.Title)
            .HasColumnType("varchar")
            .HasMaxLength(128)
            .IsRequired();
        
        // builder.Property(m => m.ReleaseDate)
        //     .HasColumnType("date");
        
        // value converter if date is stored as string of the same length
        // builder.Property(m => m.ReleaseDate)
        //     .HasColumnName("char(23)")
        //     .HasConversion<string>();
        
        //custom value converter
        builder.Property(m => m.ReleaseDate)
            .HasColumnName("char(8)")
            .HasConversion(new DateTimeToChar8Converter());
        
        builder.Property(m => m.Synopsis)
            .HasColumnType("varchar(max)")
            .HasColumnName("Plot");
        
        // builder.Property(m => m.AgeRating)
        //     .HasColumnType("varchar(32)")
        //     .HasConversion<string>();

        // create table automaticcally and bind it to movies
        builder.OwnsOne(m => m.Director)
            .ToTable("Movie_Directors");
        builder.OwnsMany(m => m.Actors)
            .ToTable("Movie_Actors");
        
        //if not using conventions - id name is not a key 
        builder.HasOne(m => m.Genre)
            .WithMany(g => g.Movies)
            .HasPrincipalKey(g => g.Id)
            .HasForeignKey(m=>m.MainGenreId);

        builder.HasData(new Movie
        {
            Id = 1,
            Title = "The Matrix",
            ReleaseDate = new DateTime(1999, 3, 10),
            Synopsis =
                "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
            MainGenreId = 1,
            AgeRating = AgeRating.R
        });
        
        builder.OwnsOne(m=>m.Director)
            .HasData(new { MovieId = 1, FirstName = "Andy", LastName = "Wachowski" });
        
        builder.OwnsMany(m=>m.Actors)
            .HasData(new { MovieId = 1, Id=1, FirstName = "Keanu", LastName = "Reeves" },
            new { MovieId = 1, Id=2, FirstName = " Laurence", LastName = "Fishburne" });
    }
}