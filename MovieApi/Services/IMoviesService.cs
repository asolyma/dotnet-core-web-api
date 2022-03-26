using MovieApi.Dtos;
using MovieApi.Models;

namespace MovieApi.Services;

public interface IMoviesService
{
    public Task<IEnumerable<MovieDetailsDto>> GetAll();
    public Task<Movie?>  GetOne(int id);
    public Task<Movie> addOne(Movie movie);

    public Task<Movie?> DeleteOne(int id);

    public Task<IEnumerable<Movie>> GetMoviesByGenre(int id);
    public  Task<Movie> UpdateOne(Movie movie);

}