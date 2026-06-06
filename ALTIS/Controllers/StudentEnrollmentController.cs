using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class StudentEnrollmentController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult Register()
    {
        List<Classroom> classrooms = new();
        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM CLASSROOMS";

        using SqlCommand cmd = new(query, conn);
        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            classrooms.Add(new Classroom
            {
                ClassroomID = (int)reader["ClassroomID"],
                Title = reader["Title"].ToString(),
                CourseID = (int)reader["CourseID"],
                SectionID = reader["SectionID"].ToString(),
                FacultyID = (int)reader["FacultyID"]
            });
        }

        return View(classrooms);
    }

    public IActionResult Enroll(int classroomId)
    {
        int? studentId = HttpContext.Session.GetInt32("UserID");
        if (studentId == null)
            return RedirectToAction("Login", "Login");

        using SqlConnection conn = new(connectionString);
        string checkQuery = "SELECT COUNT(*) FROM ENROLLMENTS WHERE StudentID = @sid AND ClassroomID = @cid";
        using SqlCommand checkCmd = new(checkQuery, conn);
        checkCmd.Parameters.AddWithValue("@sid", studentId);
        checkCmd.Parameters.AddWithValue("@cid", classroomId);

        conn.Open();
        int exists = (int)checkCmd.ExecuteScalar();
        if (exists == 0)
        {
            string insertQuery = "INSERT INTO ENROLLMENTS (StudentID, ClassroomID) VALUES (@sid, @cid)";
            using SqlCommand cmd = new(insertQuery, conn);
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@cid", classroomId);
            cmd.ExecuteNonQuery();
        }
        conn.Close();

        return RedirectToAction("MyClassrooms");
    }

    public IActionResult MyClassrooms()
    {
        int? studentId = HttpContext.Session.GetInt32("UserID");
        if (studentId == null)
            return RedirectToAction("Login", "Login");

        List<Classroom> joined = new();
        using SqlConnection conn = new(connectionString);
        string query = @"SELECT C.* FROM ENROLLMENTS E
                         JOIN CLASSROOMS C ON E.ClassroomID = C.ClassroomID
                         WHERE E.StudentID = @sid";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@sid", studentId);

        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            joined.Add(new Classroom
            {
                ClassroomID = (int)reader["ClassroomID"],
                Title = reader["Title"].ToString(),
                CourseID = (int)reader["CourseID"],
                FacultyID = (int)reader["FacultyID"],
                SectionID = reader["SectionID"].ToString()
            });
        }

        return View(joined);
    }
}
