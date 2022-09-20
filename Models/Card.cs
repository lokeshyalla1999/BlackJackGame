using System;
using System.ComponentModel.DataAnnotations;

namespace BlackJackMVC.Models
{
    public class Card 
    {
        [Key]
        public int CardId { get; set; }
        public string Suit { get; set; }
        public int Value { get; set; }
        public string Face { get; set; }
        public string ImgURL { get; set; }
        public int DeckId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Card(string s, string suit, int value)
        {
            Face = s;
            Suit = suit;
            Value = value;
            //get the image url code (KH = king of hearts)
            string faceLetter;
            if(Value != 10)
            {
                faceLetter = s[0].ToString();
            }
            else
            {
                faceLetter = "0";
            }
            string suitLetter = suit[0].ToString();
            ImgURL = "https://deckofcardsapi.com/static/img/" + faceLetter + "" + suitLetter + ".png";
        }
        public Card(){}        

        public override string ToString()
        {
            return $"{Face} of {Suit}";
        }
    }
}