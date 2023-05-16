# ASP NET base project
ASP NET Kestrel server base project with common features for developing

Current used with postgres but can be adapted to other databases easily

# Features
- Jwt bearer token authentication
- Swagger setup
- Migration application system
- Repository service pattern
- Basic filter input in controller
- Basic order by input in controller
- BCrypt for password security
- Model self validation
- Service hooks
- Entity Auto Mapper

## Overview

 - Comes with authentication and user CRUD
 
 ```sql
 create table if not exists public.user (
	id bigint primary key generated always as identity,
	login varchar(32) not null,
	password text not null,
	name varchar(32) not null,
	profile smallint not null
);
 ```

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/b550a0f8-eb56-4052-b552-44643da0c16a)

### Login

POST http://localhost:5001/Authentication/Login?login=rodrigo&password=rodrigo123 and returns the access token

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/f6d1239c-a623-4ecb-8e79-dd85c77d8a63)

### Create User

POST http://localhost:5001/User/CreateUser and returns the id

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/d39fb0e7-9b6d-424b-90b1-92ba3a705202)

### Update User

PUT http://localhost:5001/User/UpdateUser

![image](https://github.com/RodrigoPAml/ASP-NET-Base-project/assets/41243039/2a08c833-6ab0-48fe-9a9e-34a23c74c65f)

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

Use the user classes to understand how to do it

- Create migration and put in migrations folders (Folder migrations)
- Create the entity model and crud models NewEntity and UpdatedEntity with its validations (Folder Models)
- Create a database mapping for the entity model (Folder Mappings)
- Register DbContext and database mapping (Folder registration)
- Create mapping between models and entity (Folder registration)
- Create a repository class (Folder repositories)
- Register repository (Folder registration)
- Create a service class (Folder services)
- Register the service class (Folder registration)
- Create the controller (Folder controllers)
- Test it!


