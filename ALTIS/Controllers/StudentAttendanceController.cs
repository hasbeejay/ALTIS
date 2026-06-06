using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class StudentAttendanceController : Controller
{
    private readonly string connStr =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult Index(int studentId = 1001)
    {
        List<AttendanceRecord> records = new();

        using SqlConnection conn = new(connStr);
        string query = @"
            SELECT A.AttendanceID, A.ClassroomID, C.Title AS ClassroomTitle, A.Date, A.Status
            FROM ATTENDANCE A
            JOIN CLASSROOMS C ON A.ClassroomID = C.ClassroomID
            WHERE A.StudentID = @sid
            ORDER BY A.Date DESC";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@sid", studentId);
        conn.Open();

        using SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            records.Add(new AttendanceRecord
            {
                AttendanceID = (int)reader["AttendanceID"],
                ClassroomID = (int)reader["ClassroomID"],
                ClassroomTitle = reader["ClassroomTitle"].ToString(),
                Date = Convert.ToDateTime(reader["Date"]),
                Status = reader["Status"].ToString()
            });
        }

        conn.Close();
        return View(records);
    }
}
