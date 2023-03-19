//Author: Ben Petlach
//File Name: Hand.cs
//Project Name: PetlachB_MP1
//Creation Date: March. 10, 2023
//Modified Date: March 17, 2023
//Description: File for the hand object, which maintains which cards each player has in their 'hand', as well as keeping track of the number of pairs dropped (number of matches)

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
            numMatches = 0;
            cards.Clear();
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
            for (int i = 0; i < GetSize(); i++)
            {
                for (int j = i + 1; j < GetSize(); j++)
                {
                    if (cards[i].MatchCard(cards[j]))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        //FIGURE THIS OUT. SHOULD NOT START AT SAME INDEX AS FIRST ONE
        public int HasCardMatch(Card card)
        {
            for (int i = 0; i < GetSize(); i++)
            {
                if (cards[i].MatchCard(card) && cards[i].GetSuit() != card.GetSuit())
                {
                    return i;
                }
            }

            return -1;
        }

        public bool DropCards(int index1, int index2)
        {
            if (cards[index1].MatchCard(cards[index2]))
            {
                cards.RemoveAt(index2);
                cards.RemoveAt(index1);
                numMatches++;

                return true;
            }

            return false;
        }

        public Card StealCard(int index)
        {
            Card tempCard = cards[index];
            cards.RemoveAt(index);
            return tempCard;
        }
    }
}
