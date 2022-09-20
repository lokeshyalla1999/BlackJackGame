using System;
using System.Collections.Generic;

namespace BlackJackMVC.Models
{
    public class CardValues : ICardValues
    {
        public string getScoreFromHand(List<CardInHand> Hand)
        {
            int score = 0;
            foreach (CardInHand handCard in Hand)
            {
                score += handCard.Value;
            }
            return score.ToString();
        }

        public List<int> getScoreFromString(string score)
        {
            string[] scoreString = score.Split(',');
            List<int> returnInt = new List<int>();
            foreach (var s in scoreString)
                returnInt.Add(Int32.Parse(s));

            return returnInt;
        }

        public string WinnerCheck(int playerscore, int dealerscore)
        {
            if (playerscore > dealerscore && playerscore <= 21)
            {

                return ("Player Win");
            }
            else if (dealerscore > playerscore && dealerscore <= 21)
            {
                return ("Dealer Win - you Bust !");
            }
            else if (playerscore < dealerscore && playerscore <= 21)
            {
                return ("Player Win");
            }
            else if (dealerscore < playerscore && dealerscore <= 21)
            {
                return ("Dealer Win -- you Bust !");
            }
            else if (playerscore == dealerscore)
                return ("Tie -- Draw match");

            return ("Play Again");

        }
    }
}
