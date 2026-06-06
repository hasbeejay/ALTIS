using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class StudentResultController : Controller
{
    private readonly string connStr =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult View(int studentId = 1001)
    {
        Result result = null;

        using SqlConnection conn = new(connStr);
        string query = @"
            SELECT R.*, A.StaffName
            FROM RESULTS R
            JOIN ADMINS A ON R.AdminID = A.AdminID
            WHERE R.StudentID = @sid";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@sid", studentId);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            result = new Result
            {
                ResultID = (int)reader["ResultID"],
                StudentID = (int)reader["StudentID"],
                AdminID = (int)reader["AdminID"],
                GPA = Convert.ToSingle(reader["GPA"]),
                Remarks = reader["Remarks"].ToString(),
                AdminName = reader["StaffName"].ToString()
            };
        }

        conn.Close();
        return View(result);
    }
}
