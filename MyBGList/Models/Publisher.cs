using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBGList.Models;

[Table($"{nameof(Publisher)}s")]
public class Publisher
{
    [Required]
    public DateTime CreateDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }

    [Key, Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public ICollection<BoardGame>? BoardGames { get; set; }
}
