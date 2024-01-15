using System.ComponentModel.DataAnnotations;

namespace MyBGList.Models;

public class BoardGames_Categories
{
    [Required]
    public DateTime CreatedDate { get; set; }

    [Key, Required]
    public int BoardGameId { get; set; }

    public BoardGame? BoardGame { get; set; }

    [Key, Required]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
