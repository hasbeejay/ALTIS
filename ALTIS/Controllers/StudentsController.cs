using Microsoft.AspNetCore.Mvc;
using ALTIS.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

public class StudentController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Student student)
    {
        using SqlConnection conn = new(connectionString);
        string query = "INSERT INTO STUDENTS VALUES (@StudentID, @FirstName, @LastName, @Contact, @StudentMail, @StudentPassword, @Address, @SectionID)";
        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
        cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
        cmd.Parameters.AddWithValue("@LastName", student.LastName);
        cmd.Parameters.AddWithValue("@Contact", student.Contact);
        cmd.Parameters.AddWithValue("@StudentMail", student.StudentMail);
        cmd.Parameters.AddWithValue("@StudentPassword", student.StudentPassword);
        cmd.Parameters.AddWithValue("@Address", student.Address);
        cmd.Parameters.AddWithValue("@SectionID", student.SectionID);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Student student = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM STUDENTS WHERE StudentID = @id";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            student.StudentID = (int)reader["StudentID"];
            student.FirstName = reader["FirstName"].ToString();
            student.LastName = reader["LastName"].ToString();
            student.Contact = reader["Contact"].ToString();
            student.StudentMail = reader["StudentMail"].ToString();
            student.StudentPassword = reader["StudentPassword"].ToString();
            student.Address = reader["Address"].ToString();
            student.SectionID = reader["SectionID"].ToString();
        }

        conn.Close();
        return View(student);
    }

    [HttpPost]
    public IActionResult Edit(Student student)
    {
        using SqlConnection conn = new(connectionString);
        string query = @"UPDATE STUDENTS SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact,
                     StudentMail = @StudentMail, StudentPassword = @StudentPassword, Address = @Address,
                     SectionID = @SectionID WHERE StudentID = @StudentID";
        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
        cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
        cmd.Parameters.AddWithValue("@LastName", student.LastName);
        cmd.Parameters.AddWithValue("@Contact", student.Contact);
        cmd.Parameters.AddWithValue("@StudentMail", student.StudentMail);
        cmd.Parameters.AddWithValue("@StudentPassword", student.StudentPassword);
        cmd.Parameters.AddWithValue("@Address", student.Address);
        cmd.Parameters.AddWithValue("@SectionID", student.SectionID);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Student student = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM STUDENTS WHERE StudentID = @id";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            student.StudentID = (int)reader["StudentID"];
            student.FirstName = reader["FirstName"].ToString();
            student.LastName = reader["LastName"].ToString();
        }

        conn.Close();
        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        using SqlConnection conn = new(connectionString);
        string query = "DELETE FROM STUDENTS WHERE StudentID = @id";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            if (ex.Number == 547) // Foreign key constraint violation
            {
                TempData["ErrorMessage"] = "Cannot delete student. This student is linked to other records.";
            }
            else
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the student.";
            }
            return RedirectToAction("List");
        }
        finally
        {
            conn.Close();
        }

        TempData["SuccessMessage"] = "Student deleted successfully.";
        return RedirectToAction("List");
    }


    public IActionResult List()
    {
        List<Student> students = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM STUDENTS";
        using SqlCommand cmd = new(query, conn);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            students.Add(new Student
            {
                StudentID = (int)reader["StudentID"],
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Contact = reader["Contact"].ToString(),
                StudentMail = reader["StudentMail"].ToString(),
                StudentPassword = reader["StudentPassword"].ToString(),
                Address = reader["Address"].ToString(),
                SectionID = reader["SectionID"].ToString()
            });
        }

        conn.Close();
        return View(students);
    }

}
