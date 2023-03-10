using System;

namespace PetlachB_MP1
{
    class MainClass
    {
        public const int START_CARDS = 5;
        public const int NUM_PLAYERS = 2;

        //Store player info
        private const int PLAYER = 0;
        private const int CPU = 1;

        static Hand[] gameHands = new Hand[NUM_PLAYERS] {new Hand(), new Hand()};

        static private int curPlayer;

        static Deck deck = new Deck();

        //Game actions
        const int ASK_CARD = 1;
        const int DROP_PAIR = 2;
        const int DRAW_CARD = 3;

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Go Fish!\n");

            string input = "";
            while (!input.Equals("2"))
            {
                Console.Clear();

                Console.WriteLine("1. Start Game");
                Console.WriteLine("2. Exit\n");
                Console.Write("Selection: ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        SetUpGame();
                        break;
                    case "2":
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Not a valid input. Press enter to try again");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void SetUpGame()
        {
            curPlayer = PLAYER;

            deck.ResetDeck();

            DealCards();

            PlayGame();
            
            Console.ReadLine();
        }

        private static void PlayGame()
        {
            bool isGameDone = false;
            while (!isGameDone)
            {
                int input = 0;

                Console.Clear();
                DrawGame();
                DrawMenu();

                bool validInput = false;

                //TODO: Make input checking into a method?
                while (!validInput)
                {
                    if (int.TryParse(Console.ReadLine(), out input))
                    {
                        if (input == ASK_CARD || input == DROP_PAIR || input == DRAW_CARD)
                        {
                            validInput = true;
                        }
                    }

                    if (!validInput)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("That's not an option! Press <enter> to try again");
                        Console.ResetColor();
                        Console.ReadLine();

                        Console.Clear();
                        DrawGame();
                        DrawMenu();
                    }
                }

                switch (input)
                {
                    case ASK_CARD:
                        AskCard();
                        break;
                    case DRAW_CARD:
                        GoFish();
                        break;
                }
            }
        }

        private static void DrawMenu()
        {
            Console.WriteLine("1. Ask for a Card (if possible)");
            Console.WriteLine("2. Drop a pair (if possible)");
            Console.WriteLine("3. Draw a card (if possible). Ends your turn");
            Console.Write("\nChoice: ");
        }

        private static void AskCard()
        {
            int index = -1;

            bool validInput = false;

            while (!validInput)
            {
                Console.Write("Choose the card index to request a matching card: ");

                if (int.TryParse(Console.ReadLine(), out index))
                {
                    if (index < gameHands[curPlayer].GetSize())
                    {
                        validInput = true;
                    }
                }

                if (!validInput)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That index doesn't exist! Press <enter> to try again");
                    Console.ResetColor();
                    Console.ReadLine();

                    Console.Clear();
                    DrawGame();
                    DrawMenu();
                    Console.WriteLine("1");
                }
            }

            //Ask card logic
            Console.Clear();
            DrawGame();

            Card cardAsked = gameHands[curPlayer].GetCard(index);

            if (curPlayer == PLAYER)
            {
                Console.WriteLine("You asked the CPU for a " + cardAsked.GetRank());
            }

            //check if other player has the card
            if (gameHands[NextPlayer()].HasCardMatch(cardAsked) >= 0)
            {
                if (curPlayer == PLAYER)
                {
                    Console.WriteLine("The CPU has a match!");
                    Console.WriteLine("Press <enter> to steal it");
                }
                else
                {

                }

                Console.ReadLine();


                gameHands[curPlayer].AddCard(gameHands[NextPlayer()].StealCard(index));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("The card was not found. GO FISH!");
                Console.ResetColor();
                Console.WriteLine("Press <enter> to continue");
                Console.ReadLine();
                GoFish();
            }
        }

        private static int NextPlayer()
        {
            if (curPlayer + 1 < NUM_PLAYERS)
            {
                return curPlayer + 1;
            }

            return 0;
        }

        private static void GoFish()
        {
            //TODO: Logic for if there are no more cards
            gameHands[curPlayer].AddCard(deck.DrawCard());

            curPlayer = NextPlayer();
        }

        private static void DealCards()
        {
            for (int i = 0; i < START_CARDS; i++)
            {
                for (int j = 0; j < NUM_PLAYERS; j++)
                {
                    gameHands[j].AddCard(deck.DrawCard());
                }
            }
        }

        private static void DrawGame()
        {
            const int PAD_SPACING = 8;

            bool visible;

            Console.Write("HAND: ".PadRight(PAD_SPACING));

            for (int i = 0; i < NUM_PLAYERS; i++)
            {
                if (i == PLAYER)
                {
                    visible = true;
                }
                else
                {
                    visible = false;
                }

                gameHands[i].DisplayHand(visible);

                if (visible)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("\nINDEX:  ".PadRight(PAD_SPACING));
                    Console.ResetColor();

                    for (int j = 0; j < gameHands[i].GetSize(); j++)
                    {
                        string preSpace = "".PadRight(1);

                        if (j < 10)
                        {
                            if (gameHands[i].GetCard(j).GetRank() == "10")
                            {
                                preSpace = "".PadRight(2);
                            }
                            Console.Write(preSpace + j + "".PadRight(Card.CARD_SPACING));
                        }
                        else if (j == 10)
                        {
                            Console.Write(j + "".PadRight(Card.CARD_SPACING - 1));
                        }
                        else
                        {
                            Console.Write(preSpace + j + "".PadRight(Card.CARD_SPACING - 1));
                        }

                    }
                }

                Console.WriteLine("\n");

                if (i == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"DECK SIZE: {deck.GetSize()}");
                    Console.ResetColor();
                    Console.WriteLine();
                }

                if (i < NUM_PLAYERS - 1)
                {
                    Console.Write("HAND: ".PadRight(PAD_SPACING));
                }
            }
            Console.WriteLine();
        }
    }
}
