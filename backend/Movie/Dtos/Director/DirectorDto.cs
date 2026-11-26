namespace Movie.Dtos.Director;

public class DirectorDto
{
    public long Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string? ProfilePath { get; set; }
}