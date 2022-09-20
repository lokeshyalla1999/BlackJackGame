using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BlackJackMVC.Models;
using Microsoft.AspNetCore.Http;

namespace BlackJackMVC.Controllers
{
    public class HomeController : Controller
    {
        #region Constructor
        private readonly MyContext _context;

        private readonly ICardValues _cardValues;
        

        public HomeController(MyContext context, ICardValues cardValues)
        {
            _context = context;
            _cardValues = cardValues;
            
        }
        #endregion

        #region GetIndex
        [HttpGet("/Start")]
        public IActionResult Index(int id = 0)
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            Deck deck = new Deck();
            deck.Shuffle();
            Player player1 = new Player();
            List<CardInHand> player1Cards = new List<CardInHand>();
            Card firstCard = deck.Deal();
            Card secondCard = deck.Deal();

            player1Cards.Add(new CardInHand(firstCard.Face, firstCard.Suit, firstCard.Value, firstCard.ImgURL));
            player1Cards.Add(new CardInHand(secondCard.Face, secondCard.Suit, secondCard.Value, secondCard.ImgURL));
            player1.CardsInHand = player1Cards;
            string playerScore = _cardValues.getScoreFromHand(player1.CardsInHand);
            Player dealer = new Player();
            List<CardInHand> dealerCards = new List<CardInHand>();

            firstCard = deck.Deal();
            secondCard = deck.Deal();

            dealerCards.Add(new CardInHand(firstCard.Face, firstCard.Suit, firstCard.Value, firstCard.ImgURL));
            dealerCards.Add(new CardInHand(secondCard.Face, secondCard.Suit, secondCard.Value, secondCard.ImgURL, false));
            dealer.CardsInHand = dealerCards;
            string dealerScore = _cardValues.getScoreFromHand(dealer.CardsInHand);

            _context.Add(deck);
            _context.Add(player1);
            _context.Add(dealer);
            _context.SaveChanges();

            int thisDeck = _context.Decks.OrderByDescending(d => d.DeckId).FirstOrDefault().DeckId;
            int thisPlayer = _context.Players.OrderByDescending(p => p.PlayerId).FirstOrDefault().PlayerId;


            HttpContext.Session.SetInt32("PlayerId", thisPlayer);
            HttpContext.Session.SetInt32("DeckId", thisDeck);

            ViewBag.Hand = player1.CardsInHand;
            ViewBag.Dealer = dealer.CardsInHand;



            ViewBag.Display = false;
            int startplayerScore = int.Parse(playerScore);
            ViewBag.playerScore = startplayerScore;
            int startDealerScore = int.Parse(dealerScore);
            ViewBag.dealerScore = startDealerScore;

            ViewBag.winner = _cardValues.WinnerCheck(startplayerScore, startDealerScore);

            if (startplayerScore >= 21 && startplayerScore != startDealerScore)
            {
                ViewBag.Display = true;
                TempData["Winner"] = ViewBag.winner;
                TempData["playerScore"] = startplayerScore;
                TempData["dealerScore"] = startDealerScore;
                TempData["HandCard"] = player1.CardsInHand[0].Suit + "-" + player1.CardsInHand[0].Face;
                TempData["HandCard1"] = player1.CardsInHand[1].Suit + "-" + player1.CardsInHand[1].Face;
                TempData["Dealer"] = dealer.CardsInHand[0].Suit + "-" + dealer.CardsInHand[0].Face;
                TempData["Dealer1"] = dealer.CardsInHand[1].Suit + "-" + dealer.CardsInHand[1].Face;
                return RedirectToAction("Result");
            }

