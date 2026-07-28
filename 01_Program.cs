using System;
using System.Linq;
using System.Collections.Generic;
namespace RockPaperScissors;
      
public class Program
{
    public static List<Players> Player = new List<Players>();
    public static Players? CurrentPlayer;
    public static Move PlayerChoice;
    public static Move ChoiceComputer;
    public static Random random = new Random();
    public static void Main(string[] args)
    {
        Player = SaveLoad.LoadData();

        bool again = true;

        while (again)
        {
            Console.Clear();

            Console.WriteLine("-------- 🪨  Rock - 📄  Paper - ✂️  Scissors --------");
            Console.WriteLine(" 1 - ▶️  Play Game");
            Console.WriteLine(" 2 - 👥  Players");
            Console.WriteLine(" 3 - 📊  Statistics");
            Console.WriteLine(" 4 - 🏆  Achievements");
            Console.WriteLine(" 5 - 🗂️  Match History");
            Console.WriteLine(" 6 - 🔄  Reset Data");
            Console.WriteLine(" 7 - ✏️  Rename Current Player");
            Console.WriteLine(" 0 - 🚪  Exit ");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();
            Console.Write("Your Choice : ");
            int choice;
            while(!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Please enter a number !!");
                Console.Write("Your Choice : ");
            }
            switch (choice)
            {
                case 1:
                    GetPlayerName();

                    CurrentPlayer!.MatchNumber++;

                    ChooseGameMode();

                    ShowFinalWinner();
                    SaveLoad.SaveData(Player);

                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                break;

                case 2:
                    Playerss();
                break;

                case 3:
                    Statistics();
                break;

                case 4:
                    Achievements();
                break;

                case 5:
                    MatchHistory();
                break;

                case 6:
                    ResetData();
                break;

                case 7:
                    RenameCurrentPlayer();
                break;

                case 0:
                    SaveLoad.SaveData(Player);
                    again = false;
                break;
                
                default:
                    Console.WriteLine("Invalid selection : Please choose between 0 and 7 !!");
                break;
            }
        }          
    }
    public static void GetPlayerName()
    {
        if(CurrentPlayer != null && !string.IsNullOrWhiteSpace(CurrentPlayer.Name))
        {
            return;
        }
        string name;
        do
        {
            Console.WriteLine("(Type 'list' to see registered players.)");
            Console.Write("Enter your name : ");
            name = Console.ReadLine()!;
            
            if("list".Equals(name, StringComparison.OrdinalIgnoreCase) )
            {
                RegisteredPlayers();
                continue;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("The name must not be empty !!");
            }
        }while(string.IsNullOrWhiteSpace(name));
        
        CurrentPlayer = Player.FirstOrDefault(x => x.Name == name);
        

        if (CurrentPlayer == null)
        {
            CurrentPlayer = new Players();
            CurrentPlayer.Name = name;
            CurrentPlayer.Draws = 0;
            CurrentPlayer.MatchNumber = 0;
            CurrentPlayer.GamesPlayed = 0;
            CurrentPlayer.Losses = 0;
            CurrentPlayer.Wins = 0;
            CurrentPlayer.TotalWins = 0;
            CurrentPlayer.TotalLosses = 0;
            CurrentPlayer.TotalDraws = 0;
            CurrentPlayer.BestWinStreak = 0;
            CurrentPlayer.CurrentWinStreak = 0;
            CurrentPlayer.RockCount = 0;
            CurrentPlayer.PaperCount = 0;
            CurrentPlayer.ScissorsCount = 0;
            Player.Add(CurrentPlayer);
            Console.WriteLine("No profile found.");
            Console.WriteLine("A new profile has been created.");
        }
        else
        {
            SameName(name);
        }
    }
    public static void SameName(string Same)
    {
        Console.WriteLine($"The name '{Same}' already exists ");

        var names = Player.FindAll(x => x.Name == Same);
        
        int number = 1;
        foreach (var find in names)
        {
            double winRate;

            if (find.GamesPlayed == 0)
            {
                winRate = 0;
            }
            else
            {
                winRate = (double)find.TotalWins / (find.TotalDraws + find.TotalLosses + find.TotalWins) * 100;
            }

            Console.WriteLine($"{number}. {find.Name} ({find.GamesPlayed} games , {winRate:F2}% win rate )");
            number++;
        }
        Console.WriteLine($"{number}. Back");
        
        Console.Write("Your Choice : ");
        int choice;
        while(!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > number)
        {
            Console.WriteLine($"Please enter a number between 1 and {number} !!");
            Console.Write("Your Choice : ");
        }

        if (choice == number)
        {
            return;
        }
        Console.WriteLine($"Welcome back '{Same}'  😊");
        CurrentPlayer = names[choice - 1];

    }
    public static void RegisteredPlayers()
    {
        Console.WriteLine("-------- Registered Players --------");
        
        if (Player.Count == 0)
        {
            Console.WriteLine("There are no registered players at the moment.");
            return;
        }
        
        int page = 1;
        const int pageSize = 10;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("-------- Registered Players --------");
            Console.WriteLine($"Total Players : {Player.Count}");

            var playersOnPage = Player
                .Skip((page-1) * pageSize)
                .Take(pageSize);
            
            Console.WriteLine($"Showing {(page-1) * pageSize + 1} - {Math.Min(page * pageSize, Player.Count)}");
            Console.WriteLine();

            int number = (page-1) * pageSize + 1;

            foreach (var player in playersOnPage)
            {
                Console.WriteLine($"{number}. {player.Name}  ({player.GamesPlayed} games)");
                number++;
            }

            Console.WriteLine();
            string choice;
            do
            {
                Console.Write(" 'N' -> Next Page   'P' -> Previous Page   'B' -> Back : ");
                choice = Console.ReadLine()!.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(choice))
                {
                    Console.WriteLine("Please enter a letter !!");
                    continue;
                }
                
                if(!(choice == "N" || choice == "P" || choice == "B"))
                {
                    Console.WriteLine("Invalid selection :  Please enter 'N', 'P', or 'B' !!");
                }
            }while(string.IsNullOrWhiteSpace(choice) || !(choice == "N" || choice == "P" || choice == "B"));

