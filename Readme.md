README for ThAmCo Events Web App 

Introduction
This is an MVC (Model-View-Controller) Application developed using the C# programming language in Visual Studio.
The application is designed to create and manage events, book guests onto an event, book a venue and food for an event, and assign staff to an event.
The application uses Entity Framework Core as an Object-Relational Mapping (ORM) framework to interact with a SQLite database.
The application is built using .NET 6.

Installation
To install this application on your local machine, you need to follow these steps:
Open ThAmCo.sln in Visual Studio.
Build the project using by clicking on Build from the top menu.
Run the project by clicking start from the top menu.

Dependencies
This application has the following dependencies:

.NET 6.
EntityFrameworkCore version 7.
Microsoft.AspNetCore version 6.

Design Considerations
The web client had initially been designed with a services layer to manage the interactions with the catering and venues web services. The services layer was then expanded further to incorporate the guests and staff interactions. The reason for this was to make a distinction between the Events domain being at the top level which then makes use of the Guests, Staff, Venues and Catering domains at the lower levels. In the events domain, any use of the services from the views when using ajax always go via the events controller first and not directly to the controller within the service. This was to ensure the views in the events domain remain loosely coupled to the services layer. Given more time I would expand on this by tidying up some of the logic between the events controller and the services. There are improvements to error handling and the types passed between the layers which could be made.

Known issues
The use of a hard coded staff id of "1" when making a venue reservation is the first issue which has not been addressed, following from this is the absence of user access control. I began the project working in the Catering Web service but mistakenly implemented the menus and fooditems functionality into the web client, once I realised my mistake I stopped working on this and therefore this area of the application is only partially implemented and is missing the foodbookings functionality.
I did not do very much css styling in throughout the application, only what was necessary to make the application usable, this is an area which given time would benefit from making better use of space.

Challenges
The main challenges came from invalid model states when posting to the controllers. Ensuring the model state was valid on post when using HTML helper list boxes proved the hardest challenge to surmount, when needing to return an array of values from the list box. I went with the solution of using Ajax and adding a seperate array as an argument alongside the view model, as I found it beneficial to be able to combine implementing JQuery logic with Ajax.

Further development
For further developing this application I would move Events into the services layer and make this layer a dedicated infrastructure layer within the web client, solely responsible for handling the logic relevant to data persistence. Above this I would take the business logic out of the controllers and Views into a dedicated layer responsible for handling business logic, data manipulation etc. before transferring information to the controllers for persistence, or views for presentation. The benefits of the approach are that it allows for separation of concerns ensuring that components within a specific layer only handle logic pertaining to that layer. This layered architecture is also well understood, making it conducive to the implemntation of traditional design patterns. Also with responsibilities confined to the specific layers, testing and maintainability is made much easier.