using System;
using System.ComponentModel.DataAnnotations;

namespace BlackJackMVC.Models
{
    public class CardInHand
    {
        [Key]
        public int CardInHandId { get; set; }
        public string Suit { get; set; }
        public int Value { get; set; }
        public string Face { get; set; }
        public string ImgURL { get; set; }
        public int PlayerId { get; set; }
        public bool Shown { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public CardInHand(string face, string suit, int value, string imgURL)
        {
            Face = face;
            Suit = suit;
            Value = value;
            ImgURL = imgURL;
        }
        public CardInHand(string face, string suit, int value, string imgURL, bool shown)
        {
            Face = face;
            Suit = suit;
            Value = value;
            ImgURL = imgURL;
            Shown = shown;
        }

        public override string ToString()
        {
            return $"{Face} of {Suit}";
        }
    }
}