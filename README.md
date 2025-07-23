# 📘 Project Weekly Progress Report

This document outlines the weekly progress and key milestones of my project development.

---

## 🗓️ Week 1: Project Kickoff (12/5 -> 18/5)

- Attended project briefing meetings.
- Received official project assignment.
- Team structure established:
  - ✅ Appointed **Team Leader**
  - ✅ Appointed **Deputy Team Leader**

---

## 🗓️ Week 2: Planning & Initial Setup (19/5 -> 25/5)

- Organized online team meetings for planning.
- Reached consensus on the technologies and tools to be used.
- Distributed tasks among team members based on skill sets.

### 🔧 System Setup Tasks:

- Created and updated core **Models**.
- Implemented **Responsibility Pattern** to manage responsibilities effectively.
- Integrated **ASP.NET Identity** for authentication and user management.
- Applied **Entity Framework Migrations** to sync the database from Azure to local.
- Developed **Razor Pages** for account-related features.
- Configured **Identity services** and related options.
- Built a basic **view demo page** to validate UI integration.

---

## 🗓️ Week 3: Requirement Changes & Redesign (26/5 -> 1/6)

- Received new project requirements that conflicted with the existing system design.
- Identified critical issues in:
  - Entity relationships
  - Business logic
  - Key constraints (primary/foreign)

### 🛠️ Solution:

- Decided to **rebuild the system** from the ground up:
  - Redesigning data relationships.
  - Refactoring core business logic.
  - Adjusting keys and constraints for consistency.
  - Updating database migrations accordingly.

---

## 🗓️ Week 4: Model Fixes & Admin Interface (2/6 -> 8/6)

- Fixed required navigation property issues in the models.
- Validated navigation properties for correctness.
- Developed an **Admin Interface** to facilitate data management.
- Applied **ValidateNever** to the **DangKyKhoaHoc** model.
- Fixed bugs and improved the **DangKyKhoaHoc Controller**.

---

## 🗓️ Week 5: Restructuring, Registration Updates, and New Features (9/6 -> 15/6)

- Restructured the **Lop** model and updated associated views (Detail, Create, etc.).
- Enhanced the **DangKyKhoaHoc** system:
  - Enabled class selection during registration.
  - Auto-added users to the **DanhSachHocVien** upon registration.
- Removed validation navigation properties from the **ChiTietLop** table.
- Introduced **ChiTietLopController** to manage **Lop** contexts:
  - **Lop** is now a required parameter.
  - The controller now works at `/ChiTietLop?lop{id}` instead of `/ChiTietLop/index`.
- Improved **UI/UX** for **ChiTietLop** to enhance user experience and visual design.

---

## 🗓️ Week 6: API Integration, DiemDanh Feature, and Frontend Setup (16/6 -> 22/6)

- Developed the **API Controller** for **KhoaHoc**, utilizing **DTO Models** to return a flattened JSON structure, preventing circular reference issues.
- Implemented the **DiemDanh** feature:
  - Integrated logic and interfaces between **DangKyKhoaHoc** and **Lop**.
  - Built **QR Code** functionality for **DiemDanh** check-ins.
  - Improved error handling and reporting for the **DiemDanh** process.
- Implemented user search functionality by email for login:
  - Validated login results using console outputs.
- Managed **local user reset** when the account is not bound to **ChiTietLop**, with specific handling for **Admin accounts**.
- Reconfigured **DbInitializer.cs** to ensure **Admin Account** initialization during system startup.
- Merged the **User Controller** into the **Admin Controller** with **[Authorize]** access control.
- Updated system logic for role management:
  - By default, user registration assigns the role of **HocVien**.
  - Admins can toggle between **HocVien** and **GiangVien** roles.
  - Refined filtering logic for **admin** operations.
  - Enhanced interfaces for role changes in the **Admin Panel**.
- Configured **Swagger** for API testing and documentation.
- Developed and added **DTOs** for various models:
  - Updated **KhoaHocController** for Razor Pages and **KhoaHocAPIController** for JSON-based API endpoints.
  - Implemented **ModelState** validation for the **KhoaHocAPI**.
