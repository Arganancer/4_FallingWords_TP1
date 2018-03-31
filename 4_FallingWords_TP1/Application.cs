/*
 * FALLING_WORDS
 * 
 * Un petit jeu qui a comme but de trouver un mot caché. Le tout saupoudré d'un esthétisme 
 * inutilement extravagant.
 * 
 * Auteur: Jean-Raphael Auclair-Soucy
 * 
 * Co-auteur (si applicable): The functions StartWrite() and EndWrite() were written by 
 * Peter McCormick. Since these methods are very functional yet incredibly simple, I left 
 * them as they came without modifying them. Instead, I give full credit to their author.
 * 
 * 
 */
using System.IO;
using System.Collections.Generic;
using System.Media;
using System;
using System.Threading;
namespace ProgrammationJeuxVideo1
{
    // FALLINGWORDS_TP1
    /// <summary>
    /// La classe dans laquelle se retrouve le contenu du jeu au complet.
    /// </summary>
    public class FallingWords_TP1
    {

#region Variables: Static Variables used Globally

        // MAIN_MENU_and_SPLASH_SCREEN
        static int consoleWindowWidth = 134;
        static int consoleWindowHeight = 39;
        static bool splashScreenThreadRunning = true;
        static bool fallingNumbersThreadRunning = true;
        static bool randomCharactersThreadRunning = true;
        static bool splashTitleThreadRunning = true;

        // TWO_PLAYER_MODE
        // Dictates which player's turn it is to write a word.
        static int playerOneOrPlayerTwo = 1;
        static string firstPlayerName = "";
        static string secondPlayerName = "";

        // UNIVERSAL_THROUGHOUT_PROGRAM
        static bool cursorUsable = true;

        // GAME_STATE
        // 0 = Currently in the Process of Quitting
        // 1 = Currently in Single Player Mode
        // 2 = Currently in Two Player Mode
        // 3 = Currently in Story Mode
        // 4 = Currently in Main Menu
        // 5 = Currently in Splash Screen
        static int gameStateVariable = 5;

        #endregion

        // RUN
        /// <summary>
        /// La méthode Run est le point de départ (point d'entrée)
        /// dans notre application.
        /// </summary>
        public void Run()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(consoleWindowWidth, consoleWindowHeight);
            GameForewarning();

            while (gameStateVariable != 0)
            {
                // 5 Splash Screen
                Thread playIntroMusic = new Thread(PlaySplashScreenAndMainMenuMusic);
                playIntroMusic.Start();
                RunSplashScreenFunction();
                // 4 Main Menu
                RunMainMenuFunction();
                // 0 Quit
                if (gameStateVariable == 0)
                {
                    Console.Clear();
                    ChangeConsoleTitle();
                }
                // 1 Single Player
                else if (gameStateVariable == 1)
                {
                    Console.Clear();
                    ChangeConsoleTitle();
                    PlayASinglePlayerGame();
                }
                // 2 Two Player
                else if (gameStateVariable == 2)
                {
                    Console.Clear();
                    ChangeConsoleTitle();
                    MultiplayerChooseNames();
                    PlayAMultiplayerGame();
                }
                // 3 Story Mode
                else if (gameStateVariable == 3)
                {
                    Console.Clear();
                    ChangeConsoleTitle();
                }
            }
        }

        //GAME_FOREWARNING
        /// <summary>
        /// Affiche un premier message d'accueil avant l'écran titre 
        /// pour avertir l’utilisateur du volume du jeu.
        /// </summary>
        void GameForewarning()
        {
            string firstStringToBeTyped = "Hello";
            string secondStringToBeTyped = ", it's been a while.";
            string thirdStringToBeTyped = "I'm glad you decided to come back.";
            string fourthStringToBeTyped = "Be warned though";
            string fifthStringToBeTyped = ", things have gotten";
            string sixthStringToBeTyped = "Louder, ";
            string seventhStringToBeTyped = "since you were last here.";
            string eighthStringToBeTyped = "Prepare your headphones";

            int idleTime = 750;
            int typingSpeed = 0;
            int minimumTypingSpeed = 30;
            int maximumTypingSpeed = 100;
            Thread.Sleep(idleTime);

            Random rnd = new Random();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = true;

            Console.SetCursorPosition(4, 2);
            foreach (char currentChar in firstStringToBeTyped)
            {
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
            }
            Thread.Sleep(idleTime);
            foreach (char currentChar in secondStringToBeTyped)
            {
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
            }
            Thread.Sleep(idleTime);
            Console.SetCursorPosition(4, 3);
            foreach (char currentChar in thirdStringToBeTyped)
            {
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
            }
            Thread.Sleep(idleTime);
            Console.SetCursorPosition(8, 5);
            foreach (char currentChar in fourthStringToBeTyped)
            {
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
            }
            Thread.Sleep(idleTime);
            foreach (char currentChar in fifthStringToBeTyped)
            {
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
            }
            for (int count = 0; count < 3; count++)
            {
                Thread.Sleep(450);
                Console.Write(" .");
            }
            Console.SetCursorPosition(4, 6);
            foreach (char currentChar in sixthStringToBeTyped)
            {
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
            }
            Thread.Sleep(idleTime);
            foreach (char currentChar in seventhStringToBeTyped)
            {
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
            }
            Thread.Sleep(idleTime);
            Console.SetCursorPosition(4, 7);
            foreach (char currentChar in eighthStringToBeTyped)
            {
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
            }
            for (int count = 0; count < 3; count++)
            {
                Thread.Sleep(450);
                Console.Write(" .");
            }
            Thread.Sleep(idleTime);
            Console.CursorVisible = false;
        }

#region Functions: Splash Screen

        // RUN_SPLASH_SCREEN_FUNCTION
        /// <summary>
        /// Gère les "threads" d'animation de l'écran titre et la séquence 
        /// d'exécution de plusieurs fonctions durant l'écran titre.
        /// </summary>
        void RunSplashScreenFunction()
        {
            gameStateVariable = 5;
            ChangeConsoleTitle();
            DisplayFooterBox();
            DisplaySplashScreenFooterMessage();

            Thread DisplaySplashScreenThread = new Thread(DisplaySplashScreenCharStringAnimations);
            splashScreenThreadRunning = true;
            DisplaySplashScreenThread.Start();

            Thread DisplaySplashTitleThread = new Thread(DisplaySplashScreenTitle);
            DisplaySplashTitleThread.Start();
            splashTitleThreadRunning = true;

            FlushKeyBuffer();
            ReadCharacterAToZ();
            DisplayFooterBox();
            DisplayStartingFooterMessage();
            splashTitleThreadRunning = false;
            splashScreenThreadRunning = false;
            DisplaySplashScreenThread.Join();
            DisplaySplashTitleThread.Join();
            Thread.Sleep(50);
            Console.Clear();
        }

