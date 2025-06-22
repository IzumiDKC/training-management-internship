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
- Implemented **Responsibility pattern** to manage responsibilities clearly.
- Integrated **ASP.NET Identity** for authentication and user management.
- Applied **Entity Framework Migrations** (from Azure DB to local DB).
- Developed **Razor Pages (UI)** for account-related features.
- Configured **Identity services and options**.
- Built a basic **view demo page** to validate UI integration.

---

## 🗓️ Week 3: Requirement Changes & Redesign

- Received new project requirements conflicting with the existing system design.
- Identified major issues in:
  - Entity relationships
  - Business logic
  - Key constraints (primary/foreign)

### 🛠️ Solution:

- Decided to **rebuild the system** from the ground up, including:
  - Redesigning data relationships
  - Refactoring core logic
  - Adjusting keys and constraints for consistency
  - Updating database migrations

---

## 🗓️ Week 4: Model Fixes & Admin Interface

- Fixed required navigation property issues.
- Validated navigation properties in models.
- Created an **Admin Interface** to manage data more effectively.
- Applied **ValidateNever** to the **DangKyKhoaHoc** model.
- Fixed bugs in the **DangKyKhoaKhoa Controller**.

---

## 🗓️ Week 5: Restructuring, Registration Updates, and New Features

- Restructured the **Lop** model and related views (Detail, Create, etc.).
- Updated the **DangKyKhoaHoc** system:
  - Allowed class selection during registration.
  - Auto-added users to the **DanhSachHocVien** upon registration.
- Dropped validation navigation properties in the **ChiTietLop** table.
- Added the **ChiTietLopController** which operates within the **Lop** context:
  - **Lop** is required.
  - The controller does not operate at `/ChiTietLop/index` but at `/ChiTietLop?lop{id}`.
- Updated **UI/UX** for **ChiTietLop** to improve user experience and visual design.

---

## 🗓️ Week 6: API Integration, DiemDanh Feature, and Frontend Setup

- Created **API Controller** for **KhoaHoc** with **DTO Models** to return flattened JSON to avoid circular references.
- Built **DiemDanh** feature:
  - Processed logic and interface between **DangKyKhoaHoc** and **Lop**.
  - Built QR code functionality for **DiemDanh**.
  - Handled error reporting for the DiemDanh process.
- Implemented **Find user by email** during login:
  - Checked results in the console.
- **Reset local user** if account is not bound to **ChiTietLop**, handled **Admin account**.
- Reconfigured **DbInitializer.cs** to receive the **Admin Account** during initialization.
- Removed **User Controller** and merged it into the **Admin Controller** with **[Authorize]** annotation.
- Updated system logic for role management:
  - By default, account registration will assign the role **HocVien**.
  - Admin can change role between **HocVien** and **GiangVien**.
  - Adjusted filtering logic for admin.
  - Added and updated display interfaces when changing the role in the **Admin** panel.
- Configured **Swagger** for testing the API endpoints.
- Added **DTOs** for various models and APIs.
  - Updated **KhoaHocController** (Razor Page) and **KhoaHocAPIController** (JSON via `/api/{Model}`).
  - Managed **ModelState** validation for **KhoaHocAPI**.
- Updated **Home Interface** and **Account DTO** for API usage.
- Created **AccountAPI** with **RegisterConfirmation** logic.
- Added **CORS** for frontend API calls (with cookies and **AllowCredentials**).
- Optimized unused code across the project.
- Updated **email sending logic**:
  - For cases such as **ConfirmEmail**, **ForgotPassword**, **ResendEmailConfirmation**, etc.
- Updated **Login**, **AccessDenied**, **_Layout**, and **_ManageNav** interface with detailed error reporting and user-friendly designs.
- React frontend setup started, with successful API integrations for some features.

---

> 📌 *This README summarizes weekly progress, technical decisions, and structural changes in the project.*
