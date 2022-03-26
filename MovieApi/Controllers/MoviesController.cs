using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Dtos;
using MovieApi.Models;

namespace MovieApi.Controllers;
[Route("/api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly DatabaseContext _context;

    private readonly List<string> _allowedExtensions = new()
    {
        ".jpeg",
        ".jpg",
        ".png"

    };

    private readonly long _maxAllowedSize = 1048576;

    public MoviesController(DatabaseContext context)
    {
        _context = context;
    }

    // GET
    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromForm]MovieDto dto)
    {
        await using var dataStrem = new MemoryStream();

        if (_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
        {
           await dto.Poster.CopyToAsync(dataStrem); 
        }
        else
        {
            return BadRequest("only jpg an jpeg formats are allowed");
        }

        if (dto.Poster.Length>_maxAllowedSize)
        {
            return BadRequest("max size is 1 mb");
        }

        var isValidGenre = await _context.Genres.AllAsync(g => g.Id == dto.GenreId);
        if (!isValidGenre)
        {
            return BadRequest("invalid Genre");
        }

        
        var movie = new Movie(){
            GenreId = dto.GenreId, 
            Title = dto.Title,
            Poster = dataStrem.ToArray(), 
            Year = dto.Year, 
            Rate = dto.Rate,
            StoryLine = dto.StoryLine
        };
       await _context.Movies.AddAsync(movie);
       await _context.SaveChangesAsync();
       return Ok(movie);
    }
}