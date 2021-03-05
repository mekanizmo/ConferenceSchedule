using System;
using System.Collections.Generic;

namespace ConferenceSchedule
{
    public class Track
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Talk> Talks { get; set; } = new List<Talk>();
        
    }
}