            if (id == 1)
            {
                ViewBag.Display = false;
                ViewBag.playerScore = int.Parse(playerScore);
                ViewBag.DealerScore = int.Parse(dealerScore);
                ViewBag.HandCard = player1.CardsInHand[0].Suit + "-" + player1.CardsInHand[0].Face;
                ViewBag.HandCard1 = player1.CardsInHand[1].Suit + "-" + player1.CardsInHand[1].Face;
                ViewBag.Dealercard = dealer.CardsInHand[0].Suit + "-" + dealer.CardsInHand[0].Face;
                ViewBag.Dealercard1 = dealer.CardsInHand[1].Suit + "-" + dealer.CardsInHand[1].Face;
                return View("NormalGameView");
            }
            return View();
        }

        #endregion


        #region GetHit
        [HttpGet("/hit")]
        public ActionResult Hit(int id = 0)
        {
            int? playerId = HttpContext.Session.GetInt32("PlayerId");
            int? DealerId = playerId - 1;
            int? deckId = HttpContext.Session.GetInt32("DeckId");

            #region LINQ Data
            Player player1 = _context.Players
                .Include(p => p.CardsInHand)
                .FirstOrDefault(p => p.PlayerId == playerId - 1);
            if (player1 == null)
                return RedirectToAction("Index");


            Player dealer = _context.Players
                .Include(d => d.CardsInHand)
                .FirstOrDefault(p => p.PlayerId == playerId);
            var deck = _context.Decks
                .Include(d => d.CardsInDeck)
                .FirstOrDefault(d => d.DeckId == deckId);

            #endregion

            for (int i = 0; i < 52; i++)
            {
                deck.Deal();
            }
            Card hitCard = deck.Deal();

            player1.CardsInHand.Add(new CardInHand(hitCard.Face, hitCard.Suit, hitCard.Value, hitCard.ImgURL));

            string playerScore = _cardValues.getScoreFromHand(player1.CardsInHand);
            string dealerScore = _cardValues.getScoreFromHand(dealer.CardsInHand);


            ViewBag.Hand = player1.CardsInHand;
            ViewBag.Dealer = dealer.CardsInHand;
            //_context.SaveChanges();
            #region CardDataSend
            TempData["HandCard"] = player1.CardsInHand[0].Suit + "-" + player1.CardsInHand[0].Face;
            TempData["HandCard1"] = player1.CardsInHand[1].Suit + "-" + player1.CardsInHand[1].Face;
            if (player1.CardsInHand.Count > 2)
            {
                TempData["HandCard2"] = player1.CardsInHand[2].Suit + "-" + player1.CardsInHand[2].Face;
            }
            if (player1.CardsInHand.Count > 3)
            {
                TempData["HandCard3"] = player1.CardsInHand[3].Suit + "-" + player1.CardsInHand[3].Face;
            }
            TempData["Dealer"] = dealer.CardsInHand[0].Suit + "-" + dealer.CardsInHand[0].Face;
            TempData["Dealer1"] = dealer.CardsInHand[1].Suit + "-" + dealer.CardsInHand[1].Face;
            if (dealer.CardsInHand.Count > 2)
            {
                TempData["Dealer2"] = dealer.CardsInHand[2].Suit + "-" + dealer.CardsInHand[2].Face;
            }
            if (dealer.CardsInHand.Count > 3)
            {
                TempData["Dealer3"] = dealer.CardsInHand[3].Suit + "-" + dealer.CardsInHand[3].Face;
            }
            #endregion

            ViewBag.Display = false;

            int hitplayerscore = int.Parse(playerScore);
            int dealerplayerscore = int.Parse(dealerScore);
            TempData["playerScore"] = hitplayerscore;
            TempData["dealerScore"] = dealerplayerscore;
            _context.Update(deck);
            _context.SaveChanges();


            ViewBag.Winner = _cardValues.WinnerCheck(hitplayerscore, dealerplayerscore);
            TempData["winner"] = ViewBag.Winner;


            if (id == 1)
            {
                ViewBag.playerScore = int.Parse(playerScore);
                ViewBag.DealerScore = int.Parse(dealerScore);
                ViewBag.HandCard = player1.CardsInHand[0].Suit + "-" + player1.CardsInHand[0].Face;
                ViewBag.HandCard1 = player1.CardsInHand[1].Suit + "-" + player1.CardsInHand[1].Face;
                ViewBag.Dealercard = dealer.CardsInHand[0].Suit + "-" + dealer.CardsInHand[0].Face;
                ViewBag.Dealercard1 = dealer.CardsInHand[1].Suit + "-" + dealer.CardsInHand[1].Face;
                if (player1.CardsInHand.Count > 2)
                {
                    ViewBag.HandCard2 = player1.CardsInHand[2].Suit + "-" + player1.CardsInHand[2].Face;
                }
                if (player1.CardsInHand.Count > 3)
                {
                    ViewBag.HandCard3 = player1.CardsInHand[3].Suit + "-" + player1.CardsInHand[3].Face;
                }
                if (dealer.CardsInHand.Count > 2)
                {
                    ViewBag.Dealercard2 = dealer.CardsInHand[2].Suit + "-" + dealer.CardsInHand[2].Face;
                }
                if (dealer.CardsInHand.Count > 3)
                {
                    ViewBag.Dealercard3 = dealer.CardsInHand[3].Suit + "-" + dealer.CardsInHand[3].Face;
                }
                if (dealerplayerscore <= 17)
                {
                    return RedirectToAction("Stay", new { id = 1 });
                }
                ViewBag.Display = true;
                return View("NormalGameView");
            }


            if (hitplayerscore > dealerplayerscore && hitplayerscore < 21)
            {
                ViewBag.hitplayerscore = hitplayerscore;
                return View("Index");
            }
            return RedirectToAction("Result");
        }
        #endregion


