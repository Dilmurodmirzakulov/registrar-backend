# WIUT Academic Registrar Portal - Complete Fullstack Development Prompt

## PROJECT OVERVIEW
You are building a comprehensive fullstack University Academic Registrar Portal for Westminster International University in Tashkent (WIUT). This is a professional institutional web application with public-facing pages and a secure admin panel for managing academic operations.

---

## TECHNOLOGY STACK
- **Frontend**: React 19 with TypeScript
- **Styling**: Tailwind CSS (latest version)
- **UI Components**: shadcn/ui for accessible, customizable components
- **Backend**: .NET Core 9 Web API (ASP.NET Core)
- **Database**: MSSQL Server
- **Architecture**: Monorepo structure with separate frontend and backend folders
- **Package Manager**: npm or yarn

---

## BRAND GUIDELINES (FROM WIUT BRANDBOOK 2023)

### Colors
**Primary Colors:**
- Main Blue: #264F9D (RGB: 11, 69, 159)
- Burgundy: #C7293F (RGB: 196, 14, 38)

**Secondary Colors:**
- Light Blue: #4DACE1
- Yellow: #F6AC10
- Sea Wave/Teal: #358A7C
- Green: #26693A
- Light Blue Shade: #80B3E0
- Light Burgundy: #EDABBA
- Dark Blue: #303644
- White: #FFFFFF

### Typography
- **Font Family**: TT Wellington (or system fallbacks: -apple-system, BlinkMacSystemFont, 'Segoe UI')
- **Weights**: Regular (400), Medium (500), Bold (700), Italic (400 italic)
- **Sizes for Web**:
  - Headlines: Bold 24pt or 28px
  - Subheadings: Medium 16pt or 18px
  - Body Text: Regular 16pt or 14px
  - Small Text: Regular 12pt or 12px

### Logo Usage
- Use the full logo (with descriptor) on official documentation
- Use logo without accreditation descriptor for web applications
- Minimum logo size: 30mm/85px height for web
- Maintain clear exclusion zone around logo (width of letter "W")
- Can use in primary colors (blue/burgundy) or secondary colors

### Design Principles
- Geometric elements referencing university building architecture
- Photography style: warm lighting, friendly atmosphere, natural/emotional expressions
- Professional yet approachable aesthetic
- Consistent brand application across all touchpoints

---

## PROJECT STRUCTURE

### Frontend (React)
```
wiut-registrar-frontend/
├── public/
├── src/
│   ├── components/
│   │   ├── common/
│   │   │   ├── Header.tsx
│   │   │   ├── Sidebar.tsx
│   │   │   ├── Footer.tsx
│   │   │   ├── Navigation.tsx
│   │   │   └── BrandedLogo.tsx
│   │   ├── pages/
│   │   │   ├── HomePage.tsx
│   │   │   ├── NewsPage.tsx
│   │   │   ├── TeamPage.tsx
│   │   │   ├── AdmissionsPage.tsx
│   │   │   ├── StudentPerformancePage.tsx
│   │   │   ├── StudentRecordsPage.tsx
│   │   │   ├── StudentInformationPage.tsx
│   │   │   ├── CourseHandbooksPage.tsx
│   │   │   ├── AcademicPoliciesPage.tsx
│   │   │   ├── NotFoundPage.tsx
│   │   │   └── [other inner pages]
│   │   ├── admin/
│   │   │   ├── AdminDashboard.tsx
│   │   │   ├── AdminNavigation.tsx
│   │   │   ├── NewsManagement.tsx
│   │   │   ├── TeamManagement.tsx
│   │   │   ├── AdmissionsManagement.tsx
│   │   │   ├── StudentPerformanceManagement.tsx
│   │   │   ├── StudentRecordsManagement.tsx
│   │   │   ├── StudentInformationManagement.tsx
│   │   │   ├── CourseHandbooksManagement.tsx
│   │   │   ├── AcademicPoliciesManagement.tsx
│   │   │   └── UserManagement.tsx
│   │   └── forms/
│   │       ├── NewsForm.tsx
│   │       ├── TeamForm.tsx
│   │       ├── AdmissionRequirementForm.tsx
│   │       ├── CourseHandbookForm.tsx
│   │       └── AcademicPolicyForm.tsx
│   ├── hooks/
│   │   ├── useApi.ts
│   │   ├── useAuth.ts
│   │   └── useForm.ts
│   ├── services/
│   │   ├── api.ts
│   │   ├── newsService.ts
│   │   ├── studentService.ts
│   │   ├── admissionsService.ts
│   │   ├── courseService.ts
│   │   └── authService.ts
│   ├── context/
│   │   ├── AuthContext.tsx
│   │   └── AppContext.tsx
│   ├── types/
│   │   ├── index.ts
│   │   ├── news.ts
│   │   ├── student.ts
│   │   ├── course.ts
│   │   ├── user.ts
│   │   └── admin.ts
│   ├── utils/
│   │   ├── formatters.ts
│   │   ├── validators.ts
│   │   └── helpers.ts
│   ├── styles/
│   │   └── globals.css (Tailwind imports)
│   ├── App.tsx
│   └── main.tsx
├── tailwind.config.js
├── tsconfig.json
├── vite.config.ts
└── package.json
```

