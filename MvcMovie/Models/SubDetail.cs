using System;
using System.Collections.Generic;

namespace MvcMovie.Models
{
    public class SubDetail
    {
        public int Id { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int TeacherID { get; set; }
        public int MaxPeople { get; set; }
        public List<TimeDetail> SubTimeDetail {get; set;}     
        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
    }

    public class TimeDetail
    {
        public int Id { get; set; }
        public string WeekDay { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}