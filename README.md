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
- Fixed bugs in the **DangKyKhoaHoc Controller**.

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

> 📌 *This README summarizes weekly progress, technical decisions, and structural changes in the project.*
