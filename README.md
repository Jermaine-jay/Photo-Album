# Pandora Photo-Album Application
____
#### The Photo Album App is a user-friendly web application that allows you to effortlessly organize, share, and explore your cherished photo memories, as well as putting details about the photograph. Built using modern technologies such as  built using ASP.NET MVC, Entity Framework Core, AutoMapper, and SQL Server.


## Technologies Used
____
* ASP.NET MVC: The web framework used to build the application's architecture and handle user requests.

* AutoMapper: A mapping library used to simplify the process of mapping between different data models.

* SQL Server: The relational database management system used to store and manage application data.
  
* Entity Framework Core (EF Core): A powerful and flexible Object-Relational Mapping (ORM) tool for working with the application's database.
  > - *Microsoft Entity Framework core Design V 6.0.1*
  > - *Microsoft Entity Framework core Tools V 6.0.1*
  > - *Microsoft Entity Framework core SqlServer V 6.0.1*


## Features
____
* User Authentication and Authorization: Secure user registration and login system, ensuring that only authorized users can access and participate in auctions,
  Ensure only admins can make changes to the system. Note, you cannot register with an invalid email address.

* User Update: Only confirmed users will be able to log in, place bids and change details. An Email with a token will be sent when a change of password is initiated.

* Create and Organize Albums: Easily create albums and categorize your photos to keep your memories organized and easily accessible.

* Upload and Manage Photos: Effortlessly upload photos to your albums, arrange them in the desired order, and add captions or tags for better organization.

* Privacy Controls: Choose whether your albums are private, shared with specific users, or made public for everyone to enjoy.
  
* User Profiles: Users can view the status of previous bids and ongoing bids, as well as the status of cars they placed a bid. Users can change profile pictures as well as other details.

* Admin Profile: Admin can easily view all registered users and photos uploaded.


## Getting Started
_____
* Clone the repository: git clone https://github.com/Germaine-jay/Photo-Album.git

* Install required packages: dotnet restore

* Update the database connection string in the appsettings.json file to point to your SQL Server instance.

* Apply database migrations: dotnet ef database update

* Run the application: dotnet run

* Access the application in your web browser at http://localhost:7121


## Contributing
_____
If you'd like to contribute to the Car Auction App, please follow these steps:

* Fork the repository.

* Create a new branch for your feature or bug fix: git checkout -b feature/your-feature-name

* Make your changes and test thoroughly.

* Commit your changes: git commit -m "Add your commit message here"

* Push to your forked repository: git push origin feature/your-feature-name

* Create a pull request, describing your changes and the problem they solve.

## Default Users
___
| UserName   | Password   | Role       |
| ---------- | ---------- | ---------- |
| jota10     | 12345qwert | User       |
| jermaine10 | 12345qwert | SuperAdmin |
| idan10     | 12345qwert | Admin      |  
