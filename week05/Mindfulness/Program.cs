// Exceeded requirements by:
// 1. No-repeat selection within a session
// 2. Activity logging: each completed activity is appended to a simple log file
//    "mindfulness_log.txt"
// 3. Cleaner spinner and countdown animations implemented as reusable methods.
// 4. More robust input parsing and validation.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MindfulnessProgram
{
    // Base class encapsulating shared behavior
    public abstract class Activity
    {
        private string _name;
        private string _description;
        private int _durationSeconds; // private encapsulated field
        private static readonly string LogFilePath = "mindfulness_log.txt";

        protected Random _random = new Random();

        protected Activity(string name, string description)
        {
            _name = name;
            _description = description;
            _durationSeconds = 0;
        }

        // Public method to run the activity life-cycle
        public void Run()
        {
            Console.Clear();
            ShowStartMessage();
            SetDurationFromUser();
            PrepareToBegin();
            PerformActivity(); // implemented by derived classes
            ShowEndMessage();
            AppendToLog();
            PromptToContinue();
        }

        // Derived classes must implement the activity-specific behavior
        protected abstract void PerformActivity();

        // Show standardized start message
        private void ShowStartMessage()
        {
            Console.WriteLine("======================================");
            Console.WriteLine($"Starting: {_name}");
            Console.WriteLine("======================================");
            Console.WriteLine();
            Console.WriteLine(_description);
            Console.WriteLine();
        }

        // Ask user for duration in seconds; validate input
        private void SetDurationFromUser()
        {
            while (true)
            {
                Console.Write("Enter duration in seconds (e.g., 30): ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out int seconds) && seconds > 0)
                {
                    _durationSeconds = seconds;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer number of seconds.");
                }
            }
        }

        // Show "Get ready" and a short pause
        private void PrepareToBegin()
        {
            Console.WriteLine();
            Console.Write("Get ready");
            ShowSpinner(3); // 3 seconds spinner
            Console.WriteLine();
        }

        // Show standardized end message and include the duration used
        private void ShowEndMessage()
        {
            Console.WriteLine();
            Console.Write("Well done!");
            ShowSpinner(2);
            Console.WriteLine();
            Console.WriteLine($"You have completed the activity: {_name}");
            Console.WriteLine($"Duration: {_durationSeconds} seconds");
            Console.WriteLine();
            Thread.Sleep(1000);
        }

        // Get the duration for derived classes
        protected int GetDuration()
        {
            return _durationSeconds;
        }

        // Utility: show spinner for given seconds
        protected void ShowSpinner(int seconds)
        {
            int totalMs = seconds * 1000;
            int interval = 200; // spinner update interval in ms
            string[] spinnerChars = new[] { "|", "/", "-", "\\" };
            Stopwatch sw = Stopwatch.StartNew();
            int i = 0;
            while (sw.ElapsedMilliseconds < totalMs)
            {
                Console.Write(spinnerChars[i % spinnerChars.Length]);
                Thread.Sleep(interval);
                Console.Write("\b");
                i++;
            }
            sw.Stop();
        }

        // Utility: countdown (shows numbers) for given seconds (digits)
        protected void ShowCountdown(int seconds)
        {
            for (int i = seconds; i >= 1; i--)
            {
                Console.Write(i);
                Thread.Sleep(1000);
                Console.Write("\b \b"); // erase digit (works well for single digits)
            }
        }

        // countdown that prints numbers on their own line
        protected void ShowCountdownLine(int seconds)
        {
            for (int i = seconds; i >= 1; i--)
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }
        }

        // After finishing, append entry to log file (simple CSV-like lines)
        private void AppendToLog()
        {
            try
            {
                string timestamp = DateTime.UtcNow.ToString("o"); 
                string entry = $"{timestamp},\"{_name}\",{_durationSeconds}";
                
                string extra = ProvideLogSummary();
                if (!string.IsNullOrEmpty(extra))
                {
                    entry += $",\"{extra.Replace("\"", "'")}\""; 
                }
                File.AppendAllText(LogFilePath, entry + Environment.NewLine, Encoding.UTF8);
            }
            catch
            {
                
            }
        }

        
        protected virtual string ProvideLogSummary()
        {
            return "";
        }

        // Pause to let the user read and press Enter to continue
        private void PromptToContinue()
        {
            Console.WriteLine("Press Enter to return to the main menu...");
            Console.ReadLine();
        }
    }

    // BreathingActivity: alternates inhale/exhale and uses countdowns/animations
    public class BreathingActivity : Activity
    {
        public BreathingActivity() : base("Breathing Activity",
            "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
        {
        }

        protected override void PerformActivity()
        {
            int totalSeconds = GetDuration();

            Console.WriteLine();
            Console.WriteLine("Start breathing exercises. Follow the prompts below.");
            Console.WriteLine();

            Stopwatch sw = Stopwatch.StartNew();

            
            int inhale = 4;
            int exhale = 6;

            
            if (totalSeconds < 8)
            {
                inhale = Math.Max(1, totalSeconds / 2);
                exhale = totalSeconds - inhale;
            }

            while (sw.Elapsed.TotalSeconds < totalSeconds)
            {
                // Breathe in
                Console.Write("Breathe in... ");
                int remain = totalSeconds - (int)sw.Elapsed.TotalSeconds;
                int breatheTime = Math.Min(inhale, Math.Max(1, remain));
                ShowCountdownWithDots(breatheTime);
                Console.WriteLine();

                if (sw.Elapsed.TotalSeconds >= totalSeconds) break;

                // Breathe out
                Console.Write("Breathe out... ");
                remain = totalSeconds - (int)sw.Elapsed.TotalSeconds;
                breatheTime = Math.Min(exhale, Math.Max(1, remain));
                ShowCountdownWithDots(breatheTime);
                Console.WriteLine();
            }

            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("Breathing session complete.");
        }

        
        private void ShowCountdownWithDots(int seconds)
        {
            for (int i = seconds; i >= 1; i--)
            {
                Console.Write(i);
                for (int d = 0; d < 3; d++)
                {
                    Console.Write('.');
                    Thread.Sleep(250);
                }
              
                int eraseLength = (1 + 3); 
                for (int k = 0; k < eraseLength; k++)
                    Console.Write("\b \b");
            }
        }

        protected override string ProvideLogSummary()
        {
            return "Breathing session completed";
        }
    }

    // ReflectionActivitY
    public class ReflectionActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless.",
            "Think of a time when you showed resilience in the face of a challenge.",
            "Think of a time when you achieved something you are proud of."
        };

        private List<string> _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };

        
        private Queue<string> _promptQueue;
        private Queue<string> _questionQueue;

        public ReflectionActivity() : base("Reflection Activity",
            "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
        {
            _promptQueue = ShuffleToQueue(_prompts);
            _questionQueue = ShuffleToQueue(_questions);
        }

        protected override void PerformActivity()
        {
            int totalSeconds = GetDuration();

            
            string prompt = DequeueAndRefillIfNeeded(_promptQueue, _prompts);
            Console.WriteLine();
            Console.WriteLine("Consider the following prompt:");
            Console.WriteLine($"--- {prompt} ---");
            Console.WriteLine();

            Console.Write("When you're ready to reflect, press Enter to begin...");
            Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Reflect on the following questions:");
            Console.WriteLine();

            Stopwatch sw = Stopwatch.StartNew();

            
            while (sw.Elapsed.TotalSeconds < totalSeconds)
            {
                string question = DequeueAndRefillIfNeeded(_questionQueue, _questions);
                Console.WriteLine($"-> {question}");
                ShowSpinner(5); // pause with spinner for 5 seconds per question (adjustable)
                Console.WriteLine(); // spacing

                if (sw.Elapsed.TotalSeconds >= totalSeconds) break;
            }

            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("Reflection session complete.");
        }

        
        private Queue<string> ShuffleToQueue(List<string> list)
        {
            var copy = list.ToList();
            var rnd = new Random();
            for (int i = 0; i < copy.Count; i++)
            {
                int j = rnd.Next(i, copy.Count);
                var tmp = copy[i];
                copy[i] = copy[j];
                copy[j] = tmp;
            }
            return new Queue<string>(copy);
        }

        
        private string DequeueAndRefillIfNeeded(Queue<string> queue, List<string> source)
        {
            if (queue.Count == 0)
            {
                queue = ShuffleToQueue(source);
            }
            string item = queue.Dequeue();
            // reassign to the instance queue if we recreated it
            if (source == _prompts) _promptQueue = queue;
            if (source == _questions) _questionQueue = queue;
            return item;
        }

        protected override string ProvideLogSummary()
        {
            return "Reflection session completed";
        }
    }

    
    public class ListingActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?",
            "What are small things that made you smile recently?"
        };

        private Queue<string> _promptQueue;

        public ListingActivity() : base("Listing Activity",
            "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
        {
            _promptQueue = ShuffleToQueue(_prompts);
        }

        protected override void PerformActivity()
        {
            int totalSeconds = GetDuration();

            string prompt = DequeueAndRefillIfNeeded(_promptQueue, _prompts);
            Console.WriteLine();
            Console.WriteLine("List prompt:");
            Console.WriteLine($"--- {prompt} ---");
            Console.WriteLine();
            Console.WriteLine("You will have a few seconds to think, then enter as many items as you can.");
            Console.Write("Get ready");
            ShowSpinner(3);
            Console.WriteLine();

            Console.WriteLine($"Start listing items! You have {totalSeconds} seconds. Press Enter after each item.");
            Console.WriteLine();

            List<string> items = new List<string>();
            Stopwatch sw = Stopwatch.StartNew();



            StringBuilder current = new StringBuilder();
            Console.WriteLine("Start typing items (press Enter to submit each). Timer is running...");
            
            while (sw.Elapsed.TotalSeconds < totalSeconds)
            {
                while (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        string entry = current.ToString().Trim();
                        if (!string.IsNullOrEmpty(entry))
                        {
                            items.Add(entry);
                            Console.WriteLine(entry); // show what user entered
                        }
                        else
                        {
                            Console.WriteLine(); 
                        }
                        current.Clear();
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (current.Length > 0)
                        {
                            current.Length--;
                            
                            Console.Write("\b \b");
                        }
                    }
                    else
                    {
                        Console.Write(keyInfo.KeyChar);
                        current.Append(keyInfo.KeyChar);
                    }
                }

                
                int remaining = totalSeconds - (int)sw.Elapsed.TotalSeconds;
                
                Thread.Sleep(200);
            }

            // After time elapsed, if there's any partial input, count it
            if (current.Length > 0)
            {
                string last = current.ToString().Trim();
                if (!string.IsNullOrEmpty(last))
                {
                    items.Add(last);
                    Console.WriteLine();
                    Console.WriteLine(last);
                }
            }

            sw.Stop();
            Console.WriteLine();
            Console.WriteLine($"Time's up! You listed {items.Count} item(s).");
            if (items.Count > 0)
            {
                Console.WriteLine("Items you listed:");
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {items[i]}");
                }
            }
            else
            {
                Console.WriteLine("No items were entered.");
            }
        }

        // Shuffle helper and queue refill similar to ReflectionActivity
        private Queue<string> ShuffleToQueue(List<string> list)
        {
            var copy = list.ToList();
            var rnd = new Random();
            for (int i = 0; i < copy.Count; i++)
            {
                int j = rnd.Next(i, copy.Count);
                var tmp = copy[i];
                copy[i] = copy[j];
                copy[j] = tmp;
            }
            return new Queue<string>(copy);
        }

        private string DequeueAndRefillIfNeeded(Queue<string> queue, List<string> source)
        {
            if (queue.Count == 0)
            {
                queue = ShuffleToQueue(source);
            }
            string item = queue.Dequeue();
            _promptQueue = queue;
            return item;
        }

        protected override string ProvideLogSummary()
        {
            return "Listing session completed";
        }
    }

    // Program entry and menu handling
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Mindfulness Program - W05";
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                ShowMainMenu();
                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1":
                        var breathing = new BreathingActivity();
                        breathing.Run();
                        break;
                    case "2":
                        var reflection = new ReflectionActivity();
                        reflection.Run();
                        break;
                    case "3":
                        var listing = new ListingActivity();
                        listing.Run();
                        break;
                    case "4":
                        ShowAbout();
                        break;
                    case "5":
                        exit = ConfirmExit();
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please choose a valid option (1-5).");
                        Thread.Sleep(1200);
                        break;
                }
            }

            Console.WriteLine("Thank you for using the Mindfulness Program. Goodbye.");
            Thread.Sleep(1000);
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("      Mindfulness Program - Main Menu ");
            Console.WriteLine("======================================");
            Console.WriteLine("Select an activity:");
            Console.WriteLine("1) Breathing Activity");
            Console.WriteLine("2) Reflection Activity");
            Console.WriteLine("3) Listing Activity");
            Console.WriteLine();
            Console.WriteLine("4) About / Extras");
            Console.WriteLine("5) Exit");
            Console.WriteLine();
            Console.Write("Enter choice (1-5): ");
        }

        static void ShowAbout()
        {
            Console.Clear();
            Console.WriteLine("About the Mindfulness Program");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("This program was created to help users practice short mindfulness activities.");
            Console.WriteLine();
            Console.WriteLine("Enhancements included:");
            Console.WriteLine("- No-repeat prompt/question selection until all items are used in a session.");
            Console.WriteLine("- Logging of completed activities to a file: mindfulness_log.txt");
            Console.WriteLine("- Simple animations and countdowns to guide pacing.");
            Console.WriteLine();
            Console.WriteLine("Log file is saved in the application folder.");
            Console.WriteLine();
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }

        static bool ConfirmExit()
        {
            Console.Write("Are you sure you want to exit? (y/n): ");
            string ans = Console.ReadLine()?.Trim().ToLower() ?? "n";
            return ans == "y" || ans == "yes";
        }
    }
}
