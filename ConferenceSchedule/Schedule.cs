using System;
using System.Collections.Generic;

namespace ConferenceSchedule
{
    internal class Schedule
    {
        public DateTime StartDate { get; set; } = DateTime.Today;
        public List<Track> Tracks { get; set; } = new List<Track>();
    }
}