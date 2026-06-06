namespace ALTIS.Models
{
    public class Mark
    {
        public int MarkID { get; set; }
        public int StudentID { get; set; }
        public int AssignmentID { get; set; }
        public int ClassroomID { get; set; }
        public float Score { get; set; }

        // for display
        public string StudentName { get; set; }
        public string AssignmentTitle { get; set; }
    }
}
