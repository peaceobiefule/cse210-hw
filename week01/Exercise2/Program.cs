using System;

class Program
{
    static void Main(string[] args)
    {
       Console.Write("Enter grade percentage: ");
       string gradepercentage = Console.ReadLine();
       int score = int.Parse(gradepercentage);

       if (score >= 90 && score >= 97)
       {
        Console.WriteLine("You passed with an A+ Welldone!");
       }

       else if (score >= 90 && score <= 93)
       {
        Console.WriteLine("You passed with an A- Welldone!");
       }

       else if (score >= 80)
       {
        Console.WriteLine("You passed with a B!");
       }

       else if (score >= 70)
       {
        Console.WriteLine("You passed with a C!");
       }

       else if (score >= 60)
       {
        Console.WriteLine("You had a D. Keep pushing!");
       }

       else 
       {
        Console.WriteLine("You failed with an F. I know you will do better next time!");
       }

    }
}