### Backend (.NET Core 9)
```
WIUTRegistrarAPI/
├── Controllers/
│   ├── NewsController.cs
│   ├── TeamController.cs
│   ├── AdmissionsController.cs
│   ├── StudentPerformanceController.cs
│   ├── StudentRecordsController.cs
│   ├── StudentInformationController.cs
│   ├── CourseHandbooksController.cs
│   ├── AcademicPoliciesController.cs
│   └── AuthController.cs
├── Models/
│   ├── News.cs
│   ├── TeamMember.cs
│   ├── Student.cs
│   ├── StudentPerformance.cs
│   ├── StudentRecord.cs
│   ├── AdmissionRequirement.cs
│   ├── Course.cs
│   ├── CourseHandbook.cs
│   ├── AcademicPolicy.cs
│   └── User.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Services/
│   ├── INewsService.cs
│   ├── NewsService.cs
│   ├── IStudentService.cs
│   ├── StudentService.cs
│   ├── IAdmissionService.cs
│   ├── AdmissionService.cs
│   ├── ICourseService.cs
│   ├── CourseService.cs
│   └── [Other services]
├── DTOs/
│   ├── CreateNewsDto.cs
│   ├── UpdateNewsDto.cs
│   ├── StudentDto.cs
│   ├── CreateStudentDto.cs
│   └── [Other DTOs]
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs
│   ├── AuthenticationMiddleware.cs
│   └── CorsMiddleware.cs
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── WIUTRegistrarAPI.csproj
```

---

## DATABASE SCHEMA (MSSQL)

### Core Tables
```
Students
├── StudentId (PK)
├── StudentNumber (Unique)
├── FirstName
├── LastName
├── Email (Unique)
├── PhoneNumber
├── DateOfBirth
├── Gender
├── Nationality
├── EnrollmentDate
├── CurrentStatus (Active/Inactive/Graduated/Suspended)
├── Department
└── CreatedAt, UpdatedAt

StudentRecords
├── RecordId (PK)
├── StudentId (FK)
├── AcademicYear
├── Semester
├── Transcript
├── GPA
├── Status (Pass/Fail/In Progress)
└── Timestamp

StudentPerformance
├── PerformanceId (PK)
├── StudentId (FK)
├── CourseId (FK)
├── Grade
├── Score
├── Attendance
├── Participation
├── AssignmentScore
└── RecordedDate

Courses
├── CourseId (PK)
├── CourseCode (Unique)
├── CourseName
├── Department
├── Credits
├── Description
├── Handbook (File/Text)
├── PrerequisiteCourses
├── Instructor
└── IsActive

StudentInformation
├── InformationId (PK)
├── StudentId (FK)
├── Address
├── EmergencyContact
├── ParentGuardianInfo
├── HealthInformation
├── DocumentsUploaded
└── LastUpdated

AdmissionRequirements
├── RequirementId (PK)
├── ProgramId
├── AcademicLevel (Foundation/Undergraduate/Postgraduate/PhD)
├── RequirementDescription
├── MinimumGPA
├── RequiredDocuments (JSON/array)
├── DeadlineDate
└── CreatedAt

TeamMembers
├── MemberId (PK)
├── FirstName
├── LastName
├── Email
├── Position
├── Department
├── PhoneNumber
├── Office
├── PhotoUrl
├── Bio
├── Qualifications
└── IsActive

News
├── NewsId (PK)
├── Title
├── Content
├── Author
├── Category
├── FeaturedImage
├── PublishDate
├── IsPublished
├── Tags
└── UpdatedAt

AcademicPolicies
├── PolicyId (PK)
├── PolicyTitle
├── PolicyContent
├── Category (Grading/Attendance/Academic Integrity/etc)
├── EffectiveDate
├── Version
└── LastModified

Users
├── UserId (PK)
├── Username (Unique)
├── Email (Unique)
├── PasswordHash
├── FirstName
├── LastName
├── Role (Admin/Staff/Student/Guest)
├── Department
├── IsActive
├── LastLogin
└── CreatedAt
```

