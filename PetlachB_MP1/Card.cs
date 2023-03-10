using System;
namespace PetlachB_MP1
{
    public class Card
    {
        //BAD PRACTICE??
        public const int CARD_SPACING = 2;

        private const int NUM_RANKS = 13;

        private const string HEARTS = "0";
        private const string SPADES = "1";
        private const string DIAMONDS = "2";
        private const string CLUBS = "3";

        private string rank;
        private string suit;
        private ConsoleColor colour;

        public Card(int cardNum)
        {
            rank = Convert.ToString(cardNum % NUM_RANKS);
            suit = Convert.ToString(cardNum / NUM_RANKS);
        }

        public string GetRank()
        {
            if (rank == "0")
            {
                return "A";
            }
            else if (rank == "10")
            {
                return "J";
            }
            else if (rank == "11")
            {
                return "Q";
            }
            else if (rank == "12")
            {
                return "K";
            }
            else
            {
                return Convert.ToString(Convert.ToInt32(rank) + 1);
            }

            //return rank;
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
            //TODO: Logic

            return false;
        }
    }
}
