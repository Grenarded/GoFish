//Author: Ben Petlach
//File Name: Program.cs
//Project Name: PetlachB_MP1
//Creation Date: March 6, 2023
//Modified Date: March 18, 2023
//Description: Main logic for the classic card game Go Fish!, recreated on console for a player and an automated dummy AI

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

        //Game actions
        const int ASK_CARD = 1;
        const int DROP_PAIR = 2;
        const int DRAW_CARD = 3;

        static Random rng = new Random();

        static Hand[] gameHands = new Hand[NUM_PLAYERS] {new Hand(), new Hand()};

        static private int curPlayer;

        static Deck deck = new Deck();

        //Highlight
        static bool highlight = false;

        public static void Main(string[] args)
        {
            string input = "";
            while (!input.Equals("2"))
            {
                Console.Clear();

                Console.WriteLine("Welcome to Go Fish!\n");

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
            for (int i = 0; i < NUM_PLAYERS; i++)
            {
                gameHands[i].Reset();
            }

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

                //Check for a pair
                if (gameHands[curPlayer].HasAPair() != -1)
                {
                    if (curPlayer != CPU)
                    {
                        highlight = true;
                    }
                }
                else
                {
                    highlight = false;
                }

                DrawGame();

                if (gameHands[PLAYER].GetSize() != 0 && curPlayer != CPU)
                {
                    DrawMenu();

                    bool validInput = false;

                    while (!validInput)
                    {
                        if (int.TryParse(Console.ReadLine(), out input))
                        {
                            if (input == ASK_CARD || input == DROP_PAIR || input == DRAW_CARD)
                            {
                                validInput = true;

                                if (input == DRAW_CARD && deck.GetSize() <= 0)
                                {
                                    validInput = false;
                                }
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
                }
                else if (gameHands[PLAYER].GetSize() == 0 && curPlayer != CPU)
                {
                    if (deck.GetSize() == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Your hand is empty and you can't draw any more cards!");
                        Console.WriteLine("Press <enter> to end your turn");
                        Console.ResetColor();
                        Console.ReadLine();
                        curPlayer = NextPlayer();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Your hand is empty. Press <enter> to GO FISH!");
                        Console.ResetColor();
                        Console.ReadLine();
                        GoFish();
                    }
                }

                //CPU LOGIC
                if (curPlayer == CPU)
                {
                    while (DropPair())
                    {
                        DropPair();
                    }

                    if (gameHands[CPU].GetSize() > 0)
                    {
                        AskCard();
                    }
                    else if (deck.GetSize() > 0)
                    {
                        Console.WriteLine("The CPU decides to GO FISH!");
                        Console.WriteLine("Press <enter> to continue");
                        Console.ReadLine();
                        GoFish();
                    }
                }

                if (deck.GetSize() > 0)
                {
                    switch (input)
                    {
                        case ASK_CARD:
                            AskCard();
                            break;
                        case DROP_PAIR:
                            DropPair();
                            break;
                        case DRAW_CARD:
                            GoFish();
                            break;
                    }
                }
                else
                {
                    switch (input)
                    {
                        case ASK_CARD:
                            AskCard();
                            break;
                        case DROP_PAIR:
                            DropPair();
                            break;
                    }
                }

                int totalNumMatches = 0;
                for (int i = 0; i < NUM_PLAYERS; i++)
                {
                    totalNumMatches += gameHands[i].GetNumMatches();
                }

                if (totalNumMatches == Deck.DECK_SIZE / NUM_PLAYERS)
                {
                    isGameDone = true;
                }
            }
            DisplayWinner();
        }

        private static void DrawMenu()
        {
            Console.WriteLine("1. Ask for a card");
            if (highlight)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("2. Drop a pair");
            Console.ResetColor();
            if (deck.GetSize() > 0)
            {
                Console.WriteLine("3. Draw a card. Ends your turn");
            }
            Console.Write("\nChoice: ");
        }

        private static void AskCard()
        {
            int index = -1;

            if (curPlayer != CPU)
            {
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
            }
            else
            {
                index = rng.Next(gameHands[CPU].GetSize() - 1);
            }

            Console.Clear();
            DrawGame();

            //Ask card logic
            Card cardAsked = gameHands[curPlayer].GetCard(index);
            int cardMatch = gameHands[NextPlayer()].HasCardMatch(cardAsked);

            if (curPlayer != CPU)
            {
                Console.WriteLine($"You asked the CPU for a {cardAsked.GetRank()}");
            }
            else
            {
                Console.WriteLine($"The CPU asked you for a {cardAsked.GetRank()}");
            }

            //check if other player has the card
            if (cardMatch != -1)
            {
                if (curPlayer != CPU)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The CPU has a match!");
                    Console.ResetColor();
                    Console.WriteLine("Press <enter> to steal it");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Unfortunately, you have it");
                    Console.ResetColor();
                    Console.WriteLine("Press <enter> to give it");
                }

                Console.ReadLine();

                gameHands[curPlayer].AddCard(gameHands[NextPlayer()].StealCard(cardMatch));
            }
            else
            {
                if (curPlayer != CPU)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("The CPU doesn't have it. GO FISH!");
                    Console.ResetColor();
                    Console.WriteLine("Press <enter> to continue");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("You don't have it!");
                    Console.ResetColor();
                    if (deck.GetSize() > 0)
                    {
                        Console.WriteLine("Press <enter> to make the CPU GO FISH!");
                    }
                    else
                    {
                        Console.WriteLine("Press <enter> to end the CPU's turn");
                    }
                }

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

        private static bool DropPair()
        {
            Hand curHand = gameHands[curPlayer];

            if (curHand.HasAPair() != -1)
            {
                Console.Clear();
                DrawGame();

                int index1 = curHand.HasAPair();
                int index2 = curHand.HasCardMatch(curHand.GetCard(index1));

                if (curPlayer != CPU)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Press <enter> to drop down your {curHand.GetCard(index1).GetRank()}'s");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"The CPU dropped a pair of {curHand.GetCard(index1).GetRank()}'s");
                    Console.ResetColor();
                    Console.WriteLine("Press <enter> to continue");
                }

                Console.ReadLine();

                curHand.DropCards(index1, index2);

                return true;
            }
            else
            {
                if (curPlayer != CPU)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You have no pairs to drop! Press <enter> to continue");
                    Console.ResetColor();
                    Console.ReadLine();
                }
                return false;
            }
        }

        private static void GoFish()
        {
            if (deck.GetSize() > 0)
            {
                gameHands[curPlayer].AddCard(deck.DrawCard());

                curPlayer = NextPlayer();
            }
            else
            {
                if (curPlayer != CPU)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There are no more cards in the deck, so you can't draw a card.");
                    Console.WriteLine("Press <enter> to continue and end your turn");
                    Console.ResetColor();
                    Console.ReadLine();
                    curPlayer = NextPlayer();
                }
            }
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

            if (curPlayer != CPU)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("*ACTIVE* ");
                Console.ResetColor();
            }

            Console.WriteLine($"-PLAYER- (Matches: {gameHands[PLAYER].GetNumMatches()})\n");
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
                        string space = "".PadRight(1);


                        if (j < 10)
                        {
                            if (gameHands[i].GetCard(j).GetRank() == "10")
                            {
                                space = "".PadRight(2);
                            }
                            Console.Write(space + j + "".PadRight(Card.CARD_SPACING));
                        }
                        else if (j == 10)
                        {
                            if (gameHands[i].GetCard(j).GetRank() == "10")
                            {
                                Console.Write(space + j + "".PadRight(Card.CARD_SPACING));
                            }
                            else
                            {
                                Console.Write(j + "".PadRight(Card.CARD_SPACING));
                            }
                        }
                        else
                        {
                            if (gameHands[i].GetCard(j).GetRank() == "10")
                            {
                                space = "".PadRight(1);
                            }
                            else
                            {
                                space = "";
                            }

                            Console.Write(space + j + "".PadRight(Card.CARD_SPACING));
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
                    if (curPlayer == CPU)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*ACTIVE* ");
                        Console.ResetColor();
                    }

                    Console.WriteLine($"-CPU- (Matches: {gameHands[CPU].GetNumMatches()})\n");
                    Console.Write("HAND: ".PadRight(PAD_SPACING));
                }
            }
            Console.WriteLine();
        }

        private static void DisplayWinner()
        {
            Console.Clear();

            const int TIE = 2;

            int winner;

            if (gameHands[PLAYER].GetNumMatches() > gameHands[CPU].GetNumMatches())
            {
                winner = PLAYER;
            }
            else if (gameHands[PLAYER].GetNumMatches() < gameHands[CPU].GetNumMatches())
            {
                winner = CPU;
            }
            else
            {
                winner = TIE;
            }

            switch (winner)
            {
                case TIE:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("It's a tie!");
                    Console.ResetColor();
                    break;
                case PLAYER:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You are the winner!");
                    Console.ResetColor();
                    break;
                case CPU:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You lost!");
                    Console.ResetColor();
                    break;
            }
            Console.WriteLine();
            Console.WriteLine($"You had: {gameHands[PLAYER].GetNumMatches()} matches");
            Console.WriteLine($"The CPU had: {gameHands[CPU].GetNumMatches()} matches");
            Console.WriteLine("Press <enter> to return to main menu");
        }
    }
}
