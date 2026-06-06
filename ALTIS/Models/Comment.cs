namespace ALTIS.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int AnnouncementID { get; set; }
        public int StudentID { get; set; }
        public string Message { get; set; }
        public DateTime CommentedAt { get; set; }

        // display
        public string StudentName { get; set; }
    }
}