            if(choice == "N")
            {
                if(page * pageSize < Player.Count)
                    page++;
                else
                Console.WriteLine("You are already on the last page !!");
                Console.ReadKey();
            }
            else if(choice == "P")
            {
                if(page > 1)
                    page--;
                else
                Console.WriteLine("You're already on the first page !!");
                Console.ReadKey();
            }
            else if(choice == "B")
                break; 
        }
    }
    public static void ChooseGameMode()
    {
        while (true)
        {
            Console.WriteLine("-------- 🪨  Rock - 📄  Paper - ✂️  Scissors --------");
            Console.WriteLine();
            Console.WriteLine("Choose Game Mode");
            Console.WriteLine("1 - Single Match");
            Console.WriteLine("2 - Best of 3");
            Console.WriteLine("3 - Best of 5");
            Console.WriteLine("4 - Best of 10");
            Console.WriteLine("5 - Endless");
            Console.WriteLine();
            Console.Write("Select : ");
            int select;
            while(!int.TryParse(Console.ReadLine(), out select))
            {
                Console.WriteLine("Please enter a number !!");
                Console.Write("Select : ");
            }
            switch (select)
            {
                case 1:
                    CurrentPlayer!.Wins = 0;
                    CurrentPlayer!.Draws = 0;
                    CurrentPlayer!.Losses = 0;
                    CurrentPlayer.MatchHistory.Add($"===== Match {CurrentPlayer.MatchNumber} =====");
                    Round(1);
                    return;

                case 2:
                    CurrentPlayer!.Wins = 0;
                    CurrentPlayer!.Draws = 0;
                    CurrentPlayer!.Losses = 0;
                    CurrentPlayer.MatchHistory.Add($"===== Match {CurrentPlayer.MatchNumber} =====");
                    Round(3);
                    return;
            
                case 3:
                    CurrentPlayer!.Wins = 0;
                    CurrentPlayer!.Draws = 0;
                    CurrentPlayer!.Losses = 0;
                    CurrentPlayer.MatchHistory.Add($"===== Match {CurrentPlayer.MatchNumber} =====");
                    Round(5);
                    return;

                case 4:
                    CurrentPlayer!.Wins = 0;
                    CurrentPlayer!.Draws = 0;
                    CurrentPlayer!.Losses = 0;
                    CurrentPlayer.MatchHistory.Add($"===== Match {CurrentPlayer.MatchNumber} =====");
                    Round(10);
                    return;
            
                case 5:
                    CurrentPlayer!.Wins = 0;
                    CurrentPlayer!.Draws = 0;
                    CurrentPlayer!.Losses = 0;
                    CurrentPlayer.MatchHistory.Add($"===== Match {CurrentPlayer.MatchNumber} =====");
                    int round = 1;
                    while (true)
                    {
                        Console.WriteLine($"{round}. Round ");

                        if (!GetPlayerChoice(5))
                        {
                            CurrentPlayer!.GamesPlayed += round - 1;
                            break;
                        }
                        GetComputerChoice();
                        DetermineWinner(round);
                        round++;
                    }
                    return;

                default:
                    Console.WriteLine("Invalid selection : Please choose between 1 and 5 !!");
                break;
            }
        } 
    }
    public static void Round(int round)
    {
        for (int i = 1 ; i <= round; i++)
        {
            Console.WriteLine($"{i}. Round ");
            if (!GetPlayerChoice(1))
            {
                return;
            }
            GetComputerChoice();
            DetermineWinner(i);
        }
        CurrentPlayer!.GamesPlayed += round;
    }
    public static void ShowAchievement(string name, int current, int target)
    {
        if(current >= target)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"🏆  {name,-30}   ✔ UNLOCKED");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"🔒  {name,-30} ({target-current} more to unlock)");
        }
        Console.ResetColor();
    }
    public static void Achievements()
    {
        Console.Clear();
        Console.WriteLine("---------- 🏆  Achievements  🏆 ----------");
        
        if (CurrentPlayer == null)
        {
            Console.WriteLine("Please select a player first.");
            Console.ReadKey();
            return;
        }

        ShowAchievement("First Victory", CurrentPlayer!.TotalWins, 1);
        ShowAchievement("Winner  🥈  (Win 10 games)", CurrentPlayer!.TotalWins, 10);
        ShowAchievement("Champion  🎖️  (Win 50 games)", CurrentPlayer!.TotalWins, 50);
        ShowAchievement("Master of RPS  👑  (Win 150 games)", CurrentPlayer!.TotalWins, 150);
        ShowAchievement("Getting Started  🥈  (Play 10 Games)", CurrentPlayer.GamesPlayed, 10);
        ShowAchievement("Experienced Player  🥇  (Play 50 Games)", CurrentPlayer.GamesPlayed, 50);
        ShowAchievement("Veteran  💎  (Play 100 Games)", CurrentPlayer.GamesPlayed, 100);
        ShowAchievement("Legend  👑  (Play 500 Games)", CurrentPlayer.GamesPlayed, 500);
        ShowAchievement("Hot Streak  🔥  (5 Win Streak)", CurrentPlayer.BestWinStreak, 5);
        ShowAchievement("Unbreakable  🔥  (10 Win Streak)", CurrentPlayer.BestWinStreak, 10);
        ShowAchievement("Unstoppable  👑  (Win 20 games without losing)", CurrentPlayer.BestWinStreak, 20);
        ShowAchievement("First Defeat  💀", CurrentPlayer!.TotalLosses, 1);
        ShowAchievement("Never Give Up  😂", CurrentPlayer!.TotalLosses, 10);
        ShowAchievement("Comeback Stronger  💪", CurrentPlayer!.TotalLosses, 50);
        ShowAchievement("Scissors Master  ✂️  (Use Scissors 100 times)", CurrentPlayer.ScissorsCount, 100);
        ShowAchievement("Paper Master  📄  (Use Paper 100 times)", CurrentPlayer.PaperCount, 100);
        ShowAchievement("Rock Master  🪨  (Use Rock 100 times)", CurrentPlayer.RockCount, 100);
        Console.ReadKey();
    }
    public enum Move
    {
        Rock = 1,
        Paper,
        Scissors
    }
    public static bool GetPlayerChoice(int number)
    {
        if(number == 5)
        {
            Console.WriteLine("Note : It will game continue unless you write 'stop'.");
        }
        Console.WriteLine("-------- 🌟 Let The Game Begin 🌟 --------");
        Console.WriteLine(" 🪨  Rock ");
        Console.WriteLine(" 📄  Paper ");
        Console.WriteLine(" ✂️  Scissors ");
        Console.WriteLine();
        string choice;
        do
        {
            Console.Write("Choose your move : ");
            choice = Console.ReadLine()!.Trim().ToUpperInvariant();
            
            if (string.IsNullOrWhiteSpace(choice))
            {
                Console.WriteLine("Please enter a word !!");
                continue;
            }
            
            if(!(choice == "ROCK" || choice == "PAPER" || choice == "SCISSORS" || choice == "STOP"))
            {
                if(number == 5)
                {
                    Console.WriteLine("Invalid selection :  Please enter 'Rock', 'Paper', 'Scissors' or 'Stop' !!");
                }
                else
                {
                    Console.WriteLine("Invalid selection :  Please enter 'Rock', 'Paper' or 'Scissors' !!");
                }
            }
        }while(string.IsNullOrWhiteSpace(choice) || !(choice == "ROCK" || choice == "PAPER" || choice == "SCISSORS" || choice == "STOP"));

        if(choice == "ROCK")
        {
            PlayerChoice = Move.Rock;
            return true;
        }
        else if(choice == "PAPER")
        {
            PlayerChoice = Move.Paper;
            return true;
        }
        else if(choice == "SCISSORS")
        {
            PlayerChoice = Move.Scissors;
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void GetComputerChoice()
    {
        ChoiceComputer = (Move)random.Next(1,4);
    }
    public static void DetermineWinner(int round)
    {
        Dictionary <Move, Move> beats = new Dictionary <Move, Move>()
        {
            {Move.Rock, Move.Scissors},
            {Move.Paper, Move.Rock},
            {Move.Scissors, Move.Paper}
        };

        string result;

        if(PlayerChoice == ChoiceComputer)
        {
            ShowPlayerChoices();
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("🤝  It's a Draw !");
            Console.ResetColor();

            result = "Draw";

            CurrentPlayer!.Draws++;
            CurrentPlayer!.TotalDraws++;
            Console.WriteLine($"Wins : {CurrentPlayer.Wins}");
            Console.WriteLine($"Losses : {CurrentPlayer.Losses}");
            Console.ReadKey();
        }
        else if(beats[PlayerChoice] == ChoiceComputer)
        {
            ShowPlayerChoices();
            Messages(true);

            result = "Win";

            CurrentPlayer!.Wins++;
            CurrentPlayer!.TotalWins++;
            CurrentPlayer!.CurrentWinStreak++;
            if(CurrentPlayer.CurrentWinStreak > CurrentPlayer.BestWinStreak)
            {
                CurrentPlayer.BestWinStreak = CurrentPlayer.CurrentWinStreak;
            }
            Console.WriteLine($"Wins : {CurrentPlayer.Wins}");
            Console.WriteLine($"Losses : {CurrentPlayer.Losses}");
            Console.ReadKey();
        }
        else
        {
            ShowPlayerChoices();
            Messages(false);
            
            result = "Lose";

            CurrentPlayer!.Losses++;
            CurrentPlayer!.TotalLosses++;
            CurrentPlayer!.CurrentWinStreak = 0;
            Console.WriteLine($"Wins : {CurrentPlayer.Wins}");
            Console.WriteLine($"Losses : {CurrentPlayer.Losses}");
            Console.ReadKey();
        }
        CurrentPlayer.MatchHistory.Add($"Match {CurrentPlayer.MatchNumber} | Round {round} | {GetMoveEmoji(PlayerChoice)} vs {GetMoveEmoji(ChoiceComputer)} -> {result}");
    }
    public static string GetMoveEmoji(Move move)
    {
        if (move == Move.Rock)
        return "🪨  Rock";

        if (move == Move.Paper)
        return "📄  Paper";

        return "✂️  Scissors";
    }
    public static void Messages(bool isWin)
    {
        string[] winningMessages =
        {
            "🎉  You Win !",
            "🥳  Great Job !",
            "🌟  Excellent !",
            "👏  Nice Move !",
            "🔥  Amazing !",
            "💪  Well Played !",
            "🎊  Congratulations !"
        };
        string[] lossMessages =
        {
            "😔  You Lose !",
            "😢  Better luck next time !",
            "💔  Defeat !",
            "😅  The Computer Wins !",
            "🤖  Computer Wins !",
            "📉  You Lost This Round !",
            "🙃  Try Again !",
            "💪  Don't Give Up !"
        };

        if (isWin)
        {
            Console.WriteLine(winningMessages[random.Next(winningMessages.Length)]);
        }
        else
        {
            Console.WriteLine(lossMessages[random.Next(lossMessages.Length)]);
        }
    }
    public static void ShowPlayerChoices()
    {
        if(PlayerChoice == Move.Rock)
        {
            Console.WriteLine("You choose : 🪨  Rock");
            CurrentPlayer!.RockCount++;
            ShowComputerChoices();
        }
        else if(PlayerChoice == Move.Paper)
        {
            Console.WriteLine("You choose : 📄  Paper");
            CurrentPlayer!.PaperCount++;
            ShowComputerChoices();
        }
        else
        {
            Console.WriteLine("You choose : ✂️  Scissors");
            CurrentPlayer!.ScissorsCount++;
            ShowComputerChoices();
        }
    }
    public static void ShowComputerChoices()
    {
        if(ChoiceComputer == Move.Rock)
        {
            Console.WriteLine("Computer choose : 🪨  Rock");
        }
        else if(ChoiceComputer == Move.Paper)
        {
            Console.WriteLine("Computer choose : 📄  Paper");
        }
        else
        {
            Console.WriteLine("Computer choose : ✂️  Scissors");
        }
    }
    public static void ShowFinalWinner()
    {
        int total = CurrentPlayer!.Wins + CurrentPlayer!.Draws + CurrentPlayer!.Losses;
        if(total == 0)
        {
            Console.WriteLine("No rounds were played.");
            return;
        }
        
        double rate = (double)CurrentPlayer!.Wins / total * 100;
        
        Console.WriteLine($"------- Game Result -------");
        Console.WriteLine($"Win Rate : %{rate:F1}");
        if(CurrentPlayer!.Wins > CurrentPlayer!.Losses)
        {
            Console.WriteLine($"🏆  Victory !!!");
        }
        else if(CurrentPlayer!.Wins < CurrentPlayer!.Losses)
        {
            Console.WriteLine($"💀 ! Game Over ! 💀");
        }
        else
        {
            Console.WriteLine($"⚖️  It's a Draw !");
        }
    }
    public static void Playerss()
    {
      while (true)
      {
        Console.WriteLine("-------- 👥  Players --------");
        Console.WriteLine("1 - View Players");
        Console.WriteLine("2 - Delete Player");
        Console.WriteLine("3 - Rename Player");
        Console.WriteLine("0 - Back");
        Console.WriteLine("-------------------------");
        Console.WriteLine("Your Choice : ");
        int choice;
        while(!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("Please enter a number !!");
            Console.Write("Your Choice : ");
        }
        switch (choice)
        {
            case 1:
                RegisteredPlayers();
            break;
           
            case 2:
                DeletePlayer();
            break;

            case 3:
                RenamePlayer();
            break;
            
            case 0:
                return;

            default:
                Console.WriteLine("Invalid selection : Please choose between 0 and 3 !!");
            break;
        }
      }
    }
    public static void DeletePlayer()
    {
        Console.WriteLine("--------- Delete Player ---------");
        
        if (Player.Count == 0)
        {
            Console.WriteLine("There are no players to delete..");
            return;
        }

        int number = 1;
        foreach (var player in Player)
        {
            Console.WriteLine($"{number}. {player.Name}  ({player.GamesPlayed} games)");
            number++;
        }

        Console.Write("Enter the player number to be deleted : ");
        int choice;
        while(!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice >= number)
        {
            Console.WriteLine("Invalid player number !!");
            Console.Write("Enter the player number to be deleted : ");
        }

        Players selectedPlayer = Player[choice - 1];

        Console.Write($"Are you sure you want to delete '{selectedPlayer.Name}'? (Y/N) : ");

        string answer = Console.ReadLine()!.Trim().ToUpper();

        while (answer != "Y" && answer != "N")
        {
            Console.Write("Please enter Y or N : ");
            answer = Console.ReadLine()!.Trim().ToUpper();
        }
        if (answer == "Y")
        {
            Player.Remove(selectedPlayer);
            SaveLoad.SaveData(Player);

            if (CurrentPlayer == selectedPlayer)
            {
                CurrentPlayer = null;
            }

            Console.WriteLine("Player deleted successfully.");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }
    public static void RenamePlayer()
    {
        Console.WriteLine("--------- Rename Player ---------");
        
        if (Player.Count == 0)
        {
            Console.WriteLine("There are no players to rename..");
            Console.ReadKey();
            return;
        }

        int number = 1;
        foreach (var player in Player)
        {
            Console.WriteLine($"{number}. {player.Name}  ({player.GamesPlayed} games)");
            number++;
        }

        Console.Write("Enter the player number to be renamed : ");
        int choice;
        while(!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice >= number)
        {
            Console.WriteLine("Invalid player number !!");
            Console.Write("Enter the player number to be renamed : ");
        }

        Players renamePlayer = Player[choice - 1];
        
        Console.WriteLine($"Current name : {renamePlayer.Name}");

        while (true)
        {
            string newName;
            do
            {
                Console.Write("Enter the new name : ");
                newName = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("The new name cannot be empty !!");
                }

            }while(string.IsNullOrWhiteSpace(newName));

            if (Player.Any(x => x.Name == newName))
            {
                Console.WriteLine("This name is already in use.");
                continue;
            }
            else
            {
                renamePlayer.Name = newName;
                SaveLoad.SaveData(Player);
                Console.WriteLine("Player renamed successfully.");
                return;
            }
        } 
    }
    public static void Statistics()
    {
        if (CurrentPlayer == null)
        {
            Console.WriteLine("Start a game first to create or load a player profile.");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("----------- 📊 Statistics -----------");
        Console.WriteLine($"Player Name : {CurrentPlayer!.Name}");
        Console.WriteLine($"Total Games : {CurrentPlayer.GamesPlayed}");
        Console.WriteLine($"Wins : {CurrentPlayer.TotalWins}");
        Console.WriteLine($"Losses : {CurrentPlayer.TotalLosses}");
        Console.WriteLine($"Draws : {CurrentPlayer.TotalDraws}");
        
        double rateWin;
        if(CurrentPlayer.GamesPlayed > 0)
        {
            rateWin = (double)CurrentPlayer.TotalWins / (CurrentPlayer.TotalWins + CurrentPlayer.TotalDraws + CurrentPlayer.TotalLosses) * 100;
        }
        else
        {
            rateWin = 0;
        }
        Console.WriteLine($"Win Rate : {rateWin:F2}%");
        
        if(CurrentPlayer.RockCount > CurrentPlayer.PaperCount && CurrentPlayer.RockCount > CurrentPlayer.ScissorsCount)
        {
                Console.WriteLine("Favorite Choice : 🪨  Rock ");
        }
        else if (CurrentPlayer.PaperCount > CurrentPlayer.ScissorsCount && CurrentPlayer.PaperCount > CurrentPlayer.RockCount)
        {
            Console.WriteLine("Favorite Choice : 📄  Paper ");
        }
        else if(CurrentPlayer.ScissorsCount > CurrentPlayer.PaperCount && CurrentPlayer.ScissorsCount > CurrentPlayer.RockCount)
        {
            Console.WriteLine("Favorite Choice : ✂️  Scissors ");
        }
        else
        {
            Console.WriteLine("Favorite Choice : Not currently available !!");
        }

        Console.WriteLine($"Longest Win Streak : {CurrentPlayer.BestWinStreak}");
        Console.WriteLine($"Current Win Streak : {CurrentPlayer.CurrentWinStreak}");
        Console.ReadKey();
    }
    public static void MatchHistory()
    {
        Console.WriteLine("---------- 🗂️  Match History ----------");

        if (CurrentPlayer == null)
        {
            Console.WriteLine("Start a game first to create or load a player profile.");
            Console.ReadKey();
            return;
        }
        var history = CurrentPlayer!.MatchHistory;
        
        if (history.Count == 0)
        {
            Console.WriteLine("No match history.");
            return;
        }
        int page = 1;
        const int pageSize = 10;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("---------- 🗂️  Match History ----------");
            
            int totalMatches = history.Count(x => x.StartsWith("===== Match"));
            Console.WriteLine($"Total Matches : {totalMatches}");

            var historyOnPage = history
                .Skip((page-1) * pageSize)
                .Take(pageSize);

            Console.WriteLine($"Showing {(page-1) * pageSize + 1} - {Math.Min(page * pageSize, history.Count)} of {history.Count} records");
            Console.WriteLine();

            foreach (var match in historyOnPage)
            {
                Console.WriteLine(match);
            }

            Console.WriteLine();
            string choice;
            do
            {
                Console.Write(" 'N' -> Next Page   'P' -> Previous Page   'B' -> Back : ");
                choice = Console.ReadLine()!.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(choice))
                {
                    Console.WriteLine("Please enter a letter !!");
                    continue;
                }
                
                if(!(choice == "N" || choice == "P" || choice == "B"))
                {
                    Console.WriteLine("Invalid selection :  Please enter 'N', 'P', or 'B' !!");
                }
            }while(string.IsNullOrWhiteSpace(choice) || !(choice == "N" || choice == "P" || choice == "B"));

            if(choice == "N")
            {
                if(page * pageSize < history.Count)
                    page++;
                else
                Console.WriteLine("You are already on the last page !!");
                Console.ReadKey();
            }
            else if(choice == "P")
            {
                if(page > 1)
                    page--;
                else
                Console.WriteLine("You're already on the first page !!");
                Console.ReadKey();
            }
            else if(choice == "B")
                break; 
        }
    }
    public static void ResetData()
    {
        Console.WriteLine("--------- 🔄  Reset Data ---------");
        
        if (CurrentPlayer == null)
        {
            Console.WriteLine("Start a game first to create or load a player profile.");
            Console.ReadKey();
            return;
        }
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("⚠️  Warning !");
        Console.WriteLine($"This action cannot be undone.");
        Console.ResetColor();
        Console.WriteLine("The player's name will be kept.");
        string choice;
        do
        {
           Console.Write("Are you sure? (Y/N):");
           choice = Console.ReadLine()!.ToUpper();

            if(string.IsNullOrWhiteSpace(choice))
            {
                Console.WriteLine("Please do not leave it blank.");
                continue;
            }

            if(choice != "Y" && choice != "N")
            {
                Console.WriteLine("Enter 'Y' or 'N'");
            }
        }while(string.IsNullOrWhiteSpace(choice) || (choice != "Y" && choice != "N"));

        if (choice == "Y")
        {
            CurrentPlayer!.Draws = 0;
            CurrentPlayer.MatchNumber = 0;
            CurrentPlayer.GamesPlayed = 0;
            CurrentPlayer.Losses = 0;
            CurrentPlayer.Wins = 0;
            CurrentPlayer.TotalWins = 0;
            CurrentPlayer.TotalLosses = 0;
            CurrentPlayer.TotalDraws = 0;
            CurrentPlayer.BestWinStreak = 0;
            CurrentPlayer.CurrentWinStreak = 0;
            CurrentPlayer.RockCount = 0;
            CurrentPlayer.PaperCount = 0;
            CurrentPlayer.ScissorsCount = 0;
            CurrentPlayer.MatchHistory.Clear();
            SaveLoad.SaveData(Player);
            Console.WriteLine("✅  Player data has been reset successfully.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("❌  The transaction has been Operation cancelled.");
            Console.ReadKey();
        }
    }
    public static void RenameCurrentPlayer()
    {
        Console.WriteLine("----------- ✏️  Rename Current Player -----------");
        if (CurrentPlayer == null)
        {
            Console.WriteLine("Start a game first to create or load a player profile.");
            Console.ReadKey();
            return;
        }
        while (true)
        {
            string newName;
            do
            {
                Console.Write("Enter the new name : ");
                newName = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("The new name cannot be empty !!");
                }

            }while(string.IsNullOrWhiteSpace(newName));

            if (Player.Any(x => x.Name == newName))
            {
                Console.WriteLine("This name is already in use.");
                continue;
            }
            else
            {
                CurrentPlayer!.Name = newName;
                SaveLoad.SaveData(Player);
                Console.WriteLine("Player renamed successfully.");
                Console.WriteLine($"Current player name changed to '{newName}'.");
                return;
            }
        } 
    }
}