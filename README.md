# AsyncAcademy
Group Project

<!-- https://asyncacademy20241104160444.azurewebsites.net/ -->


Overview

This repository contains a .NET 8 Razor Pages application named AsyncAcademy plus a unit‑test project. The solution file references both the main web app and the test project
. The web project is defined by AsyncAcademy.csproj which includes dependencies for ASP.NET Core, Entity Framework Core, Identity, and Stripe payment integration. Unit tests live in AsyncAcademyTest with MSTest, Moq, and EF Core InMemory packages



Application Setup

Startup configuration is handled in Program.cs. It registers Razor Pages, controllers, EF Core with a SQL Server connection string, session state, and the Stripe API key, then runs seeding logic before starting the HTTP pipeline

The AsyncAcademyContext in Data/ exposes EF Core DbSets for entities such as User, Course, Enrollment, Assignment, Submission, and Payment.

Core Features

Razor Pages – User interfaces are in Pages/. For example, the login page uses a layout in Shared/LoginLayout and collects credentials for Account.Username and Account.Pass

Code-behind PageModels (e.g., LoginModel and SignupModel) handle form submissions, EF interactions, and session management.

Controllers – API-style endpoints exist in Controllers/, such as CourseController for enrolling and dropping courses and CheckOutController for Stripe payment sessions.

Models and Seed Data – Entity classes define database tables. SeedData.cs populates users, courses, departments, enrollments, assignments, and submissions when the database is empty. It hashes default passwords via PasswordHasher<User> before inserting sample instructors and students



Utilities – Utils/ contains helper classes like a custom ValidiateBirthdayAttribute for validating birthdays and a Noto helper for assembling notification data.

Static Assets – wwwroot/ hosts images, JavaScript, and uploaded submissions.

Testing – The AsyncAcademyTest project uses MSTest with EF Core’s InMemory provider. Tests cover signup, course creation, enrollment, assignments, and other behaviors (see UnitTestCourses.cs, UnitTestSignup.cs, etc.). A separate Python directory SeleniumUItests/ holds Selenium scripts to automate browser-based tests.

Getting Started

A newcomer should:

Understand the project structure: explore Program.cs for configuration, Data/ for the EF context, Models/ for entities, and Pages/ for Razor UI.

Set up a SQL Server connection: update appsettings.json or environment variables with a valid connection string (TITAN-CONNECTION-STRING) before running migrations.

Apply migrations and seed data: the first run seeds sample users, courses, and assignments.

Run unit tests: execute dotnet test within the solution to verify features. Selenium tests require a local or hosted instance to point the browser to.

Review Razor Pages: see how LoginModel and SignupModel interact with session state and database operations.

Next Steps to Explore

Authentication/Authorization – Investigate how login sessions are stored and how roles (IsProfessor) influence navigation and page access.

Payment Flow – Follow CheckOutController to see how Stripe is integrated and how payment records are updated.

Data Validation – Look at ValidiateBirthdayAttribute for custom validation examples and consider how other fields might be validated.

Extending Functionality – Add new pages or controllers following the patterns in Course Pages or Assignments.

The repository provides a basic but functional student/instructor management system with seeding scripts, session-based login, and a suite of unit tests to guide further development.
