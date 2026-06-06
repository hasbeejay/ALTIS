using ALTIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class StudentAnnouncementController : Controller
{
    private readonly string connStr =
        "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    public IActionResult List(int studentId = 1001) // hardcoded for now
    {
        List<Announcement> announcements = new();

        using SqlConnection conn = new(connStr);
        string query = @"
            SELECT A.*, C.Title AS ClassroomTitle
            FROM ANNOUNCEMENTS A
            JOIN CLASSROOMS C ON A.ClassroomID = C.ClassroomID
            WHERE A.ClassroomID IN (
                SELECT ClassroomID FROM ENROLLMENTS WHERE StudentID = @sid
            )
            ORDER BY PostedAt DESC";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@sid", studentId);
        conn.Open();

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            announcements.Add(new Announcement
            {
                AnnouncementID = (int)reader["AnnouncementID"],
                ClassroomID = (int)reader["ClassroomID"],
                Title = reader["Title"].ToString(),
                Message = reader["Message"].ToString(),
                PostedAt = Convert.ToDateTime(reader["PostedAt"]),
                ClassroomTitle = reader["ClassroomTitle"].ToString()
            });
        }

        return View(announcements);
    }

    [HttpPost]
    public IActionResult AddComment(Comment comment)
    {
        using SqlConnection conn = new(connStr);
        string query = @"INSERT INTO ANNOUNCEMENT_COMMENTS 
                         (AnnouncementID, StudentID, Message, CommentedAt)
                         VALUES (@aid, @sid, @msg, GETDATE())";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@aid", comment.AnnouncementID);
        cmd.Parameters.AddWithValue("@sid", comment.StudentID);
        cmd.Parameters.AddWithValue("@msg", comment.Message);

        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        return RedirectToAction("List", new { studentId = comment.StudentID });
    }

    public IActionResult ViewComments(int announcementId)
    {
        List<Comment> replies = new();

        using SqlConnection conn = new(connStr);
        string query = @"
            SELECT C.*, S.FirstName + ' ' + S.LastName AS StudentName
            FROM ANNOUNCEMENT_COMMENTS C
            JOIN STUDENTS S ON C.StudentID = S.StudentID
            WHERE C.AnnouncementID = @aid
            ORDER BY C.CommentedAt";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@aid", announcementId);
        conn.Open();

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            replies.Add(new Comment
            {
                CommentID = (int)reader["CommentID"],
                AnnouncementID = (int)reader["AnnouncementID"],
                StudentID = (int)reader["StudentID"],
                Message = reader["Message"].ToString(),
                CommentedAt = Convert.ToDateTime(reader["CommentedAt"]),
                StudentName = reader["StudentName"].ToString()
            });
        }

        return View(replies);
    }
}
