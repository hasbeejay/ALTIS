using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

public class ClassroomController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Classroom c)
    {
        using SqlConnection conn = new(connectionString);
        string query = @"INSERT INTO CLASSROOMS (ClassroomID, Title, CourseID, FacultyID, SectionID)
                         VALUES (@ClassroomID, @Title, @CourseID, @FacultyID, @SectionID)";
        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@ClassroomID", c.ClassroomID);
        cmd.Parameters.AddWithValue("@Title", c.Title);
        cmd.Parameters.AddWithValue("@CourseID", c.CourseID);
        cmd.Parameters.AddWithValue("@FacultyID", c.FacultyID);
        cmd.Parameters.AddWithValue("@SectionID", c.SectionID);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        try
        {
            using SqlConnection conn = new(connectionString);
            string query = "DELETE FROM CLASSROOMS WHERE ClassroomID = @id";
            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();

            if (affectedRows == 0)
            {
                TempData["ErrorMessage"] = "Classroom not found or already deleted.";
            }
            else
            {
                TempData["SuccessMessage"] = "Classroom deleted successfully.";
            }
        }
        catch (SqlException ex)
        {
            // Log exception (optional)
            TempData["ErrorMessage"] = "Cannot delete classroom: It may be in use (e.g., has students or assignments).";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An unexpected error occurred while deleting the classroom.";
        }

        return RedirectToAction("List");
    }



    public IActionResult List()
    {
        List<Classroom> classes = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM CLASSROOMS";
        using SqlCommand cmd = new(query, conn);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            classes.Add(new Classroom
            {
                ClassroomID = (int)reader["ClassroomID"],
                Title = reader["Title"].ToString(),
                CourseID = (int)reader["CourseID"],
                FacultyID = (int)reader["FacultyID"],
                SectionID = reader["SectionID"].ToString()
            });
        }

        conn.Close();
        return View(classes);
    }
}