        // DISPLAY_SPLASH_SCREEN_TITLE
        /// <summary>
        /// Affiche le titre du jeu dans l'écran titre du jeu avec une animation 
        /// constante. Cette fonction est utilisée en tant que "thread" à part de 
        /// la "main thread".
        /// </summary>
        static void DisplaySplashScreenTitle()
        {
            Console.SetCursorPosition(0, 33);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Full Disclosure; Resizing the window of this application will cause issues. DO NOT resize the window unless you wish to cause mayhem.");

            Thread.Sleep(5);
            int countTitleRow = 0;
            Random rnd = new Random();
            ConsoleColor[] choiceCharColors = new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.DarkGreen };
            char[] choiceOfChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '#' };
            char characterToWrite = ' ';
            string[] choiceOfTitleRow = new string[] {"#########    ##     ##        ##        ######  ####   ##   ########     ##          ##   ########   ########   ########    ########",
                                                      "##          ####    ##        ##          ##    ####   ##  ##      ##    ##          ##  ##      ##  ##     ##  ##     ##  ##      ##",
                                                      "##         ##  ##   ##        ##          ##    ## ##  ##  ##            ##          ##  ##      ##  ##     ##  ##     ##  ##",
                                                      "##        ##    ##  ##        ##          ##    ## ##  ##  ##             ##   ##   ##   ##      ##  ##     ##  ##     ##  ##",
                                                      "#######   ########  ##        ##          ##    ##  ## ##  ##  ######     ##  ####  ##   ##      ##  ########   ##     ##   ########",
                                                      "##        ##    ##  ##        ##          ##    ##  ## ##  ##      ##     ##  ####  ##   ##      ##  ##  ##     ##     ##          ##",
                                                      "##        ##    ##  ##        ##          ##    ##   ####  ##      ##      ####  ####    ##      ##  ##   ##    ##     ##  ##      ##",
                                                      "##        ##    ##  ########  ########  ######  ##   ####   ########       ####  ####     ########   ##    ##   ########    ########",};
            while (splashTitleThreadRunning)
            {
                Thread.Sleep(50);
                StartWriting();
                Thread.Sleep(8);
                Console.SetCursorPosition(0, 13 + countTitleRow);
                foreach (char hashTagChar in choiceOfTitleRow[countTitleRow])
                {
                    Thread.Sleep(0);
                    if (hashTagChar == ' ')
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.ForegroundColor = choiceCharColors[rnd.Next(0, choiceCharColors.Length)];
                        characterToWrite = choiceOfChars[rnd.Next(0, choiceOfChars.Length)];
                        Console.Write(characterToWrite);
                    }
                }
                countTitleRow = countTitleRow + 1;
                if (countTitleRow == 8)
                {
                    countTitleRow = 0;
                }
                StopWriting();
            }
        }

        // DISPLAY_SPLASH_SCREEN_CHAR_STRING_ANIMATIONS
        /// <summary>
        /// Affiche plusieurs caractères aléatoires à des coordonnées aléatoires 
        /// et d'une couleur aléatoire dans l'écran titre du jeu. Cette fonction 
        /// est utilisée en tant que "thread" à part de la "main thread".
        /// </summary>
        static void DisplaySplashScreenCharStringAnimations()
        {
            while (splashScreenThreadRunning)
            {
                ConsoleColor[] charColors = new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.White };
                Random rnd = new Random();
                string[] choiceOfCharacter = new string[] { " ", " ", "°", "¬", "*", " ", ",", "-", "_" };
                string finalChoiceOfStartMessage = "";
                string[] choiceOfStartMessage = new string[]
                {
                    "What year is it?",
                    "Words are made of letters",
                    "Letters are characters",
                    "Bools Bools Bools",
                    "Falling with style",
                    "π = 3.14",
                    "Hi mom!",
                    "ghost words",
                    "Indexers can use params",
                    "string interning",
                    "Variables in methods can be scoped with just braces",
                    "Enums can have extension methods",
                    "The order of static variable declaration matters",
                    "You can & and | nullable booleans with SQL compatibility",
                };
                int determineCharacter;
                int determineColor;
                int rndNumber;
                StartWriting();
                Thread.Sleep(5);
                for (int count = 0; count < 100; count++)
                {
                    // Occasionally Write Message.
                    rndNumber = rnd.Next(0, 500);
                    if (rndNumber == 1)
                    {
                        rndNumber = rnd.Next(0, 101);
                        if (rndNumber < 20)
                        {
                            determineColor = 0;
                        }
                        else if (rndNumber < 96)
                        {
                            determineColor = 1;
                        }
                        else
                        {
                            determineColor = 2;
                        }

                        //Determines whether the phrase will be above or below the "FALLING WORDS" message.
                        finalChoiceOfStartMessage = choiceOfStartMessage[rndNumber = rnd.Next(0, choiceOfStartMessage.Length)];
                        rndNumber = rnd.Next(0, 2);
                        if (rndNumber == 0)
                        {
                            Console.SetCursorPosition(rnd.Next(0, consoleWindowWidth - finalChoiceOfStartMessage.Length), rnd.Next(0, 12));
                        }
                        else
                        {
                            Console.SetCursorPosition(rnd.Next(0, consoleWindowWidth - finalChoiceOfStartMessage.Length), rnd.Next(22, 33));
                        }
                        Console.ForegroundColor = charColors[determineColor];
                        Console.Write(finalChoiceOfStartMessage);
                    }

                    // Else Random Character.
                    else
                    {
                        // Determine Random Color.
                        rndNumber = rnd.Next(0, 101);
                        if (rndNumber < 60)
                        {
                            determineColor = 0;
                        }
                        else if (rndNumber < 96)
                        {
                            determineColor = 1;
                        }
                        else
                        {
                            determineColor = 2;
                        }

                        // Determine Random Character.
                        rndNumber = rnd.Next(0, 250);
                        if (rndNumber < 175)
                        {
                            determineCharacter = 0;
                        }
                        else if (rndNumber < 205)
                        {
                            determineCharacter = 1;
                        }
                        else if (rndNumber < 220)
                        {
                            determineCharacter = 2;
                        }
                        else if (rndNumber < 230)
                        {
                            determineCharacter = 3;
                        }
                        else if (rndNumber < 235)
                        {
                            determineCharacter = 4;
                        }
                        else if (rndNumber < 240)
                        {
                            determineCharacter = 5;
                        }
                        else if (rndNumber < 243)
                        {
                            determineCharacter = 6;
                        }
                        else if (rndNumber < 248)
                        {
                            determineCharacter = 7;
                        }
                        else
                        {
                            determineCharacter = 8;
                        }

                        // Set Random Cursor Position
                        rndNumber = rnd.Next(0, 2);
                        if (rndNumber == 0)
                        {
                            Console.SetCursorPosition(rnd.Next(0, 133), rnd.Next(0, 12));
                        }
                        else
                        {
                            Console.SetCursorPosition(rnd.Next(0, 133), rnd.Next(22, 33));
                        }
                        Console.ForegroundColor = charColors[determineColor];
                        Console.Write(choiceOfCharacter[determineCharacter]);
                    }
                }
                StopWriting();
                Thread.Sleep(50);
            }
        }

        #endregion

