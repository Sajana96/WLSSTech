# WLSS Tech – Incident Management System

WLSS Tech is a cloud-ready incident management application built to demonstrate modern
.NET engineering, secure API design, and scalable application architecture.  
It reflects best practices aligned with SaaS platforms operating in compliance- and safety-critical environments.

---

## ?? Features

### Authentication & Security
- User registration and login using email/password
- JWT-based authentication for stateless API access
- Secure password hashing via ASP.NET Core Identity
- Protected API endpoints using `[Authorize]`
- Role-based access control foundation (Admin / Manager / User)
- Token persistence using browser localStorage (demo-friendly)

### Incident Management
- Create incidents with title, description, severity, and location
- View all incidents in a centralized dashboard
- Update incident status (Open ? InProgress ? Closed)
- Delete incidents
- Server-side validation and error handling

### User Experience
- Blazor Server UI with responsive layout
- Dedicated authentication layout (no navigation on login)
- Loading indicators and disabled actions during async operations
- Clean separation between authenticated and unauthenticated pages

---

## ?? Technology Stack

### Backend
- ASP.NET Core Web API (.NET 8)
- C#
- Entity Framework Core (Code-First)
- ASP.NET Core Identity
- JWT (JSON Web Tokens)
- SQL Server

### Frontend
- Blazor Server
- Razor Components
- Bootstrap
- JavaScript Interop (localStorage)

### Tooling & Patterns
- Dependency Injection
- HttpClientFactory with custom message handlers
- RESTful API design
- Structured logging with ILogger

---

## ??? Architecture Overview

- **Client Layer**  
  Blazor Server application responsible for UI and user interaction

- **API Layer**  
  ASP.NET Core Web API exposing secure REST endpoints

- **Application Layer**  
  Business logic and orchestration services

- **Data Layer**  
  Entity Framework Core DbContext and SQL Server persistence

The application follows separation of concerns and is designed to be extensible and testable.

---

## ??? Data Access Strategy

- Code-first Entity Framework Core approach
- Strongly typed domain models
- Database migrations for schema evolution
- Enums stored as strings for readability and long-term stability
- Indexed columns for efficient querying

---

## ?? Security Implementation

### JWT Authentication
- Tokens issued upon successful login
- Token validation includes:
  - Issuer
  - Audience
  - Signature
  - Expiration
- Authorization enforced per endpoint

### Token Storage (Demo)
- JWT stored in browser localStorage
- Automatically attached to API requests
- Production consideration: httpOnly cookies or BFF pattern

---

## ?? Error Handling & Logging

- Centralized exception handling in API controllers
- Meaningful HTTP status codes returned to client
- Structured logging via ILogger
- Graceful UI error messages

---

## ?? Cloud Readiness

Although deployed locally for demonstration, WLSS Tech is designed for:
- Stateless scaling
- Containerization
- Cloud deployment (Azure / AWS)
- CI/CD automation

---

## ?? Future Enhancements

- Full role-based authorization policies
- Incident audit trail
- Pagination and filtering
- File attachments
- Notifications (email / push)
- Docker and CI/CD pipeline integration

---

## ?? Summary

WLSS Tech demonstrates modern .NET development practices with a strong focus on
security, maintainability, and cloud-ready architecture suitable for compliance-driven systems.
