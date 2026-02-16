using EFCore.API.Data;
using EFCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : Controller
{
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
        _context = context;
    }
    [HttpGet]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var rsp = await _context.Movies.ToListAsync();
        return Ok(rsp);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        // Always hit db (throws if more than one find (FirstOrDefault the same but not with duplicates)
        //var rsp = await _context.Movies.SingleOrDefaultAsync(m =>m.Id == id);
        
        // Build in ef cache
        var rsp = await _context.Movies
            .Include(g => g.Genre)
            .SingleOrDefaultAsync(m => m.Id == id);
        
        return rsp == null ? NotFound() : Ok(rsp);
    }
    
    [HttpGet("until-age/{ageRating}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUntilAge([FromRoute] AgeRating ageRating)
    {
        var rsp = await _context.Movies
            .Where(m => m.AgeRating <= ageRating)
            .Select(m => new MovieTitle { Id = m.Id, Title = m.Title })
            .ToListAsync();
        
        return Ok(rsp);
    }
    
    
    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByYear([FromRoute] int year)
    {
        var rsp = await _context.Movies.Where(m => m.ReleaseDate.Year == year).ToListAsync();
        return Ok(rsp);
    }
    
    [HttpGet("titles-by-year/{year:int}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTitlesAllByYear([FromRoute] int year)
    {
        var rsp = await _context.Movies
            .Where(m => m.ReleaseDate.Year == year)
            .Select(m => new MovieTitle { Id = m.Id, Title = m.Title }).ToListAsync();
        
        return Ok(rsp);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie)
    {
        var oldMovie = await _context.Movies.FindAsync(id);
        
        if (oldMovie != null)
            return NotFound();

        oldMovie!.Title = movie.Title;
        oldMovie!.ReleaseDate = movie.ReleaseDate;
        oldMovie!.Synopsis = movie.Synopsis;
        //Use change tracker for update
        await _context.SaveChangesAsync();
        return Ok(oldMovie);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var oldMovie = await _context.Movies.FindAsync(id);
        
        if (oldMovie != null)
            return NotFound();
        
        _context.Movies.Remove(oldMovie!);
 
        await _context.SaveChangesAsync();
        return Ok();
    }
}
