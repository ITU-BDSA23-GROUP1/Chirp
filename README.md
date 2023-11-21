// For later use by us to make commit messages easier to write.
Co-authored-by: Mathilde <mgon@itu.dk>
Co-authored-by: Carmen <alcn@itu.dk>
Co-authored-by: Olivia <olmo@itu.dk>
Co-authored-by: Emilie <emru@itu.dk>
Co-authored-by: Alex <alebj@itu.dk>



// Husk at noter at test er navngivet og arrangeret ud fra microsoft anbefalinger
// link: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test

## User manual:

**Changing migrations:**
Step 1: Delete current migrations
Step 2: Navigate to src/Chirp.Infrastructure
Step 3: Write in terminal 
`dotnet ef migrations add InitialCreate --startup-project ../Chirp.Web/Chirp.Web.csproj`
(Where InitialCreate is the name)

**Build:**
In Chirp folder run the command: 
`dotnet build`

**Run program:**
Navigate to src/Chirp.Web and run the command: 
`dotnet run`

**Test program:**
In Chirp folder run the command: 
`dotnet test`

**Making a release/tag:**
Step 1: From whatever folder use git tag to see all current tags.
Step 2: Then in the terminal use 
`git tag <vx.y.z>` 
and change x, y and z to be a number
Step 3: Push tag by writing in the terminal 
`git push origin <vx.y.z>` 
where vx.y.z is what you made in step 2

**Other**
The program runs on http://localhost:5000 
(The public timeline, page 1 with the latest 32 cheeps are shown as default)
To change to page number 2, add ?page=2 in URL
To see cheeps from the user with auther id = 10, 
add /10 in the URL

______________________________________________________________________

**Docker in bash-terminal** (Linux supports sqlcmd, therefore bash)
`sudo docker exec -it [containername] "bash"`
(Login with computer password)

SQL-tool inside docker-container
`/opt/mssql-tools/bin/sqlcmd -S localhost -U SA`
(Login with database password)

Now you can run sql commmands. Add GO after all commands. 

**Show Databases**
To List all the databases on the server:
`sp_databases`
`GO`

**Use database**
To use database called Chirp:
`use Chirp`
`GO`

**List tables**
`sp_tables`
`GO`

**Show the content of the Cheeps table**
`select * from Cheeps`
`GO`
