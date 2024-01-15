using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyBGList.Models;

[Table($"{nameof(Domain)}s")]
public class Domain
{
    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }

    [Key, Required]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public ICollection<BoardGames_Domains>? BoardGames_Domains { get; set; }

    [MaxLength(200)]
    public string? Notes { get; set; }

    [Required]
    public int Flags { get; set; }
}