#region Functions: Main Menu

        // RUN_MAIN_MENU_FUNCTION
        /// <summary>
        /// Gère la séquence d'exécution du menu principal.
        /// </summary>
        void RunMainMenuFunction()
        {
            gameStateVariable = 4;
            ChangeConsoleTitle();

            Thread DisplayFallingNumbersThread = new Thread(FallingNumbersThread);
            fallingNumbersThreadRunning = true;
            DisplayFallingNumbersThread.Start();

            Thread DisplayRandomCharactersThread = new Thread(DisplayMainMenuRandomCharacters);
            randomCharactersThreadRunning = true;
            DisplayRandomCharactersThread.Start();

            DisplayFirstMainMenuAnimation();
            gameStateVariable = MainMenuReadCharacter();
            fallingNumbersThreadRunning = false;
            DisplaySecondMainMenuAnimation();
            randomCharactersThreadRunning = false;
            DisplayRandomCharactersThread.Join();
            DisplayFallingNumbersThread.Join();
        }

        // DISPLAY_FIRST_MAIN_MENU_ANIMATION
        /// <summary>
        /// Affiche la première animation du menu principale.
        /// </summary>
        void DisplayFirstMainMenuAnimation()
        {
            Random rnd = new Random();
            int typingSpeed = 0;
            int minimumTypingSpeed = 20;
            int maximumTypingSpeed = 45;
            int cursorHorizontalPosition = 90;
            int count = 0;
            int threadSleepLagFix = 12;
            Thread.Sleep(1150);
            foreach (char currentChar in "static void Main(string[] args)")
            {
                StartWriting();
                Thread.Sleep(threadSleepLagFix);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(cursorHorizontalPosition + count, 15);
                Console.Write(currentChar);
                StopWriting();
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
                count = count + 1;
            }

            StartWriting();
            Thread.Sleep(threadSleepLagFix);
            Thread.Sleep(50);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(cursorHorizontalPosition, 16);
            Console.Write("{");
            StopWriting();
            Thread.Sleep(100);

            count = +4;
            foreach (char currentChar in "1_Single_Player")
            {
                StartWriting();
                Thread.Sleep(threadSleepLagFix);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(cursorHorizontalPosition + count, 17);
                Console.Write(currentChar);
                StopWriting();
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
                count = count + 1;
            }

            count = +4;
            foreach (char currentChar in "2_Multiplayer")
            {
                StartWriting();
                Thread.Sleep(threadSleepLagFix);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(cursorHorizontalPosition + count, 18);
                Console.Write(currentChar);
                StopWriting();
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
                count = count + 1;
            }

            count = +4;
            foreach (char currentChar in "0_Quit")
            {
                StartWriting();
                Thread.Sleep(threadSleepLagFix);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(cursorHorizontalPosition + count, 19);
                Console.Write(currentChar);
                StopWriting();
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
                count = count + 1;
            }

            Thread.Sleep(50);
            StartWriting();
            Thread.Sleep(threadSleepLagFix);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(cursorHorizontalPosition, 20);
            Console.Write("}");
            StopWriting();
            count = count + 1;
            Thread.Sleep(120);

            count = 0;
            foreach (char currentChar in "Console.ReadKey(_); ")
            {
                StartWriting();
                Thread.Sleep(threadSleepLagFix);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(cursorHorizontalPosition + count, 22);
                Console.Write(currentChar);
                StopWriting();
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
                count = count + 1;
            }
            FlushKeyBuffer();
        }

        // MAIN_MENU_READ_CHARACTER
        /// <summary>
        /// Méthode qui permet de saisir un caractère à l'écran sans interrompre et rentrer 
        /// en conflit avec les "threads visuelles".
        /// </summary>
        /// <returns>
        /// Retourne une valeur int qui représente l'état du jeu:
            /// GAME_STATE
            /// 0 = Currently in the Process of Quitting
            /// 1 = Currently in Single Player Mode
            /// 2 = Currently in Two Player Mode
            /// 3 = Currently in Story Mode
            /// 4 = Currently in Main Menu
            /// 5 = Currently in Splash Screen
        /// </returns>
        int MainMenuReadCharacter()
        {
            string consoleReadKeyTest = "Console.ReadKey(";
            int consoleReadKeyLength = consoleReadKeyTest.Length;
            while (true)
            {
                if (cursorUsable && Console.KeyAvailable)
                {
                    StartWriting();
                    Thread.Sleep(8);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(90 + consoleReadKeyLength, 22);
                    char userInput = Console.ReadKey().KeyChar.ToString().ToUpper()[0];
                    FlushKeyBuffer();
                    StopWriting();
                    if (userInput == '0' || userInput == '1' || userInput == '2' || userInput == '3')
                    {
                        int gameStateNumericalValue = int.Parse(userInput.ToString());
                        return gameStateNumericalValue;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }
        }

        // DISPLAY_SECOND_MAIN_MENU_ANIMATION
        /// <summary>
        /// Affiche la deuxième animation du menu principale qui annonce au 
        /// joueur quel mode de jeu s'apprête à débuter.
        /// </summary>
        static void DisplaySecondMainMenuAnimation()
        {
            Random rnd = new Random();
            int cursorHorizontalPosition = 90;
            string textToDisplay;
            if (gameStateVariable == 1)
            {
                textToDisplay = "Executing Single Player Script";
            }
            else if (gameStateVariable == 2)
            {
                textToDisplay = "Executing Multiplayer Script";
            }
            else if (gameStateVariable == 3)
            {
                textToDisplay = "Executing Secret Story Mode";
            }
            else
            {
                textToDisplay = "Commencing end sequence";
            }
            StartWriting();
            Thread.Sleep(40);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(cursorHorizontalPosition, 24);
            Console.Write(textToDisplay);
            StopWriting();
            for (int count = 0; count < 3; count++)
            {
                Thread.Sleep(650);
                StartWriting();
                Thread.Sleep(8);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(cursorHorizontalPosition + textToDisplay.Length + count * 2, 24);
                Console.Write(" .");
                StopWriting();
            }
            FlushKeyBuffer();
        }

        // FALLING_NUMBERS_THREAD
        /// <summary>
        /// Affiche une ligne de nombres verticale qui semble tomber. 
        /// Cette fonction est utilisée dans un "thread" séparer de manière répétitive.
        /// </summary>
        public static void FallingNumbersThread()
        {
            ConsoleColor[] charColors = new ConsoleColor[] { ConsoleColor.DarkGreen, ConsoleColor.Green };
            ConsoleColor[] charColors2 = new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White };
            char[] choiceOfChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '#' };
            Random rnd = new Random();
            int determineCharacter;
            int determineColor;
            int startingCoordinateX;
            int modifiedCoordinateY;
            int startingCoordinateY;
            int determineCurrentY;
            int fallingWordLength;
            int fallingWordDuration;
            while (fallingNumbersThreadRunning)
            {
                startingCoordinateX = rnd.Next(0, 89);
                fallingWordLength = rnd.Next(7, 15);
                fallingWordDuration = rnd.Next(10, 37 - fallingWordLength);
                startingCoordinateY = rnd.Next(0, (37 - (fallingWordLength + fallingWordDuration)));
                modifiedCoordinateY = 0;
                // SPAWNING_LOOP
                for (int count1 = 0; count1 < fallingWordLength; count1++)
                {
                    StartWriting();
                    for (int count2 = 0; count2 < fallingWordLength * 2; count2++)
                    {
                        Console.SetCursorPosition(startingCoordinateX, determineCurrentY = (startingCoordinateY + (rnd.Next(0, count1))));
                        // Determine Random Color.
                        if (determineCurrentY == count1 + startingCoordinateY - 1)
                        {
                            if (rnd.Next(0, 11) < 4)
                            {
                                determineColor = 1;
                            }
                            else
                            {
                                determineColor = 0;
                            }
                            Console.ForegroundColor = charColors2[determineColor];
                        }
                        else
                        {
                            if (rnd.Next(0, 80) < 55 - determineCurrentY)
                            {
                                determineColor = 0;
                            }
                            else
                            {
                                determineColor = 1;
                            }
                            Console.ForegroundColor = charColors[determineColor];
                        }

                        determineCharacter = rnd.Next(0, choiceOfChars.Length);
                        Console.Write(choiceOfChars[determineCharacter]);
                        if (fallingNumbersThreadRunning == false)
                        {
                            break;
                        }
                    }
                    StopWriting();
                    Thread.Sleep(50);
                }
                // DESCENDING_LOOP
                for (int count1 = 0; count1 < fallingWordDuration + 1; count1++)
                {
                    modifiedCoordinateY = startingCoordinateY + count1;
                    StartWriting();
                    for (int count2 = 0; count2 < fallingWordLength * 3; count2++)
                    {
                        Console.SetCursorPosition(startingCoordinateX, determineCurrentY = (modifiedCoordinateY + (rnd.Next(0, fallingWordLength))));
                        // Determine Random Color.
                        if (determineCurrentY == modifiedCoordinateY + fallingWordLength - 1)
                        {
                            if (rnd.Next(0, 11) < 4)
                            {
                                determineColor = 1;
                            }
                            else
                            {
                                determineColor = 0;
                            }
                            Console.ForegroundColor = charColors2[determineColor];
                        }
                        else
                        {
                            if (rnd.Next(0, 80) < 55 - determineCurrentY)
                            {
                                determineColor = 0;
                            }
                            else
                            {
                                determineColor = 1;
                            }
                            Console.ForegroundColor = charColors[determineColor];
                        }

                        determineCharacter = rnd.Next(0, choiceOfChars.Length);
                        Console.Write(choiceOfChars[determineCharacter]);
                        if (fallingNumbersThreadRunning == false)
                        {
                            break;
                        }
                    }
                    Console.SetCursorPosition(startingCoordinateX, modifiedCoordinateY);
                    Console.Write(" ");
                    StopWriting();
                    Thread.Sleep(50);
                }
                // DISAPPEARING_LOOP
                for (int count1 = 0; count1 < fallingWordLength; count1++)
                {
                    modifiedCoordinateY = modifiedCoordinateY + 1;
                    StartWriting();
                    for (int count2 = 0; count2 < fallingWordLength * 2; count2++)
                    {
                        Console.SetCursorPosition(startingCoordinateX, determineCurrentY = (modifiedCoordinateY + (rnd.Next(0, fallingWordLength - count1))));
                        // Determine Random Color.
                        if (determineCurrentY == fallingWordLength - count1 + modifiedCoordinateY - 1)
                        {
                            if (rnd.Next(0, 11) < 4)
                            {
                                determineColor = 1;
                            }
                            else
                            {
                                determineColor = 0;
                            }
                            Console.ForegroundColor = charColors2[determineColor];
                        }
                        else
                        {
                            if (rnd.Next(0, 80) < 55 - determineCurrentY)
                            {
                                determineColor = 0;
                            }
                            else
                            {
                                determineColor = 1;
                            }
                            Console.ForegroundColor = charColors[determineColor];
                        }

                        determineCharacter = rnd.Next(0, choiceOfChars.Length);
                        Console.Write(choiceOfChars[determineCharacter]);
                        if (fallingNumbersThreadRunning == false)
                        {
                            break;
                        }
                    }
                    Console.SetCursorPosition(startingCoordinateX, modifiedCoordinateY);
                    Console.Write(" ");
                    StopWriting();
                    Thread.Sleep(50);
                }
                StopWriting();
                Thread.Sleep(rnd.Next(50, 500));
            }
        }

        // DISPLAY_MAIN_MENU_RANDOM_CHARACTERS
        /// <summary>
        /// Affiche des nombres qui change rapidement (donne un semblant de la matrice). 
        /// Cette fonction est utilisée dans un "thread" séparer de manière répétitive.
        /// </summary>
        static void DisplayMainMenuRandomCharacters()
        {
            ConsoleColor[] charColors = new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.DarkGreen };
            Random rnd = new Random();
            string[] choiceOfCharacter = new string[] { " ", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            int determineCharacter;
            int determineColor;
            int rndNumber;
            int maximumCoordinateX = 88;
            int currentCoordinateY = 0;
            int maximumCoordinateY = 37;
            while (randomCharactersThreadRunning)
            {
                StartWriting();
                Thread.Sleep(8);
                for (int count1 = 0; count1 < 50; count1++)
                {
                    currentCoordinateY = rnd.Next(0, maximumCoordinateY);
                    // Determine Random Color.
                    rndNumber = rnd.Next(0, 80);
                    if (rndNumber < 95 - currentCoordinateY)
                    {
                        determineColor = 1;
                    }
                    else
                    {
                        determineColor = 0;
                    }

                    // Determine Random Character.
                    rndNumber = rnd.Next(0, (int)Math.Pow(maximumCoordinateY, 3));
                    if (rndNumber > Math.Pow(currentCoordinateY, 3))
                    {
                        determineCharacter = 0;
                    }
                    else
                    {
                        determineCharacter = (rnd.Next(0, choiceOfCharacter.Length));
                    }

                    Console.SetCursorPosition(rnd.Next(0, maximumCoordinateX), (currentCoordinateY - 36) * -1);
                    Console.ForegroundColor = charColors[determineColor];
                    Console.Write(choiceOfCharacter[determineCharacter]);
                }
                StopWriting();
                Thread.Sleep(100);
            }
        }

        #endregion

