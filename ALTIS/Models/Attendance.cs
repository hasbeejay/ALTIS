namespace ALTIS.Models
{
    public class Attendance
    {
        public int ClassroomID { get; set; }
        public int StudentID { get; set; }
        public string Status { get; set; } // "Present" or "Absent"
        public DateTime Date { get; set; } = DateTime.Now;
        public string StudentName { get; set; } // for display purposes
    }
}
