using System;
using System.Collections.Generic;

namespace PetlachB_MP1
{
    public class Hand
    {
        private List<Card> cards = new List<Card>();
        private int numMatches;

        public Hand()
        {
        }

        public int GetNumMatches()
        {
            return numMatches;
        }

        public int GetSize()
        {
            return cards.Count;
        }

        public Card GetCard(int index)
        {
            return cards[index];
        }

        public void Reset()
        {

        }

        public void DisplayHand(bool visible)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].Display(visible);
            }
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int HasAPair()
        {


            return -1;
        }

        public int HasCardMatch(Card card)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].GetRank() == card.GetRank())
                {
                    return i;
                }
            }

            return -1;
        }

        //public bool DropCards(int index1, int index2)
        //{

        //}

        public Card StealCard(int index)
        {
            Card tempCard = cards[index];
            cards.RemoveAt(index);
            return tempCard;
        }
    }
}
