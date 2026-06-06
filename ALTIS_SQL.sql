-- Use ALTIS Database
USE DB_ALTIS;
GO

-- ===============================================
--              TABLE DEFINITIONS
-- ===============================================

-- 1. ADMINS
CREATE TABLE ADMINS (
    AdminID INT PRIMARY KEY,
    StaffName VARCHAR(100),
    StaffMail VARCHAR(100) UNIQUE,
    StaffPassword VARCHAR(100)
);

-- 2. SECTIONS
CREATE TABLE SECTIONS (
    SectionID VARCHAR(10) PRIMARY KEY,
    SectionName VARCHAR(50)
);

-- 3. STUDENTS
CREATE TABLE STUDENTS (
    StudentID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Contact VARCHAR(20),
    StudentMail VARCHAR(100) UNIQUE,
    StudentPassword VARCHAR(100),
    Address VARCHAR(200),
    SectionID VARCHAR(10),
    FOREIGN KEY (SectionID) REFERENCES SECTIONS(SectionID)
);

-- 4. FACULTY
CREATE TABLE FACULTY (
    FacultyID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Contact VARCHAR(20),
    FacultyMail VARCHAR(100) UNIQUE,
    FacultyPassword VARCHAR(100),
    Address VARCHAR(200)
);

-- 5. COURSES
CREATE TABLE COURSES (
    CourseID INT PRIMARY KEY,
    CourseName VARCHAR(100)
);

-- 6. COURSE_ASSIGNMENTS
CREATE TABLE COURSE_ASSIGNMENTS (
    AssignmentID INT PRIMARY KEY,
    CourseID INT,
    FacultyID INT,
    SectionID VARCHAR(10),
    AdminID INT,
    Semester VARCHAR(20),
    FOREIGN KEY (CourseID) REFERENCES COURSES(CourseID),
    FOREIGN KEY (FacultyID) REFERENCES FACULTY(FacultyID),
    FOREIGN KEY (SectionID) REFERENCES SECTIONS(SectionID),
    FOREIGN KEY (AdminID) REFERENCES ADMINS(AdminID)
);

-- 7. CLASSROOMS
CREATE TABLE CLASSROOMS (
    ClassroomID INT PRIMARY KEY,
    CourseID INT,
    FacultyID INT,
    SectionID VARCHAR(10),
    Title VARCHAR(100),
    FOREIGN KEY (CourseID) REFERENCES COURSES(CourseID),
    FOREIGN KEY (FacultyID) REFERENCES FACULTY(FacultyID),
    FOREIGN KEY (SectionID) REFERENCES SECTIONS(SectionID)
);

-- 8. ENROLLMENTS
CREATE TABLE ENROLLMENTS (
    EnrollmentID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT,
    ClassroomID INT,
    FOREIGN KEY (StudentID) REFERENCES STUDENTS(StudentID),
    FOREIGN KEY (ClassroomID) REFERENCES CLASSROOMS(ClassroomID)
);

-- 9. ASSIGNMENTS
CREATE TABLE ASSIGNMENTS (
    AssignmentID INT PRIMARY KEY,
    ClassroomID INT,
    Title VARCHAR(100),
    Description TEXT,
    DueDate DATE,
    FOREIGN KEY (ClassroomID) REFERENCES CLASSROOMS(ClassroomID)
);

-- 10. SUBMISSIONS
CREATE TABLE SUBMISSIONS (
    SubmissionID INT PRIMARY KEY,
    AssignmentID INT,
    StudentID INT,
    SubmittedAt DATE,
    FileName VARCHAR(255),
    FOREIGN KEY (AssignmentID) REFERENCES ASSIGNMENTS(AssignmentID),
    FOREIGN KEY (StudentID) REFERENCES STUDENTS(StudentID)
);

-- 11. ATTENDANCE
CREATE TABLE ATTENDANCE (
    AttendanceID INT IDENTITY(1,1) PRIMARY KEY,
    ClassroomID INT,
    StudentID INT,
    Date DATE,
    Status VARCHAR(10),
    FOREIGN KEY (ClassroomID) REFERENCES CLASSROOMS(ClassroomID),
    FOREIGN KEY (StudentID) REFERENCES STUDENTS(StudentID)
);

