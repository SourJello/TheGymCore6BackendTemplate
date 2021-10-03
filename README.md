# TheGym

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

A template repo for the backend of a web application that will grow as I have time. 

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

## Libraries, Frameworks, and Databases

- [.Net 6](https://dotnet.microsoft.com/download/dotnet/6.0)
- [EF core](https://docs.microsoft.com/en-us/ef/core/)
- [PostgreSQL](https://www.postgresql.org/)
- [Automapper](https://automapper.org/)
- [FluentValidation](https://fluentvalidation.net/)
- [Newtonsoft](https://www.newtonsoft.com/json)

## Patterns
- Unit of work and Repositories
- Clean(or layered) Architecture
- Generic Controller

## Requirements:
- Docker CLI
- .Net 6 rc.1 SDK

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
## Author

- Patrick K
- 
## License

This project is open source and available under the [MIT License](LICENSE).
