---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group `1`
author:
- "Carmen Alberte Nielsen <alcn@itu.dk>"
- "Mathilde Julie Gonzalez-Knudsen <mgon@itu.dk>"
- "Olivia Høyer Moesgaard <olmo@itu.dk>"
- "Emilie Kold Ruskjær <emru@itu.dk>"
- "Alex Bjørnskov <alebj@itu.dk>"
numbersections: true
---
Vi skal huske det her: Store all sources of your diagrams, i.e., PlantUML diagram source code or DrawIO XML files under docs in a directory called diagrams.

# Design and Architecture of _Chirp!_

## Domain model
![Illustration of the _Chirp!_ data model as UML class diagram.](images/ClassDiagram.png)
Above is a UML class diagram of the domain model for our _Chirp!_ application. 

## Architecture — In the small

![Illustration of the _Chirp!_ architecture in the small.](images/OnionArchitecture.png)
Above is an illustration of the organization of our _Chirp!_ application. We use the architectural pattern called Onion Architecture to structure our code base. For each layer we illustrate the classes, interfaces, and packages that are part of the layer. The arrows illustrate the dependencies between the layers. For simplicity's sake, we have not illustrated the dependencies between the classes and interfaces within each layer. The illustration shows how dependencies flow inward and never outward, meaning that the inner layers have no knowledge of the outer layers. Our architecture consists of three layers, each represented by a different project/package(?) in our code base. The innermost layer Chirp.Core contains the data transfer objects (DTOs) and interfaces for the repositories. The middle layer Chirp.Infrastructure contains implementations of the repositories, the database context, and the domain model. It also contains the migrations for the database. The outermost layer Chirp.Web contains the startup class, the database initializer and views/pages(?).

## Architecture of deployed application

![Illustration of the architecture of the deployed _Chirp!_ application.](images/Deployment.png)
Above is a deployment diagram that illustrates the architecture of our deployed _Chirp!_ application. It is a client-server application that is deployed to Azure, where the web app and the SQL database are hosted on different servers. Their means of communication are also illustrated. A legend is provided to the right of the diagram.

## User activities
![Activity diagram of an unauthorized user's journey registering for the _Chirp!_ application.](images/Register.png)
![Activity diagram of an unauthenticated user's journey logging in to the _Chirp!_ application.](images/Login.png)
![Activity diagram of an authenticated user's journey in the _Chirp!_ application, sending of cheep.](images/SendingCheep.png)

## Sequence of functionality/calls through _Chirp!_
![Sequence diagram of calls through the _Chirp!_ application.](images/SequenceCalls.png)

# Process

## Build, test, release, and deployment
![Activity diagram of the workflow for build and test of the _Chirp!_ application](images/BuildAndTest.png)
![Activity diagram of the workflow for release of the _Chirp!_ application.](images/ReleaseChirp.png)
![Activity diagram of the workflow for build and deploy of the _Chirp!_ application.](images/BuildAndDeploy.png)

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License
In our software we decided to use the MIT license. This was our decision as the MIT license is a permissive software license. This means that our software can be freely used and distributed by others. We however don't provide any sort of warranty in the event that anything breaks and therefore are not liable for any damages or claims.

## LLMs, ChatGPT, CoPilot, and others
