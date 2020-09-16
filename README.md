# EuroMonitor
Assessment

https://github.com/Davuter/EuroMonitor/blob/master/eur%C4%B1monitor.png

WEB UI

I used Angularjs for development.
I used an ASP.NET Core Angular template using Visual studio IDE.

API GATEWAY API

This Api UI and third party companies have been written to handle all their work from a single point.
The UI calls this api in all its operations. It collects data from microservices in API and transmits it to the frontend or third party company.
Third party companies can access other methods using the get token method and then receive the service.

IDENTITY SERVER

The application is responsible for Authantication and Authorization. It constitutes the security layer of all Applications. It is a single sign on service.
All microservices can be accessed with the token received from the UI. However, the token received from the swagger to test microservices is only valid for that service.
I used ASP.NET Core MVC and IdentityServer4. I used the database as MSSQL.

SUBSCRIPTION Api

It is the API where subscription processes are made. It has been developed as a microservice. I used MSSQL as a database. I used Mediator pattern to reduce Dependency Injection.
I also designed the API in N-Tier layered architecture. I wrote my tests using the xunit library.

CATALOG Api

It is the API where catalog operations are performed. It was developed as a microservice. I used the database MSSQL. I used the Repository and Unit of work patterns together.
I wrote my tests using the xunit library.

I tried to pay attention to SOLID principles in all applications.
I took care to construct the structure in an expandable way. For example, event driven design can be used by adding structures such as RabbitMQ.


