using System;
using System.IO;
using System.Linq;

namespace ConferenceSchedule
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Conference Schedule\n----------------");
            Console.Write("[F] Import .TXT file  \n[M] Enter Mannualy\n[X] Exit\nChoose your input method (F/M/X): ");
            string option = Console.ReadLine().ToUpper();
            switch (option)
            {
                case "F":
                    // import txt file
                    ImportFile();
                    break;
                case "M":
                    // read input
                    Console.WriteLine("Title and duration (in minutes):");
                    ConvertToTalk(Console.ReadLine());
                    break;
                case "X":
                    // exit program
                    Console.WriteLine("Bye bye!");
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
            Console.ReadLine();

        }

        static void ImportFile()
        {
            //TEST: \Projects\ConferenceSchedule\TestInput.txt
            Console.WriteLine("File location path:");
            string path = Console.ReadLine();
            if (!File.Exists(path))
            {
                Console.WriteLine("File not found!");
            }
            else
            {
                string[] tracks = File.ReadAllLines(path);
                foreach (string line in tracks)
                {
                    ConvertToTalk(line);
                }
            }

        }

        static void ConvertToTalk(string inputTrack)
        {
            Talk newTalk = new Talk();
            newTalk.Title = inputTrack;
            var tempDuration = inputTrack.Split(' ').Last().ToLower();
            if (tempDuration == "lightning")
            {
                newTalk.Duration = 5;
            }
            else
            {
                try
                {
                    newTalk.Duration = Convert.ToInt32(tempDuration.Split("min").First());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid duration input!");
                }
            }
            //input OK

            PlaceInTrack(newTalk);
            //input NOK
        }

        static void PlaceInTrack(Talk newTalk)
        {
            Console.WriteLine("new talk added to track: {0} - {1}", newTalk.Duration, newTalk.Title);
        }

        static void OutputSchedule()
        {

        }
    }
}
