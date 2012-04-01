using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace org.scriptFiend.Modules.Channel.Poker
{
    class Deck
    {
        Queue<Card> Cards { get; set; }
        bool AcesHigh { get; set; }
        bool Wilds { get; set; }
        int Decks { get; set; }

        public Deck() : this(1)
        {           
        }

        public Deck(int decks) 
        {            
            AcesHigh = true;
            Wilds = false;
            Decks = decks;
            load();
            shuffle();
        }

        public void load()
        {
            Cards = new Queue<Card>();
            for (int iii = 0; iii < Decks; iii++)
            {
                foreach (int suit in Enum.GetValues(typeof(Card.Suit)))
                {
                    if (suit != (int)Card.Suit.WILD)
                    {
                        foreach (int value in Enum.GetValues(typeof(Card.Value)))
                        {
                            if (value == (int)Card.Value.LOACE && AcesHigh
                                || value == (int)Card.Value.HIACE && !AcesHigh || value == (int)Card.Value.WILD)
                            {
                                continue;
                            }
                            else
                            {
                                Cards.Enqueue(new Card((Card.Suit)suit, (Card.Value)value));
                            }
                        }
                    }
                }
                if (Wilds)
                {
                    Cards.Enqueue(new Card(Card.Suit.WILD, Card.Value.WILD));
                    Cards.Enqueue(new Card(Card.Suit.WILD, Card.Value.WILD));
                }
            }
        }
        
        public void shuffle()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            Card[] temp = Cards.ToArray();
            int n = temp.Length;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;                
                Card value = temp[k];
                temp[k] = temp[n];
                temp[n] = value;
            }
            Cards = new Queue<Card>(temp.ToList<Card>());
        }

        public Card draw()
        {
            return Cards.Dequeue();
        }

    }
}
