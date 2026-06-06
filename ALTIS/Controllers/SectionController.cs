using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class SectionController : Controller
{
    private readonly string connectionString =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult Add() => View();

    [HttpPost]
    public IActionResult Add(Section section)
    {
        using SqlConnection conn = new(connectionString);
        string query = "INSERT INTO SECTIONS VALUES (@SectionID, @SectionName)";
        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@SectionID", section.SectionID);
        cmd.Parameters.AddWithValue("@SectionName", section.SectionName);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        TempData["SuccessMessage"] = "Section added successfully.";
        return RedirectToAction("List");
    }

    public IActionResult List()
    {
        List<Section> sections = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM SECTIONS";
        using SqlCommand cmd = new(query, conn);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            sections.Add(new Section
            {
                SectionID = reader["SectionID"].ToString(),
                SectionName = reader["SectionName"].ToString()
            });
        }
        conn.Close();
        return View(sections);
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        Section s = new();

        using SqlConnection conn = new(connectionString);
        string query = "SELECT * FROM SECTIONS WHERE SectionID = @id";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            s.SectionID = reader["SectionID"].ToString();
            s.SectionName = reader["SectionName"].ToString();
        }
        conn.Close();

        return View(s);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(string id)
    {
        using SqlConnection conn = new(connectionString);
        string query = "DELETE FROM SECTIONS WHERE SectionID = @id";
        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            TempData["SuccessMessage"] = "Section deleted successfully.";
        }
        catch (SqlException ex)
        {
            if (ex.Number == 547)
            {
                TempData["ErrorMessage"] = "Cannot delete section because it's linked to other records (e.g., classrooms or students).";
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }
        }

        return RedirectToAction("List");
    }
}
