namespace ALTIS.Models
{
    public class AttendanceRecord
    {
        public int AttendanceID { get; set; }
        public int ClassroomID { get; set; }
        public string ClassroomTitle { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
