# Chirp

// For later use by us to make commit messages easier to write.
Co-authored-by: Mathilde <mgon@itu.dk>
Co-authored-by: Carmen <alcn@itu.dk>
Co-authored-by: Olivia <olmo@itu.dk>
Co-authored-by: Emilie <emru@itu.dk>
Co-authored-by: Alex <alebj@itu.dk>



// Husk at noter at test er navnviget og arrangeret ud fra microsoft anbefalinger
// link: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test

User manual:
Test:
In Chirp folder run the command: dotnet test

Run program:
First start a server in one terminal by navigating to Chirp.CSVDBService
In there run the command: dotnet run


Then open another terminal and navigate to Chirp.CLI
In there run either the command: dotnet run read
To see the current cheeps

Or run the command: dotnet run cheep "your message"
To make a cheep