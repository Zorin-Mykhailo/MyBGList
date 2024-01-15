﻿using System.ComponentModel.DataAnnotations;

namespace MyBGList.Models;

public class BoardGames_Domains
{
    [Required]
    public DateTime CreatedDate { get; set; }

    [Key, Required]
    public int BoardGameId { get; set; }
    
    public BoardGame? BoardGame { get; set; }

    [Key, Required]
    public int DomainId { get; set; }
    
    public Domain? Domain { get; set; }
}