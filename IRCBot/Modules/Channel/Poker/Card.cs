using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.scriptFiend.Modules.Channel.Poker
{
    class Card
    {
        public enum Suit
        {
            SPADES, CLUBS, DIAMONDS, HEARTS, WILD
        }

        public enum Value
        {
            WILD, LOACE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, HIACE
        }

        Suit suit;
        Value value;

        public Card(Suit suit, Value value)
        {
            this.suit = suit;
            this.value = value;
        }

        public Suit getSuit() {
            return suit;
        }

        public Value getValue() {
            return value;
        }

        public override string ToString()
        {
            if (suit == Suit.WILD || value == Value.WILD)
            {
                return "Wild Card";
            }
            return Enum.GetName(typeof(Value), value).ToLower() + " of " + Enum.GetName(typeof(Suit), suit).ToLower();
        }
    }
}
