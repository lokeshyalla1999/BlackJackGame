using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlackJackMVC.Models
{
    public class Deck
    {
        [Key]
        public int DeckId { get; set; }
        public List<Card> CardsInDeck { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Deck()
        {
            Console.WriteLine("Creating the deck.");
            List<string> Suits = new List<string>()
            {
                "Spades","Diamonds","Hearts","Clubs"
            };


            List<int> Values = new List<int>()
            {
                1,2,3,4,5,6,7,8,9,10,10,10,10
            };

            List<string> Faces = new List<string>()
            {
                "Ace","2","3","4","5","6","7","8","9","10","Jack","Queen","King"
            };
            if(CardsInDeck == null)
            {
                CardsInDeck = new List<Card>();
                for (int i = 0; i < Suits.Count; i++)
                {
                    for (int j = 0; j < Values.Count; j++)
                    {
                        CardsInDeck.Add(new Card(Faces[j],Suits[i],Values[j]));
                    }
                }
            }
        }
        public Card Deal()
        {
            //Give the Deck a deal method that selects the "top-most" card, removes it from the list of cards, and returns the Card.
            if(this.CardsInDeck.Count > 0)
            {
                Card card_to_deal = this.CardsInDeck[0];
                this.CardsInDeck.RemoveAt(0);
                return card_to_deal;                
            }
            else 
            {
                Console.WriteLine("No cards left in deck");
                return null;
            }
        }

        public void Shuffle()
        {
            Console.WriteLine("Shuffling the Deck");
            Random rand = new Random();
            for(int i = 0; i < CardsInDeck.Count - 1; i++)
            {
                int k = rand.Next(CardsInDeck.Count - 1);
                var temp = CardsInDeck[i];
                CardsInDeck[i] = CardsInDeck[k];
                CardsInDeck[k] = temp;
            }
        }
        public void Reset()
        {
            //remove all the cards from deck
            this.CardsInDeck = null;
            //create a new deck
            CardsInDeck = new List<Card>();
            List<string> Suits = new List<string>()
            {
                "Spades","Diamonds","Hearts","Clubs"
            };

            List<int> Vals = new List<int>()
            {
                1,2,3,4,5,6,7,8,9,10,10,10,10
            };

            List<string> Faces = new List<string>()
            {
                "Ace","2","3","4","5","6","7","8","9","10","Jack","Queen","King"
            };
            for (int i = 0; i < Suits.Count; i++)
            {
                for (int j = 0; j < Vals.Count; j++)
                {
                    CardsInDeck.Add(new Card(Faces[j],Suits[i],Vals[j]));
                }
            }
        }
    }

}