# Solution Overview

#### Pre-Requisites####
- ASP.NET Core 3.0
- Redis (For distributed cache management) 

#### Solution discussion ####

Solution has 3 projects
- SW.Api : ASP.NET : web api project 
This has various folders under which the code is organized. In a real world project, each of these folders themselves could 
make up a separate class library of its own.

It makesuse of interfaces and separates various implementations. For now, IPersonSource has one concrete implementation which is PersonWebSource, which actually talks to the end point that is provided in the exercise.

-SW.App : Console application that utilizes the Api project to retrieve the data

-SW.Tests : Unit test project


#### Packages / Technologies ####

- Microsoft.Extensions.Caching.Redis

#### Known Limitations / Critique ####

- Paging could be implemented to improve performance if the data source has large number of records, with each request returning a smaller page size
- Azure deployment of web api
- Moving constants to appSettings.json


