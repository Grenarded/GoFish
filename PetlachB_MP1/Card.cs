﻿//Author: Ben Petlach
//File Name: Card.cs
//Project Name: PetlachB_MP1
//Creation Date: March 6, 2023
//Modified Date: March 16, 2023
//Description: File for the card object, maintaing info about ranks and suits

using System;
namespace PetlachB_MP1
{
    public class Card
    {
        public const int CARD_SPACING = 2;

        private const int NUM_RANKS = 13;

        private const string HEARTS = "0";
        private const string SPADES = "1";
        private const string DIAMONDS = "2";
        private const string CLUBS = "3";

        private string rank;
        private string suit;

        public Card(int cardNum)
        {
            rank = Convert.ToString(cardNum % NUM_RANKS);

            if (rank == "0")
            {
                rank = "A";
            }
            else if (rank == "10")
            {
                rank = "J";
            }
            else if (rank == "11")
            {
                rank = "Q";
            }
            else if (rank == "12")
            {
                rank = "K";
            }
            else
            {
                rank = Convert.ToString(Convert.ToInt32(rank) + 1);
            }

            suit = Convert.ToString(cardNum / NUM_RANKS);
        }

        public string GetRank()
        {
            return rank;
        }

        public string GetSuit()
        {
            if (suit == HEARTS)
            {
                return "♥";
            }
            else if (suit == SPADES)
            {
                return "♠";
            }
            else if (suit == DIAMONDS)
            {
                return "♦";
            }
            else
            {
                return "♣";
            }
        }

        public void Display(bool visible)
        {
            if (visible)
            {
                if (suit == HEARTS || suit == DIAMONDS)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                Console.Write(GetRank() + GetSuit() + "".PadRight(CARD_SPACING));

                Console.ResetColor();
            }
            else
            {
                Console.Write("** ");
            }
        }

        public bool MatchCard(Card card)
        {
            if (card.GetRank() == rank)
            {
                return true;
            }

            return false;
        }
    }
}
