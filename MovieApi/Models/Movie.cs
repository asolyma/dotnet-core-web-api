namespace MovieApi.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rate { get; set; }
    public string StoryLine { get; set; }
    public byte[] Poster { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
}