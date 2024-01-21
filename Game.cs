using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Game
    {
        #pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        private Random random = new Random();


        public Game()
        {
            Player1 = new Player();
            Player2 = new Player();
        }
        public void StartGame()
        {
            

            Player1.Deck = GameManager.LoadDeckFromFile("deck.json");
            Player2.Deck = GameManager.LoadDeckFromFile("deck.json");

            ShuffleDeck(Player1.Deck);
            ShuffleDeck(Player2.Deck);

            Player1.DrawCard(3);
            Player2.DrawCard(3);

            Player1.Name = "Player 1";
            Player2.Name = "Player 2";

            DetermineFirstPlayer();

            string json = File.ReadAllText("deck.json");

            GameManager.DisplayGameState(Player1, Player2);

            var settings = new JsonSerializerSettings();
            var cardConverter = new ICardConverter();
            settings.Converters.Add(cardConverter);

 
            List<ICard> cards = JsonConvert.DeserializeObject<List<ICard>>(json, settings);

            PlayTurn();
        }

        private void InitializePlayers()
        {
            Player1 = new Player();
            Player2 = new Player();
        }

        private void ShuffleDeck(List<ICard> deck)
        {
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                ICard value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        private void DetermineFirstPlayer()
        {
            if (random.Next(2) == 0)
            {
                Player1.IsCurrentPlayer = true;
            }
            else
            {
                Player2.IsCurrentPlayer = true;
            }
        }

        private void DisplayGameState()
        {
            Console.WriteLine("Game started!");
            Console.WriteLine($"Player 1: {Player1.Name}");
            Console.WriteLine($"Player 2: {Player2.Name}");

            Console.WriteLine($"Player 1 cards in hand: {Player1.Hand.Count}");
            Console.WriteLine($"Player 2 cards in hand: {Player2.Hand.Count}");

            Console.WriteLine($"First move by: {(Player1.IsCurrentPlayer ? Player1.Name : Player2.Name)}");
        }

        private void PlayTurn()
        {
            while (!IsGameOver())
            {
                Player currentPlayer = Player1.IsCurrentPlayer ? Player1 : Player2;

                currentPlayer.DrawCard(1);

                GameManager.DisplayPlayerHand(currentPlayer);

                Console.Write($"{currentPlayer.Name}, choose a card to play (0-{currentPlayer.Hand.Count - 1}): ");
                int selectedCardIndex;
                while (!int.TryParse(Console.ReadLine(), out selectedCardIndex) || selectedCardIndex < 0 || selectedCardIndex >= currentPlayer.Hand.Count)
                {
                    Console.Write("Invalid input. Please choose again: ");
                }

                currentPlayer.PlayCard(selectedCardIndex);

                Player1.IsCurrentPlayer = !Player1.IsCurrentPlayer;
                Player2.IsCurrentPlayer = !Player2.IsCurrentPlayer;
            }

            Console.WriteLine("Game over!");
        }

        private bool IsGameOver()
        {
            if (Player1.Hand.Count <= 0 || Player2.Hand.Count <= 0)
                return true;

            return false;
        }

        private void DisplayPlayerHand(Player player)
        {
            Console.WriteLine($"Player {player.Name}'s hand:");
            for (int i = 0; i < player.Hand.Count; i++)
            {
                Console.WriteLine($"{i}. {player.Hand[i].Name} (Cost: {player.Hand[i].Cost})");
            }
        }
    }
}
