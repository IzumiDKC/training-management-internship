# 📘 Project Weekly Progress Report

This document outlines the weekly progress and key milestones of my project development.

---

## 🗓️ Week 1: Project Kickoff

- Attended project briefing meetings.
- Received official project assignment.
- Team structure established:
  - ✅ Appointed **Team Leader**
  - ✅ Appointed **Deputy Team Leader**

---

## 🗓️ Week 2: Planning & Initial Setup

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

## 🗓️ Week 3: Requirement Changes & Redesign

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

## 🗓️ Week 4: Model Fixes & Admin Interface

- Fixed required navigation property issues in the models.
- Validated navigation properties for correctness.
- Developed an **Admin Interface** to facilitate data management.
- Applied **ValidateNever** to the **DangKyKhoaHoc** model.
- Fixed bugs and improved the **DangKyKhoaHoc Controller**.

---

## 🗓️ Week 5: Restructuring, Registration Updates, and New Features

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

## 🗓️ Week 6: API Integration, DiemDanh Feature, and Frontend Setup

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

> 📌 *This README summarizes weekly progress, technical decisions, and structural changes in the project.*
