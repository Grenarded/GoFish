//Author: Ben Petlach
//File Name: Deck.cs
//Project Name: PetlachB_MP1
//Creation Date: March 8, 2023
//Modified Date: March 13, 2023
//Description: File for the deck object, which is used throughout the game to hand out cards to the players. Cards with their respective info are stored in the deck originally, and thus any manipulations (such as shuffling) occur here

using System;
using System.Collections.Generic;
using System.Linq;

namespace PetlachB_MP1
{
    public class Deck
    {
        public const int DECK_SIZE = 52;

        private List<Card> cards = new List<Card>();
        private Random rng = new Random();

        public Deck()
        {
        }

        public void ResetDeck()
        {
            cards.Clear();
            for (int i = 0; i < DECK_SIZE; i++)
            {
                cards.Add(new Card(i));
            }

            ShuffleDeck();
        }

        private void ShuffleDeck()
        {
            int shuffleTimes = 10000;

            for (int i = 0; i <= shuffleTimes; i++)
            {
                int firstCard = rng.Next(DECK_SIZE);
                int secondCard = rng.Next(DECK_SIZE);

                Card tempCard = cards[firstCard];
                cards[firstCard] = cards[secondCard];
                cards[secondCard] = tempCard;
            }
        }

        public Card DrawCard()
        {
            Card card = cards[0];
            cards.RemoveAt(0);

            return card;
        }

        public bool IsEmpty()
        {
            if (cards.Count == 0)
            {
                return true;
            }

            return false;
        }

        public int GetSize()
        {
            return cards.Count;
        }
    }
}
