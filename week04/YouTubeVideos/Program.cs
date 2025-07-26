using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        
        List<Video> videos = new List<Video>();

        // Video 1
        Video video1 = new Video("Exploring the Amazon Jungle", "NatureWorld", 540);
        video1.AddComment(new Comment("Alice", "This was amazing!"));
        video1.AddComment(new Comment("Bob", "I love the background sounds."));
        video1.AddComment(new Comment("Cara", "Makes me want to travel!"));
        videos.Add(video1);

        // Video 2
        Video video2 = new Video("Learn Python in 10 Minutes", "CodeMaster", 600);
        video2.AddComment(new Comment("Dan", "So helpful, thank you!"));
        video2.AddComment(new Comment("Eva", "Clear and easy to follow."));
        video2.AddComment(new Comment("Frank", "Loved this tutorial!"));
        videos.Add(video2);

        // Video 3
        Video video3 = new Video("Top 10 Health Tips", "Wellness360", 420);
        video3.AddComment(new Comment("Grace", "I needed this today."));
        video3.AddComment(new Comment("Henry", "Very informative."));
        video3.AddComment(new Comment("Ivy", "Subscribed instantly!"));
        videos.Add(video3);

        
        Video video4 = new Video("How to Bake Bread at Home", "KitchenQueen", 480);
        video4.AddComment(new Comment("James", "Now I'm hungry!"));
        video4.AddComment(new Comment("Kylie", "Perfect step-by-step guide."));
        video4.AddComment(new Comment("Leo", "Trying this today!"));
        videos.Add(video4);

      
        foreach (Video video in videos)
        {
            Console.WriteLine($"Title: {video.GetTitle()}");
            Console.WriteLine($"Author: {video.GetAuthor()}");
            Console.WriteLine($"Length (seconds): {video.GetLengthInSeconds()}");
            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");
            Console.WriteLine("Comments:");

            foreach (Comment comment in video.GetComments())
            {
                Console.WriteLine($"- {comment.GetCommenterName()}: {comment.GetCommentText()}");
            }

            Console.WriteLine(); 
        }
    }
}