-- 12. MARKS
CREATE TABLE MARKS (
    MarkID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT,
    AssignmentID INT,
    ClassroomID INT,
    Score FLOAT CHECK (Score >= 0 AND Score <= 100),
    FOREIGN KEY (StudentID) REFERENCES STUDENTS(StudentID),
    FOREIGN KEY (AssignmentID) REFERENCES ASSIGNMENTS(AssignmentID),
    FOREIGN KEY (ClassroomID) REFERENCES CLASSROOMS(ClassroomID)
);

-- 13. RESULTS
CREATE TABLE RESULTS (
    ResultID INT PRIMARY KEY,
    StudentID INT,
    AdminID INT,
    GPA FLOAT CHECK (GPA >= 0.0 AND GPA <= 4.0),
    Remarks VARCHAR(200),
    FOREIGN KEY (StudentID) REFERENCES STUDENTS(StudentID),
    FOREIGN KEY (AdminID) REFERENCES ADMINS(AdminID)
);

-- 14. ANNOUNCEMENTS
CREATE TABLE ANNOUNCEMENTS (
    AnnouncementID INT PRIMARY KEY IDENTITY(1,1),
    ClassroomID INT NOT NULL,
    Title VARCHAR(200),
    Message TEXT,
    PostedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClassroomID) REFERENCES CLASSROOMS(ClassroomID)
);

-- 15. ANNOUNCEMENT_COMMENTS
CREATE TABLE ANNOUNCEMENT_COMMENTS (
    CommentID INT IDENTITY(1,1) PRIMARY KEY,
    AnnouncementID INT NOT NULL,
    StudentID INT NOT NULL,
    Message TEXT NOT NULL,
    CommentedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AnnouncementID) REFERENCES ANNOUNCEMENTS(AnnouncementID),
    FOREIGN KEY (StudentID) REFERENCES STUDENTS(StudentID)
);

-- ===============================================
--              INSERTING SAMPLE DATA
-- ===============================================

-- Admins
INSERT INTO ADMINS (AdminID, StaffName, StaffMail, StaffPassword) VALUES
(1, 'Haseeb Jalil', 'haseebjalil@admin.au.edu.pk', 'adm123'),
(2, 'Taimur Sajid', 'taimursajid@admin.au.edu.pk', 'adm456');

-- Sections
INSERT INTO SECTIONS (SectionID, SectionName) VALUES
('CS-A', 'Computer Science - A'),
('CS-B', 'Computer Science - B');

-- Faculty
INSERT INTO FACULTY (FacultyID, FirstName, LastName, Contact, FacultyMail, FacultyPassword, Address) VALUES
(101, 'Mr. Junaid', 'Nazar', '03001234567', 'junaidnazar@faculty.au.edu.pk', 'fcu123', 'Islamabad'),
(102, 'Dr. Sumera', 'Hayat', '03111234567', 'sumerahayat@faculty.au.edu.pk', 'fcu456', 'Rawalpindi'),
(103, 'Ms. Sana', 'Iqbal', '03021234567', 'sana.iqbal@faculty.au.edu.pk', 'fcu789', 'Lahore'),
(104, 'Dr. Adil', 'Saeed', '03031234567', 'adil.saeed@faculty.au.edu.pk', 'fcu321', 'Karachi');


-- Courses
INSERT INTO COURSES (CourseID, CourseName) VALUES
(201, 'Object Oriented Programming'),
(202, 'Database Systems'),
(203, 'Web Development'),
(204, 'Expository Writing');


