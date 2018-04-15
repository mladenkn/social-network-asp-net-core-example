# social-network

This is a social network application built with ASP.NET Core.

Besides just ASP.NET Core, I have also used Entity Framework Core 2, Identity, and plain JavaScript. 
Solution is split into many projects, as in Clean/Onion architecture.

Project is using some practices from functional programming such as pure methods and method chaining.
To accomplish better method chaining I have used some of my generic extension methods like Let and Also, which enables me to chain
almost any method calls. Why? Because there is no need to save some value into variable before sending it to another method.

And about the functionalities, it is a social network that enables registered users to create, edit, and rate posts.
