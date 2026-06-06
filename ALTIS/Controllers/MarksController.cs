using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class MarksController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult SelectAssignment()
    {
        List<Assignment> assignments = new();
        using SqlConnection conn = new(connectionString);
        string query = "SELECT AssignmentID, Title FROM ASSIGNMENTS";
        using SqlCommand cmd = new(query, conn);

        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            assignments.Add(new Assignment
            {
                AssignmentID = (int)reader["AssignmentID"],
                Title = reader["Title"].ToString()
            });
        }
        ViewBag.Assignments = assignments;
        return View();
    }

    [HttpGet]
    public IActionResult Grade(int assignmentId)
    {
        List<Mark> marks = new();

        using SqlConnection conn = new(connectionString);
        string query = @"
            SELECT S.StudentID, S.FirstName + ' ' + S.LastName AS StudentName,
                   A.ClassroomID, A.Title
            FROM SUBMISSIONS SB
            JOIN STUDENTS S ON SB.StudentID = S.StudentID
            JOIN ASSIGNMENTS A ON SB.AssignmentID = A.AssignmentID
            WHERE SB.AssignmentID = @aid";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@aid", assignmentId);

        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            marks.Add(new Mark
            {
                AssignmentID = assignmentId,
                ClassroomID = (int)reader["ClassroomID"],
                StudentID = (int)reader["StudentID"],
                StudentName = reader["StudentName"].ToString(),
                AssignmentTitle = reader["Title"].ToString(),
                Score = 0
            });
        }
        conn.Close();

        return View(marks);
    }

    [HttpPost]
    public IActionResult Grade(List<Mark> records)
    {
        using SqlConnection conn = new(connectionString);
        conn.Open();

        foreach (var m in records)
        {
            string query = @"INSERT INTO MARKS (StudentID, AssignmentID, ClassroomID, Score)
                             VALUES (@StudentID, @AssignmentID, @ClassroomID, @Score)";
            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@StudentID", m.StudentID);
            cmd.Parameters.AddWithValue("@AssignmentID", m.AssignmentID);
            cmd.Parameters.AddWithValue("@ClassroomID", m.ClassroomID);
            cmd.Parameters.AddWithValue("@Score", m.Score);

            cmd.ExecuteNonQuery();
        }

        conn.Close();
        return RedirectToAction("SelectAssignment");
    }
}
