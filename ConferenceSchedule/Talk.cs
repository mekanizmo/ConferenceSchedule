using System;

namespace ConferenceSchedule
{
    public class Talk
    {
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
        public Talk()
        {
            EndTime = this.StartTime.AddMinutes(Duration);
        }
    }
}

