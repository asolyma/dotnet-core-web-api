using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Dtos;
using MovieApi.Models;

namespace MovieApi.Controllers;
[Route("/api/[controller]")]
public class GenresController : ControllerBase
{
    public GenresController(DatabaseContext context)
    {
        _context = context;
    }

    private readonly DatabaseContext _context;

    [HttpGet]
    public async Task<IActionResult> GetGenres()
    {
        var genres =await _context.Genres.OrderBy(g=>g.Name).ToListAsync();
        return Ok(genres);
    }

    [HttpPost]
    public async Task<ActionResult<Genre>> AddGenres([FromBody]CreateGenreDto dto)
    {
        Genre genre = new() {Name = dto.Name};
       await _context.Genres.AddAsync(genre);
       await _context.SaveChangesAsync();
       return Ok(genre);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Genre>> updateGenre(int id ,[FromBody] CreateGenreDto dto)
    {
        try
        {
            var response = await _context.Genres.Where(g => g.Id == id).FirstOrDefaultAsync();
            response.Name = dto.Name;
      await      _context.SaveChangesAsync();
      return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound($"id:{id} dosen't exist in our databas");
        }

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Genre>> DeleteGenre(int id)
    {
        try
        {
            var g =await _context.Genres.FindAsync(id);
            if (g == null)
            {
                return NotFound();
            }
            _context.Genres.Remove(g);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500,e.Message);
        }
    }


}