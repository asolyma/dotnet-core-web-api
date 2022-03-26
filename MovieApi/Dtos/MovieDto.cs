using MovieApi.Models;

namespace MovieApi.Dtos;

public class MovieDto
{
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rate { get; set; }
    public string StoryLine { get; set; }
    public IFormFile Poster { get; set; }
    public int GenreId { get; set; }
}