using Microsoft.EntityFrameworkCore;
using MovieApi.Dtos;
using MovieApi.Models;

namespace MovieApi.Services;

public class MoviesService : IMoviesService
{
    private readonly DatabaseContext _context;

    public MoviesService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovieDetailsDto>> GetAll()
    {
        var movies = await _context.Movies.Include(m => m.Genre).Select(g => new MovieDetailsDto()
                {Title = g.Title, GenreName = g.Genre.Name, Rate = g.Rate, Year = g.Year, StoryLine = g.StoryLine})
            .ToListAsync();
        return movies;
    }

    public async Task<Movie?> GetOne(int id)
    {
        var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
        if (movie != null)
        {
            var movieDto = new MovieDetailsDto()
            {
                Rate = movie.Rate,
                Title = movie.Title,
                Year = movie.Year,
                GenreName = movie.Genre.Name,
                StoryLine = movie.StoryLine
            };
        }

        return movie;

    }

    public async Task<Movie> addOne(Movie movie)
    {

        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<Movie?> DeleteOne(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return null;
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return movie;
    }


    public async Task<IEnumerable<Movie>> GetMoviesByGenre(int id)

    {
        var movie = await _context.Movies.Include(m => m.Genre).Where(m => m.GenreId == id).ToListAsync();
        return movie;
    }

    public async Task<Movie> UpdateOne(Movie movie)
    {
        _context.Update(movie);
        await _context.SaveChangesAsync();
        return movie;

    }
}