-- Students
INSERT INTO STUDENTS (StudentID, FirstName, LastName, Contact, StudentMail, StudentPassword, Address, SectionID) VALUES
(241854, 'Abdul', 'Raheem', '03001234567', '241854@students.au.edu.pk', 'stu123', 'Multan', 'CS-A'),
(241791, 'Taimoor', 'Gillani', '03001234567', '241791@students.au.edu.pk', 'stu456', 'Multan', 'CS-A'),
(241813, 'Ali', 'Zafar', '03001234567', '241813@students.au.edu.pk', 'stu789', 'Faisalabad', 'CS-B'),
(241804, 'Abdul', 'Moiz', '03001234567', '241804@students.au.edu.pk', 'stu987', 'Faisalabad', 'CS-B'),
(241805, 'Hira', 'Butt', '03128458878', '241805@students.au.edu.pk', 'stu905', 'Peshawar', 'CS-A'),
(241806, 'Ahmed', 'Malik', '03876353712', '241806@students.au.edu.pk', 'stu138', 'Peshawar', 'CS-A'),
(241807, 'Hira', 'Butt', '03934343345', '241807@students.au.edu.pk', 'stu382', 'Karachi', 'CS-B'),
(241808, 'Hamza', 'Ali', '03845869651', '241808@students.au.edu.pk', 'stu998', 'Quetta', 'CS-B'),
(241809, 'Zara', 'Ali', '03974170580', '241809@students.au.edu.pk', 'stu137', 'Hyderabad', 'CS-B'),
(241810, 'Hamza', 'Malik', '03601830540', '241810@students.au.edu.pk', 'stu660', 'Quetta', 'CS-A');

-- Course Assignments
INSERT INTO COURSE_ASSIGNMENTS (AssignmentID, CourseID, FacultyID, SectionID, AdminID, Semester) VALUES
(301, 201, 102, 'CS-A', 1, 'Spring 2025'),
(302, 202, 101, 'CS-B', 2, 'Fall 2025'),
(303, 203, 103, 'CS-A', 1, 'Spring 2025'),
(304, 204, 104, 'CS-B', 2, 'Fall 2025');


-- Classrooms
INSERT INTO CLASSROOMS (ClassroomID, CourseID, FacultyID, SectionID, Title) VALUES
(501, 201, 102, 'CS-A', 'OOP - Spring 2025 - CS-A'),
(502, 202, 101, 'CS-B', 'DBMS - Fall 2025 - CS-B'),
(503, 203, 103, 'CS-A', 'Web Dev - Spring 2025 - CS-A'),
(504, 204, 104, 'CS-B', 'Expository Writing - Fall 2025 - CS-B');


-- Assignments
INSERT INTO ASSIGNMENTS (AssignmentID, ClassroomID, Title, Description, DueDate) VALUES
(601, 501, 'Phase 1 Project', 'Submit OOP Project Phase 1', '2025-06-20'),
(602, 502, 'ERD Assignment', 'Submit complete ERD and EERD diagrams', '2025-06-18');

-- Submissions
INSERT INTO SUBMISSIONS (SubmissionID, AssignmentID, StudentID, SubmittedAt, FileName) VALUES
(701, 601, 241813, '2025-06-15', 'oop_phase1.pdf'),
(702, 601, 241804, '2025-06-16', 'oop_phase2.docx'),
(703, 602, 241791, '2025-06-17', 'dbms_assignment.zip'),
(704, 602, 241805, '2025-06-14', 'submission_1.pdf'),
(705, 602, 241806, '2025-06-18', 'submission_2.pdf'),
(706, 601, 241807, '2025-06-12', 'submission_3.pdf'),
(707, 601, 241808, '2025-06-14', 'submission_4.pdf'),
(708, 601, 241809, '2025-06-13', 'submission_5.pdf'),
(709, 601, 241810, '2025-06-14', 'submission_6.pdf');


-- Attendance
INSERT INTO ATTENDANCE (ClassroomID, StudentID, Date, Status) VALUES
(501, 241854, '2025-06-10', 'Present'),
(501, 241791, '2025-06-10', 'Absent'),
(502, 241813, '2025-06-10', 'Present'),
(502, 241805, '2025-06-10', 'Present'),
(502, 241806, '2025-06-10', 'Present'),
(501, 241807, '2025-06-10', 'Present'),
(501, 241808, '2025-06-10', 'Present'),
(501, 241809, '2025-06-10', 'Absent'),
(501, 241810, '2025-06-10', 'Absent');

