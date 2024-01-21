using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class GameManager
{
    public static List<ICard> LoadDeckFromFile(string filePath)
    {
        string json = File.ReadAllText(filePath);
        var settings = new JsonSerializerSettings();
        var cardConverter = new ICardConverter();
        settings.Converters.Add(cardConverter);
        return JsonConvert.DeserializeObject<List<ICard>>(json, settings) ?? new List<ICard>();
    }

    public static void DisplayGameState(Player player1, Player player2)
    {
        Console.WriteLine("Game started!");
        Console.WriteLine($"Player 1: {player1.Name}");
        Console.WriteLine($"Player 2: {player2.Name}");

        Console.WriteLine($"Player 1 cards in hand: {player1.Hand?.Count ?? 0}");
        Console.WriteLine($"Player 2 cards in hand: {player2.Hand?.Count ?? 0}");

        Console.WriteLine($"First move by: {(player1.IsCurrentPlayer ? player1.Name : player2.Name)}");
    }

    public static void DisplayPlayerHand(Player player)
    {
        Console.WriteLine($"Player {player.Name}'s hand:");
        for (int i = 0; player.Hand != null && i < player.Hand.Count; i++)
        {
            Console.WriteLine($"{i}. {player.Hand[i].Name} (Cost: {player.Hand[i].Cost})");
        }
    }
}
}
