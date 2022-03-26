using Microsoft.EntityFrameworkCore;
using MovieApi.Dtos;
using MovieApi.Models;

namespace MovieApi.Services;

public class GenresService:IGenresService
{
    private readonly DatabaseContext _context;

    public GenresService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAll()
    {
        var genres =await _context.Genres.OrderBy(g=>g.Name).ToListAsync();
        return genres;

    }

    public Task<Genre> GetOne(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Genre> addOne(CreateGenreDto dto)
    {
    
        Genre genre = new() {Name = dto.Name};
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre> DeleteOne(int id)
    {
        try
        {
            var g =await _context.Genres.FindAsync(id);
            if (g == null)
            {
                return null;
            }
            _context.Genres.Remove(g);
            await _context.SaveChangesAsync();

            return g;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }    }

    public async Task<Genre> UpdateOne (int id ,CreateGenreDto dto)
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
            return null;
        }
    }

    public async Task<bool> IsvalidGenre(int id)
    {
        var valid= await _context.Genres.SingleOrDefaultAsync(g=>g.Id==id);
        return valid != null;
    }
}