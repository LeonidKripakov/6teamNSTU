using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class WitchCard : ICard
{
    public string? Name { get; set; } // Use nullable reference type syntax
    public int Cost { get; set; }

    public List<SpellType> SpellTypes { get; set; }

    public WitchCard()
    {
        SpellTypes = new List<SpellType>();
    }

    public void Use()
    {
        Console.WriteLine($"Witch card used: {Name ?? "Unnamed"}");
        Console.WriteLine("Created spells:");
        foreach (var spellType in SpellTypes)
        {
            Console.WriteLine($"- {spellType}");
        }
    }
}
}

