using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class FacultyController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Faculty f)
    {
        using SqlConnection conn = new(connectionString);
        string query = "INSERT INTO FACULTY VALUES (@FacultyID, @FirstName, @LastName, @Contact, @FacultyMail, @FacultyPassword, @Address)";
        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@FacultyID", f.FacultyID);
        cmd.Parameters.AddWithValue("@FirstName", f.FirstName);
        cmd.Parameters.AddWithValue("@LastName", f.LastName);
        cmd.Parameters.AddWithValue("@Contact", f.Contact);
        cmd.Parameters.AddWithValue("@FacultyMail", f.FacultyMail);
        cmd.Parameters.AddWithValue("@FacultyPassword", f.FacultyPassword);
        cmd.Parameters.AddWithValue("@Address", f.Address);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Faculty f = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM FACULTY WHERE FacultyID = @id";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            f.FacultyID = (int)reader["FacultyID"];
            f.FirstName = reader["FirstName"].ToString();
            f.LastName = reader["LastName"].ToString();
            f.Contact = reader["Contact"].ToString();
            f.FacultyMail = reader["FacultyMail"].ToString();
            f.FacultyPassword = reader["FacultyPassword"].ToString();
            f.Address = reader["Address"].ToString();
        }
        conn.Close();

        return View(f);
    }

    [HttpPost]
    public IActionResult Edit(Faculty f)
    {
        using SqlConnection conn = new(connectionString);
        string query = @"UPDATE FACULTY SET FirstName = @FirstName, LastName = @LastName,
                     Contact = @Contact, FacultyMail = @FacultyMail,
                     FacultyPassword = @FacultyPassword, Address = @Address
                     WHERE FacultyID = @FacultyID";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@FacultyID", f.FacultyID);
        cmd.Parameters.AddWithValue("@FirstName", f.FirstName);
        cmd.Parameters.AddWithValue("@LastName", f.LastName);
        cmd.Parameters.AddWithValue("@Contact", f.Contact);
        cmd.Parameters.AddWithValue("@FacultyMail", f.FacultyMail);
        cmd.Parameters.AddWithValue("@FacultyPassword", f.FacultyPassword);
        cmd.Parameters.AddWithValue("@Address", f.Address);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Faculty f = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM FACULTY WHERE FacultyID = @id";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            f.FacultyID = (int)reader["FacultyID"];
            f.FirstName = reader["FirstName"].ToString();
            f.LastName = reader["LastName"].ToString();
        }
        conn.Close();

        return View(f);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        using SqlConnection conn = new(connectionString);
        string query = "DELETE FROM FACULTY WHERE FacultyID = @id";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            TempData["SuccessMessage"] = "Faculty deleted successfully.";
        }
        catch (SqlException ex)
        {
            // Error code 547 = Foreign Key constraint
            if (ex.Number == 547)
            {
                TempData["ErrorMessage"] = "Cannot delete faculty because it's being referenced in another table.";
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }
        }

        return RedirectToAction("List");
    }

    public IActionResult List()
    {
        List<Faculty> facultyList = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM FACULTY";
        using SqlCommand cmd = new(query, conn);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            facultyList.Add(new Faculty
            {
                FacultyID = (int)reader["FacultyID"],
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Contact = reader["Contact"].ToString(),
                FacultyMail = reader["FacultyMail"].ToString(),
                FacultyPassword = reader["FacultyPassword"].ToString(),
                Address = reader["Address"].ToString()
            });
        }

        conn.Close();
        return View(facultyList);
    }
}
