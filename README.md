# E-Commerce RESTful API

### ASP.NET Core | Onion Architecture | Redis | Stripe | Automated Testing

A feature-rich E-Commerce RESTful API built with ASP.NET Core, designed to demonstrate practical backend engineering skills through clean architecture, secure authentication, efficient data access, caching, payment integration, and automated testing.

 Key Features

- **Product Catalog:** Product retrieval with filtering, sorting, and pagination.
- **Authentication & Authorization:** Secure authentication using ASP.NET Core Identity and JWT, with role-based access control.
- **Shopping Basket:** Redis-powered basket storage for fast data access.
- **Order Processing:** Order creation with product validation, delivery method validation, and order total calculation.
- **Payment Integration:** Stripe Payment Intents integration to support payment workflows.
- **Automated Testing:** Unit and integration tests for selected Product, Basket, Order, and Authentication scenarios.

 Architecture & Design

Built using **Onion Architecture**, with a clear separation of concerns between the domain, application logic, infrastructure, and API layers.

Design patterns and practices:

- Repository Pattern
- Unit of Work Pattern
- Specification Pattern
- Dependency Injection
- Separation of Concerns


 Tech Stack
 
Language = C# 
Framework = ASP.NET Core Web API 
Data Access =  Entity Framework Core, LINQ  Database  Microsoft SQL Server 
Caching =  Redis 
Authentication = ASP.NET Core Identity, JWT 
Payment Processing = Stripe 
Testing =  xUnit, Moq 
Version Control = Git, GitHub 

 Quality & Testing

The project includes unit and integration tests written with **xUnit** and **Moq**, covering selected service behaviors and application workflows.
Testing focuses on verifying expected outcomes, handling failure scenarios, and checking interactions with application dependencies.

 What This Project Demonstrates

- Designing maintainable backend applications using layered architecture.
- Building RESTful APIs and implementing business logic.
- Integrating relational databases and Redis.
- Implementing authentication and authorization.
- Connecting backend workflows with an external payment provider.
- Writing automated tests for service logic and integrated components.

 Author

**Youssab Hany Fayez*

[GitHub Profile](https://github.com/youssabm00732) 
[E-Commerce Repository](https://github.com/youssabm00732/ECommerceV5)


