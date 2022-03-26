using MovieApi.Dtos;
using MovieApi.Models;

namespace MovieApi.Services;

public interface IGenresService
{
    public  Task<IEnumerable<Genre>> GetAll();
    public  Task<Genre> GetOne(int id);
    public  Task<Genre> addOne(CreateGenreDto dto);
    public Task<Genre> DeleteOne(int id);
    public Task<Genre> UpdateOne(int id, CreateGenreDto dto);

    public Task<bool> IsvalidGenre(int id);


}