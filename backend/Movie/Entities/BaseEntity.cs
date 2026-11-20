using System.ComponentModel.DataAnnotations;

namespace Movie.Entities;

public abstract class BaseEntity
{
    [Key]
    public long Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}
