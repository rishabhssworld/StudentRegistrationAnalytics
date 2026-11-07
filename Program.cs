using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// This program manages student registrations, allowing users to add, update, remove, and view registrations. It also provides analytics such as enrolled students with high marks, registration counts by course, and enrolled mark statistics.
/// </summary>
class Program
{
    static List<StudentRegistration> registrations = new()
    {
        new() { StudentId = "1001", FullName = "Alex Smith",
            CourseCode = "CSC101", Credits = 3, Mark = 85,
            Status = "Enrolled" },

        new() { StudentId = "1002", FullName = "Bella Patel",
            CourseCode = "CSC101", Credits = 3, Mark = 92,
            Status = "Enrolled" },

        new() { StudentId = "1003", FullName = "Chris Lee",
            CourseCode = "MAT101", Credits = 4, Mark = 78,
            Status = "Enrolled" },

        new() { StudentId = "1004", FullName = "Diana Khan",
            CourseCode = "WEB101", Credits = 3, Mark = 85,
            Status = "Enrolled" },

        new() { StudentId = "1005", FullName = "Ethan Brown",
            CourseCode = "MAT101", Credits = 4, Mark = 65,
            Status = "Enrolled" },

        new() { StudentId = "1006", FullName = "Fatima Ali",
            CourseCode = "WEB101", Credits = 3, Mark = 70,
            Status = "Enrolled" },

        new() { StudentId = "1007", FullName = "George Wilson",
            CourseCode = "CSC101", Credits = 3, Mark = 0,
            Status = "Waitlisted" },

        new() { StudentId = "1008", FullName = "Hannah Chen",
            CourseCode = "MAT101", Credits = 4, Mark = 40,
            Status = "Withdrawn" },

        new() { StudentId = "1009", FullName = "Ishaan Sharma",
            CourseCode = "WEB101", Credits = 3, Mark = 0,
            Status = "Waitlisted" },

        new() { StudentId = "1010", FullName = "Julia Martin",
            CourseCode = "MAT101", Credits = 4, Mark = 0,
            Status = "Waitlisted" }
    };

    // Dictionary for quick lookup of registrations by student ID.

    static Dictionary<string, StudentRegistration> registrationsById = new(StringComparer.OrdinalIgnoreCase);

    // Stack to keep track of the latest three successful actions.

    static Stack<string> actionHistory = new();

    // Queue to manage waitlist requests.
    static Queue<string> waitlistRequests = new();


