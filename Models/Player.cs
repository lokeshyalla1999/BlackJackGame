using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlackJackMVC.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public List<CardInHand> CardsInHand { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}