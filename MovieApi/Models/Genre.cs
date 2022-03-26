using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models;

public class Genre
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    
}