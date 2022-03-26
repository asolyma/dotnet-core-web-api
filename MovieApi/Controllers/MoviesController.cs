using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Dtos;
using MovieApi.Models;
using MovieApi.Services;

namespace MovieApi.Controllers;
[Route("/api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMoviesService _moviesService;
    private readonly IGenresService _genresService;

    private readonly List<string> _allowedExtensions = new()
    {
        ".jpeg",
        ".jpg",
        ".png"

    };

    private readonly long _maxAllowedSize = 1048576;

    public MoviesController(IMoviesService moviesService,IGenresService genresService)
    {
        _moviesService = moviesService;
        _genresService = genresService;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _moviesService.GetAll();

        return Ok(movies);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var movie = await _moviesService.GetOne(id);

        return Ok(movie);

    }

    [HttpGet("getByGenre/{id}")]
    public async Task<IActionResult> getByGenre(int id)
    {
        var movie = await _moviesService.GetMoviesByGenre(id);
            return Ok(movie);
        
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromForm]MovieDto dto)
    {
        if (dto.Poster == null)
        {
            return BadRequest("poster is required");
        }
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

        var valid = await _genresService.IsvalidGenre(dto.GenreId);
        if (!valid )
        {
            return NotFound("invalid genre");
        }
        /*
        var isValidGenre = await _context.Genres.AllAsync(g => g.Id == dto.GenreId);
        if (!isValidGenre)
        {
            return BadRequest("invalid Genre");
        }
        */

        
        var movie = new Movie(){
            GenreId = dto.GenreId, 
            Title = dto.Title,
            Poster = dataStrem.ToArray(), 
            Year = dto.Year, 
            Rate = dto.Rate,
            StoryLine = dto.StoryLine
        };
      var m=  await _moviesService.addOne(movie);
      
       return Ok(m);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)

    {
        var movie = await _moviesService.DeleteOne(id);
        
        if (movie == null)
        {
            return Ok($"Movie with id:{id} successfully deleted");

        }

        return Ok((movie));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMovie(int id, [FromForm] MovieDto dto)
    {
        var mov = await _moviesService.GetOne(id);
        if (mov == null)
        {
            return NotFound("no movie with that id");
        }
        if (dto.Poster != null)
        {
            await using var dataStream = new MemoryStream();

            if (_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
            {
                await dto.Poster.CopyToAsync(dataStream);
                mov.Poster = dataStream.ToArray();
            }
            else
            {
                return BadRequest("only jpg an jpeg formats are allowed");
            }

            if (dto.Poster.Length>_maxAllowedSize)
            {
                return BadRequest("max size is 1 mb");
            }
            
        }

        var valid = await _genresService.IsvalidGenre(dto.GenreId);
        if (!valid)
        {
            return NotFound("invalid genre");
        }

        mov.Title = dto.Title;
           mov.Rate = dto.Rate;
           mov.Year = dto.Year;
         mov.GenreId = dto.GenreId;
        mov.StoryLine = dto.StoryLine;

        await _moviesService.UpdateOne(mov);
        return Ok(mov);

    }

}