using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MostLiked
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Checkit");
            string path = "C:\\Users\\Tomer\\Desktop\\asks.txt";
            UpdateDate(path);
            bool Continue = true;
            while (Continue)
            {
                Console.WriteLine("What would you like to do? [Erase task(E) , Write a new task(W), Open the tasks for today(O),To Exit(S)]");
                string Answer = Console.ReadLine();
                AnswerCheck(Answer, path);
                if (Answer == "S") Continue = false;
            }
        }
        public static string getRandom(Songs[] array, int i)
        {
            Random rnd = new Random();
            int ran = rnd.Next(i);
            Console.WriteLine("What is the mood today? [Good/Chill/Hype]");
            string mood = Console.ReadLine();
            while (mood != "Good" && mood != "Hype" && mood != "Chill")
            {
                Console.Write("Invalid answer! What is the mood today? [Good/Chill/Hype]");
                mood = Console.ReadLine();
            }
            while (array[ran].getMood() != mood) ran = rnd.Next(i);
            return array[ran].getSong();
        }
        public static void UpdateDate(string path)
        {
            string[] lines = File.ReadAllLines(path);
            lines[0] = "Tasks for today " + DateTime.UtcNow.Date.ToString("dd/MM/yyyy");
            File.WriteAllLines(path, lines);
        }
        public static void CleanFile(string path, string text)
        {
            if (text == "All") File.WriteAllText(path, "Tasks for today " + DateTime.UtcNow.Date.ToString("dd/MM/yyyy"));
            else
            {
                string[] linesToKeep = File.ReadAllLines(path).Where(l => l != text).ToArray();
                File.WriteAllLines(path, linesToKeep);
            }
        }
        public static void AnswerCheck(string Answer, string path)
        {
            string[] Answers = { "E", "W", "O", "S"};
            while (!Answers.Contains(Answer))
            {
                Console.WriteLine("Invalid answer, What would you like to do? [Erase task(E) , Write a new task(W), Open the tasks for today(O)]");
                Answer = Console.ReadLine();
            }
            switch (Answer)
            {
                case "E":
                    Erase(path);
                    break;
                case "W":
                    Write(path);
                    break;
                case "O":
                    Open(path);
                    break;
                case "S":
                    break;
            }
        }
        public static void Write(string path)
        {
            Console.WriteLine("What is the new task?");
            string text = Console.ReadLine();
            string[] filetext = { File.ReadAllText(path), text };
            File.WriteAllLines(path, filetext);
            CleanFile(path, String.Empty);
            Console.WriteLine("Done!");
        }
        public static void Erase(string path)
        {
            Console.WriteLine("These are the tasks you have:");
            Console.WriteLine(File.ReadAllText(path));
            Console.WriteLine("What task would you like to crossout? to wipe all tasks write All");
            string crossout = Console.ReadLine();
            while (isDeletable(path,crossout))
            {
                Console.WriteLine("Invalid asnwer, What task would you like to crossout? to wipe all tasks write All");
                crossout = Console.ReadLine();
            }
            CleanFile(path, crossout);
            Console.WriteLine("Done!");
        }
        public static void Open(string path)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(path);
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            Songs[] songs = Def();
            Process.Start("chrome.exe", getRandom(songs, 10));
            Process.Start(startInfo);
            Console.WriteLine("Done!");
        }
        public static Songs[] Def()
        {
            Songs[] Songs = new Songs[10];
            Songs[0] = new Songs("https://www.youtube.com/watch?v=LYG1Waliqbw","Good");
            Songs[1] = new Songs("https://www.youtube.com/watch?v=WYeAUpvWeI8","Chill");
            Songs[2] = new Songs("https://www.youtube.com/watch?v=JbG73nMSfSM","Hype");
            Songs[3] = new Songs("https://www.youtube.com/watch?v=3M_5oYU-IsU","Hype");
            Songs[4] = new Songs("https://www.youtube.com/watch?v=SYM-RJwSGQ8","Chill");
            Songs[5] = new Songs("https://www.youtube.com/watch?v=KWZGAExj-es","Good");
            Songs[6] = new Songs("https://www.youtube.com/watch?v=eJSik6ejkr0","Good");
            Songs[7] = new Songs("https://www.youtube.com/watch?v=gOsM-DYAEhY","Hype");
            Songs[8] = new Songs("https://www.youtube.com/watch?v=eACohWVwTOc","Hype");
            Songs[9] = new Songs("https://www.youtube.com/watch?v=zDo0H8Fm7d0","Good");
            return Songs;
        }
        public static bool isDeletable(string path,string crossout)
        {
            return ((!File.ReadAllLines(path).Contains(crossout)) && crossout != "All") || crossout == "Tasks for today " + DateTime.UtcNow.Date.ToString("dd/MM/yyyy");
        }
    }
    public class Songs
    {
        string song;
        string mood;
        public Songs(string url,string Mood){
            this.song = url;
            this.mood = Mood;
        }
        public string getMood(){
            return this.mood;
        }
        public string getSong(){
            return this.song;
        }
    }
}
