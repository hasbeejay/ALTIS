public class Submission
{
    public int SubmissionID { get; set; }
    public int AssignmentID { get; set; }
    public int StudentID { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string FileName { get; set; }

    // Display
    public string AssignmentTitle { get; set; }
    public string Status { get; set; }
}