-- Marks
INSERT INTO MARKS (StudentID, AssignmentID, ClassroomID, Score) VALUES
(241854, 601, 501, 88.5),
(241791, 601, 501, 75.0),
(241813, 602, 502, 92.0),
(241805, 602, 502, 76.3),
(241806, 602, 502, 71.6),
(241807, 601, 501, 82.4),
(241808, 601, 501, 76.5),
(241809, 601, 501, 72.0),
(241810, 601, 501, 78.7);


-- Results
INSERT INTO RESULTS (ResultID, StudentID, AdminID, GPA, Remarks) VALUES
(801, 241854, 1, 3.5, 'Good performance'),
(802, 241791, 1, 3.0, 'Satisfactory'),
(803, 241813, 1, 3.8, 'Excellent'),
(804, 241805, 2, 3.4, 'Excellent'),
(805, 241806, 2, 2.6, 'Good performance'),
(806, 241807, 2, 4.0, 'Satisfactory'),
(807, 241808, 1, 3.0, 'Excellent'),
(808, 241809, 2, 2.9, 'Excellent'),
(809, 241810, 1, 3.0, 'Excellent');

-- Announcements
INSERT INTO ANNOUNCEMENTS (ClassroomID, Title, Message) VALUES
(501, 'Midterm Reminder', 'OOP Midterm will be conducted on 20th June.'),
(502, 'DBMS Assignment', 'Submit your ERD by 18th June.');

-- Comments on Announcements
INSERT INTO ANNOUNCEMENT_COMMENTS (AnnouncementID, StudentID, Message) VALUES
(1, 241854, 'Thank you for the reminder!'),
(2, 241813, 'I have a question regarding the dataset.'),
(1, 241805, 'Thanks for the update!'),
(2, 241806, 'Can you clarify the submission time?'),
(2, 241807, 'Thanks for the update!'),
(2, 241808, 'Can you clarify the submission time?'),
(1, 241809, 'Thanks for the update!'),
(1, 241810, 'Can you clarify the submission time?');


-- ===============================================
--              Read Queries
-- ===============================================

-- Get all students with their section information
SELECT s.StudentID, s.FirstName, s.LastName, s.Contact, s.StudentMail, sec.SectionName
FROM STUDENTS s
JOIN SECTIONS sec ON s.SectionID = sec.SectionID;

-- Get faculty with their assigned courses
SELECT f.FacultyID, f.FirstName, f.LastName, c.CourseName, ca.Semester
FROM FACULTY f
JOIN COURSE_ASSIGNMENTS ca ON f.FacultyID = ca.FacultyID
JOIN COURSES c ON ca.CourseID = c.CourseID;

-- Get student enrollments with classroom details
SELECT s.StudentID, s.FirstName, s.LastName, cl.Title AS Classroom, c.CourseName
FROM STUDENTS s
JOIN ENROLLMENTS e ON s.StudentID = e.StudentID
JOIN CLASSROOMS cl ON e.ClassroomID = cl.ClassroomID
JOIN COURSES c ON cl.CourseID = c.CourseID;

-- Get assignments with due dates for a specific classroom
SELECT a.AssignmentID, a.Title, a.Description, a.DueDate
FROM ASSIGNMENTS a
WHERE a.ClassroomID = 501;

-- Get student submissions with marks
SELECT s.StudentID, st.FirstName, st.LastName, a.Title AS Assignment, 
       s.SubmittedAt, s.FileName, m.Score
FROM SUBMISSIONS s
JOIN STUDENTS st ON s.StudentID = st.StudentID
JOIN ASSIGNMENTS a ON s.AssignmentID = a.AssignmentID
LEFT JOIN MARKS m ON s.StudentID = m.StudentID AND s.AssignmentID = m.AssignmentID;

-- ===============================================
--              Update Queries
-- ===============================================

-- Update student contact information
UPDATE STUDENTS
SET Contact = '03009876543', Address = 'Lahore'
WHERE StudentID = 241854;

-- Update assignment due date
UPDATE ASSIGNMENTS
SET DueDate = '2025-06-25'
WHERE AssignmentID = 601;

