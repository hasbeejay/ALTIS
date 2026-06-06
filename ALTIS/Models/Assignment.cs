namespace ALTIS.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int ClassroomID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string ClassroomTitle { get; set; } // optional for display
    }
}
