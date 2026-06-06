namespace ALTIS.Models
{
    public class Announcement
    {
        public int AnnouncementID { get; set; }
        public int ClassroomID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime PostedAt { get; set; }

        // Display
        public string ClassroomTitle { get; set; }
    }
}
