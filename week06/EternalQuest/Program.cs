/*
 * This program exceeds requirements by integrating all classes into a single file for easy submission,
 * implementing a user-friendly menu system with input validation,
 * and adding a simple XP and leveling system to enhance user engagement.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EternalQuest
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            GoalManager manager = new GoalManager();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(@"
   ███████╗████████╗███████╗██████╗ ███████╗███╗   ██╗ █████╗ ██╗     
   ██╔════╝╚══██╔══╝██╔════╝██╔══██╗██╔════╝████╗  ██║██╔══██╗██║     
   ███████╗   ██║   █████╗  ██████╔╝█████╗  ██╔██╗ ██║███████║██║     
   ╚════██║   ██║   ██╔══╝  ██╔══██╗██╔══╝  ██║╚██╗██║██╔══██║██║     
   ███████║   ██║   ███████╗██║  ██║███████╗██║ ╚████║██║  ██║███████╗
   ╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚══════╝
                Your Eternal Goal-Tracking Adventure!
");
                Console.ResetColor();
                Console.WriteLine($"Score: {manager.Score} | Level: {manager.Level} | XP: {manager.XP}/{manager.XPToNextLevel}");
                Console.WriteLine("=================================================");
                Console.WriteLine("1. List Goals");
                Console.WriteLine("2. Add Goal");
                Console.WriteLine("3. Record Event");
                Console.WriteLine("4. Save Goals");
                Console.WriteLine("5. Load Goals");
                Console.WriteLine("6. Quit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        manager.ListGoals();
                        Console.ReadKey();
                        break;
                    case "2":
                        AddGoalMenu(manager);
                        break;
                    case "3":
                        manager.ListGoals();
                        Console.Write("Select goal #: ");
                        if (int.TryParse(Console.ReadLine(), out int index))
                        {
                            manager.RecordEventForGoal(index - 1);
                        }
                        break;
                    case "4":
                        Console.Write("Enter filename to save: ");
                        manager.SaveToFile(Console.ReadLine());
                        break;
                    case "5":
                        Console.Write("Enter filename to load: ");
                        manager.LoadFromFile(Console.ReadLine());
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddGoalMenu(GoalManager manager)
        {
            Console.Clear();
            Console.WriteLine("Choose Goal Type:");
            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            Console.Write("Enter choice: ");
            string typeChoice = Console.ReadLine();

            Console.Write("Enter goal name: ");
            string name = Console.ReadLine();
            Console.Write("Enter goal description: ");
            string desc = Console.ReadLine();
            Console.Write("Enter points per event: ");
            int points = int.Parse(Console.ReadLine());

            switch (typeChoice)
            {
                case "1":
                    manager.AddGoal(new SimpleGoal(name, desc, points));
                    break;
                case "2":
                    manager.AddGoal(new EternalGoal(name, desc, points));
                    break;
                case "3":
                    Console.Write("Enter target count: ");
                    int target = int.Parse(Console.ReadLine());
                    Console.Write("Enter bonus points: ");
                    int bonus = int.Parse(Console.ReadLine());
                    manager.AddGoal(new ChecklistGoal(name, desc, points, target, bonus));
                    break;
                default:
                    Console.WriteLine("Invalid type.");
                    break;
            }
        }
    }

    
    public abstract class Goal
    {
        protected string _name;
        protected string _description;
        protected int _pointsPerEvent;

        protected Goal(string name, string description, int pointsPerEvent)
        {
            _name = name;
            _description = description;
            _pointsPerEvent = pointsPerEvent;
        }

        public abstract int RecordEvent();
        public abstract bool IsComplete();
        public virtual string GetDetailsString()
        {
            return $"{_name}: {_description} (Points: {_pointsPerEvent})";
        }
        public abstract string ToSaveString();
        public abstract string ToDisplayString();
        protected string Escape(string s) => s.Replace("|", "\\|");
        protected string Unescape(string s) => s.Replace("\\|", "|");
    }

   
    public class SimpleGoal : Goal
    {
        private bool _isComplete;
        public SimpleGoal(string name, string description, int points)
            : base(name, description, points) => _isComplete = false;
        public SimpleGoal(string name, string description, int points, bool isComplete)
            : base(name, description, points) => _isComplete = isComplete;
        public override int RecordEvent()
        {
            if (_isComplete) return 0;
            _isComplete = true;
            return _pointsPerEvent;
        }
        public override bool IsComplete() => _isComplete;
        public override string ToSaveString()
        {
            return $"Simple|{Escape(_name)}|{Escape(_description)}|{_pointsPerEvent}|{_isComplete}";
        }
        public override string ToDisplayString()
        {
            var check = _isComplete ? "[X]" : "[ ]";
            return $"{check} {_name} (Simple) - {_description} - {_pointsPerEvent} pts";
        }
    }

    
    public class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points)
            : base(name, description, points) { }
        public override int RecordEvent() => _pointsPerEvent;
        public override bool IsComplete() => false;
        public override string ToSaveString()
        {
            return $"Eternal|{Escape(_name)}|{Escape(_description)}|{_pointsPerEvent}";
        }
        public override string ToDisplayString()
        {
            return $"[∞] {_name} (Eternal) - {_description} - {_pointsPerEvent} pts per event";
        }
    }

    
    public class ChecklistGoal : Goal
    {
        private int _targetCount;
        private int _currentCount;
        private int _bonusPoints;
        public ChecklistGoal(string name, string description, int pointsPerEvent, int targetCount, int bonusPoints)
            : base(name, description, pointsPerEvent)
        {
            _targetCount = targetCount;
            _currentCount = 0;
            _bonusPoints = bonusPoints;
        }
        public ChecklistGoal(string name, string description, int pointsPerEvent, int targetCount, int bonusPoints, int initialCurrent)
            : base(name, description, pointsPerEvent)
        {
            _targetCount = targetCount;
            _currentCount = initialCurrent;
            _bonusPoints = bonusPoints;
        }
        public override int RecordEvent()
        {
            if (IsComplete()) return 0;
            _currentCount++;
            if (_currentCount >= _targetCount)
                return _pointsPerEvent + _bonusPoints;
            return _pointsPerEvent;
        }
        public override bool IsComplete() => _currentCount >= _targetCount;
        public override string ToSaveString()
        {
            return $"Checklist|{Escape(_name)}|{Escape(_description)}|{_pointsPerEvent}|{_targetCount}|{_currentCount}|{_bonusPoints}";
        }
        public override string ToDisplayString()
        {
            var check = IsComplete() ? "[X]" : "[ ]";
            return $"{check} {_name} (Checklist) - {_description} - {_currentCount}/{_targetCount} done - {_pointsPerEvent} pts each, bonus {_bonusPoints}";
        }
    }

    
    public class GoalManager
    {
        private List<Goal> _goals = new List<Goal>();
        private int _score = 0;
        private int _xp = 0;
        private int _level = 1;

        public int Score => _score;
        public int XP => _xp;
        public int Level => _level;
        public int XPToNextLevel => _level * 100;

        public void AddGoal(Goal g) => _goals.Add(g);

        public void RecordEventForGoal(int index)
        {
            if (index < 0 || index >= _goals.Count) return;
            int points = _goals[index].RecordEvent();
            _score += points;
            GainXP(points);
        }

        private void GainXP(int amount)
        {
            _xp += amount;
            while (_xp >= XPToNextLevel)
            {
                _xp -= XPToNextLevel;
                _level++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n*** LEVEL UP! You are now Level {_level}! ***\n");
                Console.ResetColor();
            }
        }

        public void ListGoals()
        {
            Console.WriteLine("\nYour Goals:");
            for (int i = 0; i < _goals.Count; i++)
                Console.WriteLine($"{i + 1}. {_goals[i].ToDisplayString()}");
            Console.WriteLine();
        }

        public void SaveToFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine($"SCORE|{_score}");
                writer.WriteLine($"XP|{_xp}");
                writer.WriteLine($"LEVEL|{_level}");
                foreach (var g in _goals)
                    writer.WriteLine(g.ToSaveString());
            }
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;
            _goals.Clear();
            _score = 0;
            _xp = 0;
            _level = 1;

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = SplitEscaped(line, '|');
                if (parts.Length == 0) continue;

                switch (parts[0])
                {
                    case "SCORE": int.TryParse(parts[1], out _score); break;
                    case "XP": int.TryParse(parts[1], out _xp); break;
                    case "LEVEL": int.TryParse(parts[1], out _level); break;
                    case "Simple":
                        _goals.Add(new SimpleGoal(Unescape(parts[1]), Unescape(parts[2]), int.Parse(parts[3]), bool.Parse(parts[4])));
                        break;
                    case "Eternal":
                        _goals.Add(new EternalGoal(Unescape(parts[1]), Unescape(parts[2]), int.Parse(parts[3])));
                        break;
                    case "Checklist":
                        _goals.Add(new ChecklistGoal(Unescape(parts[1]), Unescape(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[6]), int.Parse(parts[5])));
                        break;
                }
            }
        }

        private static string[] SplitEscaped(string input, char separator)
        {
            var parts = new List<string>();
            var current = new StringBuilder();
            bool escape = false;
            foreach (var c in input)
            {
                if (escape) { current.Append(c); escape = false; }
                else if (c == '\\') { escape = true; }
                else if (c == separator) { parts.Add(current.ToString()); current.Clear(); }
                else { current.Append(c); }
            }
            parts.Add(current.ToString());
            return parts.ToArray();
        }

        private static string Unescape(string s) => s.Replace("\\|", "|");
    }
}