-- Update faculty information
UPDATE FACULTY
SET Contact = '03331234567', Address = 'Islamabad'
WHERE FacultyID = 101;

-- Update student marks
UPDATE MARKS
SET Score = 90.0
WHERE StudentID = 241791 AND AssignmentID = 601;

-- Update attendance status
UPDATE ATTENDANCE
SET Status = 'Present'
WHERE StudentID = 241791 AND Date = '2025-06-10';

-- ===============================================
--              Delete Queries
-- ===============================================

-- Delete a student submission
DELETE FROM SUBMISSIONS
WHERE SubmissionID = 703;

-- Delete an announcement comment
DELETE FROM ANNOUNCEMENT_COMMENTS
WHERE CommentID = 2;

-- Delete a classroom enrollment
DELETE FROM ENROLLMENTS
WHERE StudentID = 241854 AND ClassroomID = 501;

-- Delete an assignment (cascade will handle related submissions and marks)
DELETE FROM ASSIGNMENTS
WHERE AssignmentID = 602;

-- Delete a course assignment
DELETE FROM COURSE_ASSIGNMENTS
WHERE AssignmentID = 302;

-- ===============================================
--              Join Queries
-- ===============================================

-- 1. Query to get students with their enrolled courses, faculty, and marks
SELECT 
    s.StudentID, 
    s.FirstName + ' ' + s.LastName AS StudentName,
    c.CourseName,
    f.FirstName + ' ' + f.LastName AS FacultyName,
    cl.Title AS Classroom,
    a.Title AS Assignment,
    m.Score
FROM 
    STUDENTS s
JOIN 
    ENROLLMENTS e ON s.StudentID = e.StudentID
JOIN 
    CLASSROOMS cl ON e.ClassroomID = cl.ClassroomID
JOIN 
    COURSES c ON cl.CourseID = c.CourseID
JOIN 
    FACULTY f ON cl.FacultyID = f.FacultyID
LEFT JOIN 
    ASSIGNMENTS a ON cl.ClassroomID = a.ClassroomID
LEFT JOIN 
    MARKS m ON s.StudentID = m.StudentID AND a.AssignmentID = m.AssignmentID
ORDER BY 
    s.StudentID, c.CourseName;

-- 2. Query to get faculty with their assigned courses and sections
SELECT 
    f.FacultyID,
    f.FirstName + ' ' + f.LastName AS FacultyName,
    c.CourseName,
    sec.SectionName,
    ca.Semester,
    COUNT(DISTINCT e.StudentID) AS EnrolledStudents
FROM 
    FACULTY f
JOIN 
    COURSE_ASSIGNMENTS ca ON f.FacultyID = ca.FacultyID
JOIN 
    COURSES c ON ca.CourseID = c.CourseID
JOIN 
    SECTIONS sec ON ca.SectionID = sec.SectionID
LEFT JOIN 
    CLASSROOMS cl ON c.CourseID = cl.CourseID AND f.FacultyID = cl.FacultyID AND sec.SectionID = cl.SectionID
LEFT JOIN 
    ENROLLMENTS e ON cl.ClassroomID = e.ClassroomID
GROUP BY 
    f.FacultyID, f.FirstName, f.LastName, c.CourseName, sec.SectionName, ca.Semester
ORDER BY 
    f.FacultyID;

-- 3. Query to get announcements with comments and commenters
SELECT 
    a.AnnouncementID,
    cl.Title AS Classroom,
    a.Title AS AnnouncementTitle,
    a.Message AS AnnouncementContent,
    a.PostedAt,
    s.FirstName + ' ' + s.LastName AS CommenterName,
    ac.Message AS Comment,
    ac.CommentedAt
FROM 
    ANNOUNCEMENTS a
JOIN 
    CLASSROOMS cl ON a.ClassroomID = cl.ClassroomID
LEFT JOIN 
    ANNOUNCEMENT_COMMENTS ac ON a.AnnouncementID = ac.AnnouncementID
LEFT JOIN 
    STUDENTS s ON ac.StudentID = s.StudentID
ORDER BY 
    a.AnnouncementID, ac.CommentedAt;