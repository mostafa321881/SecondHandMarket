# AI Prompt Documentation

During development, AI assistance was used mainly for conceptual clarification, architecture understanding, and debugging strategies.

Below are examples of prompts used.

---

## Prompt 1 – Architecture Understanding
I am building a console-based marketplace system in C#  
I want to understand how to properly separate responsibilities between models, services, and UI classes.  
Can you explain conceptually how business logic should be isolated from user interface code and why this improves maintainability in larger applications?"

This helped clarify layered architecture and encouraged the use of dedicated service classes.

---

## Prompt 2 – Transaction and Domain Modeling
In a marketplace system, when a listing is purchased, should I store a separate transaction object instead of only marking the listing as sold?  
Please explain the design benefits and potential future extensions such as reviews or history tracking."

This supported the decision to introduce a Transaction model for historical accuracy and extensibility.

---

## Prompt 3 – LINQ Search Strategy
I want to implement flexible search functionality in a console marketplace application.  
Can you explain theoretically how LINQ can be used to build dynamic filtering queries based on optional parameters such as keyword, category, and price range?"

This guided the implementation of a reusable search method.

---

## Prompt 4 – Validation and User Experience
In console applications, what are recommended strategies for validating user input and preventing runtime crashes?  
Please explain how structured validation and exception handling contribute to better user experience."

This helped shape input validation and error-handling patterns used in UI classes.