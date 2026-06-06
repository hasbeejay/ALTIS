using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

public class StudentAssignmentController : Controller
{
    private readonly string connStr =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";
    private readonly string uploadPath = "wwwroot/uploads";

    public IActionResult List(int studentId = 1001)
    {
        List<Assignment> assignments = new();

        using SqlConnection conn = new(connStr);
        string query = @"
            SELECT A.*, C.Title AS ClassroomTitle
            FROM ASSIGNMENTS A
            JOIN CLASSROOMS C ON A.ClassroomID = C.ClassroomID
            WHERE A.ClassroomID IN (
                SELECT ClassroomID FROM ENROLLMENTS WHERE StudentID = @sid
            )";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@sid", studentId);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            assignments.Add(new Assignment
            {
                AssignmentID = (int)reader["AssignmentID"],
                Title = reader["Title"].ToString(),
                Description = reader["Description"].ToString(),
                DueDate = Convert.ToDateTime(reader["DueDate"]),
                ClassroomID = (int)reader["ClassroomID"],
                ClassroomTitle = reader["ClassroomTitle"].ToString()
            });
        }
        conn.Close();
        return View(assignments);
    }

    [HttpGet]
    public IActionResult Submit(int assignmentId)
    {
        ViewBag.AssignmentID = assignmentId;
        ViewBag.StudentID = 1001; // hardcoded student
        return View();
    }

    [HttpPost]
    public IActionResult Submit(int assignmentId, IFormFile file, int studentId = 1001)
    {
        string fileName = $"{Guid.NewGuid()}_{file.FileName}";
        string path = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        using SqlConnection conn = new(connStr);
        string query = @"INSERT INTO SUBMISSIONS (AssignmentID, StudentID, SubmittedAt, FileName)
                         VALUES (@aid, @sid, @date, @file)";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@aid", assignmentId);
        cmd.Parameters.AddWithValue("@sid", studentId);
        cmd.Parameters.AddWithValue("@date", DateTime.Now);
        cmd.Parameters.AddWithValue("@file", fileName);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List", new { studentId });
    }
}
