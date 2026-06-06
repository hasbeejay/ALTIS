using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

public class AnnouncementController : Controller
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
    public IActionResult Add(Announcement a)
    {
        using SqlConnection conn = new(connectionString);
        string query = @"INSERT INTO ANNOUNCEMENTS (ClassroomID, Title, Message, PostedAt)
                         VALUES (@ClassroomID, @Title, @Message, @PostedAt)";
        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@ClassroomID", a.ClassroomID);
        cmd.Parameters.AddWithValue("@Title", a.Title);
        cmd.Parameters.AddWithValue("@Message", a.Message);
        cmd.Parameters.AddWithValue("@PostedAt", DateTime.Now);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List");
    }

    public IActionResult List()
    {
        List<Announcement> posts = new();

        using SqlConnection conn = new(connectionString);
        string query = @"SELECT A.*, C.Title AS ClassroomTitle
                         FROM ANNOUNCEMENTS A
                         JOIN CLASSROOMS C ON A.ClassroomID = C.ClassroomID
                         ORDER BY A.PostedAt DESC";

        using SqlCommand cmd = new(query, conn);
        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            posts.Add(new Announcement
            {
                AnnouncementID = (int)reader["AnnouncementID"],
                ClassroomID = (int)reader["ClassroomID"],
                Title = reader["Title"].ToString(),
                Message = reader["Message"].ToString(),
                PostedAt = Convert.ToDateTime(reader["PostedAt"]),
                ClassroomTitle = reader["ClassroomTitle"].ToString()
            });
        }

        return View(posts);
    }
}
