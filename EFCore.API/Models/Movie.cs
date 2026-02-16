using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.API.Models;

// DO NOT USE ATTRIBUTES ON DOMAIN MODELS
// [Table("other_movies")] - renames table
public class Movie
{
    // [Key]
    public int Id { get; set; }
    // [Required] - not null in db
    public string? Title { get; set; }    
    public DateTime ReleaseDate { get; set; }
    // [Column("Plot",TypeName = "varchar(max)")] rename and type
    public string? Synopsis { get; set; }
    public AgeRating AgeRating { get; set; }
    
    public Person Director { get; set; }
    public ICollection<Person> Actors { get; set; }
    
    public Genre Genre { get; set; }
    public int MainGenreId { get; set; }
} 