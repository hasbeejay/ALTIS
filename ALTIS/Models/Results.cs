namespace ALTIS.Models
{
    public class Result
    {
        public int ResultID { get; set; }
        public int StudentID { get; set; }
        public int AdminID { get; set; }
        public float GPA { get; set; }
        public string Remarks { get; set; }

        //Display
        public string AdminName { get; set; }
    }
}
