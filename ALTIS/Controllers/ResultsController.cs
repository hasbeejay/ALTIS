using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class ResultsController : Controller
{
    private readonly string connStr = "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult List()
    {
        List<Result> results = new();
        using SqlConnection conn = new(connStr);
        string query = "SELECT * FROM RESULTS";
        SqlCommand cmd = new(query, conn);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            results.Add(new Result
            {
                ResultID = (int)reader["ResultID"],
                StudentID = (int)reader["StudentID"],
                AdminID = (int)reader["AdminID"],
                GPA = Convert.ToSingle(reader["GPA"]),
                Remarks = reader["Remarks"].ToString()
            });
        }
        conn.Close();
        return View(results);
    }

    [HttpGet]
    public IActionResult Add() => View();

    [HttpPost]
    public IActionResult Add(Result r)
    {
        using SqlConnection conn = new(connStr);
        string query = "INSERT INTO RESULTS VALUES (@ResultID, @StudentID, @AdminID, @GPA, @Remarks)";
        SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@ResultID", r.ResultID);
        cmd.Parameters.AddWithValue("@StudentID", r.StudentID);
        cmd.Parameters.AddWithValue("@AdminID", r.AdminID);
        cmd.Parameters.AddWithValue("@GPA", r.GPA);
        cmd.Parameters.AddWithValue("@Remarks", r.Remarks);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Result r = new();
        using SqlConnection conn = new(connStr);
        SqlCommand cmd = new("SELECT * FROM RESULTS WHERE ResultID = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            r.ResultID = (int)reader["ResultID"];
            r.StudentID = (int)reader["StudentID"];
            r.AdminID = (int)reader["AdminID"];
            r.GPA = Convert.ToSingle(reader["GPA"]);
            r.Remarks = reader["Remarks"].ToString();
        }
        conn.Close();
        return View(r);
    }

    [HttpPost]
    public IActionResult Edit(Result r)
    {
        using SqlConnection conn = new(connStr);
        SqlCommand cmd = new(@"UPDATE RESULTS SET GPA = @GPA, Remarks = @Remarks 
                               WHERE ResultID = @ResultID", conn);
        cmd.Parameters.AddWithValue("@ResultID", r.ResultID);
        cmd.Parameters.AddWithValue("@GPA", r.GPA);
        cmd.Parameters.AddWithValue("@Remarks", r.Remarks);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Result r = new();
        using SqlConnection conn = new(connStr);
        SqlCommand cmd = new("SELECT * FROM RESULTS WHERE ResultID = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            r.ResultID = (int)reader["ResultID"];
            r.StudentID = (int)reader["StudentID"];
        }
        conn.Close();
        return View(r);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        using SqlConnection conn = new(connStr);
        SqlCommand cmd = new("DELETE FROM RESULTS WHERE ResultID = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
        return RedirectToAction("List");
    }
}
