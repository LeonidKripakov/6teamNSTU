using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Card
{
    public string Name { get; set; }
    public int Cost { get; set; }

    public virtual void Use()
    {
        // Логика использования карты
        Console.WriteLine($"Использована карта: {Name}");
    }
}

public class CreatureCard : Card
{
    public int Attack { get; set; }
    public int Health { get; set; }

    public override void Use()
    {
        // Логика использования карты существа
        Console.WriteLine($"Использована карта существа: {Name}");
    }
}

public class SpellCard : Card
{
    public SpellType SpellType { get; set; }

    public override void Use()
    {
        // Логика использования карты заклинания
        Console.WriteLine($"Использована карта заклинания: {Name}");
    }
}

public enum SpellType
{
    Healing,
    Attack,
    Enhancement
}

public class Player
{
    public string Name { get; set; }
    public List<Card> Deck { get; set; }
    public List<Card> Hand { get; set; }
    public List<CreatureCard> Battlefield { get; set; }
    public bool IsCurrentPlayer { get; set; }

    public Player()
    {
        Deck = new List<Card>();
        Hand = new List<Card>();
        Battlefield = new List<CreatureCard>();
    }

    public void DrawCard(int CardCount)
    {
        // Предположим, что у вас есть колода, и вам нужно взять карту из колоды
        if (Deck.Count > 0)
        {
            for (int i = 0; i < CardCount; i++)
            {
                Card drawnCard = Deck[0];
                Deck.RemoveAt(0);
                Hand.Add(drawnCard);
            }

        }
    }

    public void PlayCard(int cardIndex)
    {
        if (cardIndex >= 0 && cardIndex < Hand.Count)
        {
            Card playedCard = Hand[cardIndex];
            Hand.RemoveAt(cardIndex);
            playedCard.Use();
        }
    }
}

public class Game
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    private Random random = new Random();

    private List<Card> LoadDeckFromFile
    {
        get
        {
            string json = File.ReadAllText("deck.json");
            return JsonConvert.DeserializeObject<List<Card>>(json);
        }
    }

    public void StartGame()
    {
        // Инициализация игроков и их колод
        InitializePlayers();

        // Загрузка колоды из файла
        Player1.Deck = LoadDeckFromFile;
        Player2.Deck = LoadDeckFromFile;

        // Перемешивание колоды каждого игрока
        ShuffleDeck(Player1.Deck);
        ShuffleDeck(Player2.Deck);

        // Раздача карт каждому игроку
        Player1.DrawCard(3);
        Player2.DrawCard(3);

        Player1.Name = "Игрок 1";
        Player2.Name = "Игрок 2";

        // Определение первого хода
        DetermineFirstPlayer();

        // Вывод информации о начале игры
        DisplayGameState();

        // Начало игры
        PlayTurn();


    }
    private void InitializePlayers()
    {
        Player1 = new Player();
        Player2 = new Player();
        // Инициализация колод и других свойств игроков
    }

    private void ShuffleDeck(List<Card> deck)
    {
        // Перемешивания колоды
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Card value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }

    private void DetermineFirstPlayer()
    {
        // Кому принадлежит первый ход
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
        Console.WriteLine("Игра началась!");
        Console.WriteLine($"Игрок 1: {Player1.Name}");
        Console.WriteLine($"Игрок 2: {Player2.Name}");

        Console.WriteLine($"Игрок 1 карты на руке: {Player1.Hand.Count}");
        Console.WriteLine($"Игрок 2 карты на руке: {Player2.Hand.Count}");

        Console.WriteLine($"Первым ходит: {(Player1.IsCurrentPlayer ? Player1.Name : Player2.Name)}");
    }

    private void PlayTurn()
    {
        while (!IsGameOver())
        {
            Player currentPlayer = Player1.IsCurrentPlayer ? Player1 : Player2;

            // Добор карты из колоды в начале хода
            currentPlayer.DrawCard(1);

            DisplayPlayerHand(currentPlayer);

            Console.Write($"{currentPlayer.Name}, выберите карту для игры (0-{currentPlayer.Hand.Count - 1}): ");
            int selectedCardIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedCardIndex) || selectedCardIndex < 0 || selectedCardIndex >= currentPlayer.Hand.Count)
            {
                Console.Write("Некорректный ввод. Повторите выбор: ");
            }

            // Используем карту
            currentPlayer.PlayCard(selectedCardIndex);

            // Переключение хода к другому игроку
            Player1.IsCurrentPlayer = !Player1.IsCurrentPlayer;
            Player2.IsCurrentPlayer = !Player2.IsCurrentPlayer;
        }

        Console.WriteLine("Игра завершена!");
    }


    private bool IsGameOver()
    {
        // Услвие окончания игры
        return false;
    }

    private void DisplayPlayerHand(Player player)
    {
        Console.WriteLine($"Рука игрока {player.Name}:");
        for (int i = 0; i < player.Hand.Count; i++)
        {
            Console.WriteLine($"{i}. {player.Hand[i].Name} (Стоимость: {player.Hand[i].Cost})");
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