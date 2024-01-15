﻿using System.ComponentModel.DataAnnotations;

namespace MyBGList.Models;

public class BoardGames_Mechanics
{
    [Required]
    public DateTime CreatedDate { get; set; }

    [Key, Required]
    public int BoardGameId { get; set; }
    
    public BoardGame? BoardGame { get; set; }

    [Key, Required]
    public int MechanicId { get; set; }
    
    public Mechanic? Mechanic { get; set; }
}