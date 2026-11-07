# Student Registration Analytics

A C# console application for managing student registrations and generating reports using collections and LINQ.

## Student Information

- **Name:** Rishabh Bhutani
- **Course:** STY1991 — Software Engineering Concepts
- **Assignment:** Assignment 3

## Features

- List all registrations
- Add registrations with unique student IDs
- Update student marks
- Remove registrations by student ID
- Preview the latest three successful actions
- Process waitlist requests in FIFO order
- Display enrolled students with marks of at least 70
- Show a projected report of names, courses and marks
- Count registrations by course
- Calculate enrolled students’ average, highest and lowest marks
- Search by course code without case sensitivity

## Collections Used

| Collection | Purpose |
|---|---|
| List | Primary registration storage |
| Dictionary | Lookup and duplicate checking by student ID |
| Queue | Process waitlist requests in arrival order |
| Stack | Preview successful actions, newest first |

## Validation

- Required text cannot be blank.
- Student IDs must be unique, ignoring case.
- Marks must be whole numbers from 0 to 100.
- Credits must be whole numbers from 1 to 6.
- Status must be Enrolled, Waitlisted or Withdrawn.
- Empty reports display a friendly message.

## Requirements

- .NET 10 SDK
- A terminal or an editor such as Visual Studio Code

## Run the Application

Clone the repository:

```bash
git clone https://github.com/rishabhssworld/StudentRegistrationAnalytics.git
cd StudentRegistrationAnalytics
```

Run the application:

```bash
dotnet run
```

Use the numbered menu to select an action. Enter `0` to exit.

## Project Files

- `Program.cs` — Menu, registration operations, validation and reports
- `StudentRegistration.cs` — Registration data model
- `StudentRegistrationAnalytics.csproj` — Project configuration
- `Assignment 3_report.pdf` — Design explanation, test matrix and screenshots

## Sample Data

The application starts with 10 registrations across three courses. It includes tied marks and three waitlisted students.

Data is stored in memory. Restarting the application restores the original sample data.

## Limitations

- One registration per student
- No database or permanent storage
- Waitlist processing does not check course capacity
- Action history is a preview; actual undo is not implemented
