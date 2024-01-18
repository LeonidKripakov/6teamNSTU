using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

// Interface for cards
public interface ICard
{
    string Name { get; set; }
    int Cost { get; set; }

    void Use();
}

// Witch class that implements ICard and can create both good and bad spells
public class WitchCard : ICard
{
    public string Name { get; set; }
    public int Cost { get; set; }

    public List<SpellType> SpellTypes { get; set; }

    public WitchCard()
    {
        SpellTypes = new List<SpellType>();
    }

    public void Use()
    {
        Console.WriteLine($"Witch card used: {Name}");
        Console.WriteLine("Created spells:");
        foreach (var spellType in SpellTypes)
        {
            Console.WriteLine($"- {spellType}");
        }
    }
}

// Other card classes
public class CreatureCard : ICard
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public int Attack { get; set; }
    public int Health { get; set; }

    public void Use()
    {
        Console.WriteLine($"Creature card used: {Name}");
    }
}

public class SpellCard : ICard
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public SpellType SpellType { get; set; }

    public void Use()
    {
        Console.WriteLine($"Spell card used: {Name}");
    }
}

public enum SpellType
{
    Healing,
    Attack,
    Enhancement
}

// Updated Player class to handle ICard
public class Player
{
    public string Name { get; set; }
    public List<ICard> Deck { get; set; }
    public List<ICard> Hand { get; set; }
    public List<CreatureCard> Battlefield { get; set; }
    public bool IsCurrentPlayer { get; set; }

    public Player()
    {
        Deck = new List<ICard>();
        Hand = new List<ICard>();
        Battlefield = new List<CreatureCard>();
    }

    public void DrawCard(int CardCount)
    {
        if (Deck.Count > 0)
        {
            for (int i = 0; i < CardCount; i++)
            {
                ICard drawnCard = Deck[0];
                Deck.RemoveAt(0);
                Hand.Add(drawnCard);
            }
        }
    }

    public void PlayCard(int cardIndex)
    {
        if (cardIndex >= 0 && cardIndex < Hand.Count)
        {
            ICard playedCard = Hand[cardIndex];
            Hand.RemoveAt(cardIndex);
            playedCard.Use();
        }
    }
}

// Updated Game class to use ICard
public class Game
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    private Random random = new Random();

    private List<ICard> LoadDeckFromFile
    {
        get
        {
            string json = File.ReadAllText("deck.json");
            return JsonConvert.DeserializeObject<List<ICard>>(json);
        }
    }

    public void StartGame()
    {
        InitializePlayers();

        Player1.Deck = LoadDeckFromFile;
        Player2.Deck = LoadDeckFromFile;

        ShuffleDeck(Player1.Deck);
        ShuffleDeck(Player2.Deck);

        Player1.DrawCard(3);
        Player2.DrawCard(3);

        Player1.Name = "Player 1";
        Player2.Name = "Player 2";

        DetermineFirstPlayer();

        DisplayGameState();

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

            DisplayPlayerHand(currentPlayer);

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
        // Game over condition
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

class Program
{
    static void Main()
    {
        Game game = new Game();
        game.StartGame();
    }
}
