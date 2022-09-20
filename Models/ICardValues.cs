using System.Collections.Generic;

namespace BlackJackMVC.Models
{
    public interface ICardValues
    {
        string getScoreFromHand(List<CardInHand> Hand);

        List<int> getScoreFromString(string score);

        string WinnerCheck(int playerscore, int dealerscore);

    }
}
