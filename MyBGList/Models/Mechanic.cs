using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBGList.Models;

[Table($"{nameof(Mechanic)}s")]
public class Mechanic
{
    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }

    [Key, Required]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public ICollection<BoardGames_Mechanics>? BoardGames_Mechanics { get; set; }

    [MaxLength(200)]
    public string? Notes { get; set; }

    [Required]
    public int Flags { get; set; }
}