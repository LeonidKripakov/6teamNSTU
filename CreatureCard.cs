using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
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
}