#region Functions: Single Player

        // PLAY_A_SINGLE_PLAYER_GAME
        /// <summary>
        /// Appelle toutes les fonctions nécessaires pour jouer une partie seul, 
        /// et gère en part les options d'affichage durant la partie.
        /// </summary>
        void PlayASinglePlayerGame()
        {
            int totalMissedWords = 0;
            int totalWordsFound = 0;

            // true = Word Found
            // false = Word Missed
            bool currentWordRoundResult = true;

            Thread displayRandomCharacters = new Thread(DisplaySinglePlayerRandomCharacters);
            randomCharactersThreadRunning = true;
            displayRandomCharacters.Start();

            // Function stays in this loop until the word is found or until the user runs out
            // of lives.
            while (true)
            {
                DisplayFooterBox();
                if (totalMissedWords > 2)
                {
                    // PLACEHOLDERTEXT_This whole section up to the next text bracket was originally
                    // meant to be temporary, however I unfortunately ran out of time to change it.
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(4, 36);
                    Console.Write("FAILURE");
                    Thread.Sleep(3500);
                    randomCharactersThreadRunning = false;
                    displayRandomCharacters.Join();
                    return;
                    // End of Placeholder Section.
                }
                DisplayStatisticsSinglePlayer(totalWordsFound, totalMissedWords);
                currentWordRoundResult = PlayAWord();
                if (currentWordRoundResult == true)
                {
                    totalWordsFound = totalWordsFound + 1;
                    PlaySoundFound();
                }
                else
                {
                    totalMissedWords = totalMissedWords + 1;
                    PlaySoundMissed();
                }
            }
        }

        // DISPLAY_STATISTICS_SINGLE_PLAYER
        /// <summary>
        /// Affiche les statistiques de la partie et les informations du projet à 
        /// l'intérieur du pied de page.
        /// </summary>
        /// 
        /// <param name="totalWordsFound">
        /// Représente le nombre de mots trouvés au total.
        /// </param>
        /// 
        /// <param name="totalLivesLeft">
        /// Représente le nombre de vies perdu au total.
        /// </param>
        void DisplayStatisticsSinglePlayer(int totalWordsFound, int totalLivesLeft)
        {
            string createdByText = "Created by Streetlamp";
            string createdForText = "for the class \"Programmation de Jeux Vidéo 1\"";
            StartWriting();
            Thread.Sleep(8);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(4, 35);
            Console.Write("Words Found {0}", totalWordsFound);
            Console.SetCursorPosition(4, 36);
            Console.Write("Attempts Remaining {0}", 3 - totalLivesLeft);

            Console.SetCursorPosition(consoleWindowWidth / 2 - createdByText.Length / 2, 36);
            Console.Write(createdByText);
            Console.SetCursorPosition(consoleWindowWidth / 2 - createdForText.Length / 2, 37);
            Console.Write(createdForText);
            StopWriting();
        }

        // DETERMINE_RANDOM_WORD
        /// <summary>
        /// Choisit un mot de manière aléatoire parmi une liste de mots possibles.
        /// </summary>
        /// 
        /// <returns>
        /// Le mot choisi de manière aléatoire.
        /// </returns>
        string DetermineRandomWord()
        {
            string line;
            List<string> listeDeMotsPossibles = new List<string>();
            StreamReader reader = new StreamReader("listOfPossibleWords.txt");
            while (true)
            {
                line = reader.ReadLine();
                if(line == null)
                {
                    break;
                }
                string[] lineSplit = line.Split(new char[] {' ','\n','\r'},StringSplitOptions.RemoveEmptyEntries);
                foreach (string l in lineSplit)
                {
                    listeDeMotsPossibles.Add(l);
                }
            }
            Random rnd = new Random();
            return listeDeMotsPossibles[rnd.Next(0, listeDeMotsPossibles.Count)].ToUpper();
        }

        // SINGLE_PLAYER_FALLING_NUMBERS_THREAD
        ///
        /// PLACEHOLDERTEXT This function was meant to be displayed during a single player game on each 
        /// side of the screen, however I ran out of time to modify it accordingly. Currently this
        /// function is not called anywhere in the program.
        /// 
        /// <summary>
        /// Affiche une ligne de nombres verticale qui semble tomber. 
        /// Cette fonction est utilisée dans un "thread" séparer de manière répétitive.
        /// </summary>
        public static void SinglePlayerFallingNumbersThread()
        {
            ConsoleColor[] charColors = new ConsoleColor[] { ConsoleColor.DarkGreen, ConsoleColor.Green };
            ConsoleColor[] charColors2 = new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White };
            char[] choiceOfChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '#' };
            Random rnd = new Random();
            int determineCharacter;
            int determineColor;
            int startingCoordinateX;
            int modifiedCoordinateY;
            int startingCoordinateY;
            int determineCurrentY;
            int fallingWordLength;
            int fallingWordDuration;
            while (fallingNumbersThreadRunning)
            {
                startingCoordinateX = rnd.Next(0, 89);
                fallingWordLength = rnd.Next(7, 15);
                fallingWordDuration = rnd.Next(10, 37 - fallingWordLength);
                startingCoordinateY = rnd.Next(0, (37 - (fallingWordLength + fallingWordDuration)));
                modifiedCoordinateY = 0;
                // SPAWNING_LOOP (Starting)
                for (int count1 = 0; count1 < fallingWordLength; count1++)
                {
                    StartWriting();
                    for (int count2 = 0; count2 < fallingWordLength * 2; count2++)
                    {
                        Console.SetCursorPosition(startingCoordinateX, determineCurrentY = (startingCoordinateY + (rnd.Next(0, count1))));
                        // Determine Random Color.
                        if (determineCurrentY == count1 + startingCoordinateY - 1)
                        {
                            if (rnd.Next(0, 11) < 4)
                            {
                                determineColor = 1;
                            }
                            else
                            {
                                determineColor = 0;
                            }
                            Console.ForegroundColor = charColors2[determineColor];
                        }
                        else
                        {
                            if (rnd.Next(0, 80) < 55 - determineCurrentY)
                            {
                                determineColor = 0;
                            }
                            else
                            {
                                determineColor = 1;
                            }
                            Console.ForegroundColor = charColors[determineColor];
                        }

                        determineCharacter = rnd.Next(0, choiceOfChars.Length);
                        Console.Write(choiceOfChars[determineCharacter]);
                        if (fallingNumbersThreadRunning == false)
                        {
                            break;
                        }
                    }
                    StopWriting();
                    Thread.Sleep(50);
                }
                // DESCENDING_LOOP (Middle)
                for (int count1 = 0; count1 < fallingWordDuration + 1; count1++)
                {
                    modifiedCoordinateY = startingCoordinateY + count1;
                    StartWriting();
                    for (int count2 = 0; count2 < fallingWordLength * 3; count2++)
                    {
                        Console.SetCursorPosition(startingCoordinateX, determineCurrentY = (modifiedCoordinateY + (rnd.Next(0, fallingWordLength))));
                        // Determine Random Color.
                        if (determineCurrentY == modifiedCoordinateY + fallingWordLength - 1)
                        {
                            if (rnd.Next(0, 11) < 4)
                            {
                                determineColor = 1;
                            }
                            else
                            {
                                determineColor = 0;
                            }
                            Console.ForegroundColor = charColors2[determineColor];
                        }
                        else
                        {
                            if (rnd.Next(0, 80) < 55 - determineCurrentY)
                            {
                                determineColor = 0;
                            }
                            else
                            {
                                determineColor = 1;
                            }
                            Console.ForegroundColor = charColors[determineColor];
                        }

                        determineCharacter = rnd.Next(0, choiceOfChars.Length);
                        Console.Write(choiceOfChars[determineCharacter]);
                        if (fallingNumbersThreadRunning == false)
                        {
                            break;
                        }
                    }
                    Console.SetCursorPosition(startingCoordinateX, modifiedCoordinateY);
                    Console.Write(" ");
                    StopWriting();
                    Thread.Sleep(50);
                }
                // DISAPPEARING_LOOP (Ending)
                for (int count1 = 0; count1 < fallingWordLength; count1++)
                {
                    modifiedCoordinateY = modifiedCoordinateY + 1;
                    StartWriting();
                    for (int count2 = 0; count2 < fallingWordLength * 2; count2++)
                    {
                        Console.SetCursorPosition(startingCoordinateX, determineCurrentY = (modifiedCoordinateY + (rnd.Next(0, fallingWordLength - count1))));
                        // Determine Random Color.
                        if (determineCurrentY == fallingWordLength - count1 + modifiedCoordinateY - 1)
                        {
                            if (rnd.Next(0, 11) < 4)
                            {
                                determineColor = 1;
                            }
                            else
                            {
                                determineColor = 0;
                            }
                            Console.ForegroundColor = charColors2[determineColor];
                        }
                        else
                        {
                            if (rnd.Next(0, 80) < 55 - determineCurrentY)
                            {
                                determineColor = 0;
                            }
                            else
                            {
                                determineColor = 1;
                            }
                            Console.ForegroundColor = charColors[determineColor];
                        }

                        determineCharacter = rnd.Next(0, choiceOfChars.Length);
                        Console.Write(choiceOfChars[determineCharacter]);
                        if (fallingNumbersThreadRunning == false)
                        {
                            break;
                        }
                    }
                    Console.SetCursorPosition(startingCoordinateX, modifiedCoordinateY);
                    Console.Write(" ");
                    StopWriting();
                    Thread.Sleep(50);
                }
                StopWriting();
                Thread.Sleep(rnd.Next(50, 500));
            }
        }

        // DISPLAY_SINGLE_PLAYER_RANDOM_CHARACTERS
        /// <summary>
        /// Affiche des nombres qui change rapidement (donne un semblant de la matrice). 
        /// Cette fonction est utilisée dans un "thread" séparer de manière répétitive.
        /// </summary>
        static void DisplaySinglePlayerRandomCharacters()
        {
            ConsoleColor[] charColors = new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.DarkGreen };
            Random rnd = new Random();
            string[] choiceOfCharacter = new string[] { " ", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            int determineCharacter;
            int determineColor;
            int rndNumber;
            int currentCoordinateY = 0;
            int maximumCoordinateY = 34;

            // Keeps looping until the global static variable "randomCharactersThreadRunning" is set to false.
            while (randomCharactersThreadRunning)
            {
                StartWriting();
                Thread.Sleep(8);
                for (int count1 = 0; count1 < 50; count1++)
                {
                    currentCoordinateY = rnd.Next(0, maximumCoordinateY);
                    // Determine Random Color.
                    rndNumber = rnd.Next(0, 80);
                    if (rndNumber < 95 - currentCoordinateY)
                    {
                        determineColor = 1;
                    }
                    else
                    {
                        determineColor = 0;
                    }

                    // Determine Random Character.
                    rndNumber = rnd.Next(0, 50653);
                    if (rndNumber > currentCoordinateY * currentCoordinateY * currentCoordinateY)
                    {
                        determineCharacter = 0;
                    }
                    else
                    {
                        determineCharacter = (rnd.Next(0, choiceOfCharacter.Length));
                    }

                    rndNumber = rnd.Next(0, 2);
                    if (rndNumber == 0)
                    {
                        Console.SetCursorPosition(rnd.Next(0, 15), (currentCoordinateY - 33) * -1);
                    }
                    else
                    {
                        Console.SetCursorPosition(rnd.Next(consoleWindowWidth - 15, consoleWindowWidth), (currentCoordinateY - 33) * -1);
                    }
                    Console.ForegroundColor = charColors[determineColor];
                    Console.Write(choiceOfCharacter[determineCharacter]);
                }
                StopWriting();
                Thread.Sleep(100);
            }
        }

        #endregion

