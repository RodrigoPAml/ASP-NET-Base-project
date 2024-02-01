# ASP NET base project
Base ASP NET Kestrel server project with common features for developing with microservices

Current used with postgres but can be adapted to other databases easily

Works with this front-end https://github.com/RodrigoPAml/Next-JS-Base-Repo

## Features
- Clean Architecture (DDD)
- MVC (with front-end represeting the view in NextJS)
- Unit and Integration Test
- Jwt bearer token authentication
- Swagger setup
- Migration application system
- Repository-Service pattern
- Basic filter by input in controller
- Basic order by input in controller
- BCrypt for password security
- Model self validation
- Service hooks
- Entity Auto Mapper

## MVC

MVC, which stands for Model-View-Controller, is a software architectural pattern commonly used in the design and development of interactive and user-friendly applications. It separates an application into three interconnected components, each with its own responsibilities:

- Model: The Model represents the application's data and business logic. It encapsulates the application's data and the rules and operations that can be performed on that data. The Model component is responsible for managing data storage, retrieval, and manipulation. It should be independent of the user interface and the presentation layer.

- View: The View is responsible for presenting the data to the user and for handling user interface elements such as buttons, forms, and menus. It represents the graphical user interface (GUI) or the presentation layer of the application. Views display the data from the Model to the user and receive user input, which is then forwarded to the Controller for processing.

- Controller: The Controller acts as an intermediary between the Model and the View. It receives user input from the View, processes it, and makes appropriate updates to the Model or the View, or both. It contains the application's logic for handling user interactions and business rules. The Controller ensures that the Model and the View remain separate and do not directly communicate with each other. Instead, they communicate through the Controller.

The MVC pattern promotes the separation of concerns in software design, making applications more modular, maintainable, and scalable. It also allows for easier testing of individual components since each component has distinct responsibilities. Many modern software frameworks and technologies, especially in web development, use variations of the MVC pattern or similar architectural patterns to create well-structured and organized applications.

## Project Layers

Clean Architecture implemeted

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/d00bea74-4562-4d88-84b2-a847e4d3f66b)

## Overview

 - Comes with authentication and user, movie and session CRUD
 
 ```sql
 create table if not exists public.user (
	id bigint primary key generated always as identity,
	login varchar(32) not null,
	password text not null,
	name varchar(32) not null,
	profile smallint not null
);

create table if not exists public.movie (
	id bigint primary key generated always as identity,
	name varchar(64) not null,
	synopsis varchar(512),
	duration decimal not null,
	genre smallint not null
);

create table if not exists public.session (
	id bigint primary key generated always as identity,
	movie_id bigint not null,
	date timestamptz not null,
	CONSTRAINT fk_movie FOREIGN KEY (movie_id) REFERENCES movie (id)
);
 ```

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/38120351-d917-427d-b837-fda5f033824d)

### Login

POST http://localhost:5001/Authentication/Login?login=rodrigo&password=rodrigo123 and returns the access token

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/f6d1239c-a623-4ecb-8e79-dd85c77d8a63)

### Create User

POST http://localhost:5001/User/CreateUser and returns the id

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/d39fb0e7-9b6d-424b-90b1-92ba3a705202)

### Get User Paged

GET http://localhost:5001/User/GetPaged?page=1&pageSize=10

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/f50429e7-44be-4547-b468-0ac27c3ec7b7)

and returns

```JSON
{
    "success": true,
    "message": "Records retrieved successfully",
    "content": {
        "data": [
            {
                "id": 1,
                "name": "rodrigo",
                "login": "rodrigo",
                "profile": 0
            },
            {
                "id": 2,
                "name": "Teste",
                "login": "teste",
                "profile": 0
            },
            {
                "id": 3,
                "name": "Teste",
                "login": "test",
                "profile": 0
            },
            {
                "id": 4,
                "name": "newName",
                "login": "test123",
                "profile": 0
            }
        ],
        "page": 1,
        "pageSize": 10,
        "totalRegisters": 4
    }
}
```

### Get paged with filters and order by

#### Filter format

It's an array with the following items, between then its and AND operation

The possibles operations are ```=, !=, >, <, >=, <=, in```

```JSON
[
  {
      "field": "id",
      "value": "1",
      "operation": "="
  }
]
```

and the results

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/56331754-43e3-400a-84a8-6508ae5305e6)

```JSON
{
    "success": true,
    "message": "Records retrieved successfully",
    "content": {
        "data": [
            {
                "id": 1,
                "name": "rodrigo",
                "login": "rodrigo",
                "profile": 0
            }
        ],
        "page": 1,
        "pageSize": 10,
        "totalRegisters": 1
    }
}
```

#### Order by format

It's and object with the following format

```JSON
{
    "field": "id",
    "ascending": false,
}
```

and the results

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/59c71d34-6292-41e6-9824-aab3c7db4b35)

```JSON
{
    "success": true,
    "message": "Records retrieved successfully",
    "content": {
        "data": [
            {
                "id": 4,
                "name": "newName",
                "login": "test123",
                "profile": 0
            },
            {
                "id": 3,
                "name": "Teste",
                "login": "test",
                "profile": 0
            },
            {
                "id": 2,
                "name": "Teste",
                "login": "teste",
                "profile": 0
            },
            {
                "id": 1,
                "name": "rodrigo",
                "login": "rodrigo",
                "profile": 0
            }
        ],
        "page": 1,
        "pageSize": 10,
        "totalRegisters": 4
    }
}
```

## Implementing a new database object

Use the movie and/or session classes to understand how to do it

- Create migration and put in migrations folders (Folder migrations in infra layer)
- Create the entity model and a validator if needed (Domain layer)
- Create a repository for the entity (Domain Layer and Infra layer)
- Create a database mapping for the entity model (Folder Mappings in Infra layer)
- Register in DbContext and database mapping (Infra layer)
- Create a service for the entity (Domain Layer)
- Create a app service and crud models NewEntity, UpdatedEntity (Application Layer)
- Create mapping between domain models and application models (Application Layer)
- Create the controller (Folder controllers in WebAPI layer)
- Don't forget to register all the services (IoC project)
- Test it!


