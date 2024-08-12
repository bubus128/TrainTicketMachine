[![LinkedIn][linkedin-badge]][linkedin-url]

<!-- 
Shields.io workflow statuses dont work with privat repositories
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/bubus128/TrainTicketMachine/run_unit_tests.yml?branch=develop&link=https%3A%2F%2Fgithub.com%2Fbubus128%2FTrainTicketMachine%2Factions%2Fworkflows%2Frun_unit_tests.yml)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/bubus128/TrainTicketMachine/run_functional_tests.yml?branch=develop&link=https%3A%2F%2Fgithub.com%2Fbubus128%2FTrainTicketMachine%2Factions%2Fworkflows%2Frun_functional_tests.yml)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/bubus128/TrainTicketMachine/run_integration_tests.yml?branch=develop&link=https%3A%2F%2Fgithub.com%2Fbubus128%2FTrainTicketMachine%2Factions%2Fworkflows%2Frun_integration_tests.yml)
-->

## Test Results

Test Statuses

![Functional Tests](https://github.com/bubus128/TrainTicketMachine/actions/workflows/run_functional_tests.yml/badge.svg?branch=develop)\
![Integration Tests](https://github.com/bubus128/TrainTicketMachine/actions/workflows/run_integration_tests.yml/badge.svg?branch=develop)\
![Unit Tests](https://github.com/bubus128/TrainTicketMachine/actions/workflows/run_unit_tests.yml/badge.svg?branch=develop) 

# TrainTicketMachine

## Description
Station to the Train	Ticket	Machine task

## How to use
1. Clone
2. Run docker-compose up
3. Send get request with "prefix" parameter eg.: localhost:80/Stations?prefix=Ad
4. Response:
  ```json
   {
    "stationsNames": [
        "Adderley Park",
        "Addiewell",
        "Addlestone",
        "Adisham",
        "Adlington (Cheshire)",
        "Adlington (Lancs)",
        "Adwick"
    ],
    "nextLetters": [
        "d",
        "i",
        "l",
        "w"
    ]
  }
 ```
5. Url with station data source is sotred in [appsettings.json](src/TrainTicketMachine.Api/appsettings.json) in section InfrastructureConfig:StationsApiUrl
   
## Quick postman tests results
1. [localhost:80/Stations?prefix=Ad](http://localhost:80/Stations?prefix=Ad)
 ```json
   {
    "stationsNames": [
        "Adderley Park",
        "Addiewell",
        "Addlestone",
        "Adisham",
        "Adlington (Cheshire)",
        "Adlington (Lancs)",
        "Adwick"
    ],
    "nextLetters": [
        "d",
        "i",
        "l",
        "w"
    ]
  }
 ```
2. [localhost:80/Stations?prefix=Adderley](http://localhost:80/Stations?prefix=Adderley)
```json
{
    "stationsNames": [
        "Adderley Park"
    ],
    "nextLetters": [
        " "
    ]
}
```
3. [localhost:80/Stations?prefix=NotAStation](http://localhost:80/Stations?prefix=NotAStation)
```json
{
    "stationsNames": [],
    "nextLetters": []
}
```
 

[linkedin-badge]: https://img.shields.io/badge/LinkedIn-Świsłocki-blue?logo=linkedin
[linkedin-url]: https://www.linkedin.com/in/jakub-swislocki/
