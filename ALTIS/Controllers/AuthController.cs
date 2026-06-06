using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

public class AuthController : Controller
{
    private readonly string connectionString = "Server=MACBOOKAIR-2017\\SQLEXPRESS;Database=DB_ALTIS;Trusted_Connection=True;";

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Email, string Password, string Role)
    {
        string query = Role switch
        {
            "Admin" => "SELECT AdminID FROM ADMINS WHERE StaffMail = @Email AND StaffPassword = @Password",
            "Faculty" => "SELECT FacultyID FROM FACULTY WHERE FacultyMail = @Email AND FacultyPassword = @Password",
            "Student" => "SELECT StudentID FROM STUDENTS WHERE StudentMail = @Email AND StudentPassword = @Password",
            _ => ""
        };

        if (string.IsNullOrWhiteSpace(query))
        {
            ViewBag.Error = "Invalid role selected.";
            return View();
        }

        using SqlConnection conn = new SqlConnection(connectionString);
        using SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Email", Email);
        cmd.Parameters.AddWithValue("@Password", Password);

        conn.Open();
        var result = cmd.ExecuteScalar();

        if (result != null)
        {
            int userId = Convert.ToInt32(result);

            // ✅ Store in session
            HttpContext.Session.SetInt32("UserID", userId);
            HttpContext.Session.SetString("UserRole", Role);

            // ✅ Redirect to dashboard based on role
            return Role switch
            {
                "Admin" => RedirectToAction("DashboardAdmin"),
                "Faculty" => RedirectToAction("DashboardFaculty"),
                "Student" => RedirectToAction("DashboardStudent"),
                _ => RedirectToAction("Login")
            };
        }
        else
        {
            ViewBag.Error = "Invalid credentials. Please try again.";
            return View();
        }
    }

    public IActionResult DashboardAdmin()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
            return RedirectToAction("Login");

        return View();
    }

    public IActionResult DashboardFaculty()
    {
        if (HttpContext.Session.GetString("UserRole") != "Faculty")
            return RedirectToAction("Login");

        return View();
    }

    public IActionResult DashboardStudent()
    {
        if (HttpContext.Session.GetString("UserRole") != "Student")
            return RedirectToAction("Login");

        ViewBag.StudentId = HttpContext.Session.GetInt32("UserID");
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