    static void Main()
    {
        // Initialize the dictionary and waitlist queue based on the initial registrations.
        InitializeDictionary();
        InitializeWaitlist();
        bool running = true;

        //Main menu loop to handle user input and perform actions.
        while (running)
        {
            Console.WriteLine("\n--- Registration Menu ---");
            Console.WriteLine("1. List registrations");
            Console.WriteLine("2. Add registration");
            Console.WriteLine("3. Update mark");
            Console.WriteLine("4. Remove registration");
            Console.WriteLine("5. Preview action history");
            Console.WriteLine("6. Process waitlist");
            Console.WriteLine("7. Show enrolled students with marks >= 70");
            Console.WriteLine("8. Show names, courses and marks");
            Console.WriteLine("9. Show registration counts by course");
            Console.WriteLine("10. Show enrolled mark statistics");
            Console.WriteLine("11. Search by course code");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            // Read user input and handle null values.

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    ListRegistrations();
                    break;

                case "2":
                    AddRegistration();
                    break;

                case "3":
                    UpdateMark();
                    break;

                case "4":
                    RemoveRegistration();
                    break;

                case "5":
                    ShowActionHistory();
                    break;

                case "6":
                    ProcessWaitlist();
                    break;

                case "7":
                    ShowHighMarks();
                    break;

                case "8":
                    ShowProjectedReport();
                    break;

                case "9":
                    ShowCourseCounts();
                    break;

                case "10":
                    ShowEnrolledStatistics();
                    break;

                case "11":
                    SearchByCourse();
                    break;

                case "0":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    // Displays the list of all student registrations in a formatted manner.
    static void ListRegistrations()
    {
        Console.WriteLine("\n--- Student Registrations ---");

        if (registrations.Count == 0)
        {
            Console.WriteLine("No registrations found.");
            return;
        }

        foreach (StudentRegistration registration in registrations)
        {
            Console.WriteLine(
                $"{registration.StudentId} | {registration.FullName} | " +
                $"{registration.CourseCode} | Credits: {registration.Credits} | " +
                $"Mark: {registration.Mark} | {registration.Status}");
        }
    }

    // Initializes the dictionary for quick lookup of registrations by student ID.
    static void InitializeDictionary()
    {
        foreach (StudentRegistration registration in registrations)
        {
            registrationsById.Add(
                registration.StudentId,
                registration);
        }
    }

    static int ReadNumber(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int number)
                && number >= minimum
                && number <= maximum)
            {
                return number;
            }

            Console.WriteLine(
                $"Please enter a whole number from {minimum} to {maximum}.");
        }
    }

    static string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? "").Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.WriteLine("This field cannot be empty.");
        }
    }

    static void AddRegistration()
    {
        Console.WriteLine("\n--- Add Registration ---");

        string studentId = ReadRequiredText("Enter student ID: ");

        if (registrationsById.ContainsKey(studentId))
        {
            Console.WriteLine("A registration with this student ID already exists.");
            return;
        }

        string fullName = ReadRequiredText("Enter full name: ");
        string courseCode = ReadRequiredText("Enter course code: ").ToUpperInvariant();

        int credits = ReadNumber("Enter credits (1–6): ", 1, 6);
        int mark = ReadNumber("Enter mark (0–100): ", 0, 100);

        Console.WriteLine("1. Enrolled");
        Console.WriteLine("2. Waitlisted");
        Console.WriteLine("3. Withdrawn");

        int statusChoice = ReadNumber("Choose status (1–3): ", 1, 3);

        string status = statusChoice switch
        {
            1 => "Enrolled",
            2 => "Waitlisted",
            _ => "Withdrawn"
        };

        StudentRegistration registration = new()
        {
            StudentId = studentId,
            FullName = fullName,
            CourseCode = courseCode,
            Credits = credits,
            Mark = mark,
            Status = status
        };

        // Keep the list and dictionary synchronized.
        registrations.Add(registration);
        registrationsById.Add(studentId, registration);
        if (registration.Status == "Waitlisted")
        {
            waitlistRequests.Enqueue(studentId);
        }
        actionHistory.Push($"Added registration: {studentId}");

        Console.WriteLine("Registration added successfully.");
    }

    // Find a student by ID and update the mark after validating its range.
    static void UpdateMark()
    {
        Console.WriteLine("\n--- Update Mark ---");

        string studentId = ReadRequiredText("Enter student ID: ");

        if (!registrationsById.TryGetValue(studentId, out var registration))
        {
            Console.WriteLine("No registration found with that student ID.");
            return;
        }

        Console.WriteLine(
            $"{registration.FullName} — Current mark: {registration.Mark}");

        int newMark = ReadNumber("Enter new mark (0–100): ", 0, 100);

        int oldMark = registration.Mark;
        // Both collections reference this object, so the list reflects this change too.
        registration.Mark = newMark;
        actionHistory.Push($"Updated mark: {studentId}, {oldMark} -> {newMark}");

        Console.WriteLine(
            $"Mark updated successfully: {oldMark} -> {newMark}");
    }

    // Remove a registration from both the list and dictionary.
    static void RemoveRegistration()
    {
        Console.WriteLine("\n--- Remove Registration ---");

        string studentId = ReadRequiredText("Enter student ID: ");

        if (!registrationsById.ContainsKey(studentId))
        {
            Console.WriteLine("No registration found with that student ID.");
            return;
        }

        // Use the index to remove the matching record from the list.
        for (int i = 0; i < registrations.Count; i++)
        {
            if (string.Equals(
                registrations[i].StudentId,
                studentId,
                StringComparison.OrdinalIgnoreCase))
            {
                string fullName = registrations[i].FullName;

                // Remove from both stores, then return before continuing the changed list.
                registrations.RemoveAt(i);
                registrationsById.Remove(studentId);
                actionHistory.Push($"Removed registration: {studentId}");

                Console.WriteLine(
                    $"Registration removed successfully for {fullName}.");
                return;
            }
        }
    }
    //Preview up to three successful actions in LIFO order; no actual undo is performed.
    static void ShowActionHistory()
    {
        Console.WriteLine("\n--- Latest Three Successful Actions ---");

        if (actionHistory.Count == 0)
        {
            Console.WriteLine("No successful actions recorded yet.");
            return;
        }

        int displayed = 0;

        // Stack enumeration shows the newest action first.
        foreach (string action in actionHistory)
        {
            Console.WriteLine(action);
            displayed++;

            // Keep the stored history but stop displaying after the third entry.
            if (displayed == 3)
            {
                break;
            }
        }
    }

    // Queue seeded waitlisted students in their order in the list.
    static void InitializeWaitlist()
    {
        foreach (StudentRegistration registration in registrations)
        {
            if (registration.Status == "Waitlisted")
            {
                waitlistRequests.Enqueue(registration.StudentId);
            }
        }
    }

    // Process all queued requests in FIFO order and enroll valid waitlisted students.
    static void ProcessWaitlist()
    {
        Console.WriteLine("\n--- Process Waitlist ---");

        if (waitlistRequests.Count == 0)
        {
            Console.WriteLine("No waitlist requests remaining.");
            return;
        }

        while (waitlistRequests.Count > 0)
        {
            // Dequeue removes the oldest request first.
            string studentId = waitlistRequests.Dequeue();

            // A queued registration may have been deleted.
            if (!registrationsById.TryGetValue(studentId, out var registration)
                || registration.Status != "Waitlisted")
            {
                Console.WriteLine($"Skipped inactive request: {studentId}");
                continue;
            }

            // This simplified model enrolls the student without a course-capacity check
            registration.Status = "Enrolled";

            actionHistory.Push($"Enrolled waitlisted student: {studentId}");

            Console.WriteLine(
                $"Processed {studentId}: {registration.FullName} is now Enrolled.");
        }
    }

    // Filter enrolled students with marks of at least 70 and sort tied marks by name.
    static void ShowHighMarks()
    {
        Console.WriteLine("\n--- Enrolled Students: Mark >= 70 ---");

        var results = registrations
            .Where(r => r.Status == "Enrolled" && r.Mark >= 70)
            .OrderByDescending(r => r.Mark)
            .ThenBy(r => r.FullName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No enrolled students have marks of 70 or higher.");
            return;
        }

        foreach (var registration in results)
        {
            Console.WriteLine(
                $"{registration.FullName} | {registration.CourseCode} | " +
                $"Mark: {registration.Mark}");
        }
    }

    // Project each registration into a result containing only the three required fields.
    static void ShowProjectedReport()
    {
        Console.WriteLine("\n--- Names, Courses and Marks ---");

        var results = registrations
            .Select(r => new
            {
                r.FullName,
                r.CourseCode,
                r.Mark
            })
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No registrations available.");
            return;
        }

        foreach (var registration in results)
        {
            Console.WriteLine(
                $"{registration.FullName} | {registration.CourseCode} | " +
                $"Mark: {registration.Mark}");
        }
    }

    // Group course codes without case sensitivity and count records across all statuses.
    static void ShowCourseCounts()
    {
        Console.WriteLine("\n--- Registration Counts by Course ---");

        var results = registrations
            .GroupBy(r => r.CourseCode, StringComparer.OrdinalIgnoreCase)
            .Select(group => new
            {
                CourseCode = group.Key,
                RegistrationCount = group.Count()
            })
            .OrderBy(r => r.CourseCode)
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No registrations available.");
            return;
        }

        foreach (var course in results)
        {
            Console.WriteLine(
                $"{course.CourseCode}: {course.RegistrationCount} registrations");
        }
    }

    // Calculate the average, highest and lowest marks for enrolled students only.
    static void ShowEnrolledStatistics()
    {
        Console.WriteLine("\n--- Enrolled Mark Statistics ---");

        var enrolled = registrations
            .Where(r => r.Status == "Enrolled")
            .ToList();

        // Average, Min and Max require a non-empty sequence.
        if (enrolled.Count == 0)
        {
            Console.WriteLine("No enrolled students available.");
            return;
        }

        double average = enrolled.Average(r => r.Mark);
        int highest = enrolled.Max(r => r.Mark);
        int lowest = enrolled.Min(r => r.Mark);

        Console.WriteLine($"Enrolled students: {enrolled.Count}");
        // F2 displays the average with two decimal places.
        Console.WriteLine($"Average mark: {average:F2}");
        Console.WriteLine($"Highest mark: {highest}");
        Console.WriteLine($"Lowest mark: {lowest}");
    }

    // Find course registrations using an exact, case-insensitive course code comparison.
    static void SearchByCourse()
    {
        Console.WriteLine("\n--- Search by Course Code ---");

        string courseCode = ReadRequiredText("Enter course code: ");

        var results = registrations
            .Where(r => string.Equals(
                r.CourseCode,
                courseCode,
                StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No registrations found for that course.");
            return;
        }

        foreach (var registration in results)
        {
            Console.WriteLine(
                $"{registration.StudentId} | {registration.FullName} | " +
                $"{registration.CourseCode} | Mark: {registration.Mark} | " +
                $"{registration.Status}");
        }
    }
}