---

## FRONTEND DEVELOPMENT PHASES

### PHASE 1: UI DESIGN & COMPONENT LIBRARY

#### Step 1: Initial Setup
1. Create React + TypeScript + Vite project
2. Install Tailwind CSS 4 with latest configuration
3. Add shadcn/ui components library
4. Create custom brand theme with WIUT colors in `tailwind.config.js`:
   - Primary: Blue (#264F9D) and Burgundy (#C7293F)
   - Secondary: Teal, Yellow, Green shades
   - Implement CSS variables for easy theming
5. Setup directory structure as outlined above
6. Configure TypeScript paths for clean imports

#### Step 2: Common Components
Create reusable branded components styled with Tailwind and WIUT colors:

**Header Component:**
- WIUT logo (left side) - use logo without accreditation descriptor
- Navigation menu (Home, About, Academics, Admissions, Contact)
- Search functionality
- Mobile hamburger menu
- Sticky header with shadow on scroll

**Sidebar Navigation:**
- For admin panel only
- Collapsible with hamburger toggle
- Icons with labels for each section
- Active state highlighting in primary blue
- User profile dropdown

**Footer:**
- WIUT branding with full logo
- Quick links (Privacy, Terms, Contact)
- Address: 12 Istiqbol Street, Tashkent 100047
- Contact info: +998 71 238 74 17/44, info@wiut.uz
- Social media links
- Copyright notice

**Cards, Buttons, Forms:**
- Use shadcn/ui as base
- Override colors to use WIUT brand palette
- Implement form validation
- Create loading states and error boundaries

#### Step 3: Layout System
- Create MainLayout (for public pages)
- Create AdminLayout (for admin panel)
- Implement responsive grid system using Tailwind
- Mobile-first approach

---

### PHASE 2: PUBLIC WEBSITE PAGES

#### Home Page
- Hero banner with WIUT value proposition
- Quick stats (Students, Programs, Faculty)
- Featured news section
- Quick links to key sections (Admissions, News, Team)
- Call-to-action buttons
- Images following WIUT photography style

#### News Page
- List of news articles in card format
- Filters by category and date
- Search functionality
- Pagination (10-15 items per page)
- Detail page for individual news articles
- Related news section
- Share buttons

#### Team / Organizational Structure Page
- Organizational hierarchy visualization
- Team member cards with:
  - Photo
  - Name
  - Position
  - Department
  - Contact email
- Filter by department
- Search functionality
- Detail page showing bio and qualifications

#### Admissions Page
- Program overview cards (Foundation, Undergraduate, Postgraduate, PhD)
- Admission requirements by program level
- Application process steps (visual timeline)
- Download application forms
- FAQ section
- Contact admissions office button
- Key dates and deadlines

#### Student Performance Page (Public - Anonymized)
- General university performance statistics
- Program completion rates
- Alumni success stories
- Visualizations with charts (bar/line charts)
- Not individual student data

#### Student Records Page (Public Info)
- General information about record keeping
- How to request transcripts
- Document verification process
- Form to request official documents
- FAQ about records

#### Student Information & Compliance Page
- Privacy policy
- Data protection information
- GDPR/Data handling compliance
- Student conduct code
- Policies and procedures documents (downloadable PDFs)

#### Course Handbooks & Programme Specifications Page
- List of all programs/courses
- Filter by level and faculty
- Download handbooks and specifications (PDF files)
- Search functionality
- Course overview details
- Learning outcomes
- Assessment methods

#### Academic Policies and Guides Page
- Academic integrity policy
- Attendance policy
- Grading policy
- Assessment guidelines
- Student rights and responsibilities
- Downloadable documents
- Expandable/collapsible sections

---

### PHASE 3: ADMIN PANEL

#### Admin Dashboard
- Overview statistics:
  - Total students
  - Active courses
  - Pending applications
  - Recent news/updates
- Quick action buttons
- Calendar widget showing important dates
- System health/status indicators

#### Admin Navigation
- Sidebar with admin-specific menu
- Logout functionality
- User profile management
- Settings link

#### Content Management Sections (CRUD Operations)
Each management page includes:
- Table view with columns (ID, Name, Date, Status, Actions)
- Sorting and filtering
- Pagination
- Search bar
- Create/Edit/Delete buttons
- Bulk actions (delete multiple)
- Data export to CSV

**News Management:**
- Add/Edit/Delete news articles
- Rich text editor for content
- Image upload for featured image
- Publish/Draft status
- Category assignment
- Date scheduling

**Team Management:**
- Add/Edit/Delete team members
- Photo upload
- Form fields: Name, Position, Department, Email, Phone, Office, Bio, Qualifications
- Active/Inactive status toggle

**Admissions Management:**
- Manage admission requirements
- Set requirements by program level
- Define required documents
- Set application deadlines
- View application statistics

**Student Performance Management:**
- Upload/manage performance data
- Bulk import CSV
- View performance reports
- Generate analytics

**Student Records Management:**
- Search and view student records
- Upload/attach documents
- Mark records as verified
- Generate transcripts
- Export capabilities

**Student Information Management:**
- View/edit student information
- Update contact details
- Emergency contact management
- Document upload/verification
- Compliance tracking

**Course Handbooks Management:**
- Upload/update course handbooks
- Manage programme specifications
- Version control
- Assign to programs/courses
- Download/preview files

**Academic Policies Management:**
- Create/Edit/Delete policies
- Rich text editor
- Version history
- Effective date setting
- Policy categorization

**User Management:**
- Create admin users
- Assign roles/permissions
- View user activity logs
- Reset user passwords
- Deactivate accounts

---

## BACKEND DEVELOPMENT PHASES

### PHASE 1: PROJECT SETUP

1. Create .NET Core 9 Web API project
2. Install NuGet packages:
   - Microsoft.EntityFrameworkCore.SqlServer
   - Microsoft.EntityFrameworkCore.Tools
   - Microsoft.EntityFrameworkCore.Design
   - Swashbuckle.AspNetCore (Swagger)
3. Configure database connection string in `appsettings.json`
4. Setup CORS policy for React frontend (localhost:3000, localhost:5173, production URL)
5. Setup dependency injection in Program.cs
6. Configure middleware pipeline

### PHASE 2: DATABASE & ENTITY MODELS

1. Create ApplicationDbContext inheriting from DbContext
2. Define DbSet properties for all entities
3. Configure entity relationships and constraints using Fluent API
4. Create migrations:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
5. Implement data seeding for initial data (sample news, teams, policies)

### PHASE 3: SERVICES LAYER

Create service interfaces and implementations for:
- **NewsService**: CRUD operations for news
- **StudentService**: Student data management
- **AdmissionService**: Admission requirements and applications
- **CourseService**: Course and handbook management
- **TeamService**: Team member management
- **StudentPerformanceService**: Performance tracking
- **StudentRecordsService**: Academic records
- **AuthService**: User authentication and JWT tokens
- **AcademicPolicyService**: Policy management

Each service should have:
- Interface definition (ISomethingService.cs)
- Implementation (SomethingService.cs)
- Dependency injection in Program.cs
- Error handling and logging

### PHASE 4: DTOS & MODELS

Create Data Transfer Objects for:
- Requests (Create/Update operations)
- Responses (Return data to frontend)
- Maintain separation between database models and API contracts

Example structure:
```csharp
// Request DTO
public class CreateNewsDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Category { get; set; }
    public DateTime PublishDate { get; set; }
}

// Response DTO
public class NewsDto
{
    public int NewsId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### PHASE 5: API CONTROLLERS

Create RESTful endpoints:

**NewsController:**
- GET /api/news - List all news (with pagination, filters)
- GET /api/news/{id} - Get single news item
- POST /api/news - Create news (admin only)
- PUT /api/news/{id} - Update news (admin only)
- DELETE /api/news/{id} - Delete news (admin only)

**StudentController:**
- GET /api/students - List all students (admin only)
- GET /api/students/{id} - Get student details
- POST /api/students - Create student record
- PUT /api/students/{id} - Update student
- DELETE /api/students/{id} - Delete student (soft delete)

**AdmissionsController:**
- GET /api/admissions/requirements - Get admission requirements
- GET /api/admissions/requirements/{level} - By program level
- POST /api/admissions/requirements - Create requirement (admin)
- PUT /api/admissions/requirements/{id} - Update requirement (admin)
- DELETE /api/admissions/requirements/{id} - Delete requirement (admin)

**CourseController:**
- GET /api/courses - List all courses
- GET /api/courses/{id} - Get course details
- POST /api/courses - Create course (admin)
- PUT /api/courses/{id} - Update course (admin)
- DELETE /api/courses/{id} - Delete course (admin)
- GET /api/courses/{id}/handbook - Get course handbook

Similar patterns for other controllers.

### PHASE 6: AUTHENTICATION & AUTHORIZATION

1. Implement JWT token generation
2. Create login endpoint
3. Add authorization middleware
4. Configure role-based access control (Admin, Staff, Student)
5. Implement refresh token mechanism
6. Add authentication headers to Swagger

### PHASE 7: ERROR HANDLING & MIDDLEWARE

1. Global exception handling middleware
2. Validation error responses
3. Consistent error response format
4. Logging setup (Console/File)
5. Request/Response logging middleware

### PHASE 8: TESTING & DOCUMENTATION

1. Swagger/OpenAPI documentation
2. API endpoint testing
3. Postman collection generation
4. Unit test setup (xUnit)

---

## FRONTEND-BACKEND INTEGRATION

### API Service Layer (Frontend)

Create `src/services/api.ts`:
```typescript
import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

// Add JWT token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
```

Create service files for each resource:
- newsService.ts
- studentService.ts
- admissionsService.ts
- courseService.ts
- authService.ts

### State Management

Use React Context API for:
- Authentication state (user, token, role)
- Application state (loading, errors)
- User preferences

Consider Redux if needed for complex state.

---

## KEY FEATURES TO IMPLEMENT

### Public Website Features
1. ✅ Responsive design (Mobile, Tablet, Desktop)
2. ✅ SEO-optimized (Meta tags, semantic HTML)
3. ✅ Fast loading (Code splitting, lazy loading)
4. ✅ Accessibility (WCAG 2.1 AA compliant)
5. ✅ Search functionality across all pages
6. ✅ Breadcrumb navigation
7. ✅ Print-friendly versions of documents
8. ✅ PDF download capabilities

### Admin Panel Features
1. ✅ Role-based access control
2. ✅ Audit logs (track changes)
3. ✅ Backup/Export functionality
4. ✅ Data validation and sanitization
5. ✅ File upload management
6. ✅ Bulk operations (import/export CSV)
7. ✅ Advanced filtering and search
8. ✅ Real-time notifications
9. ✅ Dashboard charts and statistics
10. ✅ User activity monitoring

### Security Features
1. ✅ HTTPS only
2. ✅ CORS properly configured
3. ✅ Input validation (Frontend & Backend)
4. ✅ SQL injection prevention (Parameterized queries)
5. ✅ XSS protection
6. ✅ CSRF tokens
7. ✅ Rate limiting
8. ✅ Password hashing (bcrypt)
9. ✅ JWT token expiration
10. ✅ Secure file upload

---

## DEVELOPMENT WORKFLOW

### Step-by-Step Execution Order

1. **Frontend Setup**
   - Initialize React project with TypeScript
   - Configure Tailwind CSS with WIUT brand colors
   - Setup shadcn/ui
   - Create folder structure

2. **Backend Setup**
   - Create .NET Core 9 API project
   - Setup database and DbContext
   - Create entity models
   - Setup dependency injection

3. **Design System**
   - Create common components (Header, Footer, Sidebar)
   - Build layout system
   - Establish design tokens
   - Create component library documentation

4. **Public Pages (Frontend)**
   - Build Home page
   - Build News page and detail page
   - Build Team page
   - Build Admissions page
   - Build other public pages
   - Implement routing

5. **Backend API Endpoints**
   - Implement News endpoints
   - Implement Student endpoints
   - Implement Course endpoints
   - Implement other endpoints
   - Setup authentication

6. **Integration**
   - Connect frontend to backend APIs
   - Implement loading states and error handling
   - Add authentication flow

7. **Admin Panel**
   - Build admin dashboard
   - Create management pages for each resource
   - Implement CRUD operations
   - Setup admin routing and protection

8. **Testing & Refinement**
   - Test all features
   - Cross-browser testing
   - Mobile responsiveness testing
   - Performance optimization

---

## BRAND CONSISTENCY CHECKLIST

- [ ] Logo properly positioned on every page (Header, Footer)
- [ ] Primary blue (#264F9D) used for main CTAs and highlights
- [ ] Burgundy (#C7293F) used for secondary accents
- [ ] TT Wellington font applied to all text (or system fallbacks)
- [ ] Spacing and margins consistent throughout
- [ ] Color palette limited to official WIUT colors
- [ ] Photography follows WIUT style guidelines (warm, natural)
- [ ] Exclusion zone maintained around logos
- [ ] Responsive design maintains brand integrity
- [ ] Dark mode (if implemented) uses approved color variations
- [ ] Print styles respect brand guidelines
- [ ] PDFs use consistent branding

---

## DEPLOYMENT CONSIDERATIONS

### Frontend (React)
- Build optimization: `npm run build`
- Deploy to: Vercel, Netlify, Azure Static Web Apps, or hosting provider
- Environment variables: API URL, analytics keys
- CDN for static assets

### Backend (.NET Core)
- Publish: `dotnet publish -c Release`
- Deploy to: Azure App Service, AWS, Docker Container, VPS
- Database migrations in production
- Connection string management (secrets)
- Logging and monitoring setup

---

## TESTING STRATEGY

### Frontend
- Unit tests: Jest + React Testing Library
- Component tests for key components
- Integration tests for user flows
- E2E tests: Cypress or Playwright
- Performance testing
- Accessibility testing

### Backend
- Unit tests: xUnit
- Integration tests with test database
- API endpoint testing
- Error handling verification
- Security testing

---

## PERFORMANCE OPTIMIZATION

### Frontend
- Code splitting for admin and public sections
- Lazy loading of components and routes
- Image optimization (WebP format, responsive sizes)
- Minification and compression
- Caching strategies
- Reduce bundle size

### Backend
- Database query optimization
- Pagination for large datasets
- Caching (Redis if needed)
- Async/await for non-blocking operations
- Connection pooling
- API response compression

---

## DOCUMENTATION TO MAINTAIN

1. **API Documentation**: Auto-generated by Swagger
2. **Component Library**: Storybook or similar
3. **Database Schema**: ER diagram
4. **Deployment Guide**: Step-by-step instructions
5. **Environment Setup**: Local development guide
6. **Architecture Overview**: System design document

---

## NOTES FOR CURSOR AI

When implementing:
1. Always use TypeScript for type safety
2. Follow React best practices (functional components, hooks)
3. Use semantic HTML for accessibility
4. Implement error boundaries for React components
5. Add loading and error states to all async operations
6. Use environment variables for configuration
7. Maintain consistent file naming conventions
8. Add meaningful comments only where logic is complex
9. Follow DRY principle - extract reusable components
10. Test as you develop, not at the end
11. Keep components focused and single-responsibility
12. Use descriptive variable and function names
13. Implement proper CRUD patterns in backend
14. Use async/await instead of promises when possible
15. Validate data both frontend and backend

---

## QUESTIONS FOR CLARIFICATION (If Needed)

Before starting implementation, clarify:
1. Will there be a student login portal? (Separate from admin)
2. Do students need to view their own grades/records?
3. Any specific report formats needed?
4. Integration with external systems needed?
5. Expected user volume for performance planning?
6. Backup and disaster recovery requirements?
7. Payment/Fee management module needed?
8. Email notifications required?
9. Multi-language support needed?
10. Specific authentication method (Local/Azure AD/OAuth)?

---

## START HERE

To begin development in Cursor, follow this workflow:

**For Frontend:**
```
1. "Create a React 19 project with TypeScript, Tailwind CSS 4, and shadcn/ui"
2. "Configure tailwind.config.ts with WIUT brand colors"
3. "Create the folder structure and common components"
4. "Build the Header and Footer components with WIUT branding"
5. "Create the Home page"
```

**For Backend:**
```
1. "Create a .NET Core 9 Web API project"
2. "Setup Entity Framework Core with MSSQL"
3. "Create all database models based on the schema provided"
4. "Setup DbContext and configure relationships"
5. "Create News API endpoints"
```

Each prompt should reference this master document for context and consistency.
