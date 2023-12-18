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

Above is a UML class diagram of the domain model for our _Chirp!_ application. Here you can see the fields the objects contain and how they associate with each other.

## Architecture — In the small

![Illustration of the _Chirp!_ architecture in the small.](images/OnionArchitecture.png)

Above is an illustration of the organization of our _Chirp!_ application. We use the architectural pattern called Onion Architecture to structure our code base. For each layer represented by a different nuance of gray, we illustrate the classes, interfaces, and packages that are part of the layer. The arrows illustrate the dependencies between the layers. For simplicity's sake, we have not illustrated the dependencies between the classes and interfaces within each layer. The illustration shows how dependencies flow inward and never outward, meaning that the inner layers have no knowledge of the outer layers. Our architecture consists of three layers, each represented by a different project in our code base. 

## Architecture of deployed application

![Illustration of the architecture of the deployed _Chirp!_ application.](images/Deployment.png)

Above is a deployment diagram that illustrates the architecture of our deployed _Chirp!_ application. It is a client-server application that is deployed to Azure, where the web app and the SQL database are hosted on different servers. Their means of communication are also illustrated. A legend is provided to the right of the diagram.

## User activities
Below are illustrations that show four different user journeys, which are common in the _Chirp!_ application.

![Activity diagram of an unauthorized user's journey registering for the _Chirp!_ application.](images/Register.png)

Above is an activity diagram of an unauthorized user's journey registering for the _Chirp!_ application.

![Activity diagram of an unauthenticated user's journey logging in to the _Chirp!_ application.](images/Login.png)

Above is an activity diagram of an unauthenticated user's journey logging in to the _Chirp!_ application.

![Activity diagram of an authenticated user's journey in the _Chirp!_ application, sending of cheep.](images/SendingCheep.png) 

Above is an activity diagram of an authenticated user's journey in the _Chirp!_ application, sending of cheep.

![Activity diagram of an unauthenticated user's journey using the _Chirp!_ application.](images/unauthenticated_user_acitivity.png) 

Above is an activity diagram of an unauthenticated user's journey using the _Chirp!_ application.

## Sequence of functionality/calls through _Chirp!_
![Sequence diagram of calls through the _Chirp!_ application.](images/SequenceCalls.png)

The illustration above shows a sequence diagram of calls through the _Chirp!_ application. There are four lifelines; 'Web browser', 'Chirp.Web', 'chirpdb' and 'OAuth' in the diagram. The web browser should be interpreted as the client, Chirp.Web as the web application of the program, chirpdb as the database and OAuth as the web protocol that handles the user authentication. The diagram illustrates some of the communication that goes through the lifelines when using the _Chirp!_ application.

# Process
OBS remember to write about our 'logbog'

## Build, test, release, and deployment
Below are three illustrations of our workflows 'Build and Deploy', 'Build and Test' and 'Release Chirp'. 
![Activity diagram of the workflow for build and test of the _Chirp!_ application](images/BuildAndTest.png)

The illustration above shows how the activities in our workflow 'Build and Test' are activated after each other. This workflow runs on pushes and pull-request to Main. This is done to make sure that none of our new changes or merges have destroyed our ability to build and test the program. 
In the workflow, first our GitHub action version is checked out and chosen so that our workflow can access it. Next .NET is set up with version 7 before restoring our dependencies by running the command 'dotnet restore'. This command ensures that the packages that out program depend on are downloaded and have no conflict between them. After this the program is ready to be build. The command 'dotnet build --no-restore' is run which build the project and its dependencies into a set of binaries. After the build our testes are run with the command 'dotnet test --no-build --verbosity normal' which will run our test.


![Activity diagram of the workflow for release of the _Chirp!_ application.](images/ReleaseChirp.png)

The above illustration shows how the release of our Chirp Application is run. The workflow is activated when a push to Main happens that contains a tag. By using a tag when pushing to main we are able to mark a checkpoint in the project and give them a "name" or "title". These can be small or larger depending on the tag. 

In the workflow first 'Checkout' and 'Setup dotnet' with version 7 commands are run followed by 'Restore Dependencies'. After these commands four builds are run after each other. A Windows, Linux, MacOS and MacOS Arm exeuteable are build. 

After the builds have finished these are published. The executeable are created and released as zip-files that can be downladed to your computer and ran. 


![Activity diagram of the workflow for build and deploy of the _Chirp!_ application.](images/BuildAndDeploy.png)

 The illustration above shows the Deployment of our Chirp Application. The workflow is activated on pushes to Main. In the workflow we first run 'Checkout' and 'Setup dotnet' with version 7 commands. These are follwed by the building command 'dotnet build src/Chirp.Web/ --configuration Release' 



## Team work
![Activity diagram of the workflow for issues in the GitHub project.](images/Issues.png)

The activity diagram above shows how we have been working with the requirements for the project. We have aimed to create issues weekly as the requirements were published. We usually tried to prioritze all the issues, so most were initially put to 'in progress'. Some of the issues that related to features that were just 'nice to have', we did not always get to, so they were moved to 'on hold' and either reviewed again or automatically deleted. That way, our backlog did not get too cluttered, and we were able to navigate the most important issues.

<img src="images/logbook-clip.png" alt="Screenshot of some of the logbook." style="width:400px;height:auto;">

To further track the progress on the project, we kept an unformal logbook in Google Docs. This was merely done for our own sake. Above is a screenshot of some clipped-together content from the logbook. For each of our meetings, we wrote down what was done that day together with potential questions for our meetings with our TA. We also wrote down some of the agenda of the next meeting so it would be easy to get started. The logbook enabled us to attain more structure to our process as we were able to keep track of additional information that did not fit on the project board. Furthermore, it eased the process of catching up on the project if one of the group members did not attend a meeting.

## How to make _Chirp!_ work locally
In order to run the _Chirp!_ application locally, the repository from GitHub needs to be cloned, which can be done from the following address: https://github.com/ITU-BDSA23-GROUP1/Chirp. 

Next, a container needs to be set up. This is done so that an SQL Server database can work locally. This can for example be done with the Docker platform. To install Docker, follow this guide: https://docs.docker.com/engine/install/.

To run a SQL Server database, the following command needs to be run:

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/mssql/server:2022-latest

Here, you can replace <YourStrong@Passw0rd> with a password of your choice. The command will setup, configure and start up a database server.

To connect the program and the database server, an appsettings.json file should be made inside the Chirp.Web folder. In this file, a connection string should be set up like so:

{
“your_ConnectionString”: "Data Source=localhost,1433;Initial Catalog=Chirp;User=sa;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True"
}

This connection string should contain the same port and password as in the command to setup the database server. 

## How to run test suite locally
To run the test suite locally it is first needed to run the program such that our playwright tests can run. First the docker container with the local database should be started. It is then needed to have two terminals running. One should be navigated to "Chirp.Web". After being navigated here the program should be run with the command: 'dotnet run' to which the program should start up and begin running locally.
Another terminal should now be opened and navigated to the top folder "Chirp". Then the command 'dotnet test' should be run in this terminal which should run all our tests and give back their test results in the terminal.

- Briefly describe what kinds of tests you have in your test suites and what they are testing.
In our test suite we have unit-tests and UI-tests. Our unit-tests test our methods in

# Ethics

## License
In our software we decided to use the MIT license. This was our decision as the MIT license is a permissive software license. This means that our software can be freely used and distributed by others. However, we don't provide any sort of warranty in the event that anything breaks and therefore are not liable for any damages or claims.

## LLMs, ChatGPT, CoPilot, and others
During the development of our project we used ChatGPT and Copilot. We only used ChatGPT once in the beginning of our project, when we were trying to create a process for running a bash script. We had prompted ChatGPT with our issue and asked for its help with generating some code. The given code was then fitted such that it fulfilled our needs. This code has since been deleted.

We used Copilot more frequently during our development. This was activated for most of our development and therefore gave suggestions and helped autofill the code we were writing. We did also prompt Copilot sometimes where it gave us suggestions on how to fix our current prompt. Or when having an error in our code asking it how to fix it. 

Often when we were coding and Copilot gave suggestions they were helpful and for the most parts completely correct. Additionally, we needed to make some small fixes. When we prompted Copilot to do something specific it did however not always go as well. The suggestions often went far from what we had intended or wanted. Especially when we asked about errors in our code it did not help at all.

We do believe that using LLMs sped up our development. It made it much faster when writing, since it autocompleted lines or even entire methods for us. Prompting the LLMs with questions did not always provide the answers we needed, but it did however provide some insights to how the LLM thought it could be done and in which direction we did not want to go.