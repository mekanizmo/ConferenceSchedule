using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConferenceSchedule
{
    class Program
    {


        static void Main(string[] args)
        {
            Console.Clear();

            Schedule schedule = new Schedule();
            Track initTrack = new Track { Id = 1, Date = schedule.StartDate };
            schedule.Tracks.Add(initTrack);
            var targetTrack = schedule.Tracks.Last();

            Talk lunch = new Talk { Title = "Lunch", Duration = 1, StartTime = new DateTime(targetTrack.Date.Year, targetTrack.Date.Month, targetTrack.Date.Day, 12, 0, 0), EndTime = new DateTime(targetTrack.Date.Year, targetTrack.Date.Month, targetTrack.Date.Day, 13, 0, 0) };
            Talk network = new Talk { Title = "Network Event", StartTime = new DateTime(targetTrack.Date.Year, targetTrack.Date.Month, targetTrack.Date.Day, 17, 0, 0) };

            Console.WriteLine("Conference Schedule\n----------------");
            Console.Write("[F] Import .TXT file  \n[M] Enter Mannualy\n[X] Exit\nChoose your input method (F/M/X): ");
            string option = Console.ReadLine().ToUpper();
            switch (option)
            {
                case "F":
                    // import txt file
                    var talks = ImportFromFile();
                    foreach (var talk in talks)
                    {
                        if (targetTrack.Talks.Count > 0)
                        {
                            var lastTalk = schedule.Tracks.Last().Talks.Last();
                            Console.WriteLine(lastTalk.EndTime.ToString());
                            if (talk.EndTime > network.StartTime)
                            {
                                schedule.Tracks.Add(new Track { Id = targetTrack.Id + 1, Date = targetTrack.Date.AddDays(1) });
                                targetTrack = schedule.Tracks.Last();
                                //network.StartTime = new DateTime(targetTrack.Date.Year, targetTrack.Date.Month, targetTrack.Date.Day, 17, 0, 0);
                            }
                            //else if(lastTalk.EndTime > lunch.StartTime && lastTalk.StartTime < lunch.EndTime)
                            //{
                            //    AddTalkToTrack(lunch, targetTrack);
                            //}

                        }


                        AddTalkToTrack(talk, targetTrack);
                    }
                    break;
                case "M":
                    // read input
                    Console.WriteLine("Title and duration (in minutes):");
                    var talkInput = ConvertToTalk(Console.ReadLine());
                    targetTrack = schedule.Tracks.Last();
                    AddTalkToTrack(talkInput, targetTrack);
                    break;
                case "X":
                    // exit program
                    Console.WriteLine("Bye bye!");
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }

            OutputSchedule(schedule);
            Console.ReadLine();

        }

        static List<Talk> ImportFromFile() //TEST: \Projects\ConferenceSchedule\TestInput.txt
        {

            Console.WriteLine("File location path:");
            string path = Console.ReadLine();

            List<Talk> talks = new List<Talk>();

            if (!File.Exists(path))
            {
                Console.WriteLine("File not found!");
                return null;
            }
            else
            {
                string[] tracks = File.ReadAllLines(path);
                foreach (string line in tracks)
                {
                    talks.Add(ConvertToTalk(line));
                }
                return talks;
            }

        }

        static Talk ConvertToTalk(string inputTalk)
        {
            bool valid = false;
            Talk newTalk = new Talk
            {
                Title = inputTalk
            };
            string tempDuration = inputTalk.Split(' ').Last().ToLower();
            if (tempDuration == "lightning")
            {
                newTalk.Duration = 5;
                valid = true;
            }
            else
            {
                try
                {
                    newTalk.Duration = Convert.ToInt32(tempDuration.Split("min").First());
                    valid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid duration input!");
                }
            }


            if (valid) { return newTalk; }
            else { return null; }

        }

        static void AddTalkToTrack(Talk newTalk, Track track)
        {

            if (track.Talks.Count <= 0)
            {
                newTalk.StartTime = new DateTime(track.Date.Year, track.Date.Month, track.Date.Day, 9, 0, 0);
                newTalk.EndTime = newTalk.StartTime.AddMinutes(newTalk.Duration);
            }
            else
            {
                newTalk.StartTime = track.Talks.Last().StartTime.AddMinutes(track.Talks.Last().Duration);
                newTalk.EndTime = newTalk.StartTime.AddMinutes(newTalk.Duration);

                if (newTalk.EndTime > new DateTime(track.Date.Year, track.Date.Month, track.Date.Day, 12, 0, 0) && newTalk.EndTime <= new DateTime(track.Date.Year, track.Date.Month, track.Date.Day, 13, 0, 0))
                {
                    Talk lunch = new Talk { Title = "Lunch", Duration = 1, StartTime = new DateTime(track.Date.Year, track.Date.Month, track.Date.Day, 12, 0, 0) };
                    lunch.EndTime = lunch.StartTime.AddMinutes(lunch.Duration);
                    track.Talks.Add(lunch);

                    newTalk.StartTime = new DateTime(track.Date.Year, track.Date.Month, track.Date.Day, 13, 0, 0);
                    newTalk.EndTime = newTalk.StartTime.AddMinutes(newTalk.Duration);
                }

                else if (newTalk.EndTime > new DateTime(track.Date.Year, track.Date.Month, track.Date.Day, 17, 0, 0))
                {
                    Talk network = new Talk { Title = "Network Event", StartTime = new DateTime(track.Date.Year, track.Date.Month, track.Date.Day, 17, 0, 0) };
                    track.Talks.Add(network);

                }

            }

            track.Talks.Add(newTalk);
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
