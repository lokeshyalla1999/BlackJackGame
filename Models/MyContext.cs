using Microsoft.EntityFrameworkCore;

namespace BlackJackMVC.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {
          
           
        }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> CardsInDeck { get; set; }
        public DbSet<Player> Players { get; set; }

    

    }
}