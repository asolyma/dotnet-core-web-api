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
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _context.Movies.Include(m=>m.Genre).Select(g=>new MovieDetailsDto(){ Title = g.Title,GenreName = g.Genre.Name,Rate = g.Rate,Year = g.Year,StoryLine = g.StoryLine}).ToListAsync();
        return Ok(movies);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var movie =await _context.Movies.Include(m=>m.Genre).SingleOrDefaultAsync(m=>m.Id==id);
        
        if (movie ==null)
        {
            return NotFound("movie not found");
        }

        var moviedto = new MovieDetailsDto()
        {
            Rate = movie.Rate,
            Title = movie.Title,
            Year = movie.Year,
            GenreName = movie.Genre.Name,
            StoryLine = movie.StoryLine
        };
        
        return Ok(moviedto);

    }

    [HttpGet("getByGenre/{id}")]
    public async Task<IActionResult> getByGenre(int id)
    {
        var movie = await _context.Movies.Include(m => m.Genre).Where(m => m.GenreId == id).ToListAsync();
            return Ok(movie);
        
    }

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