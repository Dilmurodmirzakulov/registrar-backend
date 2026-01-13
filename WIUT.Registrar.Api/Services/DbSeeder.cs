using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Services;

public static class DbSeeder
{
    public static void Seed(AppDbContext dbContext)
    {
        // News
        if (!dbContext.News.Any())
        {
            dbContext.News.AddRange([
                new News
                {
                    Title = "Welcome to WIUT Academic Registrar",
                    Slug = "welcome-to-wiut-registrar",
                    Summary = "Launching our refreshed Registrar portal",
                    BodyHtml = "<p>Welcome to the new WIUT Academic Registrar website! Explore news, policies, and services.</p>",
                    IsPublished = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-2),
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new News
                {
                    Title = "Admissions 2025 now open",
                    Slug = "admissions-2025-open",
                    Summary = "Undergraduate and postgraduate admissions are open.",
                    BodyHtml = "<p>Applications are open. Review requirements and deadlines.</p>",
                    IsPublished = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            ]);
        }

        // Departments
        if (!dbContext.Departments.Any())
        {
            var registrarDept = new Department { Name = "Academic Registrar Office", CreatedAt = DateTime.UtcNow };
            var admissionsDept = new Department { Name = "Admissions", CreatedAt = DateTime.UtcNow };
            dbContext.Departments.AddRange(registrarDept, admissionsDept);
            dbContext.SaveChanges();

            // Team Members
            if (!dbContext.TeamMembers.Any())
            {
                dbContext.TeamMembers.AddRange([
                    new TeamMember
                    {
                        FirstName = "Dilmurod",
                        LastName = "Mirzoqulov",
                        Email = "dmirzoqulov@wiut.uz",
                        Title = "Registrar Staff",
                        DepartmentId = registrarDept.Id,
                        Phone = "+998 71 238 74 00",
                        CreatedAt = DateTime.UtcNow
                    },
                    new TeamMember
                    {
                        FirstName = "Admin",
                        LastName = "Admissions",
                        Email = "admissions@wiut.uz",
                        Title = "Admissions Officer",
                        DepartmentId = admissionsDept.Id,
                        Phone = "+998 71 238 74 01",
                        CreatedAt = DateTime.UtcNow
                    }
                ]);
            }
        }

        // Pages
        if (!dbContext.Pages.Any())
        {
            dbContext.Pages.AddRange([
                new Page
                {
                    Title = "Academic Policies",
                    Slug = "academic-policies",
                    Type = PageType.Policies,
                    BodyHtml = "<h2>Academic Integrity</h2><p>All students must adhere to academic integrity policies.</p>",
                    CreatedAt = DateTime.UtcNow
                },
                new Page
                {
                    Title = "Student Records",
                    Slug = "student-records",
                    Type = PageType.Records,
                    BodyHtml = "<p>Request transcripts and verify documents via the Registrar Office.</p>",
                    CreatedAt = DateTime.UtcNow
                }
            ]);
        }

        dbContext.SaveChanges();

        // Site settings
        if (!dbContext.SiteSettings.Any())
        {
            dbContext.SiteSettings.AddRange([
                new SiteSetting { Key = "hero.title", Value = "WIUT Academic Registrar's Office", Category = "hero", CreatedAt = DateTime.UtcNow },
                new SiteSetting { Key = "hero.subtitle", Value = "WIUT Academic Registrar’s Office is a front line of the university providing excellence in students’ services in any designated ARO areas both externally and internally", Category = "hero", CreatedAt = DateTime.UtcNow },
                new SiteSetting { Key = "contact.emailPrimary", Value = "information@wiut.uz", Category = "contact", CreatedAt = DateTime.UtcNow },
                new SiteSetting { Key = "contact.emailSecondary", Value = "registry@wiut.uz", Category = "contact", CreatedAt = DateTime.UtcNow },
                new SiteSetting { Key = "contact.phone", Value = "+99871 238 74 33", Category = "contact", CreatedAt = DateTime.UtcNow },
                new SiteSetting { Key = "contact.hours", Value = "Mon-Fri: 9AM-6PM EST", Category = "contact", CreatedAt = DateTime.UtcNow }
            ]);
            dbContext.SaveChanges();
        }

        if (!dbContext.QuickLinks.Any())
        {
            dbContext.QuickLinks.AddRange([
                new QuickLink
                {
                    Title = "Services",
                    Description = "Comprehensive services offered by the Academic Registrar's Office.",
                    LinkUrl = "/services",
                    IconKey = "services",
                    ThemeKey = "purple",
                    DisplayOrder = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new QuickLink
                {
                    Title = "Calendar & Events",
                    Description = "Academic calendar highlights and important deadlines.",
                    LinkUrl = "/calendar",
                    IconKey = "calendar",
                    ThemeKey = "orange",
                    DisplayOrder = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new QuickLink
                {
                    Title = "Policies & Guides",
                    Description = "Academic policies, procedures, and guidelines.",
                    LinkUrl = "/policies",
                    IconKey = "policies",
                    ThemeKey = "teal",
                    DisplayOrder = 3,
                    CreatedAt = DateTime.UtcNow
                }
            ]);
            dbContext.SaveChanges();
        }
    }
}