- Improved the **Home Interface** and implemented **Account DTO** for better API integration.
- Created the **AccountAPI** for **RegisterConfirmation** processes.
- Added **CORS** support for frontend API calls, ensuring compatibility with cookies and **AllowCredentials**.
- Optimized the codebase by removing unused code and improving overall performance.
- Updated **email sending workflows** for various operations:
  - ConfirmEmail, ForgotPassword, ResendEmailConfirmation, etc.
- Refined the **Login**, **AccessDenied**, **_Layout**, and **_ManageNav** interfaces with more detailed error reporting.
- Started the React frontend setup, successfully integrating the frontend with several API endpoints.

---


## 🗓️ Week 7: Class API, DiemDanh Updates, and Bug Fixes (23/6 -> 29/6)

- Created **ClassAPI** for managing class-related data:
  - Refactored and enhanced **ClassAPI**:
    - Added functionality to display more information.
    - Synchronized processing for more reliable data handling.
  - Renamed and reorganized the **HocVienSelector** model into **Dtos** for better management.
  - Created global variables for **Dtos** to standardize and optimize the process.

- Added **ChiTietLopController** to manage detailed class information.
  
- Made significant changes to the **DiemDanh Controller**:
  - Improved interface and handling for more efficient data processing.
  
- Fixed several bugs in the **update-DiemDanhAPI** branch:
  - Updated the **Note** field in the **DiemDanh** table.
  - Resolved issues related to missing entries in the **GiangVien** and **HocVien** tables:
    - Ensured that users are correctly recorded in these tables even when the roles are present.
    - Added **GiangVien** and **HocVien** test accounts for validation.
    - Updated passwords for the 3 default accounts.
  - Synchronized changes in **Program.cs** to reflect the updates.
  
- Enhanced the **QR scanning** process for **DiemDanh**:
  - Displayed time when a QR scan is successful.
  - Added a **Note** field in **DiemDanh** and fixed bugs:
    - Corrected issues where the **Note** field was not displaying the correct value.
    - Resolved foreign key errors when verifying the **Note** field.
  - Deleted the **DiemDanhAPI** for testing, synchronization, and will update again later.
  
- Created the **DanhSachHocVien API** to provide class attendance data:
  - Developed new **DTOs** for **DiemDanh** and **DanhSachHocVien** to improve API communication.
  - Modified the **DiemDanh** API to handle different display behavior when no attendance is taken (null).
  
- **QR processing** for **DiemDanh** is still in progress, with ongoing improvements.

---

## 🗓️ Week 8: JWT Auth, API Integration & Frontend Fixes (30/6 → 13/7)

- **JWT Authentication**:
  - Configured `Key`, `Issuer`, `Audience`, `Subject` for secure token generation.
  - Added `Role` claim to enable role-based API communication.
  - Enabled JWT authentication in **Swagger** for secure API testing.
  - Updated `[Authorize]` logic across multiple APIs; temporarily disabled for testing.

- **DiemDanhAPI**:
  - Updated route to accept `LopId` parameter.
  - Applied route-specific authorization.
  - Enabled QR code generation for frontend (`localhost:3000`), no backend view needed.
  - Added `Note` field handling and bug fixes related to foreign keys.

- **AccountAPI**:
  - Added token-based authorization checks.
  - Deprecated Razor login; token is now logged after login and reused in API calls.

- **ChiTietLop**:
  - Changed `ResetCheckIn/Out` to receive `ChiTietLopId` from route.
  - Allowed adding students after class creation.

- **Frontend (HTTP/HTTPS Issue)**:
  - Resolved dual-protocol conflict:
    - Installed `openssl` via `choco`.
    - Configured 3 `.pem` files for SSL.
    - Frontend now supports both `HTTP` and `HTTPS`.
    - Default mode is `HTTP` (non-SSL) for development flexibility.

> ✅ Focus: Securing authentication, improving QR/Check-in flows, and unifying FE–BE communication.


---

## 🗓️ Week 9: (7/7 → 13/7)



---

> 📌 *This README summarizes weekly progress, technical decisions, and structural changes in the project.*
