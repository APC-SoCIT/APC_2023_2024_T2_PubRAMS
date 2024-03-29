using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace webapp.Models;

[Index(nameof(Name), IsUnique = true)]
public class Group
{
    public int Id { get; set; }

    [MaxLength(64)]
    public required string Name { get; set; }

    [MinLength(36), MaxLength(36)]
    public string? LeaderId { get; set; }
    public virtual Student? Leader { get; set; }
}
