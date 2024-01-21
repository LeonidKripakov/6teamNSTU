using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public enum SpellType
    {
        Healing,
        Attack,
        Enhancement
    }
    public class SpellCard : ICard
    {
        public string? Name { get; set; }
        public int Cost { get; set; }
        public SpellType SpellType { get; set; }

        public void Use()
        {
            Console.WriteLine($"Spell card used: {Name}");
        }
    }
}
