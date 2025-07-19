using System;
using System.Collections.Generic;
using System.Linq;



class Program
{
    static void Main(string[] args)
    {
        List<(string reference, string text)> scriptures = new List<(string, string)>()
        {
            ("John 3:16", "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have everlasting life."),
            ("Proverbs 3:5-6", "Trust in the Lord with all thine heart; and lean not unto thine own understanding. In all thy ways acknowledge him, and he shall direct thy paths."),
            ("Psalm 23:1", "The Lord is my shepherd; I shall not want.")
        };

        
        var rand = new Random();
        var selected = scriptures[rand.Next(scriptures.Count)];
        Reference reference = new Reference(selected.reference);
        Scripture scripture = new Scripture(reference, selected.text);

        while (!scripture.AllWordsHidden())
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nPress Enter to hide more words or type 'quit' to exit:");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit")
                break;

            scripture.HideRandomWords();
        }

        Console.Clear();
        Console.WriteLine(scripture.GetDisplayText());
        Console.WriteLine("\nAll words are now hidden. Goodbye!");
    }
}
