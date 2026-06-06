using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

public class AttendanceController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    [HttpGet]
    public IActionResult SelectClassroom()
    {
        List<Classroom> classrooms = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM CLASSROOMS"; // TODO: filter by FacultyID if login is active
        using SqlCommand cmd = new(query, conn);

        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            classrooms.Add(new Classroom
            {
                ClassroomID = (int)reader["ClassroomID"],
                Title = reader["Title"].ToString()
            });
        }
        conn.Close();

        return View(classrooms);
    }

    [HttpGet]
    public IActionResult Mark(int classroomId)
    {
        List<Attendance> students = new();

        using SqlConnection conn = new(connectionString);
        string query = @"
            SELECT S.StudentID, S.FirstName + ' ' + S.LastName AS StudentName
            FROM ENROLLMENTS E
            JOIN STUDENTS S ON E.StudentID = S.StudentID
            WHERE E.ClassroomID = @cid";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@cid", classroomId);

        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Attendance
            {
                StudentID = (int)reader["StudentID"],
                StudentName = reader["StudentName"].ToString(),
                ClassroomID = classroomId,
                Status = "Present", // default
                Date = DateTime.Now
            });
        }
        conn.Close();

        return View(students);
    }

    [HttpGet]
    public IActionResult History(int classroomId)
    {
        List<Attendance> records = new();
        using SqlConnection conn = new(connectionString);
        string q = @"SELECT A.StudentID, S.FirstName + ' ' + S.LastName AS Name, A.Date, A.Status
        FROM ATTENDANCE A
        JOIN STUDENTS S ON A.StudentID = S.StudentID
        WHERE A.ClassroomID = @cid ORDER BY A.Date DESC";
        SqlCommand cmd = new(q, conn);
        cmd.Parameters.AddWithValue("@cid", classroomId);
        conn.Open();
        var r = cmd.ExecuteReader();
        while (r.Read())
        {
            records.Add(new Attendance
            {
                StudentID = (int)r["StudentID"],
                StudentName = r["Name"].ToString(),
                Date = (DateTime)r["Date"],
                Status = r["Status"].ToString(),
                ClassroomID = classroomId
            });
        }
        conn.Close();
        return View(records);
    }
        [HttpPost]
    public IActionResult Mark(List<Attendance> records)
    {
        using SqlConnection conn = new(connectionString);
        conn.Open();

        foreach (var rec in records)
        {
            string query = @"INSERT INTO ATTENDANCE (ClassroomID, StudentID, Date, Status)
                             VALUES (@ClassroomID, @StudentID, @Date, @Status)";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@ClassroomID", rec.ClassroomID);
            cmd.Parameters.AddWithValue("@StudentID", rec.StudentID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Status", rec.Status);

            cmd.ExecuteNonQuery();
        }

        conn.Close();
        return RedirectToAction("SelectClassroom");
    }
}
