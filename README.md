# Social Network

This is a web api for social network application built with ASP.NET Core using domain driven design and CQRS patterns/principles.

Frameworks/libraries used:
* ASP.NET Core
* Entity Framework Core
* MediatR
* AutoMapper
* FluentValidation

Project contains ApplicationKernel libraries which can be reused on other projects which use similar technologies.

### The Use case pattern
Every use case / request to the system is either a command or a query, and each is coded in a separate file. It consists of a request model structure, the use case handler/executor (which is called by mediator), and optionally a fluent validator.
Since all of the domain logic is coded in the use cases, controller actions usually consist of just one line that dispatches the request to the mediator.
Result of the use case executor is an object of type Response, which is automatically converted into IActionResult in ApiRequestHandler class, before it's returned to controller.
