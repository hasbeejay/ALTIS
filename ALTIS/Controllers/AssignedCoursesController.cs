using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

public class AssignedCoursesController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult Index()
    {
        List<CourseAssignment> assigned = new();

        using SqlConnection conn = new(connectionString);
        string query = @"
            SELECT CA.AssignmentID, CA.CourseID, CA.FacultyID, CA.SectionID, CA.AdminID, CA.Semester,
                   C.CourseName, S.SectionName
            FROM COURSE_ASSIGNMENTS CA
            JOIN COURSES C ON CA.CourseID = C.CourseID
            JOIN SECTIONS S ON CA.SectionID = S.SectionID";

        using SqlCommand cmd = new(query, conn);
        conn.Open();

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            assigned.Add(new CourseAssignment
            {
                AssignmentID = (int)reader["AssignmentID"],
                CourseID = (int)reader["CourseID"],
                FacultyID = (int)reader["FacultyID"],
                SectionID = reader["SectionID"].ToString(),
                AdminID = (int)reader["AdminID"],
                Semester = reader["Semester"].ToString(),
                CourseName = reader["CourseName"].ToString(),
                SectionName = reader["SectionName"].ToString()
            });
        }

        return View(assigned);
    }
}
