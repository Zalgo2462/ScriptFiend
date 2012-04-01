using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.scriptFiend.Modules.Channel.Poker
{
    class Hand
    {
        List<Card> Cards { get; set; }

        public Hand()
        {
            Cards = new List<Card>();
        }

        public Card drawCard(Card card)
        {
            Cards.Add(card);
            return card;
        }

        public Card drawCard(Deck deck)
        {
            return drawCard(deck.draw());
        }

        public Card playCard(Card card)
        {
            Cards.Remove(card);
            return card;
        }

        public Card[] getCards()
        {
            return Cards.ToArray();
        }
    }
}
