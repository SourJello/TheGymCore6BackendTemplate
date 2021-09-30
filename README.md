# TheGym
A template repo for the backend of a web application that will grow as I have time. 

Libraries, frameworks, and databases used: .Net Core 6 rc1, EF core, postgres, npgSql, Automapper, and Newtonsoft.
Patterns used: Unit of work and Repositories, Clean(or layered) Architecture, and a Generic Controller. 

Requirements: Docker CLI, .Net 6 rc.1 SDK

To Run:
- Run '$docker-compose up' from src\TheGymInfrastructure\Docker
- Use the package manager within visual studios to
    create migrations from within the infrastructure project src\TheGymInfrastructure
    (if you don't have VS and the package manager to make your migrations 
    with you can install the EF core tools extension for the dotnet cli
    here https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- PM> add-migration new
- PM> update-database
- Run the application layer at src\TheGymApplication
- $ dotnet run

TODO:
- Create new template that builds a docker image of the application
- Maybe Create new template with Authorization and Authentication?
- Maybe a converter from utc to the https requests local time would be cool if that is possible