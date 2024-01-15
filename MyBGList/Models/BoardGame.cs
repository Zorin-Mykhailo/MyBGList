using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyBGList.Models;

[Table($"{nameof(BoardGame)}s")]
public class BoardGame
{
    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }

    [Key, Required]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? AlternateNames { get; set; }

    [Required]
    public int PublisherId { get; set; }

    public Publisher? Publisher { get; set; }

    public ICollection<BoardGames_Categories>? BoardGames_Categories { get; set; }

    public ICollection<BoardGames_Domains>? BoardGames_Domains { get; set; }

    public ICollection<BoardGames_Mechanics>? BoardGames_Mechanics { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int Flags { get; set; }

    [Required]
    public int MinPlayers { get; set; }

    [Required]
    public int MaxPlayers { get; set; }

    [Required]
    public int PlayTime { get; set; }

    [Required]
    public int MinAge { get; set; }

    [Required]
    public int UsersRelated { get; set; }

    [Required, Precision(4, 2)]
    public decimal RatingAverage { get; set; }

    [Required]
    public int BGGRank { get; set; }

    [Required, Precision(4, 2)]
    public decimal ComplexityAverage { get; set; }

    [Required]
    public int OwnedUsers { get; set; }
}