        #region GetStayRegion
        [HttpGet("/stay")]
        public ActionResult Stay(int id = 0)
        {
            int? playerId = HttpContext.Session.GetInt32("PlayerId");
            int? DealerId = playerId - 1;
            int? deckId = HttpContext.Session.GetInt32("DeckId");

            Player player1 = _context.Players
                .Include(p => p.CardsInHand)
                .FirstOrDefault(p => p.PlayerId == playerId - 1);
            if (player1 == null)
                return RedirectToAction("Index");

            Player dealer = _context.Players
                .Include(d => d.CardsInHand)
                .FirstOrDefault(p => p.PlayerId == playerId);

            dealer.CardsInHand[0].Shown = true;
            var deck = _context.Decks
                .Include(d => d.CardsInDeck)
                .FirstOrDefault(d => d.DeckId == deckId);

            foreach (var c in deck.CardsInDeck)
                Console.WriteLine(c);
            for (int i = 0; i < 52; i++)
            {
                deck.Deal();
            }

            string playerScore = _cardValues.getScoreFromHand(player1.CardsInHand);
            string dealerScore = _cardValues.getScoreFromHand(dealer.CardsInHand);

            Console.WriteLine(dealerScore);
            List<int> dealerScoreList = _cardValues.getScoreFromString(dealerScore);
            foreach (var _ in dealerScoreList)
                Console.WriteLine(_);

            while (dealerScoreList[0] < 17)
            {
                //deal the card to dealer
                Card hitCard = deck.Deal();
                dealer.CardsInHand.Add(new CardInHand(hitCard.Face, hitCard.Suit, hitCard.Value, hitCard.ImgURL));
                dealerScore = _cardValues.getScoreFromHand(dealer.CardsInHand);
                dealerScoreList = _cardValues.getScoreFromString(dealerScore);
            }
            ViewBag.Hand = player1.CardsInHand;
            ViewBag.Dealer = dealer.CardsInHand;
            _context.Update(deck);
            _context.SaveChanges();

            #region CardDataSend
            TempData["HandCard"] = player1.CardsInHand[0].Suit + "-" + player1.CardsInHand[0].Face;
            TempData["HandCard1"] = player1.CardsInHand[1].Suit + "-" + player1.CardsInHand[1].Face;

            if (player1.CardsInHand.Count > 2)
            {
                TempData["HandCard2"] = player1.CardsInHand[2].Suit + "-" + player1.CardsInHand[2].Face;
            }
            if (player1.CardsInHand.Count > 3)
            {
                TempData["HandCard3"] = player1.CardsInHand[3].Suit + "-" + player1.CardsInHand[3].Face;
            }

            TempData["Dealer"] = dealer.CardsInHand[0].Suit + "-" + dealer.CardsInHand[0].Face;
            TempData["Dealer1"] = dealer.CardsInHand[1].Suit + "-" + dealer.CardsInHand[1].Face;

            if (dealer.CardsInHand.Count >2 )
            {
                TempData["Dealer2"] = dealer.CardsInHand[2].Suit + "-" + dealer.CardsInHand[2].Face;
            }
            if (dealer.CardsInHand.Count > 3)
            {
                TempData["Dealer3"] = dealer.CardsInHand[3].Suit + "-" + dealer.CardsInHand[3].Face;
            }
            #endregion


            ViewBag.Display = true;

            int a = int.Parse(playerScore);
            int b = int.Parse(dealerScore);
            TempData["playerScore"] = a;
            TempData["dealerScore"] = b;

            ViewBag.winner = _cardValues.WinnerCheck(a, b);
            TempData["winner"] = ViewBag.Winner;
            if (id == 1)
            {

                ViewBag.playerScore = int.Parse(playerScore);
                ViewBag.DealerScore = int.Parse(dealerScore);
                ViewBag.HandCard = player1.CardsInHand[0].Suit + "-" + player1.CardsInHand[0].Face;
                ViewBag.HandCard1 = player1.CardsInHand[1].Suit + "-" + player1.CardsInHand[1].Face;
                ViewBag.Dealercard = dealer.CardsInHand[0].Suit + "-" + dealer.CardsInHand[0].Face;
                ViewBag.Dealercard1 = dealer.CardsInHand[1].Suit + "-" + dealer.CardsInHand[1].Face;
                if (player1.CardsInHand.Count > 2)
                {
                    ViewBag.HandCard2 = player1.CardsInHand[2].Suit + "-" + player1.CardsInHand[2].Face;
                }
                if (player1.CardsInHand.Count > 3)
                {
                    ViewBag.HandCard3 = player1.CardsInHand[3].Suit + "-" + player1.CardsInHand[3].Face;
                }
                if (dealer.CardsInHand.Count > 2)
                {
                    ViewBag.Dealercard2 = dealer.CardsInHand[2].Suit + "-" + dealer.CardsInHand[2].Face;
                }
                if (dealer.CardsInHand.Count > 3)
                {
                    ViewBag.Dealercard3 = dealer.CardsInHand[3].Suit + "-" + dealer.CardsInHand[3].Face;
                }
                ViewBag.Display = true;
                return View("NormalGameView");
            }



            Console.WriteLine("Staying");
            return RedirectToAction("Result", "Home");
        }
        

        #endregion


        #region ResultAction
        public IActionResult Result()
        {
            if (TempData["HandCard"] == null)
            {
                return RedirectToAction("Index", "StartGame");
            }


            ViewBag.CardInHand = TempData["HandCard"];
            ViewBag.CardInHand1 = TempData["HandCard1"];
            ViewBag.CardInHand2 = TempData["HandCard2"];
            ViewBag.CardInHand3 = TempData["HandCard3"];
            ViewBag.CardInDeal = TempData["Dealer"];
            ViewBag.CardInDeal1 = TempData["Dealer1"];
            ViewBag.CardInDeal2 = TempData["Dealer2"];
            ViewBag.CardInDeal3 = TempData["Dealer3"];
            ViewBag.Pscore = TempData["playerScore"];
            ViewBag.dscore = TempData["dealerScore"];
            ViewBag.winner = TempData["winner"];

         
            return View();

        }
        #endregion

    }
}
    


