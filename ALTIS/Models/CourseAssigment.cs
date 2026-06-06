namespace ALTIS.Models
{
    public class CourseAssignment
    {
        public int AssignmentID { get; set; }
        public int CourseID { get; set; }
        public int FacultyID { get; set; }
        public string SectionID { get; set; }
        public int AdminID { get; set; }
        public string Semester { get; set; }

        // Display only
        public string CourseName { get; set; }
        public string SectionName { get; set; }
    }
}
