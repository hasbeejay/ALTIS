using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

public class AssignmentController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    [HttpGet]
    public IActionResult Add()
    {
        List<Classroom> classrooms = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT ClassroomID, Title FROM CLASSROOMS";
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

        ViewBag.Classrooms = classrooms;
        return View();
    }

    [HttpPost]
    public IActionResult Add(Assignment a)
    {
        using SqlConnection conn = new(connectionString);
        string query = @"INSERT INTO ASSIGNMENTS (AssignmentID, ClassroomID, Title, Description, DueDate)
                         VALUES (@AssignmentID, @ClassroomID, @Title, @Description, @DueDate)";
        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@AssignmentID", a.AssignmentID);
        cmd.Parameters.AddWithValue("@ClassroomID", a.ClassroomID);
        cmd.Parameters.AddWithValue("@Title", a.Title);
        cmd.Parameters.AddWithValue("@Description", a.Description);
        cmd.Parameters.AddWithValue("@DueDate", a.DueDate);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List");
    }

    public IActionResult List()
    {
        List<Assignment> assignments = new();

        using SqlConnection conn = new(connectionString);
        string query = @"SELECT A.*, C.Title AS ClassroomTitle
                         FROM ASSIGNMENTS A
                         JOIN CLASSROOMS C ON A.ClassroomID = C.ClassroomID";

        using SqlCommand cmd = new(query, conn);
        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            assignments.Add(new Assignment
            {
                AssignmentID = (int)reader["AssignmentID"],
                ClassroomID = (int)reader["ClassroomID"],
                Title = reader["Title"].ToString(),
                Description = reader["Description"].ToString(),
                DueDate = Convert.ToDateTime(reader["DueDate"]),
                ClassroomTitle = reader["ClassroomTitle"].ToString()
            });
        }

        return View(assignments);
    }
}