#region Functions: Multiplayer

        // PLAY_A_MULTIPLAYER_GAME
        /// <summary>
        /// Appelle toutes les fonctions nécessaires pour jouer une partie multijoueur, 
        /// et gère une partie des options d'affichage durant la partie.
        /// </summary>
        void PlayAMultiplayerGame()
        {
            int firstPlayerLivesRemaining = 3;
            int secondPlayerLivesRemaining = 3;
            int totalWordsFound = 0;

            string firstPlayerLoseMessage = "First player loses";
            string secondPlayerLoseMessage = "Second player loses";

            // true = Word Found
            // false = Word Missed
            bool currentWordRoundResult = true;

            // PLACEHOLDERTEXT_Here I would have displayed a visually interesting thread
            // similar to the one in Single Player mode, but unfortunately I ran out of time.

            // Function stays inside of this loop until one of the two players loses.
            while (true)
            {
                Console.Clear();
                DisplayFooterBox();
                DisplayStatisticsMultiplayer(firstPlayerLivesRemaining, secondPlayerLivesRemaining, totalWordsFound);

                // This statement is entered if the first player loses all of his lives.
                if (firstPlayerLivesRemaining == 0)
                {
                    // PLACEHOLDER
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(consoleWindowWidth / 2 - firstPlayerLoseMessage.Length/2, consoleWindowHeight / 2);
                    Console.Write(firstPlayerLoseMessage);
                    Thread.Sleep(3500);
                    return;
                }

                // This statement is entered if the second player loses all of his lives.
                else if (secondPlayerLivesRemaining == 0)
                {
                    // PLACEHOLDER
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(consoleWindowWidth / 2 - secondPlayerLoseMessage.Length/2, consoleWindowHeight / 2);
                    Console.Write(secondPlayerLoseMessage);
                    Thread.Sleep(3500);
                    return;
                }

                currentWordRoundResult = PlayAWord();
                Console.Clear();

                // This statement is entered if the current word was guessed correctly. The
                // "totalWordsFound" variable belongs to both players (no points are awarded individually
                // for guessed words).
                if (currentWordRoundResult == true)
                {
                    totalWordsFound = totalWordsFound + 1;
                    if (playerOneOrPlayerTwo == 1)
                    {
                        playerOneOrPlayerTwo = 2;
                    }
                    else if (playerOneOrPlayerTwo == 2)
                    {
                        playerOneOrPlayerTwo = 1;
                    }
                    PlaySoundFound();
                }

                // If the current word was no guessed in the specified amount of attempts, this statement
                // is entered and the active player loses a life.
                else
                {
                    if (playerOneOrPlayerTwo == 1)
                    {
                        playerOneOrPlayerTwo = 2;
                        secondPlayerLivesRemaining = secondPlayerLivesRemaining - 1;
                    }
                    else if (playerOneOrPlayerTwo == 2)
                    {
                        playerOneOrPlayerTwo = 1;
                        firstPlayerLivesRemaining = firstPlayerLivesRemaining - 1;
                    }
                    PlaySoundMissed();
                }
            }
        }

        // DISPLAY_STATISTICS_MULTIPLAYER
        /// <summary>
        /// Affiche les statistiques de la partie et les informations du projet à 
        /// l'intérieur du pied de page.
        /// </summary>
        /// 
        /// <param name="firstPlayerLivesRemaining">
        /// Représente les vies restantes du premier joueur.
        /// </param>
        /// 
        /// <param name="secondPlayerLivesRemaining">
        /// Représente les vies restantes du deuxième joueur.
        /// </param>
        /// 
        /// <param name="totalWordsFound">
        /// Représente le nombre de mots trouvés au total.
        /// </param>
        void DisplayStatisticsMultiplayer(int firstPlayerLivesRemaining, int secondPlayerLivesRemaining, int totalWordsFound)
        {
            string wordsFoundString = ("Total words found: ");
            string createdByText = "Created by Streetlamp";
            string createdForText = "for the class \"Programmation de Jeux Vidéo 1\"";
            StartWriting();
            Thread.Sleep(8);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(4, 35);
            Console.Write(firstPlayerName);
            Console.SetCursorPosition(4, 36);
            Console.Write("Lives remaining {0}", firstPlayerLivesRemaining);

            Console.SetCursorPosition(consoleWindowWidth / 2 - wordsFoundString.Length / 2, 35);
            Console.Write(wordsFoundString + totalWordsFound);
            Console.SetCursorPosition(consoleWindowWidth / 2 - createdByText.Length / 2, 36);
            Console.Write(createdByText);
            Console.SetCursorPosition(consoleWindowWidth / 2 - createdForText.Length / 2, 37);
            Console.Write(createdForText);

            Console.SetCursorPosition(consoleWindowWidth - 21, 35);
            Console.Write(secondPlayerName);
            Console.SetCursorPosition(consoleWindowWidth - 21, 36);
            Console.Write("Lives remaining {0}", secondPlayerLivesRemaining);
            StopWriting();
        }

        // MULTIPLAYER_CHOOSE_NAMES
        /// <summary>
        /// Demande aux deux joueurs de saisir leurs noms d'utilisateur.
        /// </summary>
        void MultiplayerChooseNames()
        {
            Random rnd = new Random();
            string promptFirstPlayerName = "Player one type your name then press enter:";
            string promptSecondPlayerName = "Player two type your name then press enter:";
            int typingSpeed = 0;
            int minimumTypingSpeed = 20;
            int maximumTypingSpeed = 45;
            int count = 0;
            Thread.Sleep(1150);

            Console.CursorVisible = true;
            // This loop simulates "typing" on the screen.
            foreach (char currentChar in promptFirstPlayerName)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(consoleWindowWidth / 2 - promptFirstPlayerName.Length + count, consoleWindowHeight / 2);
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
                count = count + 1;
            }
            Console.SetCursorPosition(consoleWindowWidth / 2 + 1, consoleWindowHeight / 2);
            firstPlayerName = Console.ReadLine();
            // PLACEHOLDERTEXT_I was going to include length validation for the names of the players
            // so they wouldn't surpass 16 characters, but I ran out of time to do this.

            count = 0;
            Thread.Sleep(120);
            Console.Clear();

            // This loop does the same as the one above (simulates "typing" on the screen).
            foreach (char currentChar in promptSecondPlayerName)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(consoleWindowWidth / 2 - promptSecondPlayerName.Length + count, consoleWindowHeight / 2);
                Console.Write(currentChar);
                typingSpeed = rnd.Next(minimumTypingSpeed, maximumTypingSpeed);
                Thread.Sleep(typingSpeed);
                count = count + 1;
            }
            Console.SetCursorPosition(consoleWindowWidth / 2 + 1, consoleWindowHeight / 2);
            secondPlayerName = Console.ReadLine();
            // PLACEHOLDERTEXT_Same as above.

            Console.CursorVisible = false;
        }

        // MULTIPLAYER_CHOOSE_WORD
        /// <summary>
        /// Demande au joueur actif de saisir le mot courant qui sera deviné par 
        /// l'autre joueur.
        /// </summary>
        /// 
        /// <returns>
        /// Retourne le mot courant qui devra être deviné.
        /// </returns>
        string MultiplayerChooseWord()
        {
            string messageToFirstPlayer = ", it is your turn to write a word to be guessed: ";
            string messageToSecondPlayer = ", it is your turn to write a word to be guessed: ";
            string wordToBeGuessed = "";
            StartWriting();
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Green;

            // If player one is the active player, the function enters here and asks 
            // player one to enter the word to be guessed.
            if (playerOneOrPlayerTwo == 1)
            {
                Console.SetCursorPosition(consoleWindowWidth / 2 - messageToFirstPlayer.Length - firstPlayerName.Length, consoleWindowHeight / 2);
                Console.Write(firstPlayerName + messageToFirstPlayer);
            }

            // If player two is the active player, the function enters here and asks 
            // player two to enter the word to be guessed.
            else
            {
                Console.SetCursorPosition(consoleWindowWidth / 2 - messageToSecondPlayer.Length - secondPlayerName.Length, consoleWindowHeight / 2);
                Console.Write(secondPlayerName + messageToSecondPlayer);
            }
            Console.SetCursorPosition(consoleWindowWidth / 2, consoleWindowHeight / 2);
            wordToBeGuessed = Console.ReadLine();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, consoleWindowHeight / 2);

            // "Erases" the whole line by replacing it with "space" characters.
            for (int count = 0; count <= consoleWindowWidth; count++)
            {
                Console.Write(" ");
            }
            wordToBeGuessed = wordToBeGuessed.ToUpper();
            StopWriting();
            return wordToBeGuessed;
        }

        #endregion

