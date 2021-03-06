using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConferenceSchedule
{

    /*TEST: 
     
        \Projects\ConferenceSchedule\TestInput.txt

    */

    class Program
    {
        static void Main()
        {
            Schedule schedule = new Schedule { StartDate = DateTime.Today };            
            Track targetTrack = new Track { Id = 1, Date = schedule.StartDate };
            schedule.Tracks.Add(targetTrack);

            bool exit = false;
            while (!exit)
            {
                targetTrack = schedule.Tracks.Last();

                Console.Clear();

                Console.WriteLine("Conference Schedule\n----------------");
                Console.Write("[F] Import .TXT file  \n[M] Enter Mannualy\n[V] View Schedule\n[X] Exit\nChoose [ F | M | V | X ]: ");
                string option = Console.ReadLine().ToUpper();
                switch (option)
                {
                    case "F":
                        UpdateScheduleTrack(targetTrack, ImportFromFile(), schedule);
                        break;
                    case "M":
                        UpdateScheduleTrack(targetTrack, ConvertToTalks(ReadInputs()), schedule);
                        break;
                    case "V":
                        Console.Clear();
                        OutputSchedule(schedule);
                        Console.WriteLine("\nBack [ENTER]");
                        Console.ReadLine();
                        break;
                    case "X":
                        Console.WriteLine("Bye bye!");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        static string[] ReadInputs()
        {
            List<string> talkInputs = new List<string>();
            bool valid = true;
            while (valid)
            {
                Console.WriteLine("Insert new talk (Title and duration):\nBack [Enter]");
                var input = Console.ReadLine();
                if (input != "") 
                {
                    talkInputs.Add(input);
                }
                else { valid = false; }
                
            }
            Console.WriteLine("\n{0} talks will be inserted!\nPress Enter",talkInputs.Count);
            Console.ReadLine();
            return talkInputs.ToArray();
        }

        static List<Talk> ImportFromFile()
        {
            Console.WriteLine("File location path:");
            string path = Console.ReadLine();

            if (!File.Exists(path))
            {
                Console.WriteLine("File not found!");
                return null;
            }
            else
            {
                var talkInputs = ConvertToTalks(File.ReadAllLines(path));
                Console.WriteLine("\n{0} talks will be inserted!\nPress Enter", talkInputs.Count);
                Console.ReadLine();
                return talkInputs;
            }
        }

        static List<Talk> ConvertToTalks(string[] inputTalks)
        {
            List<Talk> listTalk = new List<Talk>();

            foreach (var talk in inputTalks)
            {
                bool success = false;
                Talk newTalk = new Talk
                {
                    Title = talk
                };
                string tempDuration = talk.Split(' ').Last().ToLower();
                if (tempDuration == "lightning")
                {
                    newTalk.Duration = 5;
                    success = true;
                }
                else
                {
                    try
                    {
                        newTalk.Duration = Convert.ToInt32(tempDuration.Split("min").First());
                        success = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid duration input!");
                    }
                }
                if (success)
                {
                    listTalk.Add(newTalk);
                }
            }
            return listTalk;
        }

        static void AddTalkToTrack(Talk newTalk, Track track)
        {
            track.Talks.Add(newTalk);
        }

        static void UpdateScheduleTrack(Track targetTrack, List<Talk> talkIn, Schedule schedule)
        {
            Talk lunch = new Talk { Title = "Lunch", Duration = 60, StartTime = new DateTime(schedule.StartDate.Year, schedule.StartDate.Month, schedule.StartDate.Day, 12, 0, 0), EndTime = new DateTime(schedule.StartDate.Year, schedule.StartDate.Month, schedule.StartDate.Day, 13, 0, 0) };
            Talk network = new Talk { Title = "Network Event", StartTime = new DateTime(schedule.StartDate.Year, schedule.StartDate.Month, schedule.StartDate.Day, 17, 0, 0) };

            foreach (var talk in talkIn)
            {
                if (targetTrack.Talks.Any())
                {
                    var lastTalk = schedule.Tracks.Last().Talks.Last();
                    talk.StartTime = lastTalk.EndTime;
                    talk.EndTime = talk.StartTime.AddMinutes(talk.Duration);

                    if (talk.EndTime >= lunch.StartTime && talk.EndTime <= lunch.EndTime)
                    {
                        AddTalkToTrack(lunch, targetTrack);

                        talk.StartTime = lunch.EndTime;
                        talk.EndTime = talk.StartTime.AddMinutes(talk.Duration);
                    }
                    else if (talk.StartTime >= lunch.EndTime && talk.EndTime >= network.StartTime)
                    {
                        AddTalkToTrack(network, targetTrack);
                        schedule.Tracks.Add(new Track { Id = targetTrack.Id + 1, Date = targetTrack.Date.AddDays(1) });
                        targetTrack = schedule.Tracks.Last();

                        network.StartTime = network.StartTime.AddDays(1);

                        lunch.StartTime = lunch.StartTime.AddDays(1);
                        lunch.EndTime = lunch.StartTime.AddMinutes(lunch.Duration);

                        talk.StartTime = new DateTime(targetTrack.Date.Year, targetTrack.Date.Month, targetTrack.Date.Day, 9, 0, 0);
                        talk.EndTime = talk.StartTime.AddMinutes(talk.Duration);
                    }
                    AddTalkToTrack(talk, targetTrack);
                }
                else
                {
                    talk.StartTime = new DateTime(targetTrack.Date.Year, targetTrack.Date.Month, targetTrack.Date.Day, 9, 0, 0);
                    talk.EndTime = talk.StartTime.AddMinutes(talk.Duration);
                    AddTalkToTrack(talk, targetTrack);
                }
            }
        }

        static void OutputSchedule(Schedule schedule)
        {
            foreach (var track in schedule.Tracks)
            {
                Console.WriteLine("\nTrack {0}: \n", track.Id);
                foreach (var talk in track.Talks)
                {
                    //Console.WriteLine("{0} - {1}",talk.StartTime.ToString("hh:mmtt"), talk.Title);
                    Console.WriteLine("{0} - {1}", talk.StartTime.ToString("HH:mm"), talk.Title);
                }
            }
        }
    }
}
