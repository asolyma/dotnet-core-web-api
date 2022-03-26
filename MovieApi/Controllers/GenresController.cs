using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Dtos;
using MovieApi.Models;
using MovieApi.Services;

namespace MovieApi.Controllers;
[Route("/api/[controller]")]
public class GenresController : ControllerBase
{
    public GenresController(IGenresService genresService)
    {
        _genresService = genresService;
    }

    private readonly IGenresService _genresService;

    [HttpGet]
    public async Task<IActionResult> GetGenres()
    {
        var genres = await _genresService.GetAll();
        return Ok(genres);
    }

    [HttpPost]
    public async Task<ActionResult<Genre>> AddGenres([FromBody]CreateGenreDto dto)
    {
        var genre = await _genresService.addOne(dto);
       return Ok(genre);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Genre>> updateGenre(int id ,[FromBody] CreateGenreDto dto)
    {
        var genre = await _genresService.UpdateOne(id, dto);
        if (genre == null)
        {
            return NotFound("invalid id");
            
        }

        return genre;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Genre>> DeleteGenre(int id)
    {
        var genre = await _genresService.DeleteOne(id);
        return genre;
    }


}