#region Functions: Global functions

        // PLAY_SPLASH_SCREEN_AND_MAIN_MENU_MUSIC
        /// <summary>
        /// Enchaine les trois différentes compositions qui jouent pendant que 
        /// l’état du jeu est celui de l’écran principal ou du menu principal. 
        /// La deuxième chanson joue en boucle jusqu'au moment ou l’état du jeu 
        /// n’est plus valide, et une fois terminer la troisième chanson brève est jouer une fois.
        /// </summary>
        static void PlaySplashScreenAndMainMenuMusic()
        {
            SoundPlayer splashScreenSoundPlayer = new SoundPlayer();

            List<Stream> splashScreenSoundStream = new List<Stream>();


            splashScreenSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\SplashScreenSounds\\DS_start.wav")));
            splashScreenSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\SplashScreenSounds\\DS_mainLoop.wav")));
            splashScreenSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\SplashScreenSounds\\DS_end.wav")));
            splashScreenSoundPlayer.Stream = splashScreenSoundStream[0];
            splashScreenSoundPlayer.PlaySync();

            // Loops the second (main) track until the game state changes.
            while (gameStateVariable == 5 || gameStateVariable == 4)
            {
                splashScreenSoundPlayer.Stream = splashScreenSoundStream[1];
                splashScreenSoundPlayer.PlaySync();
            }
            splashScreenSoundPlayer.Stream = splashScreenSoundStream[2];
            splashScreenSoundPlayer.PlaySync();
        }

        // PLAY_SOUND_FOUND
        /// <summary>
        /// Émets un son choisi aléatoirement parmi cinq options chaque fois qu'un mot est trouvé.
        /// </summary>
        void PlaySoundFound()
        {
            Random rnd = new Random();
            SoundPlayer foundWordSoundPlayer = new SoundPlayer();

            List<Stream> foundWordSoundStream = new List<Stream>();
            foundWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\FoundWordSounds\\WordFound_1.wav")));
            foundWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\FoundWordSounds\\WordFound_2.wav")));
            foundWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\FoundWordSounds\\WordFound_3.wav")));
            foundWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\FoundWordSounds\\WordFound_4.wav")));
            foundWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\FoundWordSounds\\WordFound_5.wav")));
            foundWordSoundPlayer.Stream = foundWordSoundStream[rnd.Next(0, 4)];
            foundWordSoundPlayer.Play();
        }

        // PLAY_SOUND_MISSED
        /// <summary>
        /// Émets un son choisi aléatoirement parmi cinq options chaque fois qu'un mot est manqué.
        /// </summary>
        void PlaySoundMissed()
        {
            Random rnd = new Random();
            SoundPlayer missedWordSoundPlayer = new SoundPlayer();

            List<Stream> missedWordSoundStream = new List<Stream>();
            missedWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\MissedWordSounds\\WordMissed_1.wav.")));
            missedWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\MissedWordSounds\\WordMissed_2.wav.")));
            missedWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\MissedWordSounds\\WordMissed_3.wav.")));
            missedWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\MissedWordSounds\\WordMissed_4.wav.")));
            missedWordSoundStream.Add(File.OpenRead(Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory + "\\MissedWordSounds\\WordMissed_5.wav.")));
            missedWordSoundPlayer.Stream = missedWordSoundStream[rnd.Next(0, 4)];
            missedWordSoundPlayer.Play();
        }

        // PLAY_A_WORD
        /// <summary>
        /// Fais appel à toutes les fonctions nécessaires pour jouer un mot, et détermine l'état 
        /// du mot courant.
        /// </summary>
        /// 
        /// <returns>
        /// Retourne "Vrai" si le mot a été trouvé, et "Faux" si tout les essaie ont été manqués.
        /// </returns>
        bool PlayAWord()
        {
            string currentWordToFind = "";
            string currentWordState = "";
            char currentUserCharInput = ' ';
            string userInputConvertedToString = "";
            bool validateCharacterIsInCurrentWord = true;
            int currentWordFailedFindAttempts = 0;

            // If the game state is "Single Player", the word to find is generated randomly.
            if (gameStateVariable == 1)
            {
                currentWordToFind = DetermineRandomWord();
            }

            // If the game state is "Multiplayer", the word to find is written by a player.
            else if (gameStateVariable == 2)
            {
                currentWordToFind = MultiplayerChooseWord();
            }
            currentWordState = HideWord(currentWordToFind);

            // Function stays inside of this loop until either the word is found, or the player
            // loses all chances to find the word.
            while (true)
            {

                // This "if" checks to see if the word has been found completely.
                if (currentWordToFind == currentWordState)
                {
                    // PLACEHOLDERTEXT_Was going to display "WordFoundVictoryMessage" here, but ran out of time.
                    return true;
                }
                DisplayHiddenWord(currentWordFailedFindAttempts, currentWordState);
                currentUserCharInput = ReadCharacterAToZ();
                userInputConvertedToString = currentUserCharInput.ToString();
                validateCharacterIsInCurrentWord = currentWordToFind.Contains(userInputConvertedToString);

                // Function enters here if the inputted character is present in the word to find.
                if (validateCharacterIsInCurrentWord == true)
                {
                    string buildWordToDisplay = "";
                    // This loop "constructs" the updated "current state" of the word one character at a time.
                    for (int count = 0; count < currentWordToFind.Length; count++)
                    {
                        if (currentWordToFind[count] == currentUserCharInput)
                        {
                            buildWordToDisplay = buildWordToDisplay + userInputConvertedToString;
                        }
                        else if (currentWordToFind[count] == currentWordState[count])
                        {
                            buildWordToDisplay = buildWordToDisplay + currentWordToFind[count];
                        }
                        else
                        {
                            buildWordToDisplay = buildWordToDisplay + "#";
                        }
                    }
                    currentWordState = buildWordToDisplay;
                }

                // If the inputted character was not present in the word to find, +1 gets added to the failed
                // attempts, then "false" is returned for the function if max failed attempts have been reached.
                else
                {
                    currentWordFailedFindAttempts = currentWordFailedFindAttempts + 1;
                    if (currentWordFailedFindAttempts > 2)
                    {
                        return false;
                    }
                }
            }

        }

        // HIDE_WORD
        /// <summary>
        /// Cache chaque caractère du mot à trouver avec des "#".
        /// </summary>
        /// 
        /// <param name="currentWordToFind">
        /// Représente le mot qui doit être trouvé.
        /// </param>
        /// 
        /// <returns>
        /// Retourne une chaine de caractère de la même longueur que le mot à trouver et chaque 
        /// caractère est un "#".
        /// </returns>
        string HideWord(string currentWordToFind)
        {
            return new string('#', currentWordToFind.Length);
        }

        // DISPLAY_CURRENT_WORD_STATE
        /// <summary>
        /// Affiche l'état du mot courant et change sa position sur l'écran tout dépendant du 
        /// nombre de vies restant au joueur. 
        /// </summary>
        /// 
        /// <param name="currentWordFailedFindAttempts">
        /// Représente le nombre de fois que le joueur a deviné un mauvais caractère.
        /// </param>
        /// 
        /// <param name="currentWordState">
        /// Représente la variable qui affiche le mot caché avec tous les caractères devinés 
        /// affichés.
        /// </param>
        void DisplayHiddenWord(int currentWordFailedFindAttempts, string currentWordState)
        {
            string coverOldLine = "";
            // Creates a string of empty spaces that is the same length as the displayed word.
            for (int count = 0; count < currentWordState.Length; count++)
            {
                coverOldLine = coverOldLine + " ";
            }

            // Displays the word at the its highest point on the screen.
            if (currentWordFailedFindAttempts == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(17, 20);
                Console.Write(coverOldLine);
                Console.SetCursorPosition(17, 30);
                Console.Write(coverOldLine);
                Console.SetCursorPosition(17, 10);
                Console.Write(currentWordState);
            }

            // Displays the word at its mid-point on the screen.
            else if (currentWordFailedFindAttempts == 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(17, 10);
                Console.Write(coverOldLine);
                Console.SetCursorPosition(17, 20);
                Console.Write(currentWordState);
            }

            // Displays the word at its lowest point on the screen (last chance to guess correctly).
            else if (currentWordFailedFindAttempts == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(17, 20);
                Console.Write(coverOldLine);
                Console.SetCursorPosition(17, 30);
                Console.Write(currentWordState);
            }
        }

        // READ_CHARACTER
        /// <summary>
        /// Méthode qui permet de saisir un caractère à l'écran sans interrompre et rentrer 
        /// en conflit avec les "threads visuelles".
        /// </summary>
        /// 
        /// <returns>
        /// Retourne le caractère saisi par l'utilisateur en majuscule.
        /// </returns>
        char ReadCharacterAToZ()
        {
            // When the main thread arrives here, it will cycle indefinitely until the user 
            // presses a key. When the user presses a key, the loop will wait until the cursor
            // is not being used by another function, then it will enter the if statement and 
            // return the key inputted by the user.
            while (true)
            {
                if (cursorUsable && Console.KeyAvailable)
                {
                    StartWriting();
                    Thread.Sleep(8);
                    char userInput = Console.ReadKey(true).KeyChar.ToString().ToUpper()[0];
                    StopWriting();
                    return userInput;
                }
            }
        }

        // CHANGE_CONSOLE_TITLE
        /// <summary>
        /// Change le titre de la console. La valeur du nouveau titre dépendra de l'état 
        /// du jeu (la variable static gameStateVariable).
        /// </summary>
        static void ChangeConsoleTitle()
        {
            string[] consoleTitle = new string[] 
            {
                "Ending_Process",
                "Single_Player_Mode",
                "Multiplayer_Mode",
                "Story_Mode",
                "Main_Menu",
                "Title_Screen"
            };
            Console.Title = consoleTitle[gameStateVariable];
        }

        // START_WRITING
        /// <summary>
        /// S'assure que les fonctions n'essayent pas d'écrire en même temps (ne rentre 
        /// pas en conflit l'un avec l'autre) en limitant l'accès d'affichage à une seule 
        /// thread à la fois.
        /// </summary>
        static void StartWriting()
        {
            // Threads loop here (wait their turn) until the cursor is usable.
            while (true)
            {

                if (cursorUsable == true)
                {

                    cursorUsable = false;
                    break;

                }

            }

        }

        // STOP_WRITING
        /// <summary>
        /// Replace l'état du curseur à "utilisable", ce qui permet à la prochaine thread 
        /// d'écrire.
        /// </summary>
        static void StopWriting()
        {
            cursorUsable = true;
        }

        static void FlushKeyBuffer()
        {
            while(Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }

        #endregion

#region Functions: Footer 

        // DISPLAY_FOOTER_BOX
        /// <summary>
        /// Affiche la boite en pied de page en utilisant des ‘#’.
        /// </summary>
        static void DisplayFooterBox()
        {
            StartWriting();
            int footerWidth = 132;
            int footerheight = 4;
            string footerTopBottomLines = "";
            string footercenterLines = "#";
            Console.SetCursorPosition(0, 34);

            // This loop creates the top and bottom lines of the box.
            for (int count = 0; count <= footerWidth; count++)
            {
                footerTopBottomLines = footerTopBottomLines + "#";
            }

            // This loop creates the center lines of the box.
            for (int count = 0; count <= footerWidth - 2; count++)
            {
                footercenterLines = footercenterLines + " ";
            }
            footercenterLines = footercenterLines + "#";
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(footerTopBottomLines);
            
            // This loop displays all of the center lines of the box.
            for (int count = 0; count <= footerheight - 2; count++)
            {
                Console.WriteLine(footercenterLines);
            }
            Console.Write(footerTopBottomLines);
            StopWriting();
        }

        // DISPLAY_SLASH_SCREEN_FOOTER_MESSAGE
        /// <summary>
        /// Affiche le premier message à être affiché dans le pied de page à l’écran de départ.
        /// </summary>
        void DisplaySplashScreenFooterMessage()
        {
            string startMessageToBeDisplayedInsideFooter = "Press any key to start";
            StartWriting();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition((consoleWindowWidth - startMessageToBeDisplayedInsideFooter.Length) / 2, 36);
            Console.Write(startMessageToBeDisplayedInsideFooter);
            StopWriting();
        }

        // DISPLAY_STARTING_FOOTER_MESSAGE
        /// <summary>
        /// Affiche un message en pied de page après que l'utilisateur a appuyé une touche pour continuer.
        /// </summary>
        void DisplayStartingFooterMessage()
        {
            string startingMessage = "Thread_Starting";
            StartWriting();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition((consoleWindowWidth / 2) - (startingMessage.Length / 2), 36);
            Console.Write(startingMessage);
            StopWriting();

            // Progressively writes 3 periods after the starting message.
            for (int count = 0; count < 3; count++)
            {
                Thread.Sleep(650);
                StartWriting();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition((consoleWindowWidth / 2) + (startingMessage.Length / 2) + 1 + count * 2, 36);
                Console.Write(" .");
                StopWriting();
            }
            Thread.Sleep(650);
        }

        #endregion

#region Functions: PlayStoryMode and associated Functions.

        // There was going to be a story mode, but unfortunately I ran out of time.
        // hint: pressing '3' in the Main Menu triggers the "secret story mode" branch of 
        // the game to be executed.

        #endregion

    }
} 