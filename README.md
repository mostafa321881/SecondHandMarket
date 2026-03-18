# SecondHandMarket Console Application

## Project Description
SecondHandMarket is a console-based marketplace application developed in C# using .NET.  
The system allows users to register accounts, create listings, search for items, purchase products, view transaction history, and leave reviews for sellers.

The application demonstrates object-oriented programming principles, layered architecture (Models, Services, UI), and LINQ-based data filtering.

---

## Features
- User registration and login
- Create, edit, and remove listings
- Browse and search listings using filters
- Purchase items from other users
- Transaction history for buyers and sellers
- Seller review system with rating (1–6)
- Average seller rating calculation
- Menu-driven console interface

---

## Technologies Used
- C# (.NET Console Application)
- Object-Oriented Programming
- LINQ for filtering and searching
- Git for version control

---

## How to Run the Application
1. Open the solution file in any .NET-supported IDE (e.g., JetBrains Rider or Visual Studio).
2. Restore dependencies and build the solution.
3. Run the console application.
4. Follow the on-screen menu instructions to use the marketplace features.
---

## Design Decisions
The application follows a layered architecture:

- **Models** represent core domain entities such as User, Listing, Transaction, and Review.
- **Services** contain business logic, including purchasing, searching, and review validation.
- **UI layer** handles console interaction and user navigation.

This separation improves readability, maintainability, and testability.  
LINQ was used in search functionality to provide flexible filtering and demonstrate modern C# querying techniques.

---

## AI Usage
AI tools were used to support conceptual understanding of architecture design, error handling strategies, and implementation approaches.  
Prompts used during development are documented in the file **AI_PROMPTS.md** inside Doc Folder.

---

## Git Repository
Full Git history is included in the submission as required.