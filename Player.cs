using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
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
}
